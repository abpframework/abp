using System.Text.RegularExpressions;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Localization
{
    public class TemplateLocalizer : ITemplateLocalizer, ITransientDependency
    {
        public string Localize(IStringLocalizer localizer, string text)
        {
            return new Regex("\\{\\{#L:.+?\\}\\}")
                .Replace(
                    text,
                    match => localizer[match.Value.Substring(5, match.Length - 7)]
                );
        }
    }
}