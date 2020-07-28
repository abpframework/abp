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

        [Fact(Skip = "StringValueTypeJsonConverter is not implemented yet!")]
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
                    }

                    //TODO: Add more to test
                }
            };

            var serialized = _jsonSerializer.Serialize(featureListDto);

            var featureListDto2 = _jsonSerializer.Deserialize<FeatureListDto>(serialized);

            featureListDto2.Features[0].ValueType.ShouldBeOfType<FreeTextStringValueType>();
            featureListDto2.Features[0].ValueType.Validator.ShouldBeOfType<BooleanValueValidator>();
        }
    }
}
