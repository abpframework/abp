using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;

namespace Volo.Abp.SimpleStateChecking;

public static class SimpleStateCheckerSerializerExtensions
{
    public static string? Serialize<TState>(
        this ISimpleStateCheckerSerializer serializer, 
        IList<ISimpleStateChecker<TState>> stateCheckers)
        where TState : IHasSimpleStateCheckers<TState>
    {
        switch (stateCheckers.Count)
        {
            case 0:
                return null;
            case 1:
                var serializedChecker = serializer.Serialize(stateCheckers.Single());
                return serializedChecker != null
                    ? $"[{serializedChecker}]"
                    : null;
            default:
                var serializedCheckers = new List<string>(stateCheckers.Count);
                
                foreach (var stateChecker in stateCheckers)
                {
                    var serialized = serializer.Serialize(stateChecker);
                    if (serialized != null)
                    {
                        serializedCheckers.Add(serialized);
                    }
                }

                return serializedCheckers.Any()
                    ? $"[{serializedCheckers.JoinAsString(",")}]"
                    : null;
        }
    }

    public static ISimpleStateChecker<TState>[] DeserializeArray<TState>(
        this ISimpleStateCheckerSerializer serializer,
        string value,
        TState state)
        where TState : IHasSimpleStateCheckers<TState>
    {
        if (value.IsNullOrWhiteSpace())
        {
            return Array.Empty<ISimpleStateChecker<TState>>();
        }
        
        var array = JsonNode.Parse(value) as JsonArray;
        if (array == null || array.Count == 0)
        {
            return Array.Empty<ISimpleStateChecker<TState>>();
        }
        
        if (array.Count == 1)
        {
            var jsonObject = array[0] as JsonObject;
            if (jsonObject == null)
            {
                throw new AbpException("JSON value is not an array of objects: " + value);
            }

            var checker = serializer.Deserialize(jsonObject, state);
            if (checker == null)
            {
                return Array.Empty<ISimpleStateChecker<TState>>();
            }
            
            return new[] { checker };
        }

        var checkers = new List<ISimpleStateChecker<TState>?>();

        for (var i = 0; i < array.Count; i++)
        {
            if (array[i] is not JsonObject jsonObject)
            {
                throw new AbpException("JSON value is not an array of objects: " + value);
            }

            checkers.Add(serializer.Deserialize(jsonObject, state));
        }

        return checkers.Where(x => x != null).ToArray()!;
    }
}