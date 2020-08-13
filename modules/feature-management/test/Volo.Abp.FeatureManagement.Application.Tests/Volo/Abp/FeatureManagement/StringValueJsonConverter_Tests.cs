﻿using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shouldly;
using Volo.Abp.Json;
using Volo.Abp.Validation.StringValues;
using Xunit;

namespace Volo.Abp.FeatureManagement
{
    public class StringValueJsonConverter_Tests : FeatureManagementApplicationTestBase
    {
        private readonly IJsonSerializer _jsonSerializer;

        public StringValueJsonConverter_Tests()
        {
            _jsonSerializer = GetRequiredService<IJsonSerializer>();
        }

        [Fact]
        public void Should_Serialize_And_Deserialize()
        {
            var featureListDto = new FeatureListDto
            {
                Features = new List<FeatureDto>
                {
                    new FeatureDto
                    {
                        ValueType = new FreeTextStringValueType
                        {
                            Validator = new BooleanValueValidator()
                        }
                    },
                    new FeatureDto
                    {
                        ValueType = new SelectionStringValueType
                        {
                            ItemSource = new StaticSelectionStringValueItemSource(
                                new LocalizableSelectionStringValueItem
                                {
                                    Value = "TestValue",
                                    DisplayText = new LocalizableStringInfo("TestResourceName", "TestName")
                                }),
                            Validator = new AlwaysValidValueValidator()
                        }
                    },
                    new FeatureDto
                    {
                        ValueType = new ToggleStringValueType
                        {
                            Validator = new NumericValueValidator
                            {
                                MaxValue = 1000,
                                MinValue = 10
                            }
                        }
                    }
                }
            };

            var serialized = _jsonSerializer.Serialize(featureListDto, indented: true);
            var featureListDto2 = _jsonSerializer.Deserialize<FeatureListDto>(serialized);

            featureListDto2.Features[0].ValueType.ShouldBeOfType<FreeTextStringValueType>();
            featureListDto2.Features[0].ValueType.Validator.ShouldBeOfType<BooleanValueValidator>();

            featureListDto2.Features[1].ValueType.ShouldBeOfType<SelectionStringValueType>();
            featureListDto2.Features[1].ValueType.Validator.ShouldBeOfType<AlwaysValidValueValidator>();
            featureListDto2.Features[1].ValueType.As<SelectionStringValueType>().ItemSource.Items.ShouldBeOfType<LocalizableSelectionStringValueItem[]>();
            featureListDto2.Features[1].ValueType.As<SelectionStringValueType>().ItemSource.Items.ShouldContain(x =>
                x.Value == "TestValue" && x.DisplayText.ResourceName == "TestResourceName" &&
                x.DisplayText.Name == "TestName");

            featureListDto2.Features[2].ValueType.ShouldBeOfType<ToggleStringValueType>();
            featureListDto2.Features[2].ValueType.Validator.ShouldBeOfType<NumericValueValidator>();
            featureListDto2.Features[2].ValueType.Validator.As<NumericValueValidator>().MaxValue.ShouldBe(1000);
            featureListDto2.Features[2].ValueType.Validator.As<NumericValueValidator>().MinValue.ShouldBe(10);
        }
    }
}
