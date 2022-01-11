using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.LIbs;

public class ResourceMapping
{
    public Dictionary<string, string> Aliases { get; set; }

    public List<string> Clean { get; set; }

    public Dictionary<string, string> Mappings { get; set; }

    public ResourceMapping()
    {
        Aliases = new Dictionary<string, string>
            {
                {"@node_modules", "./node_modules"},
                {"@libs", "./wwwroot/libs"},
            };
        Clean = new List<string>();
        Mappings = new Dictionary<string, string>();
    }

    public void ReplaceAliases()
    {
        for (var i = 0; i < Mappings.Count; i++)
        {
            var mapping = Mappings.ElementAt(i);
            Mappings.Remove(mapping.Key);

            var key = mapping.Key;
            var value = mapping.Value;

            foreach (var alias in Aliases)
            {
                key = key.Replace(alias.Key, alias.Value);
                value = value.Replace(alias.Key, alias.Value);
            }

            Mappings[key] = value;
        }

        for (var i = 0; i < Clean.Count; i++)
        {
            foreach (var alias in Aliases)
            {
                Clean[i] = Clean[i].Replace(alias.Key, alias.Value);
            }
        }
    }
}
