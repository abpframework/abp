using System;

namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

[Serializable]
public class CurrentCultureDto
{
    public string DisplayName { get; set; } = default!;

    public string EnglishName { get; set; } = default!;

    public string ThreeLetterIsoLanguageName { get; set; } = default!;

    public string TwoLetterIsoLanguageName { get; set; } = default!;

    public bool IsRightToLeft { get; set; }

    public string CultureName { get; set; } = default!;

    public string Name { get; set; } = default!;

    public string NativeName { get; set; } = default!;

    public DateTimeFormatDto DateTimeFormat { get; set; } = default!;
}
