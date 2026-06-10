using System;
using System.Linq;

namespace Volo.Abp.IdentityServer;

public static class AllowedSigningAlgorithmsConverter
{
    private const char Separator = ',';
    
    public static string[] SplitToArray(string algorithms)
    {
        if (string.IsNullOrWhiteSpace(algorithms))
        {
            return [];
        }
        
        return algorithms
            .Split([Separator], StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}