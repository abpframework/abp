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
    public void JsonLocalizationDictionaryBuilder_Should_Handle_Duplicates()
    {
        var input = """
            {
              "culture": "en",
              "texts": {
                "ThisFieldIsRequired": "This field is required",
                "MaxLenghtErrorMessage": "This field can be maximum of '{0}' chars",
                "Enum:BookType.Undefined": "Undefined from ValidationResource",
                "Enum:BookType.0": "Undefined with value 0 from ValidationResource",
                "BookType.Adventure": "Adventure from ValidationResource",
                "BookType.1": "Adventure with value 1 from ValidationResource",
                "Biography": "Biography from ValidationResource",
                "ThisFieldIsRequired": "This field is required again"
              }
            }
            """;

        var localizationDictionary = JsonLocalizationDictionaryBuilder.BuildFromJsonString(input);
        var localizationString = localizationDictionary.GetOrNull("ThisFieldIsRequired");
        localizationString.ShouldNotBeNull();

        localizationString.Value.ShouldBe("This field is required again");
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
