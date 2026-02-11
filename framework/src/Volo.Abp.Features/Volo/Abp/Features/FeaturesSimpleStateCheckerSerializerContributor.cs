using System.Linq;
using System.Text.Json.Nodes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Features;

public class FeaturesSimpleStateCheckerSerializerContributor :
    ISimpleStateCheckerSerializerContributor,
    ISingletonDependency
{
    public const string CheckerShortName = "F";
    
    public string? SerializeToJson<TState>(ISimpleStateChecker<TState> checker)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (checker is not RequireFeaturesSimpleStateChecker<TState> featuresSimpleStateChecker)
        {
            return null;
        }

        var jsonObject = new JsonObject {
            ["T"] = CheckerShortName,
            ["A"] = featuresSimpleStateChecker.RequiresAll
        };

        var nameArray = new JsonArray();
        foreach (var featureName in featuresSimpleStateChecker.FeatureNames)
        {
            nameArray.Add(featureName);
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

        return new RequireFeaturesSimpleStateChecker<TState>(
            (bool?)jsonObject["A"] ?? false,
            nameArray.Select(x => x!.ToString()).ToArray()
        );
    }
}