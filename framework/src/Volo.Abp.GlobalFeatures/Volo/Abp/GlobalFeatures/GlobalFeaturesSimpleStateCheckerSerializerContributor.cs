using System.Linq;
using System.Text.Json.Nodes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.GlobalFeatures;

public class GlobalFeaturesSimpleStateCheckerSerializerContributor :
    ISimpleStateCheckerSerializerContributor,
    ISingletonDependency
{
    public const string CheckerShortName = "G";

    public string? SerializeToJson<TState>(ISimpleStateChecker<TState> checker)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (checker is not RequireGlobalFeaturesSimpleStateChecker<TState> globalFeaturesSimpleStateChecker)
        {
            return null;
        }

        var jsonObject = new JsonObject {
            ["T"] = CheckerShortName,
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

    public ISimpleStateChecker<TState>? Deserialize<TState>(JsonObject jsonObject, TState state)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (jsonObject["T"]?.ToString() != CheckerShortName)
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
            nameArray.Select(x => x!.ToString()).ToArray()
        );
    }
}