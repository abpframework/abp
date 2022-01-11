namespace Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

public class CurrentCultureDto
{
    public string DisplayName { get; set; }

    public string EnglishName { get; set; }

    public string ThreeLetterIsoLanguageName { get; set; }

    public string TwoLetterIsoLanguageName { get; set; }

    public bool IsRightToLeft { get; set; }

    public string CultureName { get; set; }

    public string Name { get; set; }

    public string NativeName { get; set; }

    public DateTimeFormatDto DateTimeFormat { get; set; }
}
