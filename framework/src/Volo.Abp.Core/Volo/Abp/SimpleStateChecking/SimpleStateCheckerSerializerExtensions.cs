using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;

namespace Volo.Abp.SimpleStateChecking;

public static class SimpleStateCheckerSerializerExtensions
{
    public static string Serialize<TState>(
        this ISimpleStateCheckerSerializer serializer, 
        IList<ISimpleStateChecker<TState>> stateCheckers)
        where TState : IHasSimpleStateCheckers<TState>
    {
        switch (stateCheckers.Count)
        {
            case 0:
                return null;
            case 1:
                return $"[{serializer.Serialize(stateCheckers.Single())}]";
            default:
                var stringBuilder = new StringBuilder("[");
                
                for (var i = 0; i < stateCheckers.Count; i++)
                {
                    if (i > 0)
                    {
                        stringBuilder.Append(",");
                    }
            
                    stringBuilder.Append(serializer.Serialize(stateCheckers[i]));
                }

                stringBuilder.Append("]");
        
                return stringBuilder.ToString();
        }
    }

    public static ISimpleStateChecker<TState>[] DeserializeArray<TState>(
        this ISimpleStateCheckerSerializer serializer,
        string value)
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

            return new[] { serializer.Deserialize<TState>(jsonObject) };
        }

        var checkers = new ISimpleStateChecker<TState>[array.Count];

        for (var i = 0; i < array.Count; i++)
        {
            if (array[i] is not JsonObject jsonObject)
            {
                throw new AbpException("JSON value is not an array of objects: " + value);
            }

            checkers[i] = serializer.Deserialize<TState>(jsonObject);
        }

        return checkers;
    }
}