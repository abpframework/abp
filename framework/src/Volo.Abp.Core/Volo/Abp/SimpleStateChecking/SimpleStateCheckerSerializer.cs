using System.Collections.Generic;
using System.Text.Json.Nodes;
using JetBrains.Annotations;
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
    
    [CanBeNull]
    public string Serialize<TState>(ISimpleStateChecker<TState> checker) 
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

    [CanBeNull]
    public ISimpleStateChecker<TState> Deserialize<TState>(string value)
        where TState : IHasSimpleStateCheckers<TState>
    {
        var jsonObject = JsonNode.Parse(value) as JsonObject;
        if (jsonObject == null)
        {
            throw new AbpException("The value is not a JSON object: " + value);
        }
        
        foreach (var contributor in _contributors)
        {
            var result = contributor.Deserialize<TState>(jsonObject);
            if (result != null)
            {
                return result;
            }
        }
        
        return null;
    }
}