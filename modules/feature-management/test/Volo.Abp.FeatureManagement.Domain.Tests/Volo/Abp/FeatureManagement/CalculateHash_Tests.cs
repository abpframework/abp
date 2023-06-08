using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization.Metadata;
using Shouldly;
using Volo.Abp.Json.SystemTextJson.Modifiers;
using Xunit;

namespace Volo.Abp.FeatureManagement;

public class CalculateHash_Tests : FeatureManagementDomainTestBase
{
    [Fact]
    public void Test()
    {
        var jsonSerializerOptions = new JsonSerializerOptions
        {
            TypeInfoResolver = new DefaultJsonTypeInfoResolver
            {
                Modifiers =
                {
                    new AbpIgnorePropertiesModifiers<FeatureGroupDefinitionRecord, Guid>().CreateModifyAction(x => x.Id),
                    new AbpIgnorePropertiesModifiers<FeatureDefinitionRecord, Guid>().CreateModifyAction(x => x.Id)
                }
            }
        };

        var id = Guid.NewGuid();
        var json = JsonSerializer.Serialize(new List<FeatureGroupDefinitionRecord>()
            {
                new FeatureGroupDefinitionRecord(id, "Test", "Test")
            },
            jsonSerializerOptions);

        json.ShouldNotContain("\"Id\"");
        json.ShouldNotContain(id.ToString("D"));

        json = JsonSerializer.Serialize(new List<FeatureDefinitionRecord>()
            {
                new FeatureDefinitionRecord(id, "Test", "Test", "Test", "Test", "Test", "Test", true, true,"Test", "Test")
            },
            jsonSerializerOptions);

        json.ShouldNotContain("\"Id\"");
        json.ShouldNotContain(id.ToString("D"));
    }
}
