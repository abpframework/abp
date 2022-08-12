using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}