using System;
using Shouldly;
using Volo.Abp.Validation.StringValues;
using Xunit;

namespace Volo.Abp.FeatureManagement;

public class StringValueTypeSerializer_Tests : FeatureManagementDomainTestBase
{
    private readonly StringValueTypeSerializer _serializer;

    public StringValueTypeSerializer_Tests()
    {
        _serializer = GetRequiredService<StringValueTypeSerializer>();
    }

    [Fact]
    public void Serialize_And_Deserialize_Test()
    {
        // Arrange

        var valueType = new SelectionStringValueType
        {
            ItemSource = new StaticSelectionStringValueItemSource(
                new LocalizableSelectionStringValueItem
                {
                    Value = "TestValue",
                    DisplayText = new LocalizableStringInfo("TestResourceName", "TestName")
                }),
            Validator = new AlwaysValidValueValidator()
        };

        // Act

        var valueTypeJson = _serializer.Serialize(valueType);

        //Assert
        valueTypeJson.ShouldBe("{\"itemSource\":{\"items\":[{\"value\":\"TestValue\",\"displayText\":{\"resourceName\":\"TestResourceName\",\"name\":\"TestName\"}}]},\"name\":\"SelectionStringValueType\",\"properties\":{},\"validator\":{\"name\":\"NULL\",\"properties\":{}}}");

        // Act
        var valueType2 = _serializer.Deserialize(valueTypeJson);

        //Assert
        valueType2.ShouldBeOfType<SelectionStringValueType>();
        valueType2.Validator.ShouldBeOfType<AlwaysValidValueValidator>();
        valueType2.As<SelectionStringValueType>().ItemSource.Items.ShouldBeOfType<LocalizableSelectionStringValueItem[]>();
        valueType2.As<SelectionStringValueType>().ItemSource.Items.ShouldContain(x =>
            x.Value == "TestValue" && x.DisplayText.ResourceName == "TestResourceName" &&
            x.DisplayText.Name == "TestName");
    }

}
