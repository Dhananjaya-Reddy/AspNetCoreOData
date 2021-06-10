﻿// Copyright (c) Microsoft Corporation.  All rights reserved.
// Licensed under the MIT License.  See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Query.Validator;
using Microsoft.AspNetCore.OData.Tests.Commons;
using Microsoft.OData;
using Microsoft.OData.Edm;
using Xunit;

namespace Microsoft.AspNetCore.OData.Tests.Query.Validator
{
    public class TopQueryValidatorTest
    {
        private TopQueryValidator _validator;
        private ODataQueryContext _context;

        public TopQueryValidatorTest()
        {
            _validator = new TopQueryValidator();
            _context = ValidationTestHelper.CreateCustomerContext();
        }

        [Fact]
        public void ValidateTopQueryValidator_ThrowsOnNullOption()
        {
            // Arrange & Act & Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => _validator.Validate(null, new ODataValidationSettings()));
        }

        [Fact]
        public void ValidateTopQueryValidator_ThrowsOnNullSettings()
        {
            // Arrange & Act & Assert
            ExceptionAssert.Throws<ArgumentNullException>(() => _validator.Validate(new TopQueryOption("2", _context), null));
        }

        [Fact]
        public void ValidateTopQueryValidator_ThrowsWhenLimitIsExceeded()
        {
            // Arrange
            ODataValidationSettings settings = new ODataValidationSettings()
            {
                MaxTop = 10
            };

            // Act & Assert
            ExceptionAssert.Throws<ODataException>(() => _validator.Validate(new TopQueryOption("11", _context), settings),
                "The limit of '10' for Top query has been exceeded. The value from the incoming request is '11'.");
        }

        [Fact]
        public void ValidateTopQueryValidator_PassWhenLimitIsReached()
        {
            // Arrange
            ODataValidationSettings settings = new ODataValidationSettings()
            {
                MaxTop = 10
            };

            // Act
            ExceptionAssert.DoesNotThrow(() => _validator.Validate(new TopQueryOption("10", _context), settings));
        }

        [Fact]
        public void ValidateTopQueryValidator_PassWhenLimitIsNotReached()
        {
            // Arrange
            ODataValidationSettings settings = new ODataValidationSettings()
            {
                MaxTop = 10
            };

            // Act & Assert
            ExceptionAssert.DoesNotThrow(() => _validator.Validate(new TopQueryOption("9", _context), settings));
        }

        [Fact]
        public void ValidateTopQueryValidator_PassWhenQuerySettingsLimitIsNotReached()
        {
            // Arrange
            ODataValidationSettings settings = new ODataValidationSettings()
            {
                MaxTop = 20
            };
            ModelBoundQuerySettings modelBoundQuerySettings = new ModelBoundQuerySettings();
            modelBoundQuerySettings.MaxTop = 20;
            ODataQueryContext context = ValidationTestHelper.CreateCustomerContext();
            context.Model.SetAnnotationValue(context.ElementType as IEdmStructuredType, modelBoundQuerySettings);

            // Act & Assert
            ExceptionAssert.DoesNotThrow(() => _validator.Validate(new TopQueryOption("20", context), settings));
        }

        [Fact]
        public void ValidateTopQueryValidator_ThrowsWhenQuerySettingsLimitIsExceeded()
        {
            // Arrange
            ODataValidationSettings settings = new ODataValidationSettings()
            {
                MaxTop = 20
            };
            ModelBoundQuerySettings modelBoundQuerySettings = new ModelBoundQuerySettings();
            modelBoundQuerySettings.MaxTop = 10;
            ODataQueryContext context = ValidationTestHelper.CreateCustomerContext();
            context.Model.SetAnnotationValue(context.ElementType as IEdmStructuredType, modelBoundQuerySettings);

            // Act & Assert
            ExceptionAssert.Throws<ODataException>(() => _validator.Validate(new TopQueryOption("11", context), settings),
                "The limit of '10' for Top query has been exceeded. The value from the incoming request is '11'.");
        }
    }
}