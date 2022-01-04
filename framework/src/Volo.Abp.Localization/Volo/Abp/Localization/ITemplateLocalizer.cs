using Microsoft.Extensions.Localization;

namespace Volo.Abp.Localization;

public interface ITemplateLocalizer
{
    string Localize(IStringLocalizer localizer, string text);
}
