using System.Linq;
using System.Text.Json.Nodes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.GlobalFeatures;

public class GlobalFeaturesSimpleStateCheckerSerializerContributor :
    ISimpleStateCheckerSerializerContributor,
    ITransientDependency
{
    public string SerializeToJson<TState>(ISimpleStateChecker<TState> checker)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (checker is not RequireGlobalFeaturesSimpleStateChecker<TState> globalFeaturesSimpleStateChecker)
        {
            return null;
        }

        var jsonObject = new JsonObject {
            ["T"] = "GF",
            ["A"] = globalFeaturesSimpleStateChecker.RequiresAll
        };

        var nameArray = new JsonArray();
        foreach (var globalFeatureName in globalFeaturesSimpleStateChecker.GlobalFeatureNames)
        {
            nameArray.Add(globalFeatureName);
        }

        jsonObject["N"] = nameArray;
        return jsonObject.ToJsonString();
    }

    public ISimpleStateChecker<TState> Deserialize<TState>(JsonObject jsonObject)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (jsonObject["T"]?.ToString() != "GF")
        {
            return null;
        }

        var nameArray = jsonObject["N"] as JsonArray;
        if (nameArray == null)
        {
            throw new AbpException("'N' is not an array in the serialized state checker! JsonObject: " + jsonObject.ToJsonString());
        }

        return new RequireGlobalFeaturesSimpleStateChecker<TState>(
            (bool?)jsonObject["A"] ?? false,
            nameArray.Select(x => x.ToString()).ToArray()
        );
    }
}