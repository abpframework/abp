using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.ObjectExtending.TestObjects;
using Xunit;

namespace Volo.Abp.ObjectExtending
{
    public class HasExtraPropertiesObjectExtendingExtensions_Tests : AbpObjectExtendingTestBase
    {
        private readonly ExtensibleTestPerson _person;

        public HasExtraPropertiesObjectExtendingExtensions_Tests()
        {
            _person = new ExtensibleTestPerson()
                .SetProperty("Name", "John")
                .SetProperty("Age", 42)
                .SetProperty("ChildCount", 2)
                .SetProperty("Sex", "male");
        }
        
        [Fact]
        public void MapExtraPropertiesTo_Should_Only_Map_Defined_Properties_By_Default()
        {
            var personDto = new ExtensibleTestPersonDto();
            
            _person.MapExtraPropertiesTo(personDto);

            personDto.GetProperty<string>("Name").ShouldBe("John"); //Defined in both classes
            personDto.HasProperty("Age").ShouldBeFalse(); //Not defined on the destination
            personDto.HasProperty("ChildCount").ShouldBeFalse(); //Not defined in the source
            personDto.HasProperty("Sex").ShouldBeFalse(); //Not defined in both classes
        }

        [Fact]
        public void MapExtraPropertiesTo_Should_Only_Map_Source_Defined_Properties_If_Requested()
        {
            var personDto = new ExtensibleTestPersonDto();

            _person.MapExtraPropertiesTo(personDto, MappingPropertyDefinitionCheck.Source);

            personDto.GetProperty<string>("Name").ShouldBe("John"); //Defined in both classes
            personDto.GetProperty<int>("Age").ShouldBe(42); //Defined in source
            personDto.HasProperty("ChildCount").ShouldBeFalse(); //Not defined in the source
            personDto.HasProperty("Sex").ShouldBeFalse(); //Not defined in both classes
        }

        [Fact]
        public void MapExtraPropertiesTo_Should_Only_Map_Destination_Defined_Properties_If_Requested()
        {
            var personDto = new ExtensibleTestPersonDto();

            _person.MapExtraPropertiesTo(personDto, MappingPropertyDefinitionCheck.Destination);

            personDto.GetProperty<string>("Name").ShouldBe("John"); //Defined in both classes
            personDto.GetProperty<int>("ChildCount").ShouldBe(2); //Defined in destination
            personDto.HasProperty("Age").ShouldBeFalse(); //Not defined in destination
            personDto.HasProperty("Sex").ShouldBeFalse(); //Not defined in both classes
        }

        [Fact]
        public void MapExtraPropertiesTo_Should_Copy_all_With_No_Property_Definition_Check()
        {
            var personDto = new ExtensibleTestPersonDto();

            _person.MapExtraPropertiesTo(personDto, MappingPropertyDefinitionCheck.None);

            personDto.GetProperty<string>("Name").ShouldBe("John");
            personDto.GetProperty<int>("Age").ShouldBe(42);
            personDto.GetProperty<int>("ChildCount").ShouldBe(2);
            personDto.GetProperty<string>("Sex").ShouldBe("male");
        }
    }
}
