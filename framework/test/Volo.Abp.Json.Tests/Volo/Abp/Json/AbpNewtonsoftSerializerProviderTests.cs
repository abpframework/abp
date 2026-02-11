using System;
using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace Volo.Abp.Json;

[Collection("AbpJsonNewtonsoftJsonTest")]
public class AbpNewtonsoftSerializerProviderTests : AbpJsonNewtonsoftJsonTestBase
{
    protected IJsonSerializer JsonSerializer;

    public AbpNewtonsoftSerializerProviderTests()
    {
        JsonSerializer = GetRequiredService<IJsonSerializer>();
    }

    public class File
    {
        public string FileName { get; set; }

        public Dictionary<string, int> ExtraProperties { get; set; }
    }

    [Fact]
    public void Serialize_Deserialize_Test()
    {
        var defaultIndent = "  "; // Default indent is 2 spaces
        var newLine = Environment.NewLine;
        var file = new File()
        {
            FileName = "abp",
            ExtraProperties = new Dictionary<string, int>()
            {
                { "One", 1 },
                { "Two", 2 }
            }
        };

        var json = JsonSerializer.Serialize(file, camelCase: true);
        json.ShouldBe("{\"fileName\":\"abp\",\"extraProperties\":{\"One\":1,\"Two\":2}}");

        json = JsonSerializer.Serialize(file, camelCase: true, indented: true);
        json.ShouldBe($"{{{newLine}{defaultIndent}\"fileName\": \"abp\",{newLine}{defaultIndent}\"extraProperties\": {{{newLine}{defaultIndent}{defaultIndent}\"One\": 1,{newLine}{defaultIndent}{defaultIndent}\"Two\": 2{newLine}{defaultIndent}}}{newLine}}}");

        json = JsonSerializer.Serialize(file, camelCase: false);
        json.ShouldBe("{\"FileName\":\"abp\",\"ExtraProperties\":{\"One\":1,\"Two\":2}}");

        json = JsonSerializer.Serialize(file, camelCase: false, indented: true);
        json.ShouldBe($"{{{newLine}{defaultIndent}\"FileName\": \"abp\",{newLine}{defaultIndent}\"ExtraProperties\": {{{newLine}{defaultIndent}{defaultIndent}\"One\": 1,{newLine}{defaultIndent}{defaultIndent}\"Two\": 2{newLine}{defaultIndent}}}{newLine}}}");
    }
}
