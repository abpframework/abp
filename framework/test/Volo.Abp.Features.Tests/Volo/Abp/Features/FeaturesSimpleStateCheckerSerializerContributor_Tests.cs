using System.Collections.Generic;
using System.Text.Json.Nodes;
using Shouldly;
using Volo.Abp.SimpleStateChecking;
using Xunit;

namespace Volo.Abp.Features;

public class FeaturesSimpleStateCheckerSerializerContributor_Tests
{
    [Fact]
    public void Should_Serialize_RequireGlobalFeaturesSimpleStateChecker()
    {
        var serializer = new FeaturesSimpleStateCheckerSerializerContributor();
        
        var result = serializer.SerializeToJson(
            new RequireFeaturesSimpleStateChecker<MyState>(
                "FeatureA",
                "FeatureB"
            )
        );
        
        result.ShouldBe("{\"T\":\"F\",\"A\":true,\"N\":[\"FeatureA\",\"FeatureB\"]}");
    }
    
    [Fact]
    public void Should_Deserialize_RequireGlobalFeaturesSimpleStateChecker()
    {
        var serializer = new FeaturesSimpleStateCheckerSerializerContributor();

        var jsonObject = (JsonObject)JsonNode.Parse("{\"T\":\"F\",\"A\":true,\"N\":[\"FeatureA\",\"FeatureB\"]}");
        var checker = serializer.Deserialize<MyState>(jsonObject, new MyState());
        
        checker.ShouldBeOfType<RequireFeaturesSimpleStateChecker<MyState>>();
        var globalFeaturesSimpleStateChecker = checker as RequireFeaturesSimpleStateChecker<MyState>;
        globalFeaturesSimpleStateChecker.ShouldNotBeNull();
        globalFeaturesSimpleStateChecker.RequiresAll.ShouldBeTrue();
        globalFeaturesSimpleStateChecker.FeatureNames[0].ShouldBe("FeatureA");
        globalFeaturesSimpleStateChecker.FeatureNames[1].ShouldBe("FeatureB");
    }

    private class MyState : IHasSimpleStateCheckers<MyState>
    {
        public List<ISimpleStateChecker<MyState>> StateCheckers { get; } = new();
    }
}