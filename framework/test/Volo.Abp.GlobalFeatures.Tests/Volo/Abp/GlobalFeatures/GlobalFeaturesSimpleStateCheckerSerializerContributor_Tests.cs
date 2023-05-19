using System.Collections.Generic;
using System.Text.Json.Nodes;
using Shouldly;
using Volo.Abp.SimpleStateChecking;
using Xunit;

namespace Volo.Abp.GlobalFeatures;

public class GlobalFeaturesSimpleStateCheckerSerializerContributor_Tests
{
    [Fact]
    public void Should_Serialize_RequireGlobalFeaturesSimpleStateChecker()
    {
        var serializer = new GlobalFeaturesSimpleStateCheckerSerializerContributor();
        
        var result = serializer.SerializeToJson(
            new RequireGlobalFeaturesSimpleStateChecker<MyState>(
                "FeatureA",
                "FeatureB"
            )
        );
        
        result.ShouldBe("{\"T\":\"G\",\"A\":true,\"N\":[\"FeatureA\",\"FeatureB\"]}");
    }
    
    [Fact]
    public void Should_Deserialize_RequireGlobalFeaturesSimpleStateChecker()
    {
        var serializer = new GlobalFeaturesSimpleStateCheckerSerializerContributor();

        var jsonObject = (JsonObject)JsonNode.Parse("{\"T\":\"G\",\"A\":true,\"N\":[\"FeatureA\",\"FeatureB\"]}");
        var checker = serializer.Deserialize<MyState>(jsonObject, new MyState());
        
        checker.ShouldBeOfType<RequireGlobalFeaturesSimpleStateChecker<MyState>>();
        var globalFeaturesSimpleStateChecker = checker as RequireGlobalFeaturesSimpleStateChecker<MyState>;
        globalFeaturesSimpleStateChecker.ShouldNotBeNull();
        globalFeaturesSimpleStateChecker.RequiresAll.ShouldBeTrue();
        globalFeaturesSimpleStateChecker.GlobalFeatureNames[0].ShouldBe("FeatureA");
        globalFeaturesSimpleStateChecker.GlobalFeatureNames[1].ShouldBe("FeatureB");
    }

    private class MyState : IHasSimpleStateCheckers<MyState>
    {
        public List<ISimpleStateChecker<MyState>> StateCheckers { get; } = new();
    }
}