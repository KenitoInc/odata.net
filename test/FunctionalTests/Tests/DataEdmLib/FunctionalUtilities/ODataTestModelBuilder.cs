﻿//---------------------------------------------------------------------
// <copyright file="ODataTestModelBuilder.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

namespace EdmLibTests.FunctionalUtilities
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;

    public static class ODataTestModelBuilder
    {
        public static IEnumerable<XElement> ODataTestModelBasicTest
        {
            [CustomCsdlSchemaCompliantTest]
            get
            {
                return ConvertCsdlsToXElements(@"
    <Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <ComplexType Name=""Address"">
    <Property Name=""Street"" Nullable=""true"" Type=""Edm.String"" />
    <Property Name=""Zip"" Nullable=""false"" Type=""Edm.Int32"" />
  </ComplexType>
  <EntityType BaseType=""TestModel.CityType"" Name=""CityOpenType"" OpenType=""true"" />
  <EntityType Name=""CityType"">
    <Key>
      <PropertyRef Name=""Id"" />
    </Key>
    <NavigationProperty Name=""CityHall"" Type=""Collection(TestModel.OfficeType)"" />
    <NavigationProperty Name=""DOL"" Type=""Collection(TestModel.OfficeType)"" />
    <NavigationProperty Name=""PoliceStation"" Nullable=""false"" Type=""TestModel.OfficeType"" />
    <Property Name=""Id"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""MetroLanes"" Type=""Collection(Edm.String)"" />
    <Property Name=""Name"" Nullable=""true"" Type=""Edm.String"" />
    <Property Name=""Skyline"" Nullable=""false"" Type=""Edm.Stream"" />
  </EntityType>
  <EntityType BaseType=""TestModel.CityType"" Name=""CityWithMapType"" />
  <EntityType BaseType=""TestModel.Person"" Name=""Employee"">
    <Property Name=""CompanyName"" Nullable=""true"" Type=""Edm.String"" />
  </EntityType>
  <EntityType BaseType=""TestModel.Employee"" Name=""Manager"">
    <Property Name=""Level"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""OfficeType"">
    <Key>
      <PropertyRef Name=""Id"" />
    </Key>
    <Property Name=""Address"" Nullable=""false"" Type=""TestModel.Address"" />
    <Property Name=""Id"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""Person"">
    <Key>
      <PropertyRef Name=""Id"" />
    </Key>
    <NavigationProperty Name=""Friend"" Type=""Collection(TestModel.Person)"" />
    <Property Name=""Id"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <Action Name=""ServiceOperation1"">
    <Parameter Name=""a"" Nullable=""true"" Type=""Edm.Int32"" />
    <Parameter Name=""b"" Nullable=""true"" Type=""Edm.String"" />
    <ReturnType Type=""Edm.Int32"" />
  </Action>
      <EntityContainer Name=""DefaultContainer"">
        <EntitySet Name=""OfficeType"" EntityType=""TestModel.OfficeType"" />
        <EntitySet Name=""CityType"" EntityType=""TestModel.CityType"">
          <NavigationPropertyBinding Path=""CityHall"" Target=""OfficeType"" />
          <NavigationPropertyBinding Path=""DOL"" Target=""OfficeType"" />
          <NavigationPropertyBinding Path=""PoliceStation"" Target=""OfficeType"" />
        </EntitySet>
        <EntitySet Name=""Person"" EntityType=""TestModel.Person"">
          <NavigationPropertyBinding Path=""Friend"" Target=""Person"" />
        </EntitySet>
        <ActionImport Name=""ServiceOperation1"" Action=""TestModel.ServiceOperation1"" />
      </EntityContainer>
    </Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelAnnotationTestWithAnnotations
        {
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"">
        <EntitySet Name=""PersonType"" EntityType=""TestModel.PersonType"" />
        <EntitySet Name=""People"" EntityType=""TestModel.PersonType"" />
        <ActionImport Name=""ServiceOperation1"" Action=""TestModel.ServiceOperation1"" p3:MimeType=""img/jpeg"" xmlns:p3=""http://docs.oasis-open.org/odata/ns/metadata""/>
    </EntityContainer>
  <ComplexType Name=""Address"">
    <Property Name=""Street"" Nullable=""true"" Type=""Edm.String"" />
    <Property Name=""Zip"" Nullable=""false"" Type=""Edm.Int32"" p3:MimeType=""text/plain"" xmlns:p3=""http://docs.oasis-open.org/odata/ns/metadata"" />
  </ComplexType>
  <EntityType Name=""PersonType"">
    <Key>
      <PropertyRef Name=""Id"" />
    </Key>
    <Property Name=""Address"" Nullable=""false"" Type=""TestModel.Address"" />
    <Property Name=""Id"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Name"" Nullable=""false"" Type=""Edm.String"" p3:MimeType=""text/plain"" xmlns:p3=""http://docs.oasis-open.org/odata/ns/metadata"" />
    <Property Name=""Picture"" Nullable=""false"" Type=""Edm.Stream"" />
  </EntityType>
  <Action Name=""ServiceOperation1"">
    <Parameter Name=""a"" Nullable=""true"" Type=""Edm.Int32"" />
    <Parameter Name=""b"" Nullable=""true"" Type=""Edm.String"" />
    <ReturnType Type=""Edm.Int32"" Nullable=""false"" />
  </Action>
</Schema>");
            }
        }

        public static IEnumerable<XElement> ImmediateAnnotationRoundTrip
        {
            [CustomCsdlSchemaCompliantTest]
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""DefaultNamespace"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"" p3:MimeType=""text/plain"" xmlns:p3=""http://docs.oasis-open.org/odata/ns/metadata"" />
    <EntityType Name=""PersonType"" p3:MimeType=""text/plain"" xmlns:p3=""http://docs.oasis-open.org/odata/ns/metadata"">
        <Key>
            <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Int32"" Nullable=""false"" />
    </EntityType>
    <ComplexType Name=""Address"" p3:MimeType=""text/plain"" xmlns:p3=""http://docs.oasis-open.org/odata/ns/metadata"" />
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelAnnotationTestWithoutAnnotations
        {
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"">
        <EntitySet EntityType=""TestModel.PersonType"" Name=""People"" />
        <EntitySet EntityType=""TestModel.PersonType"" Name=""PersonType"" />
        <ActionImport Action=""TestModel.ServiceOperation1"" Name=""ServiceOperation1"" />
    </EntityContainer>
    <ComplexType Name=""Address"">
        <Property Name=""Street"" Type=""String"" />
        <Property Name=""Zip"" Type=""Int32"" Nullable=""false"" />
    </ComplexType>
    <EntityType Name=""PersonType"">
        <Key>
            <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Int32"" Nullable=""false"" />
        <Property Name=""Name"" Type=""String"" Nullable=""false"" />
        <Property Name=""Address"" Type=""TestModel.Address"" Nullable=""false"" />
        <Property Name=""Picture"" Type=""Stream"" Nullable=""false"" />
    </EntityType>
    <Action Name=""ServiceOperation1"">
        <Parameter Name=""a"" Nullable=""true"" Type=""Edm.Int32"" />
        <Parameter Name=""b"" Nullable=""true"" Type=""Edm.String"" />
        <ReturnType Type=""Edm.Int32"" Nullable=""false"" />
    </Action>
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelWithFunctionImport
        {
            get
            {
                // Added using statements to csdl to allow for csdl comparison to pass
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestNS"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""TestContainer"">
        <ActionImport Action=""TestNS.FunctionImport_Primitive"" Name=""FunctionImport_Primitive"" />
        <ActionImport Action=""TestNS.FunctionImport_PrimitiveCollection"" Name=""FunctionImport_PrimitiveCollection"" />
        <ActionImport Action=""TestNS.FunctionImport_Complex"" Name=""FunctionImport_Complex"" />
        <ActionImport Action=""TestNS.FunctionImport_ComplexCollection"" Name=""FunctionImport_ComplexCollection"" />
        <ActionImport Action=""TestNS.FunctionImport_Entry"" Name=""FunctionImport_Entry"" />
        <ActionImport Action=""TestNS.FunctionImport_Feed"" Name=""FunctionImport_Feed"" />
        <ActionImport Action=""TestNS.FunctionImport_Stream"" Name=""FunctionImport_Stream"" />
        <ActionImport Action=""TestNS.FunctionImport_Enum"" Name=""FunctionImport_Enum"" />
    </EntityContainer>
    <EntityType Name=""EntityType"">
        <Key>
            <PropertyRef Name=""ID"" />
        </Key>
        <Property Name=""ID"" Type=""Int32"" Nullable=""false"" />
        <Property Name=""ComplexProperty"" Type=""TestModel.ComplexType"" Nullable=""false"" />
    </EntityType>
    <EnumType Name=""EnumType"" />
    <Action Name=""FunctionImport_Primitive"">
        <Parameter Name=""primitive"" Nullable=""true"" Type=""Edm.String"" />
    </Action>
    <Action Name=""FunctionImport_PrimitiveCollection"">
        <Parameter Name=""primitiveCollection"" Nullable=""true"" Type=""Collection(Edm.String)"" />
    </Action>
    <Action Name=""FunctionImport_Complex"">
        <Parameter Name=""complex"" Nullable=""true"" Type=""TestModel.ComplexType"" />
    </Action>
    <Action Name=""FunctionImport_ComplexCollection"">
        <Parameter Name=""complexCollection"" Nullable=""true"" Type=""Collection(TestModel.ComplexType)"" />
    </Action>
    <Action Name=""FunctionImport_Entry"">
        <Parameter Name=""entry"" Nullable=""true"" Type=""TestNS.EntityType"" />
    </Action>
    <Action Name=""FunctionImport_Feed"">
        <Parameter Name=""feed"" Nullable=""true"" Type=""Collection(TestNS.EntityType)"" />
    </Action>
    <Action Name=""FunctionImport_Stream"">
        <Parameter Name=""stream"" Nullable=""true"" Type=""Edm.Stream"" />
    </Action>
    <Action Name=""FunctionImport_Enum"">
        <Parameter Name=""enum"" Nullable=""true"" Type=""TestNS.EnumType"" />
    </Action>
</Schema>", @"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <ComplexType Name=""ComplexType"">
        <Property Name=""PrimitiveProperty"" Type=""String"" Nullable=""false"" />
        <Property Name=""ComplexProperty"" Type=""TestModel.ComplexType"" Nullable=""false"" />
    </ComplexType>
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelWithActionImport
        {
            get
            {
                // Added using statements to csdl to allow for csdl comparison to pass
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestNS"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
      <Action Name=""MedianAge"">
        <Parameter Name=""PeopleAge"" Type=""Collection(Edm.Int32)"" />
        <ReturnType Type=""Edm.Int32"" />
      </Action>

    <EntityContainer Name=""TestContainer"">
        <ActionImport Name=""MedianAge"" Action=""TestNS.MedianAge"" />
    </EntityContainer>
    <EntityType Name=""EntityType"">
        <Key>
            <PropertyRef Name=""ID"" />
        </Key>
        <Property Name=""ID"" Type=""Int32"" Nullable=""false"" />
        <Property Name=""ComplexProperty"" Type=""TestModel.ComplexType"" Nullable=""false"" />
    </EntityType>
    <EnumType Name=""EnumType"" />
</Schema>", @"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <ComplexType Name=""ComplexType"">
        <Property Name=""PrimitiveProperty"" Type=""String"" Nullable=""false"" />
        <Property Name=""ComplexProperty"" Type=""TestModel.ComplexType"" Nullable=""false"" />
    </ComplexType>
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelDefaultModel
        {
            [CustomCsdlSchemaCompliantTest]
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""DefaultNamespace"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"">
        <EntitySet EntityType=""DefaultNamespace.Barcode"" Name=""Barcode"">
            <NavigationPropertyBinding Path=""BadScans"" Target=""IncorrectScan"" />
            <NavigationPropertyBinding Path=""GoodScans"" Target=""IncorrectScan"" />
            <NavigationPropertyBinding Path=""Detail"" Target=""BarcodeDetail"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.BarcodeDetail"" Name=""BarcodeDetail"" />
        <EntitySet EntityType=""DefaultNamespace.Car"" Name=""Car"">
            <NavigationPropertyBinding Path=""SpecialEmployee"" Target=""Person"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.Complaint"" Name=""Complaint"">
            <NavigationPropertyBinding Path=""Customer"" Target=""Customer"" />
            <NavigationPropertyBinding Path=""Resolution"" Target=""Resolution"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.Computer"" Name=""Computer"">
            <NavigationPropertyBinding Path=""ComputerDetail"" Target=""ComputerDetail"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.ComputerDetail"" Name=""ComputerDetail"" />
        <EntitySet EntityType=""DefaultNamespace.Customer"" Name=""Customer"">
            <NavigationPropertyBinding Path=""Orders"" Target=""Order"" />
            <NavigationPropertyBinding Path=""Logins"" Target=""Login"" />
            <NavigationPropertyBinding Path=""Wife"" Target=""Customer"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.CustomerInfo"" Name=""CustomerInfo"">
            <NavigationPropertyBinding Path=""Customer"" Target=""Customer"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.Driver"" Name=""Driver"">
            <NavigationPropertyBinding Path=""License"" Target=""License"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.IncorrectScan"" Name=""IncorrectScan"" />
        <EntitySet EntityType=""DefaultNamespace.LastLogin"" Name=""LastLogin"">
            <NavigationPropertyBinding Path=""SmartCard"" Target=""SmartCard"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.License"" Name=""License"" />
        <EntitySet EntityType=""DefaultNamespace.Login"" Name=""Login"">
            <NavigationPropertyBinding Path=""SentMessages"" Target=""Message"" />
            <NavigationPropertyBinding Path=""ReceivedMessages"" Target=""Message"" />
            <NavigationPropertyBinding Path=""Orders"" Target=""Order"" />
            <NavigationPropertyBinding Path=""LastLogin"" Target=""LastLogin"" />
            <NavigationPropertyBinding Path=""SuspiciousActivity"" Target=""SuspiciousActivity"" />
            <NavigationPropertyBinding Path=""RSAToken"" Target=""RSAToken"" />
            <NavigationPropertyBinding Path=""SmartCard"" Target=""SmartCard"" />
            <NavigationPropertyBinding Path=""PasswordResets"" Target=""PasswordReset"" />
            <NavigationPropertyBinding Path=""PageViews"" Target=""PageView"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.MappedEntityType"" Name=""MappedEntityType"" />
        <EntitySet EntityType=""DefaultNamespace.Message"" Name=""Message"" />
        <EntitySet EntityType=""DefaultNamespace.Order"" Name=""Order"">
            <NavigationPropertyBinding Path=""Notes"" Target=""OrderNote"" />
            <NavigationPropertyBinding Path=""OrderQualityCheck"" Target=""OrderQualityCheck"" />
            <NavigationPropertyBinding Path=""OrderLines"" Target=""OrderLine"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.OrderLine"" Name=""OrderLine"" />
        <EntitySet EntityType=""DefaultNamespace.OrderNote"" Name=""OrderNote"" />
        <EntitySet EntityType=""DefaultNamespace.OrderQualityCheck"" Name=""OrderQualityCheck"" />
        <EntitySet EntityType=""DefaultNamespace.PageView"" Name=""PageView"" />
        <EntitySet EntityType=""DefaultNamespace.PasswordReset"" Name=""PasswordReset"" />
        <EntitySet EntityType=""DefaultNamespace.Person"" Name=""Person"">
            <NavigationPropertyBinding Path=""PersonMetadata"" Target=""PersonMetadata"" />
            <NavigationPropertyBinding Path=""DefaultNamespace.Employee/Manager"" Target=""Person"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.PersonMetadata"" Name=""PersonMetadata"" />
        <EntitySet EntityType=""DefaultNamespace.Product"" Name=""Product"">
            <NavigationPropertyBinding Path=""OrderLines"" Target=""OrderLine"" />
            <NavigationPropertyBinding Path=""RelatedProducts"" Target=""Product"" />
            <NavigationPropertyBinding Path=""Detail"" Target=""ProductDetail"" />
            <NavigationPropertyBinding Path=""Reviews"" Target=""ProductReview"" />
            <NavigationPropertyBinding Path=""Photos"" Target=""ProductPhoto"" />
            <NavigationPropertyBinding Path=""Barcodes"" Target=""Barcode"" />
            <NavigationPropertyBinding Path=""Suppliers"" Target=""Supplier"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.ProductDetail"" Name=""ProductDetail"" />
        <EntitySet EntityType=""DefaultNamespace.ProductPhoto"" Name=""ProductPhoto"" />
        <EntitySet EntityType=""DefaultNamespace.ProductReview"" Name=""ProductReview"" />
        <EntitySet EntityType=""DefaultNamespace.ProductWebFeature"" Name=""ProductWebFeature"">
            <NavigationPropertyBinding Path=""Photo"" Target=""ProductPhoto"" />
            <NavigationPropertyBinding Path=""Review"" Target=""ProductReview"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.RSAToken"" Name=""RSAToken"" />
        <EntitySet EntityType=""DefaultNamespace.Resolution"" Name=""Resolution"" />
        <EntitySet EntityType=""DefaultNamespace.SmartCard"" Name=""SmartCard"" />
        <EntitySet EntityType=""DefaultNamespace.Supplier"" Name=""Supplier"">
            <NavigationPropertyBinding Path=""SupplierInfo"" Target=""SupplierInfo"" />
            <NavigationPropertyBinding Path=""Logo"" Target=""SupplierLogo"" />
        </EntitySet>
        <EntitySet EntityType=""DefaultNamespace.SupplierInfo"" Name=""SupplierInfo"" />
        <EntitySet EntityType=""DefaultNamespace.SupplierLogo"" Name=""SupplierLogo"" />
        <EntitySet EntityType=""DefaultNamespace.SuspiciousActivity"" Name=""SuspiciousActivity"" />
     </EntityContainer>
    
  <ComplexType Name=""Aliases"">
    <Property MaxLength=""10"" Name=""AlternativeNames"" Nullable=""false"" Type=""Collection(Edm.String)"" />
  </ComplexType>
  <ComplexType Name=""AuditInfo"">
    <Property Name=""Concurrency"" Nullable=""false"" Type=""DefaultNamespace.ConcurrencyInfo"" />
    <Property MaxLength=""50"" Name=""ModifiedBy"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""ModifiedDate"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
  </ComplexType>
  <ComplexType Name=""ComplexToCategory"">
    <Property Name=""Label"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""Scheme"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""Term"" Nullable=""false"" Type=""Edm.String"" />
  </ComplexType>
  <ComplexType Name=""ConcurrencyInfo"">
    <Property Name=""QueriedDateTime"" Nullable=""true"" Type=""Edm.DateTimeOffset"" />
    <Property MaxLength=""20"" Name=""Token"" Nullable=""false"" Type=""Edm.String"" />
  </ComplexType>
  <ComplexType Name=""ContactDetails"">
    <Property MaxLength=""10"" Name=""AlternativeNames"" Nullable=""false"" Type=""Collection(Edm.String)"" />
    <Property Name=""ContactAlias"" Nullable=""false"" Type=""DefaultNamespace.Aliases"" />
    <Property MaxLength=""32"" Name=""EmailBag"" Nullable=""false"" Type=""Collection(Edm.String)"" />
    <Property Name=""HomePhone"" Nullable=""false"" Type=""DefaultNamespace.Phone"" />
    <Property Name=""MobilePhoneBag"" Nullable=""false"" Type=""Collection(DefaultNamespace.Phone)"" />
    <Property Name=""WorkPhone"" Nullable=""false"" Type=""DefaultNamespace.Phone"" />
  </ComplexType>
  <ComplexType Name=""Dimensions"">
    <Property Name=""Depth"" Nullable=""false"" Precision=""10"" Scale=""3"" Type=""Edm.Decimal"" />
    <Property Name=""Height"" Nullable=""false"" Precision=""10"" Scale=""3"" Type=""Edm.Decimal"" />
    <Property Name=""Width"" Nullable=""false"" Precision=""10"" Scale=""3"" Type=""Edm.Decimal"" />
  </ComplexType>
  <ComplexType Name=""Phone"">
    <Property MaxLength=""16"" Name=""Extension"" Nullable=""true"" Type=""Edm.String"" />
    <Property MaxLength=""16"" Name=""PhoneNumber"" Nullable=""false"" Type=""Edm.String"" />
  </ComplexType>
  <EntityType BaseType=""DefaultNamespace.OrderLine"" Name=""BackOrderLine"" />
  <EntityType BaseType=""DefaultNamespace.BackOrderLine"" Name=""BackOrderLine2"" />
  <EntityType Name=""Barcode"">
    <Key>
      <PropertyRef Name=""Code"" />
    </Key>
    <NavigationProperty Name=""BadScans"" Partner=""ExpectedBarcode"" Type=""Collection(DefaultNamespace.IncorrectScan)"" />
    <NavigationProperty Name=""Detail"" Partner=""Barcode"" Type=""DefaultNamespace.BarcodeDetail"" />
    <NavigationProperty Name=""GoodScans"" Partner=""ActualBarcode"" Type=""Collection(DefaultNamespace.IncorrectScan)"" />
    <NavigationProperty Name=""Product"" Nullable=""false"" Partner=""Barcodes"" Type=""DefaultNamespace.Product"">
      <ReferentialConstraint Property=""ProductId"" ReferencedProperty=""ProductId"" />
    </NavigationProperty>
    <Property Name=""Code"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""ProductId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Text"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""BarcodeDetail"">
    <Key>
      <PropertyRef Name=""Code"" />
    </Key>
    <NavigationProperty Name=""Barcode"" Nullable=""false"" Partner=""Detail"" Type=""DefaultNamespace.Barcode"">
      <ReferentialConstraint Property=""Code"" ReferencedProperty=""Code"" />
    </NavigationProperty>
    <Property Name=""Code"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""RegisteredTo"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""Car"">
    <Key>
      <PropertyRef Name=""VIN"" />
    </Key>
    <NavigationProperty Name=""SpecialEmployee"" Partner=""Car"" Type=""Collection(DefaultNamespace.SpecialEmployee)"" />
    <Property MaxLength=""100"" Name=""Description"" Nullable=""true"" Type=""Edm.String"" />
    <Property Name=""Photo"" Nullable=""false"" Type=""Edm.Stream"" />
    <Property Name=""VIN"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Video"" Nullable=""false"" Type=""Edm.Stream"" />
  </EntityType>
  <EntityType Name=""Complaint"">
    <Key>
      <PropertyRef Name=""ComplaintId"" />
    </Key>
    <NavigationProperty Name=""Customer"" Partner=""Complaints"" Type=""DefaultNamespace.Customer"">
      <ReferentialConstraint Property=""CustomerId"" ReferencedProperty=""CustomerId"" />
    </NavigationProperty>
    <NavigationProperty Name=""Resolution"" Partner=""Complaint"" Type=""DefaultNamespace.Resolution"" />
    <Property Name=""ComplaintId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""CustomerId"" Nullable=""true"" Type=""Edm.Int32"" />
    <Property Name=""Details"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""Logged"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
  </EntityType>
  <EntityType Name=""Computer"">
    <Key>
      <PropertyRef Name=""ComputerId"" />
    </Key>
    <NavigationProperty Name=""ComputerDetail"" Nullable=""false"" Partner=""Computer"" Type=""DefaultNamespace.ComputerDetail"" />
    <Property Name=""ComputerId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Name"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""ComputerDetail"">
    <Key>
      <PropertyRef Name=""ComputerDetailId"" />
    </Key>
    <NavigationProperty Name=""Computer"" Nullable=""false"" Partner=""ComputerDetail"" Type=""DefaultNamespace.Computer"" />
    <Property Name=""ComputerDetailId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Dimensions"" Nullable=""false"" Type=""DefaultNamespace.Dimensions"" />
    <Property Name=""Manufacturer"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""Model"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""PurchaseDate"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
    <Property Name=""Serial"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""SpecificationsBag"" Nullable=""false"" Type=""Collection(Edm.String)"" />
  </EntityType>
  <EntityType Name=""Customer"">
    <Key>
      <PropertyRef Name=""CustomerId"" />
    </Key>
    <NavigationProperty Name=""Complaints"" Partner=""Customer"" Type=""Collection(DefaultNamespace.Complaint)"" />
    <NavigationProperty Name=""Husband"" Partner=""Wife"" Type=""DefaultNamespace.Customer"" />
    <NavigationProperty Name=""Info"" Partner=""Customer"" Type=""DefaultNamespace.CustomerInfo"" />
    <NavigationProperty Name=""Logins"" Partner=""Customer"" Type=""Collection(DefaultNamespace.Login)"" />
    <NavigationProperty Name=""Orders"" Partner=""Customer"" Type=""Collection(DefaultNamespace.Order)"" />
    <NavigationProperty Name=""Wife"" Partner=""Husband"" Type=""DefaultNamespace.Customer"" />
    <Property Name=""Auditing"" Nullable=""false"" Type=""DefaultNamespace.AuditInfo"" />
    <Property Name=""BackupContactInfo"" Nullable=""false"" Type=""Collection(DefaultNamespace.ContactDetails)"" />
    <Property Name=""CustomerId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property MaxLength=""100"" Name=""Name"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""PrimaryContactInfo"" Nullable=""false"" Type=""DefaultNamespace.ContactDetails"" />
    <Property Name=""Thumbnail"" Nullable=""false"" Type=""Edm.Stream"" />
    <Property Name=""Video"" Nullable=""false"" Type=""Edm.Stream"" />
  </EntityType>
  <EntityType Name=""CustomerInfo"">
    <Key>
      <PropertyRef Name=""CustomerInfoId"" />
    </Key>
    <NavigationProperty Name=""Customer"" Nullable=""false"" Partner=""Info"" Type=""DefaultNamespace.Customer"" />
    <Property Name=""CustomerInfoId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Information"" Nullable=""true"" Type=""Edm.String"" />
  </EntityType>
  <EntityType BaseType=""DefaultNamespace.Product"" Name=""DiscontinuedProduct"">
    <Property Name=""Discontinued"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
    <Property Name=""DiscontinuedPhone"" Nullable=""false"" Type=""DefaultNamespace.Phone"" />
    <Property Name=""ReplacementProductId"" Nullable=""true"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""Driver"">
    <Key>
      <PropertyRef Name=""Name"" />
    </Key>
    <NavigationProperty Name=""License"" Nullable=""false"" Partner=""Driver"" Type=""DefaultNamespace.License"" />
    <Property Name=""BirthDate"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
    <Property MaxLength=""100"" Name=""Name"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType BaseType=""DefaultNamespace.Person"" Name=""Employee"">
    <NavigationProperty Name=""EmployeeToManager"" Partner=""Manager"" Type=""Collection(DefaultNamespace.Employee)"" />
    <NavigationProperty Name=""Manager"" Nullable=""false"" Partner=""EmployeeToManager"" Type=""DefaultNamespace.Employee"">
      <ReferentialConstraint Property=""ManagersPersonId"" ReferencedProperty=""PersonId"" />
    </NavigationProperty>
    <Property Name=""ManagersPersonId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Salary"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Title"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""IncorrectScan"">
    <Key>
      <PropertyRef Name=""IncorrectScanId"" />
    </Key>
    <NavigationProperty Name=""ActualBarcode"" Partner=""GoodScans"" Type=""DefaultNamespace.Barcode"">
      <ReferentialConstraint Property=""ActualCode"" ReferencedProperty=""Code"" />
    </NavigationProperty>
    <NavigationProperty Name=""ExpectedBarcode"" Nullable=""false"" Partner=""BadScans"" Type=""DefaultNamespace.Barcode"">
      <ReferentialConstraint Property=""ExpectedCode"" ReferencedProperty=""Code"" />
    </NavigationProperty>
    <Property Name=""ActualCode"" Nullable=""true"" Type=""Edm.Int32"" />
    <Property Name=""Details"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""ExpectedCode"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""IncorrectScanId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""ScanDate"" Nullable=""false"" Type=""Collection(Edm.DateTimeOffset)"" />
  </EntityType>
  <EntityType Name=""LastLogin"">
    <Key>
      <PropertyRef Name=""Username"" />
    </Key>
    <NavigationProperty Name=""Login"" Nullable=""false"" Partner=""LastLogin"" Type=""DefaultNamespace.Login"">
      <ReferentialConstraint Property=""Username"" ReferencedProperty=""Username"" />
    </NavigationProperty>
    <NavigationProperty Name=""SmartCard"" Partner=""LastLogin"" Type=""DefaultNamespace.SmartCard"" />
    <Property Name=""LoggedIn"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
    <Property Name=""LoggedOut"" Nullable=""true"" Type=""Edm.DateTimeOffset"" />
    <Property MaxLength=""50"" Name=""Username"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""License"">
    <Key>
      <PropertyRef Name=""Name"" />
    </Key>
    <NavigationProperty Name=""Driver"" Nullable=""false"" Partner=""License"" Type=""DefaultNamespace.Driver"">
      <ReferentialConstraint Property=""Name"" ReferencedProperty=""Name"" />
    </NavigationProperty>
    <Property Name=""ExpirationDate"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
    <Property Name=""LicenseClass"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""LicenseNumber"" Nullable=""false"" Type=""Edm.String"" />
    <Property MaxLength=""100"" Name=""Name"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""Restrictions"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""Login"">
    <Key>
      <PropertyRef Name=""Username"" />
    </Key>
    <NavigationProperty Name=""Customer"" Nullable=""false"" Partner=""Logins"" Type=""DefaultNamespace.Customer"">
      <ReferentialConstraint Property=""CustomerId"" ReferencedProperty=""CustomerId"" />
    </NavigationProperty>
    <NavigationProperty Name=""LastLogin"" Partner=""Login"" Type=""DefaultNamespace.LastLogin"" />
    <NavigationProperty Name=""Orders"" Partner=""Login"" Type=""Collection(DefaultNamespace.Order)"" />
    <NavigationProperty Name=""PageViews"" Partner=""Login"" Type=""Collection(DefaultNamespace.PageView)"" />
    <NavigationProperty Name=""PasswordResets"" Partner=""Login"" Type=""Collection(DefaultNamespace.PasswordReset)"" />
    <NavigationProperty Name=""RSAToken"" Partner=""Login"" Type=""DefaultNamespace.RSAToken"" />
    <NavigationProperty Name=""ReceivedMessages"" Partner=""Recipient"" Type=""Collection(DefaultNamespace.Message)"" />
    <NavigationProperty Name=""SentMessages"" Partner=""Sender"" Type=""Collection(DefaultNamespace.Message)"" />
    <NavigationProperty Name=""SmartCard"" Partner=""Login"" Type=""DefaultNamespace.SmartCard"" />
    <NavigationProperty Name=""SuspiciousActivity"" Partner=""Login"" Type=""Collection(DefaultNamespace.SuspiciousActivity)"" />
    <Property Name=""CustomerId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property MaxLength=""50"" Name=""Username"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""MappedEntityType"">
    <Key>
      <PropertyRef Name=""Id"" />
    </Key>
    <Property Name=""BagOfBytes"" Nullable=""false"" Type=""Collection(Edm.Byte)"" />
    <Property Name=""BagOfComplexToCategories"" Nullable=""false"" Type=""Collection(DefaultNamespace.ComplexToCategory)"" />
    <Property Name=""BagOfDecimals"" Nullable=""false"" Type=""Collection(Edm.Decimal)"" Scale=""Variable"" />
    <Property Name=""BagOfDoubles"" Nullable=""false"" Type=""Collection(Edm.Double)"" />
    <Property Name=""BagOfGuids"" Nullable=""false"" Type=""Collection(Edm.Guid)"" />
    <Property Name=""BagOfInt16s"" Nullable=""false"" Type=""Collection(Edm.Int16)"" />
    <Property Name=""BagOfInt32s"" Nullable=""false"" Type=""Collection(Edm.Int32)"" />
    <Property Name=""BagOfInt64s"" Nullable=""false"" Type=""Collection(Edm.Int64)"" />
    <Property Name=""BagOfPrimitiveToLinks"" Nullable=""false"" Type=""Collection(Edm.String)"" />
    <Property Name=""BagOfSingles"" Nullable=""false"" Type=""Collection(Edm.Single)"" />
    <Property Name=""Href"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""HrefLang"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""Id"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Length"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Title"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""Type"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""Message"">
    <Key>
      <PropertyRef Name=""FromUsername"" />
      <PropertyRef Name=""MessageId"" />
    </Key>
    <NavigationProperty Name=""Recipient"" Nullable=""false"" Partner=""ReceivedMessages"" Type=""DefaultNamespace.Login"">
      <ReferentialConstraint Property=""ToUsername"" ReferencedProperty=""Username"" />
    </NavigationProperty>
    <NavigationProperty Name=""Sender"" Nullable=""false"" Partner=""SentMessages"" Type=""DefaultNamespace.Login"">
      <ReferentialConstraint Property=""FromUsername"" ReferencedProperty=""Username"" />
    </NavigationProperty>
    <Property Name=""Body"" Nullable=""true"" Type=""Edm.String"" />
    <Property MaxLength=""50"" Name=""FromUsername"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""IsRead"" Nullable=""false"" Type=""Edm.Boolean"" />
    <Property Name=""MessageId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Sent"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
    <Property Name=""Subject"" Nullable=""false"" Type=""Edm.String"" />
    <Property MaxLength=""50"" Name=""ToUsername"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""Order"">
    <Key>
      <PropertyRef Name=""OrderId"" />
    </Key>
    <NavigationProperty Name=""Customer"" Partner=""Orders"" Type=""DefaultNamespace.Customer"">
      <ReferentialConstraint Property=""CustomerId"" ReferencedProperty=""CustomerId"" />
    </NavigationProperty>
    <NavigationProperty Name=""Login"" Partner=""Orders"" Type=""DefaultNamespace.Login"" />
    <NavigationProperty Name=""Notes"" Partner=""Order"" Type=""Collection(DefaultNamespace.OrderNote)"">
      <OnDelete Action=""Cascade"" />
    </NavigationProperty>
    <NavigationProperty Name=""OrderLines"" Partner=""Order"" Type=""Collection(DefaultNamespace.OrderLine)"" />
    <NavigationProperty Name=""OrderQualityCheck"" Partner=""Order"" Type=""DefaultNamespace.OrderQualityCheck"" />
    <Property Name=""Concurrency"" Nullable=""false"" Type=""DefaultNamespace.ConcurrencyInfo"" />
    <Property Name=""CustomerId"" Nullable=""true"" Type=""Edm.Int32"" />
    <Property Name=""OrderId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""OrderLine"">
    <Key>
      <PropertyRef Name=""OrderId"" />
      <PropertyRef Name=""ProductId"" />
    </Key>
    <NavigationProperty Name=""Order"" Nullable=""false"" Partner=""OrderLines"" Type=""DefaultNamespace.Order"">
      <ReferentialConstraint Property=""OrderId"" ReferencedProperty=""OrderId"" />
    </NavigationProperty>
    <NavigationProperty Name=""Product"" Nullable=""false"" Partner=""OrderLines"" Type=""DefaultNamespace.Product"">
      <ReferentialConstraint Property=""ProductId"" ReferencedProperty=""ProductId"" />
    </NavigationProperty>
    <Property Name=""ConcurrencyToken"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""OrderId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""OrderLineStream"" Nullable=""false"" Type=""Edm.Stream"" />
    <Property Name=""ProductId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Quantity"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""OrderNote"">
    <Key>
      <PropertyRef Name=""NoteId"" />
    </Key>
    <NavigationProperty Name=""Order"" Nullable=""false"" Partner=""Notes"" Type=""DefaultNamespace.Order"" />
    <Property Name=""Note"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""NoteId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""OrderQualityCheck"">
    <Key>
      <PropertyRef Name=""OrderId"" />
    </Key>
    <NavigationProperty Name=""Order"" Nullable=""false"" Partner=""OrderQualityCheck"" Type=""DefaultNamespace.Order"">
      <ReferentialConstraint Property=""OrderId"" ReferencedProperty=""OrderId"" />
    </NavigationProperty>
    <Property Name=""CheckedBy"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""CheckedDateTime"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
    <Property Name=""OrderId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""PageView"">
    <Key>
      <PropertyRef Name=""PageViewId"" />
    </Key>
    <NavigationProperty Name=""Login"" Nullable=""false"" Partner=""PageViews"" Type=""DefaultNamespace.Login"">
      <ReferentialConstraint Property=""Username"" ReferencedProperty=""Username"" />
    </NavigationProperty>
    <Property MaxLength=""500"" Name=""PageUrl"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""PageViewId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property MaxLength=""50"" Name=""Username"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""Viewed"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
  </EntityType>
  <EntityType Name=""PasswordReset"">
    <Key>
      <PropertyRef Name=""ResetNo"" />
      <PropertyRef Name=""Username"" />
    </Key>
    <NavigationProperty Name=""Login"" Nullable=""false"" Partner=""PasswordResets"" Type=""DefaultNamespace.Login"">
      <ReferentialConstraint Property=""Username"" ReferencedProperty=""Username"" />
    </NavigationProperty>
    <Property Name=""EmailedTo"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""ResetNo"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""TempPassword"" Nullable=""false"" Type=""Edm.String"" />
    <Property MaxLength=""50"" Name=""Username"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""Person"">
    <Key>
      <PropertyRef Name=""PersonId"" />
    </Key>
    <NavigationProperty Name=""PersonMetadata"" Partner=""Person"" Type=""Collection(DefaultNamespace.PersonMetadata)"" />
    <Property Name=""Name"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""PersonId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""PersonMetadata"">
    <Key>
      <PropertyRef Name=""PersonMetadataId"" />
    </Key>
    <NavigationProperty Name=""Person"" Nullable=""false"" Partner=""PersonMetadata"" Type=""DefaultNamespace.Person"">
      <ReferentialConstraint Property=""PersonId"" ReferencedProperty=""PersonId"" />
    </NavigationProperty>
    <Property Name=""PersonId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""PersonMetadataId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""PropertyName"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""PropertyValue"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""Product"">
    <Key>
      <PropertyRef Name=""ProductId"" />
    </Key>
    <NavigationProperty Name=""Barcodes"" Partner=""Product"" Type=""Collection(DefaultNamespace.Barcode)"" />
    <NavigationProperty Name=""Detail"" Partner=""Product"" Type=""DefaultNamespace.ProductDetail"" />
    <NavigationProperty Name=""OrderLines"" Partner=""Product"" Type=""Collection(DefaultNamespace.OrderLine)"" />
    <NavigationProperty Name=""Photos"" Partner=""Product"" Type=""Collection(DefaultNamespace.ProductPhoto)"" />
    <NavigationProperty Name=""ProductToRelatedProducts"" Nullable=""false"" Partner=""RelatedProducts"" Type=""DefaultNamespace.Product"" />
    <NavigationProperty Name=""RelatedProducts"" Partner=""ProductToRelatedProducts"" Type=""Collection(DefaultNamespace.Product)"" />
    <NavigationProperty Name=""Reviews"" Partner=""Product"" Type=""Collection(DefaultNamespace.ProductReview)"" />
    <NavigationProperty Name=""Suppliers"" Partner=""Products"" Type=""Collection(DefaultNamespace.Supplier)"" />
    <Property Name=""BaseConcurrency"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""ComplexConcurrency"" Nullable=""false"" Type=""DefaultNamespace.ConcurrencyInfo"" />
    <Property MaxLength=""1000"" Name=""Description"" Nullable=""true"" Type=""Edm.String"" />
    <Property Name=""Dimensions"" Nullable=""false"" Type=""DefaultNamespace.Dimensions"" />
    <Property Name=""NestedComplexConcurrency"" Nullable=""false"" Type=""DefaultNamespace.AuditInfo"" />
    <Property Name=""Picture"" Nullable=""false"" Type=""Edm.Stream"" />
    <Property Name=""ProductId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""ProductDetail"">
    <Key>
      <PropertyRef Name=""ProductId"" />
    </Key>
    <NavigationProperty Name=""Product"" Nullable=""false"" Partner=""Detail"" Type=""DefaultNamespace.Product"">
      <ReferentialConstraint Property=""ProductId"" ReferencedProperty=""ProductId"" />
    </NavigationProperty>
    <Property Name=""Details"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""ProductId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType BaseType=""DefaultNamespace.PageView"" Name=""ProductPageView"">
    <Property Name=""ProductId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""ProductPhoto"">
    <Key>
      <PropertyRef Name=""PhotoId"" />
      <PropertyRef Name=""ProductId"" />
    </Key>
    <NavigationProperty Name=""Features"" Partner=""Photo"" Type=""Collection(DefaultNamespace.ProductWebFeature)"" />
    <NavigationProperty Name=""Product"" Nullable=""false"" Partner=""Photos"" Type=""DefaultNamespace.Product"">
      <ReferentialConstraint Property=""ProductId"" ReferencedProperty=""ProductId"" />
    </NavigationProperty>
    <Property Name=""Photo"" Nullable=""false"" Type=""Edm.Binary"" />
    <Property Name=""PhotoId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""ProductId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""ProductReview"">
    <Key>
      <PropertyRef Name=""ProductId"" />
      <PropertyRef Name=""ReviewId"" />
    </Key>
    <NavigationProperty Name=""Features"" Partner=""Review"" Type=""Collection(DefaultNamespace.ProductWebFeature)"" />
    <NavigationProperty Name=""Product"" Nullable=""false"" Partner=""Reviews"" Type=""DefaultNamespace.Product"">
      <ReferentialConstraint Property=""ProductId"" ReferencedProperty=""ProductId"" />
    </NavigationProperty>
    <Property Name=""ProductId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Review"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""ReviewId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""ProductWebFeature"">
    <Key>
      <PropertyRef Name=""FeatureId"" />
    </Key>
    <NavigationProperty Name=""Photo"" Partner=""Features"" Type=""DefaultNamespace.ProductPhoto"">
      <ReferentialConstraint Property=""PhotoId"" ReferencedProperty=""ProductId"" />
      <ReferentialConstraint Property=""ProductId"" ReferencedProperty=""PhotoId"" />
    </NavigationProperty>
    <NavigationProperty Name=""Review"" Partner=""Features"" Type=""DefaultNamespace.ProductReview"">
      <ReferentialConstraint Property=""ReviewId"" ReferencedProperty=""ProductId"" />
      <ReferentialConstraint Property=""ProductId"" ReferencedProperty=""ReviewId"" />
    </NavigationProperty>
    <Property Name=""FeatureId"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""Heading"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""PhotoId"" Nullable=""true"" Type=""Edm.Int32"" />
    <Property Name=""ProductId"" Nullable=""true"" Type=""Edm.Int32"" />
    <Property Name=""ReviewId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""RSAToken"">
    <Key>
      <PropertyRef Name=""Serial"" />
    </Key>
    <NavigationProperty Name=""Login"" Nullable=""false"" Partner=""RSAToken"" Type=""DefaultNamespace.Login"" />
    <Property Name=""Issued"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
    <Property MaxLength=""20"" Name=""Serial"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType Name=""Resolution"">
    <Key>
      <PropertyRef Name=""ResolutionId"" />
    </Key>
    <NavigationProperty Name=""Complaint"" Nullable=""false"" Partner=""Resolution"" Type=""DefaultNamespace.Complaint"" />
    <Property Name=""Details"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""ResolutionId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""SmartCard"">
    <Key>
      <PropertyRef Name=""Username"" />
    </Key>
    <NavigationProperty Name=""LastLogin"" Partner=""SmartCard"" Type=""DefaultNamespace.LastLogin"" />
    <NavigationProperty Name=""Login"" Nullable=""false"" Partner=""SmartCard"" Type=""DefaultNamespace.Login"">
      <ReferentialConstraint Property=""Username"" ReferencedProperty=""Username"" />
    </NavigationProperty>
    <Property Name=""CardSerial"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""Issued"" Nullable=""false"" Type=""Edm.DateTimeOffset"" />
    <Property MaxLength=""50"" Name=""Username"" Nullable=""false"" Type=""Edm.String"" />
  </EntityType>
  <EntityType BaseType=""DefaultNamespace.Employee"" Name=""SpecialEmployee"">
    <NavigationProperty Name=""Car"" Nullable=""false"" Partner=""SpecialEmployee"" Type=""DefaultNamespace.Car"">
      <ReferentialConstraint Property=""CarsVIN"" ReferencedProperty=""VIN"" />
    </NavigationProperty>
    <Property Name=""Bonus"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""CarsVIN"" Nullable=""false"" Type=""Edm.Int32"" />
    <Property Name=""IsFullyVested"" Nullable=""false"" Type=""Edm.Boolean"" />
  </EntityType>
  <EntityType Name=""Supplier"">
    <Key>
      <PropertyRef Name=""SupplierId"" />
    </Key>
    <NavigationProperty Name=""Logo"" Partner=""Supplier"" Type=""DefaultNamespace.SupplierLogo"" />
    <NavigationProperty Name=""Products"" Partner=""Suppliers"" Type=""Collection(DefaultNamespace.Product)"" />
    <NavigationProperty Name=""SupplierInfo"" Partner=""Supplier"" Type=""Collection(DefaultNamespace.SupplierInfo)"">
      <OnDelete Action=""Cascade"" />
    </NavigationProperty>
    <Property Name=""Name"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""SupplierId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""SupplierInfo"">
    <Key>
      <PropertyRef Name=""SupplierInfoId"" />
    </Key>
    <NavigationProperty Name=""Supplier"" Nullable=""false"" Partner=""SupplierInfo"" Type=""DefaultNamespace.Supplier"" />
    <Property Name=""Information"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""SupplierInfoId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""SupplierLogo"">
    <Key>
      <PropertyRef Name=""SupplierId"" />
    </Key>
    <NavigationProperty Name=""Supplier"" Nullable=""false"" Partner=""Logo"" Type=""DefaultNamespace.Supplier"">
      <ReferentialConstraint Property=""SupplierId"" ReferencedProperty=""SupplierId"" />
    </NavigationProperty>
    <Property MaxLength=""500"" Name=""Logo"" Nullable=""false"" Type=""Edm.Binary"" />
    <Property Name=""SupplierId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <EntityType Name=""SuspiciousActivity"">
    <Key>
      <PropertyRef Name=""SuspiciousActivityId"" />
    </Key>
    <NavigationProperty Name=""Login"" Partner=""SuspiciousActivity"" Type=""DefaultNamespace.Login"" />
    <Property Name=""Activity"" Nullable=""false"" Type=""Edm.String"" />
    <Property Name=""SuspiciousActivityId"" Nullable=""false"" Type=""Edm.Int32"" />
  </EntityType>
  <Function Name=""EntityProjectionReturnsCollectionOfComplexTypes"">
    <ReturnType Type=""Collection(DefaultNamespace.ContactDetails)"" />
  </Function>
  <Function Name=""GetArgumentPlusOne"">
    <Parameter Name=""arg1"" Nullable=""false"" Type=""Edm.Int32"" />
    <ReturnType Type=""Edm.Int32"" />
  </Function>
  <Function Name=""GetCustomerCount"">
    <ReturnType Type=""Edm.Int32"" />
  </Function>
  <Function Name=""GetPrimitiveString"">
    <ReturnType Type=""Edm.String"" />
  </Function>
  <Function Name=""GetSpecificCustomer"">
    <Parameter Name=""Name"" Nullable=""false"" Type=""Edm.String"" />
    <ReturnType Type=""Collection(DefaultNamespace.Customer)"" />
  </Function>
  <Action Name=""SetArgumentPlusOne"">
    <Parameter Name=""arg1"" Nullable=""false"" Type=""Edm.Int32"" />
    <ReturnType Type=""Edm.Int32"" />
  </Action>
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelEmptyModel
        {
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""DefaultNamespace"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"" />
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelWithSingleEntityType
        {
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"">
        <EntitySet Name=""SingletonEntityType"" EntityType=""TestModel.SingletonEntityType"" />
    </EntityContainer>
    <EntityType Name=""SingletonEntityType"">
        <Key>
            <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Int32"" Nullable=""false"" />
        <Property Name=""Name"" Type=""String"" />
    </EntityType>
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelWithSingleComplexType
        {
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"" />
    <ComplexType Name=""SingletonComplexType"">
        <Property Name=""City"" Type=""String"" />
    </ComplexType>
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelWithCollectionProperty
        {
            [CustomCsdlSchemaCompliantTest]
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"" />
    <ComplexType Name=""EntityTypeWithCollection"">
        <Property Name=""Cities"" Type=""Collection(Edm.String)"" />
    </ComplexType>
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelWithOpenType
        {
            [CustomCsdlSchemaCompliantTest]
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"">
        <EntitySet Name=""OpenEntityType"" EntityType=""TestModel.OpenEntityType"" />
    </EntityContainer>
    <EntityType OpenType=""true"" Name=""OpenEntityType"">
        <Key>
            <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Int32"" Nullable=""false"" />
    </EntityType>
</Schema>");
            }
        }

        public static IEnumerable<XElement> ODataTestModelWithNamedStream
        {
            get
            {
                return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"">
        <EntitySet Name=""NamedStreamEntityType"" EntityType=""TestModel.NamedStreamEntityType"" />
    </EntityContainer>
    <EntityType Name=""NamedStreamEntityType"">
        <Key>
            <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Int32"" Nullable=""false"" />
        <Property Name=""NamedStream"" Type=""Stream"" Nullable=""false"" />
    </EntityType>
</Schema>");
            }
        }

        public static class InvalidCsdl
        {
            public static IEnumerable<XElement> EntityTypeWithoutKey
            {
                get
                {
                    return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"">
        <EntitySet Name=""EntityTypeWithoutKey"" EntityType=""TestModel.EntityTypeWithoutKey"" />
        <Singleton Name=""SingletonWhoseEntityTypeWithoutKey"" Type=""TestModel.EntityTypeWithoutKey"" />
    </EntityContainer>
    <EntityType Name=""EntityTypeWithoutKey"" />
</Schema>");
                }
            }

            public static IEnumerable<XElement> DuplicateEntityTypes
            {
                get
                {
                    return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"">
        <EntitySet Name=""DuplicateEntityType"" EntityType=""TestModel.DuplicateEntityType"" />
        <EntitySet Name=""DuplicateEntityType"" EntityType=""TestModel.DuplicateEntityType"" />
    </EntityContainer>
    <EntityType Name=""DuplicateEntityType"">
        <Key>
            <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Int32"" Nullable=""false"" />
    </EntityType>
    <EntityType Name=""DuplicateEntityType"">
        <Key>
            <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Int32"" Nullable=""false"" />
    </EntityType>
</Schema>");
                }
            }

            public static IEnumerable<XElement> DuplicateComplexTypes
            {
                get
                {
                    return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"" />
    <ComplexType Name=""DuplicateComplexTypes"">
       <Property Name=""Data"" Type=""Edm.String"" />
    </ComplexType>
    <ComplexType Name=""DuplicateComplexTypes"">
       <Property Name=""Data"" Type=""Edm.String"" />
    </ComplexType>
</Schema>");
                }
            }

            public static IEnumerable<XElement> ComplexTypeWithDuplicateProperties
            {
                get
                {
                    return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"" />
    <ComplexType Name=""DuplicatePropertiesComplexType"">
        <Property Name=""Duplicate"" Type=""String"" />
        <Property Name=""Duplicate"" Type=""String"" />
    </ComplexType>
</Schema>");
                }
            }

            public static IEnumerable<XElement> EntityTypeWithDuplicateProperties
            {
                get
                {
                    return ConvertCsdlsToXElements(@"
<Schema Namespace=""TestModel"" xmlns=""http://docs.oasis-open.org/odata/ns/edm"">
    <EntityContainer Name=""DefaultContainer"">
        <EntitySet Name=""DuplicatePropertiesEntityType"" EntityType=""TestModel.DuplicatePropertiesEntityType"" />
    </EntityContainer>
    <EntityType Name=""DuplicatePropertiesEntityType"">
        <Key>
            <PropertyRef Name=""Id"" />
        </Key>
        <Property Name=""Id"" Type=""Int32"" Nullable=""false"" />
        <Property Name=""Duplicate"" Type=""String"" />
        <Property Name=""Duplicate"" Type=""String"" />
    </EntityType>
</Schema>");
                }
            }
        }

        private static IEnumerable<XElement> ConvertCsdlsToXElements(params string[] csdls)
        {
            return csdls.Select(e => XElement.Parse(e, LoadOptions.SetLineInfo));
        }
    }
}
