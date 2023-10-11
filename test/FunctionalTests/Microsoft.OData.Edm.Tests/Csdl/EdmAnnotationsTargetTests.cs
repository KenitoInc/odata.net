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
        #region Optional Parameters

        [Fact]
        public void ShouldWriteOutOfLineOptionalParametersOverwriteInLineOptionalParameter()
        {
            // Arrange
            var stringTypeReference = new EdmStringTypeReference(EdmCoreModel.Instance.GetPrimitiveType(EdmPrimitiveTypeKind.String), false);
            var model = new EdmModel();
            var function = new EdmFunction("NS", "TestFunction", stringTypeReference);
            //var optionalParamWithDefault = new EdmOptionalParameter(function, "optionalParamWithDefault", stringTypeReference, "InlineDefaultValue");
            var optionalParamWithDefault = new EdmOptionalParameter(function, "optionalParamWithDefault", stringTypeReference, "InlineDefaultValue");
            function.AddParameter(optionalParamWithDefault);
            model.AddElement(function);

            EdmAnnotationsTarget annotationsTarget = new EdmAnnotationsTarget("NS.TestFunction(Edm.String)/optionalParamWithDefault", optionalParamWithDefault);

            // parameter with default value
            IEdmComplexType optionalParameterType = CoreVocabularyModel.Instance.FindDeclaredType("Org.OData.Core.V1.OptionalParameterType") as IEdmComplexType;
            Assert.NotNull(optionalParameterType);

            IEdmRecordExpression optionalParameterRecord = new EdmRecordExpression(
                    new EdmComplexTypeReference(optionalParameterType, false),
                    new EdmPropertyConstructor("DefaultValue", new EdmStringConstant("OutofLineValue")));
            //EdmVocabularyAnnotation annotation = new EdmVocabularyAnnotation(optionalParamWithDefault, CoreVocabularyModel.OptionalParameterTerm, optionalParameterRecord);
            EdmVocabularyAnnotation annotation = new EdmVocabularyAnnotation(annotationsTarget, CoreVocabularyModel.OptionalParameterTerm, optionalParameterRecord);
            annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.OutOfLine);
            model.SetVocabularyAnnotation(annotation);

            // Act & Assert for XML
            WriteAndVerifyXml(model, "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
            "<edmx:Edmx Version=\"4.0\" xmlns:edmx=\"http://docs.oasis-open.org/odata/ns/edmx\">" +
              "<edmx:DataServices>" +
                "<Schema Namespace=\"NS\" xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">" +
                  "<Function Name=\"TestFunction\">" +
                    "<Parameter Name=\"optionalParamWithDefault\" Type=\"Edm.String\" Nullable=\"false\" />" +
                    "<ReturnType Type=\"Edm.String\" Nullable=\"false\" />" +
                  "</Function>" +
                  "<Annotations Target=\"NS.TestFunction(Edm.String)/optionalParamWithDefault\">" +
                   "<Annotation Term=\"Org.OData.Core.V1.OptionalParameter\">" +
                     "<Record Type=\"Org.OData.Core.V1.OptionalParameterType\">" +
                       "<PropertyValue Property=\"DefaultValue\" String=\"OutofLineValue\" />" +
                     "</Record>" +
                  "</Annotation>" +
                 "</Annotations>" +
                "</Schema>" +
              "</edmx:DataServices>" +
            "</edmx:Edmx>");

            // Act & Assert for JSON
            WriteAndVerifyJson(model, @"{
  ""$Version"": ""4.0"",
  ""NS"": {
    ""TestFunction"": [
      {
        ""$Kind"": ""Function"",
        ""$Parameter"": [
          {
            ""$Name"": ""optionalParamWithDefault""
          }
        ],
        ""$ReturnType"": {}
      }
    ],
    ""$Annotations"": {
      ""NS.TestFunction(Edm.String)/optionalParamWithDefault"": {
        ""@Org.OData.Core.V1.OptionalParameter"": {
          ""@type"": ""Org.OData.Core.V1.OptionalParameterType"",
          ""DefaultValue"": ""OutofLineValue""
        }
      }
    }
  }
}");
        }

        #endregion

        [Fact]
        public void ShouldWriteAnnotationForEnumMember()
        {
            // Arrange
            EdmModel model = new EdmModel();
            EdmEnumType appliance = new EdmEnumType("NS", "Appliance", EdmPrimitiveTypeKind.Int64, isFlags: true);
            model.AddElement(appliance);

            var stove = new EdmEnumMember(appliance, "Stove", new EdmEnumMemberValue(1));
            appliance.AddMember(stove);

            var washer = new EdmEnumMember(appliance, "Washer", new EdmEnumMemberValue(2));
            appliance.AddMember(washer);

            EdmAnnotationsTarget target = new EdmAnnotationsTarget("NS.Appliance/Stove", stove);

            EdmVocabularyAnnotation annotation = new EdmVocabularyAnnotation(target, CoreVocabularyModel.LongDescriptionTerm, new EdmStringConstant("Stove Inline LongDescription"));
            annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.Inline);
            model.SetVocabularyAnnotation(annotation);

            annotation = new EdmVocabularyAnnotation(washer, CoreVocabularyModel.LongDescriptionTerm, new EdmStringConstant("Washer OutOfLine LongDescription"));
            annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.OutOfLine);
            model.SetVocabularyAnnotation(annotation);

            EdmTerm term = new EdmTerm("NS", "MyTerm", EdmCoreModel.Instance.GetString(true));
            model.AddElement(term);
            annotation = new EdmVocabularyAnnotation(stove, term, new EdmStringConstant("Stove OutOfLine MyTerm Value"));
            annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.OutOfLine);
            model.SetVocabularyAnnotation(annotation);

            annotation = new EdmVocabularyAnnotation(washer, term, new EdmStringConstant("Washer Inline MyTerm Value"));
            annotation.SetSerializationLocation(model, EdmVocabularyAnnotationSerializationLocation.Inline);
            model.SetVocabularyAnnotation(annotation);

            WriteAndVerifyXml(model, "<?xml version=\"1.0\" encoding=\"utf-16\"?>" +
              "<edmx:Edmx Version=\"4.0\" xmlns:edmx=\"http://docs.oasis-open.org/odata/ns/edmx\">" +
                "<edmx:DataServices>" +
                  "<Schema Namespace=\"NS\" xmlns=\"http://docs.oasis-open.org/odata/ns/edm\">" +
                    "<EnumType Name=\"Appliance\" UnderlyingType=\"Edm.Int64\" IsFlags=\"true\">" +
                      "<Member Name=\"Stove\" Value=\"1\">" +
                        "<Annotation Term=\"Org.OData.Core.V1.LongDescription\" String=\"Stove Inline LongDescription\" />" +
                      "</Member>" +
                      "<Member Name=\"Washer\" Value=\"2\">" +
                        "<Annotation Term=\"NS.MyTerm\" String=\"Washer Inline MyTerm Value\" />" +
                      "</Member>" +
                    "</EnumType>" +
                    "<Term Name=\"MyTerm\" Type=\"Edm.String\" />" +
                    "<Annotations Target=\"NS.Appliance/Washer\">" +
                      "<Annotation Term=\"Org.OData.Core.V1.LongDescription\" String=\"Washer OutOfLine LongDescription\" />" +
                    "</Annotations>" +
                    "<Annotations Target=\"NS.Appliance/Stove\">" +
                      "<Annotation Term=\"NS.MyTerm\" String=\"Stove OutOfLine MyTerm Value\" />" +
                    "</Annotations>" +
                  "</Schema>" +
                "</edmx:DataServices>" +
              "</edmx:Edmx>");

            // Act & Assert for JSON
            WriteAndVerifyJson(model, @"{
  ""$Version"": ""4.0"",
  ""NS"": {
    ""Appliance"": {
      ""$Kind"": ""EnumType"",
      ""$UnderlyingType"": ""Edm.Int64"",
      ""$IsFlags"": true,
      ""Stove"": 1,
      ""Stove@Org.OData.Core.V1.LongDescription"": ""Stove Inline LongDescription"",
      ""Washer"": 2,
      ""Washer@NS.MyTerm"": ""Washer Inline MyTerm Value""
    },
    ""MyTerm"": {
      ""$Kind"": ""Term"",
      ""$Nullable"": true
    },
    ""$Annotations"": {
      ""NS.Appliance/Stove"": {
        ""@NS.MyTerm"": ""Stove OutOfLine MyTerm Value""
      },
      ""NS.Appliance/Washer"": {
        ""@Org.OData.Core.V1.LongDescription"": ""Washer OutOfLine LongDescription""
      }
    }
  }
}");
        }
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

        [Fact]
        public void ParsingEnumMemberWithAnnotationsWorks()
        {
            string types =
@"<EnumType Name=""Color"" >
  <Member Name=""Red"" Value=""1"" >
    <Annotation String=""Inline Description"" Term=""Org.OData.Core.V1.LongDescription""/>
    <Annotation String=""Inline MyTerm Value"" Term=""NS.MyTerm""/>
  </Member>
  <Member Name=""Blue"" Value=""2"" />
</EnumType>
<Term Name=""MyTerm"" Type=""Edm.String""/>
<Annotations Target=""NS.Color/Blue"" >
  <Annotation String=""OutOfLine Description"" Term=""Org.OData.Core.V1.LongDescription""/>
  <Annotation String=""OutOfLine MyTerm Value"" Term=""NS.MyTerm""/>
</Annotations>";

            IEdmModel model = GetEdmModel(types: types);
            Assert.NotNull(model);

            IEnumerable<EdmError> errors;
            Assert.True(model.Validate(out errors), String.Format("Errors in validating model. {0}", String.Concat(errors.Select(e => e.ErrorMessage))));

            var color = model.SchemaElements.OfType<IEdmEnumType>().FirstOrDefault(c => c.Name == "Color");
            Assert.NotNull(color);

            IEdmTerm fooBarTerm = model.FindDeclaredTerm("NS.MyTerm");
            Assert.NotNull(fooBarTerm);

            // Red
            var red = color.Members.FirstOrDefault(c => c.Name == "Red");
            Assert.NotNull(red);
            string redAnnotation = GetStringAnnotation(model, red, CoreVocabularyModel.LongDescriptionTerm, EdmVocabularyAnnotationSerializationLocation.Inline);
            Assert.Equal("Inline Description", redAnnotation);

            redAnnotation = GetStringAnnotation(model, red, fooBarTerm, EdmVocabularyAnnotationSerializationLocation.Inline);
            Assert.Equal("Inline MyTerm Value", redAnnotation);

            // Blue
            var blue = color.Members.FirstOrDefault(c => c.Name == "Blue");
            Assert.NotNull(blue);
            string blueAnnotation = GetStringAnnotation(model, blue, CoreVocabularyModel.LongDescriptionTerm, EdmVocabularyAnnotationSerializationLocation.OutOfLine, false);
            Assert.Equal("OutOfLine Description", blueAnnotation);

            blueAnnotation = GetStringAnnotation(model, blue, fooBarTerm, EdmVocabularyAnnotationSerializationLocation.OutOfLine, false);
            Assert.Equal("OutOfLine MyTerm Value", blueAnnotation);
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

        private string GetStringAnnotation(IEdmModel model, IEdmVocabularyAnnotatable target, IEdmTerm term, EdmVocabularyAnnotationSerializationLocation location, bool inlineAnnotation=true)
        {
            IEdmVocabularyAnnotation annotation = model.FindVocabularyAnnotations<IEdmVocabularyAnnotation>(target, term).FirstOrDefault();
            if (annotation != null)
            {
                // We only create an instance of EdmAnnotationsTarget for OutOfLine annotations. Inline annotations don't have the Target attribute.
                if (inlineAnnotation)
                {
                    Assert.False(annotation.Target.GetType().IsAssignableFrom(typeof(IEdmAnnotationsTarget)));
                }
                else
                {
                    Assert.True(typeof(IEdmAnnotationsTarget).IsAssignableFrom(annotation.Target.GetType()));
                }

                Assert.True(annotation.GetSerializationLocation(model) == location);

                IEdmStringConstantExpression stringConstant = annotation.Value as IEdmStringConstantExpression;
                if (stringConstant != null)
                {
                    return stringConstant.Value;
                }
            }

            return null;
        }
    }
}
