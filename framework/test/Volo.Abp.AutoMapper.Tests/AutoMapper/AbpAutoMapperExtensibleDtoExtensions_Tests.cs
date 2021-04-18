using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using Volo.Abp.AutoMapper;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending.TestObjects;
using Volo.Abp.Testing;
using Xunit;

namespace AutoMapper
{
    public class AbpAutoMapperExtensibleDtoExtensions_Tests : AbpIntegratedTest<AutoMapperTestModule>
    {
        private readonly Volo.Abp.ObjectMapping.IObjectMapper _objectMapper;

        public AbpAutoMapperExtensibleDtoExtensions_Tests()
        {
            _objectMapper = ServiceProvider.GetRequiredService<Volo.Abp.ObjectMapping.IObjectMapper>();
        }

        [Fact]
        public void MapExtraPropertiesTo_Should_Only_Map_Defined_Properties_By_Default()
        {
            var person = new ExtensibleTestPerson()
                .SetProperty("Name", "John")
                .SetProperty("Age", 42)
                .SetProperty("ChildCount", 2)
                .SetProperty("Sex", "male")
                .SetProperty("CityName", "Adana");

            var personDto = new ExtensibleTestPersonDto()
                .SetProperty("ExistingDtoProperty", "existing-value");

            _objectMapper.Map(person, personDto);

            personDto.GetProperty<string>("Name").ShouldBe("John"); //Defined in both classes
            personDto.GetProperty<string>("ExistingDtoProperty").ShouldBe("existing-value"); //Should not clear existing values
            personDto.GetProperty<int>("ChildCount").ShouldBe(0); //Not defined in the source, but was set to the default value by ExtensibleTestPersonDto constructor
            personDto.GetProperty("CityName").ShouldBeNull(); //Ignored, but was set to the default value by ExtensibleTestPersonDto constructor
            personDto.HasProperty("Age").ShouldBeFalse(); //Not defined on the destination
            personDto.HasProperty("Sex").ShouldBeFalse(); //Not defined in both classes
        }

        [Fact]
        public void MapExtraProperties_Also_Should_Map_To_RegularProperties()
        {
            var person = new ExtensibleTestPerson()
                .SetProperty("Name", "John")
                .SetProperty("Age", 42);

            var personDto = new ExtensibleTestPersonWithRegularPropertiesDto()
                .SetProperty("IsActive", true);

            _objectMapper.Map(person, personDto);

            //Defined in both classes
            personDto.HasProperty("Name").ShouldBe(false);
            personDto.Name.ShouldBe("John");

            //Defined in both classes
            personDto.HasProperty("Age").ShouldBe(false);
            personDto.Age.ShouldBe(42);

            //Should not clear existing values
            personDto.HasProperty("IsActive").ShouldBe(false);
            personDto.IsActive.ShouldBe(true);
        }
    }
}
