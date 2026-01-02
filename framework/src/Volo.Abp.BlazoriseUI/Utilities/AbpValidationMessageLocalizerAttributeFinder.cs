using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Blazorise.Utilities;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlazoriseUI.Utilities;

[Dependency(ReplaceServices = true)]
[ExposeServices(typeof(AbpValidationMessageLocalizerAttributeFinder), typeof(IValidationMessageLocalizerAttributeFinder))]
public class AbpValidationMessageLocalizerAttributeFinder : IValidationMessageLocalizerAttributeFinder, ISingletonDependency
{
    public IEnumerable<(string Index, string Argument)> FindAll(string first, string second)
    {
        if (string.IsNullOrEmpty(first) || string.IsNullOrEmpty(second))
        {
            yield break;
        }
        
        if (first == second)
        {
            yield break;
        }

        const string placeholderPattern = @"\{(\d+)\}";
        var matches = Regex.Matches(second, placeholderPattern);
        
        if (matches.Count == 0)
        {
            yield break;
        }

        var placeholderIndices = new List<string>();
        foreach (Match match in matches)
        {
            placeholderIndices.Add(match.Groups[1].Value);
        }

        var pattern = placeholderIndices.Aggregate(second, (current, index) => current.Replace("{" + index + "}", "(.+)"));

        var valueMatch = Regex.Match(first, pattern);
        if (!valueMatch.Success)
        {
            yield break;
        }

        for (var i = 0; i < placeholderIndices.Count && i + 1 < valueMatch.Groups.Count; i++)
        {
            yield return (placeholderIndices[i], valueMatch.Groups[i + 1].Value);
        }
    }
}