﻿//---------------------------------------------------------------------
// <copyright file="ODataJsonLightContextUriParserTests.cs" company="Microsoft">
//      Copyright (C) Microsoft Corporation. All rights reserved. See License.txt in the project root for license information.
// </copyright>
//---------------------------------------------------------------------

using System;
using System.Linq;
using FluentAssertions;
using Microsoft.OData.JsonLight;
using Microsoft.OData.Edm;
using Xunit;
using ErrorStrings = Microsoft.OData.Strings;

namespace Microsoft.OData.Tests.JsonLight
{
    public class ODataJsonLightContextUriParserTests
    {
        private const string ContextUriForNullProperty = "http://service/$metadata#Edm.Null";

        [Fact]
        public void ParseNullPropertyContextUriShouldThrowForPayloadKindsExceptPropertyAndUnsupported()
        {
            foreach (ODataPayloadKind payloadKind in Enum.GetValues(typeof(ODataPayloadKind)))
            {
                if (payloadKind != ODataPayloadKind.Property && payloadKind != ODataPayloadKind.Unsupported)
                {
                    Action parseContextUri = () => ODataJsonLightContextUriParser.Parse(new EdmModel(), ContextUriForNullProperty, payloadKind, null, true);
                    parseContextUri.ShouldThrow<ODataException>().WithMessage(ErrorStrings.ODataJsonLightContextUriParser_ContextUriDoesNotMatchExpectedPayloadKind(ContextUriForNullProperty, payloadKind.ToString()));
                }
            }
        }

        [Fact]
        public void ParseNullPropertyContextUriShouldReturnPropertyWhenExpectedPayloadKindIsProperty()
        {
            var parseResult = ODataJsonLightContextUriParser.Parse(new EdmModel(), ContextUriForNullProperty, ODataPayloadKind.Property, null, true);
            parseResult.DetectedPayloadKinds.Single().Should().Be(ODataPayloadKind.Property);
            parseResult.IsNullProperty.Should().BeTrue();
        }

        [Fact]
        public void ParseNullPropertyContextUriShouldReturnPropertyWhenExpectedPayloadKindIsUnsupported()
        {
            var parseResult = ODataJsonLightContextUriParser.Parse(new EdmModel(), ContextUriForNullProperty, ODataPayloadKind.Unsupported, null, true);
            parseResult.DetectedPayloadKinds.Single().Should().Be(ODataPayloadKind.Property);
            parseResult.IsNullProperty.Should().BeTrue();
        }

        // TODO: Support relative context uri and resolving other relative uris
        [Fact]
        public void ParseRelativeContextUrlShouldThrowException()
        {
            string relativeUrl = "$metadata#R";
            Action parseContextUri = () => ODataJsonLightContextUriParser.Parse(new EdmModel(), relativeUrl, ODataPayloadKind.Unsupported, null, true);
            parseContextUri.ShouldThrow<ODataException>().WithMessage(ErrorStrings.ODataJsonLightContextUriParser_TopLevelContextUrlShouldBeAbsolute(relativeUrl));
        }
    }
}
