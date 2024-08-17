using System.Collections.Generic;
using System.Text.Json.Nodes;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.SimpleStateChecking;

public class SimpleStateCheckerSerializer : 
    ISimpleStateCheckerSerializer,
    ISingletonDependency
{
    private readonly IEnumerable<ISimpleStateCheckerSerializerContributor> _contributors;

    public SimpleStateCheckerSerializer(IEnumerable<ISimpleStateCheckerSerializerContributor> contributors)
    {
        _contributors = contributors;
    }
    
    public string? Serialize<TState>(ISimpleStateChecker<TState> checker) 
        where TState : IHasSimpleStateCheckers<TState>
    {
        foreach (var contributor in _contributors)
        {
            var result = contributor.SerializeToJson(checker);
            if (result != null)
            {
                return result;
            }
        }

        return null;
    }

    public ISimpleStateChecker<TState>? Deserialize<TState>(JsonObject jsonObject, TState state)
        where TState : IHasSimpleStateCheckers<TState>
    {
        foreach (var contributor in _contributors)
        {
            var result = contributor.Deserialize(jsonObject, state);
            if (result != null)
            {
                return result;
            }
        }
        
        return null;
    }
}