using Shouldly;
using Volo.Abp.Localization.Json;
using Xunit;

namespace Volo.Abp.Localization;

/// <summary>
/// Testing edge cases for <see cref="JsonLocalizationDictionaryBuilder"/>
/// </summary>
public class JsonLocalizationDictionaryBuilder_Tests
{
    [Fact]
    public void Should_Use_Last_Value_When_Json_Contains_Duplicate_Keys()
    {
        // This test locks the behavior of System.Text.Json when deserializing duplicate JSON property names.
        // If STJ changes this behavior in a future version, this test will catch the regression.
        var input = """
            {
              "culture": "en",
              "texts": {
                "ThisFieldIsRequired": "This field is required",
                "MaxLengthErrorMessage": "This field can be maximum of '{0}' chars",
                "ThisFieldIsRequired": "This field is required again"
              }
            }
            """;

        var localizationDictionary = JsonLocalizationDictionaryBuilder.BuildFromJsonString(input);
        localizationDictionary.ShouldNotBeNull();
        var localizationString = localizationDictionary.GetOrNull("ThisFieldIsRequired");
        localizationString.ShouldNotBeNull();
        localizationString.Value.ShouldBe("This field is required again");
    }

    [Fact]
    public void Should_Use_Nested_Value_When_Flat_Key_Is_Defined_Before_Nested_Object()
    {
        // When a flat key (e.g. "Foo__Bar") appears before a nested object (e.g. "Foo": {"Bar": ...}),
        // the nested value wins because FlattenTexts processes keys in order and last-write wins.
        var input = """
            {
              "culture": "en",
              "texts": {
                "DeepLocalizationKey__DeepKey": "FlatValue",
                "DeepLocalizationKey": { "DeepKey": "NestedValue" }
              }
            }
            """;

        var localizationDictionary = JsonLocalizationDictionaryBuilder.BuildFromJsonString(input);
        localizationDictionary.ShouldNotBeNull();
        var localizationString = localizationDictionary.GetOrNull("DeepLocalizationKey__DeepKey");
        localizationString.ShouldNotBeNull();
        localizationString.Value.ShouldBe("NestedValue");
    }

    [Fact]
    public void JsonLocalizationDictionaryBuilder_Should_Handle_Deep_Duplicates()
    {
        var input = """
            {
              "culture": "en",
              "texts": {
                "ThisFieldIsRequired": "This field is required",
                "DeepLocalizationKey": { "DeepKey": "DeepValue" },
                "DeepLocalizationKey__DeepKey": "Another translation"
              }
            }
            """;

        var localizationDictionary = JsonLocalizationDictionaryBuilder.BuildFromJsonString(input);
        localizationDictionary.ShouldNotBeNull();
        var localizationString = localizationDictionary.GetOrNull("DeepLocalizationKey__DeepKey");
        localizationString.ShouldNotBeNull();
        localizationString.Value.ShouldBe("Another translation");
    }
}
