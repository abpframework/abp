using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending.TestObjects;
using Xunit;

namespace Volo.Abp.ObjectExtending
{
    public class HasExtraPropertiesObjectExtendingExtensions_Tests : AbpObjectExtendingTestBase
    {
        private readonly ExtensibleTestPerson _person;
        private readonly ExtensibleTestPersonDto _personDto;

        public HasExtraPropertiesObjectExtendingExtensions_Tests()
        {
            _person = new ExtensibleTestPerson()
                .SetProperty("Name", "John")
                .SetProperty("Age", 42)
                .SetProperty("ChildCount", 2)
                .SetProperty("Sex", "male")
                .SetProperty("NoPairCheck", "test-value")
                .SetProperty("CityName", "Adana");

            _personDto = new ExtensibleTestPersonDto()
                .SetProperty("ExistingDtoProperty", "existing-value");
        }

        [Fact]
        public void MapExtraPropertiesTo_Should_Only_Map_Defined_Properties_By_Default()
        {
            _person.MapExtraPropertiesTo(_personDto);

            _personDto.GetProperty<string>("Name").ShouldBe("John"); //Defined in both classes
            _personDto.GetProperty<string>("CityName").ShouldBe("Adana"); //Defined in both classes
            _personDto.GetProperty<string>("NoPairCheck").ShouldBe("test-value"); //CheckPairDefinitionOnMapping = false
            _personDto.GetProperty<string>("ExistingDtoProperty").ShouldBe("existing-value"); //Should not clear existing values
            _personDto.HasProperty("Age").ShouldBeFalse(); //Not defined on the destination
            _personDto.GetProperty<int>("ChildCount").ShouldBe(0); //Not defined in the source, but was set to the default in the constructor
            _personDto.HasProperty("Sex").ShouldBeFalse(); //Not defined in both classes
        }

        [Fact]
        public void MapExtraPropertiesTo_Should_Ignore_Desired_Properties()
        {
            _person.MapExtraPropertiesTo(_personDto, ignoredProperties: new[] { "CityName" });

            _personDto.GetProperty<string>("Name").ShouldBe("John"); //Defined in both classes
            _personDto.GetProperty<string>("NoPairCheck").ShouldBe("test-value"); //CheckPairDefinitionOnMapping = false
            _personDto.GetProperty<string>("ExistingDtoProperty").ShouldBe("existing-value"); //Should not clear existing values
            _personDto.GetProperty<string>("CityName").ShouldBeNull(); //Ignored, but was set to the default in the constructor
            _personDto.HasProperty("Age").ShouldBeFalse(); //Not defined on the destination
            _personDto.GetProperty<int>("ChildCount").ShouldBe(0); //Not defined in the source, but was set to the default in the constructor
            _personDto.HasProperty("Sex").ShouldBeFalse(); //Not defined in both classes
        }

        [Fact]
        public void MapExtraPropertiesTo_Should_Only_Map_Source_Defined_Properties_If_Requested()
        {
            _person.MapExtraPropertiesTo(_personDto, MappingPropertyDefinitionChecks.Source);

            _personDto.GetProperty<string>("Name").ShouldBe("John"); //Defined in both classes
            _personDto.GetProperty<string>("CityName").ShouldBe("Adana"); //Defined in both classes
            _personDto.GetProperty<int>("Age").ShouldBe(42); //Defined in source
            _personDto.GetProperty<string>("ExistingDtoProperty").ShouldBe("existing-value"); //Should not clear existing values
            _personDto.GetProperty<int>("ChildCount").ShouldBe(0); //Not defined in the source, but was set to the default in the constructor
            _personDto.HasProperty("Sex").ShouldBeFalse(); //Not defined in both classes
        }

        [Fact]
        public void MapExtraPropertiesTo_Should_Only_Map_Destination_Defined_Properties_If_Requested()
        {
            _person.MapExtraPropertiesTo(_personDto, MappingPropertyDefinitionChecks.Destination);

            _personDto.GetProperty<string>("Name").ShouldBe("John"); //Defined in both classes
            _personDto.GetProperty<string>("CityName").ShouldBe("Adana"); //Defined in both classes
            _personDto.GetProperty<int>("ChildCount").ShouldBe(2); //Defined in destination
            _personDto.GetProperty<string>("ExistingDtoProperty").ShouldBe("existing-value"); //Should not clear existing values
            _personDto.HasProperty("Age").ShouldBeFalse(); //Not defined in destination
            _personDto.HasProperty("Sex").ShouldBeFalse(); //Not defined in both classes
        }

        [Fact]
        public void MapExtraPropertiesTo_Should_Copy_all_With_No_Property_Definition_Check()
        {
            _person.MapExtraPropertiesTo(_personDto, MappingPropertyDefinitionChecks.None);

            _personDto.GetProperty<string>("Name").ShouldBe("John");
            _personDto.GetProperty<string>("CityName").ShouldBe("Adana");
            _personDto.GetProperty<int>("Age").ShouldBe(42);
            _personDto.GetProperty<int>("ChildCount").ShouldBe(2);
            _personDto.GetProperty<string>("Sex").ShouldBe("male");
            _personDto.GetProperty<string>("ExistingDtoProperty").ShouldBe("existing-value"); //Should not clear existing values
        }
    }
}
