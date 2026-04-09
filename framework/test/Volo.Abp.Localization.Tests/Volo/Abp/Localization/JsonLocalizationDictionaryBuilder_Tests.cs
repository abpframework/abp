using System;
using System.Collections.Generic;
using System.Text;
using NSubstitute.ExceptionExtensions;
using Shouldly;
using Volo.Abp.Localization.Json;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Localization;

/// <summary>
/// Testing edge cases for <see cref="JsonLocalizationDictionaryBuilder"/>
/// </summary>
public class JsonLocalizationDictionaryBuilder_Tests : AbpIntegratedTest<AbpLocalizationTestModule>
{
    [Fact]
    public void JsonLocalizationDictionaryBuilder_Should_Handle_Duplicates()
    {
        var localizationDictionary = JsonLocalizationDictionaryBuilder
            .BuildFromJsonString("{\r\n  \"culture\": \"en\",\r\n  \"texts\": {\r\n    \"ThisFieldIsRequired\": \"This field is required\",\r\n    \"MaxLenghtErrorMessage\": \"This field can be maximum of '{0}' chars\",\r\n    \"Enum:BookType.Undefined\": \"Undefined from ValidationResource\",\r\n    \"Enum:BookType.0\": \"Undefined with value 0 from ValidationResource\",\r\n    \"BookType.Adventure\": \"Adventure from ValidationResource\",\r\n    \"BookType.1\": \"Adventure with value 1 from ValidationResource\",\r\n    \"Biography\": \"Biography from ValidationResource\",\r\n    \"ThisFieldIsRequired\": \"This field is required again\"\r\n  }\r\n}");

        var localizationString = localizationDictionary.GetOrNull("ThisFieldIsRequired");
        localizationString.ShouldNotBeNull();

        localizationString.Value.ShouldBe("This field is required again");
    }

    [Fact]
    public void JsonLocalizationDictionaryBuilder_Should_Handle_Deep_Duplicates()
    {
        var input = @"{
""culture"": ""en"",
""texts"": {
    ""ThisFieldIsRequired"": ""This field is required"",
    ""DeepLocaliaztionKey"": {""DeepKey"": ""DeepValue""},
    ""DeepLocaliaztionKey__DeepKey"": ""Another translation""
}
}";

        var localizationDictionary = JsonLocalizationDictionaryBuilder.BuildFromJsonString(input);
        localizationDictionary.ShouldNotBeNull();
        var localizationString = localizationDictionary.GetOrNull("DeepLocaliaztionKey__DeepKey");
        localizationString.ShouldNotBeNull();
        localizationString.Value.ShouldBe("Another translation");
    }
}
