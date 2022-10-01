using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;
using Volo.Abp.IO;

namespace Volo.Abp.Cli.Memory;

public class MemoryService : ITransientDependency
{
    private const string KeyValueSeparator = "|||";
    
    [ItemCanBeNull]
    public async Task<string> GetAsync(string key)
    {
        if (!File.Exists(CliPaths.Memory))
        {
            return null;
        }
        
        return (await FileHelper.ReadAllTextAsync(CliPaths.Memory))
            .Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None)
            .FirstOrDefault(x => x.StartsWith($"{key} "))?.Split(KeyValueSeparator).Last().Trim();
    }

    public async Task SetAsync(string key, string value)
    {
        if (!File.Exists(CliPaths.Memory))
        {
            File.WriteAllText(CliPaths.Memory, 
                $"{key} {KeyValueSeparator} {value}"
            );
            return;
        }
        
        var memoryContentLines = (await FileHelper.ReadAllTextAsync(CliPaths.Memory))
            .Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.None)
            .ToList();

        memoryContentLines.RemoveAll(x => x.StartsWith(key));
        memoryContentLines.Add($"{key} {KeyValueSeparator} {value}");
        
        File.WriteAllText(CliPaths.Memory, 
            memoryContentLines.JoinAsString(Environment.NewLine)
        );
    }
}