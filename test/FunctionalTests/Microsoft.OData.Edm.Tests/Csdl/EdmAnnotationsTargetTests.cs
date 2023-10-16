//---------------------------------------------------------------------
// <copyright file="EdmAnnotationsTargetTests.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
#if NETCOREAPP3_1
using System.Text.Encodings.Web;
using System.Text.Json;
#endif
using System.Xml;
using System.Xml.Linq;
using Microsoft.OData.Edm.Csdl;
using Microsoft.OData.Edm.Validation;
using Microsoft.OData.Edm.Vocabularies;
using Microsoft.OData.Edm.Vocabularies.V1;
using Xunit;

namespace Microsoft.OData.Edm.Tests.Csdl
{
    public class EdmAnnotationsTargetTests
    {
        #region Write Annotations
        [Fact]
        public void ShouldWriteAnnotationForEntitySetProperty()
        {
            // Arrange
            EdmModel model = new EdmModel();
            EdmEntityType customer = new EdmEntityType("NS", "Customer");
            customer.AddKeys(customer.AddStructuralProperty("Id", EdmCoreModel.Instance.GetInt32(false)));
            customer.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String, isNullable: false);
            model.AddElement(customer);

            EdmEntityContainer container = new EdmEntityContainer("NS", "Default");
            EdmEntitySet entitySet = new EdmEntitySet(container, "Customers", customer);
            container.AddElement(entitySet);
            model.AddElement(container);

            IEdmProperty nameProperty = customer.DeclaredProperties.Where(x => x.Name == "Name").FirstOrDefault();

            EdmAnnotationsTarget target = new EdmAnnotationsTarget(container, entitySet, nameProperty);

            EdmTerm term = new EdmTerm("NS", "MyTerm", EdmCoreModel.Instance.GetString(true));
            model.AddElement(term);
            EdmVocabularyAnnotation annotation = new EdmVocabularyAnnotation(target, term, new EdmStringConstant("Name OutOfLine MyTerm Value"));
            annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.OutOfLine);
            model.SetVocabularyAnnotation(annotation);

            WriteAndVerifyXml(model, "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
              "<edmx:Edmx Version=\"4.0\" xmlns:edmx=\"http://docs.oasis-open.org/odata/ns/edmx\">" +
                "<edmx:DataServices>" +
                  "<Schema Namespace=\"NS\" xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">" +
                    "<EntityType Name=\"Customer\">" +
                      "<Key>" +
                        "<PropertyRef Name=\"Id\" />" +
                      "</Key>" +
                      "<Property Name=\"Id\" Type=\"Edm.Int32\" Nullable=\"false\" />" +
                      "<Property Name=\"Name\" Type=\"Edm.String\" Nullable=\"false\" />" +
                    "</EntityType>" +
                    "<Term Name=\"MyTerm\" Type=\"Edm.String\" />" +
                    "<EntityContainer Name=\"Default\">" +
                       "<EntitySet Name=\"Customers\" EntityType=\"NS.Customer\" />" +
                    "</EntityContainer>" +
                    "<Annotations Target=\"NS.Default/Customers/Name\">" +
                      "<Annotation Term=\"NS.MyTerm\" String=\"Name OutOfLine MyTerm Value\" />" +
                    "</Annotations>" +
                  "</Schema>" +
                "</edmx:DataServices>" +
              "</edmx:Edmx>");

            // Act & Assert for JSON
            WriteAndVerifyJson(model, @"{
  ""$Version"": ""4.0"",
  ""$EntityContainer"": ""NS.Default"",
  ""NS"": {
    ""Customer"": {
      ""$Kind"": ""EntityType"",
      ""$Key"": [
        ""Id""
      ],
      ""Id"": {
        ""$Type"": ""Edm.Int32""
      },
      ""Name"": {}
    },
    ""MyTerm"": {
      ""$Kind"": ""Term"",
      ""$Nullable"": true
    },
    ""Default"": {
      ""$Kind"": ""EntityContainer"",
      ""Customers"": {
        ""$Collection"": true,
        ""$Type"": ""NS.Customer""
      }
    },
    ""$Annotations"": {
      ""NS.Default/Customers/Name"": {
        ""@NS.MyTerm"": ""Name OutOfLine MyTerm Value""
      }
    }
  }
}");
        }

        #endregion

        #region Read Annotations

        [Fact]
        public void ParsingEntitySetPropertyOutOfLineAnnotationsWorks()
        {
            string types =
@"<Term Name=""MyTerm"" Type=""Edm.String""/>
<Annotations Target=""NS.Default/Customers/Name"" >
  <Annotation Term=""NS.MyTerm"" String=""Name OutOfLine MyTerm Value"" />
</Annotations>";

            string properties =
                @"<Property Name=""Name"" Type=""Edm.String"" Nullable=""false"" />";

            IEdmModel model = GetEdmModel(types: types, properties: properties);
            Assert.NotNull(model);

            IEnumerable<EdmError> errors;
            Assert.True(model.Validate(out errors), String.Format("Errors in validating model. {0}", String.Concat(errors.Select(e => e.ErrorMessage))));

            var customer = model.SchemaElements.OfType<IEdmEntityType>().FirstOrDefault(c => c.Name == "Customer");
            Assert.NotNull(customer);

            IEdmTerm fooBarTerm = model.FindDeclaredTerm("NS.MyTerm");
            Assert.NotNull(fooBarTerm);

            // Name
            IEdmProperty nameProperty = customer.DeclaredProperties.Where(x => x.Name == "Name").FirstOrDefault();
            Assert.NotNull(nameProperty);

            EdmAnnotationsTarget target = new EdmAnnotationsTarget(model.ResolveAnnotationsTarget("NS.Default/Customers/Name"));

            string nameAnnotation = GetStringAnnotation(model, target, fooBarTerm, EdmVocabularyAnnotationSerializationLocation.OutOfLine);
            Assert.Equal("Name OutOfLine MyTerm Value", nameAnnotation);
        }

        #endregion

        #region Helper Methods
        internal static void WriteAndVerifyXml(IEdmModel model, string expected, CsdlTarget target = CsdlTarget.OData)
        {
            using (StringWriter sw = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Encoding = System.Text.Encoding.UTF8;

                using (XmlWriter xw = XmlWriter.Create(sw, settings))
                {
                    IEnumerable<EdmError> errors;
                    CsdlWriter.TryWriteCsdl(model, xw, target, out errors);
                    xw.Flush();
                }

                string actual = sw.ToString();
                Assert.Equal(expected, actual);
            }
        }

        internal void WriteAndVerifyJson(IEdmModel model, string expected, bool indented = true, bool isIeee754Compatible = false)
        {
#if NETCOREAPP3_1
            using (MemoryStream memStream = new MemoryStream())
            {
                JsonWriterOptions options = new JsonWriterOptions
                {
                    Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    Indented = indented,
                    SkipValidation = false
                };

                using (Utf8JsonWriter jsonWriter = new Utf8JsonWriter(memStream, options))
                {
                    CsdlJsonWriterSettings settings = CsdlJsonWriterSettings.Default;
                    settings.IsIeee754Compatible = isIeee754Compatible;
                    IEnumerable<EdmError> errors;
                    bool ok = CsdlWriter.TryWriteCsdl(model, jsonWriter, settings, out errors);
                    jsonWriter.Flush();
                    Assert.True(ok);
                }

                memStream.Seek(0, SeekOrigin.Begin);
                string actual = new StreamReader(memStream).ReadToEnd();
                Assert.Equal(expected, actual);
            }
#endif
        }

        private static IEdmModel GetEdmModel(string types = "", string properties = "")
        {
            const string template = @"<edmx:Edmx Version=""4.0"" xmlns:edmx=""http://docs.oasis-open.org/odata/ns/edmx"">
  <edmx:DataServices>
    <Schema Namespace=""NS"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <EntityType Name=""Customer"">
        <Key>
          <PropertyRef Name=""ID"" />
        </Key>
        <Property Name=""ID"" Type=""Edm.Int32"" Nullable=""false"" />
        {1}
      </EntityType>
      <ComplexType Name=""Address"">
        <Property Name=""Street"" Type=""Edm.String"" />
        {1}
      </ComplexType>
      {0}
      <EntityContainer Name =""Default"">
         <EntitySet Name=""Customers"" EntityType=""NS.Customer"" />
      </EntityContainer>
    </Schema>
  </edmx:DataServices>
</edmx:Edmx>";
            string modelText = string.Format(template, types, properties);

            IEdmModel model;
            IEnumerable<EdmError> errors;

            bool result = CsdlReader.TryParse(XElement.Parse(modelText).CreateReader(), out model, out errors);
            Assert.True(result);
            return model;
        }

        private string GetStringAnnotation(IEdmModel model, IEdmVocabularyAnnotatable target, IEdmTerm term, EdmVocabularyAnnotationSerializationLocation location)
        {
            IEdmVocabularyAnnotation annotation = model.FindVocabularyAnnotations<IEdmVocabularyAnnotation>(target, term).FirstOrDefault();
            if (annotation != null)
            {
                Assert.True(typeof(IEdmAnnotationsTarget).IsAssignableFrom(annotation.Target.GetType()));

                Assert.True(annotation.GetSerializationLocation(model) == location);

                IEdmStringConstantExpression stringConstant = annotation.Value as IEdmStringConstantExpression;
                if (stringConstant != null)
                {
                    return stringConstant.Value;
                }
            }

            return null;
        }

        #endregion
    }
}
