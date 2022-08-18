using System.Text.Json.Nodes;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SimpleStateChecking;

namespace Volo.Abp.Authorization.Permissions;

public class AuthenticatedSimpleStateCheckerSerializerContributor : 
    ISimpleStateCheckerSerializerContributor,
    ISingletonDependency
{
    public string SerializeToJson<TState>(ISimpleStateChecker<TState> checker) 
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (checker is not RequireAuthenticatedSimpleStateChecker<TState> requireAuthenticatedSimpleStateChecker)
        {
            return null;
        }

        var jsonObject = new JsonObject {
            ["T"] = "A"
        };

        return jsonObject.ToJsonString();
    }

    public ISimpleStateChecker<TState> Deserialize<TState>(JsonObject jsonObject)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (jsonObject["T"]?.ToString() != "A")
        {
            return null;
        }
        
        return new RequireAuthenticatedSimpleStateChecker<TState>();
    }
}