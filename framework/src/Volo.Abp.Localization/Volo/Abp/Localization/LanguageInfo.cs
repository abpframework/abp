using System;
using System.Globalization;
using JetBrains.Annotations;

namespace Volo.Abp.Localization;

[Serializable]
public class LanguageInfo : ILanguageInfo
{
    [NotNull]
    public virtual string CultureName { get; protected set; } = default!;

    [NotNull]
    public virtual string UiCultureName { get; protected set; } = default!;

    [NotNull]
    public virtual string DisplayName { get; protected set; } = default!;

    [NotNull]
    public virtual string TwoLetterISOLanguageName { get; protected set; } = default!;

    protected LanguageInfo()
    {

    }

    public LanguageInfo(
        string cultureName,
        string? uiCultureName = null,
        string? displayName = null)
    {
        ChangeCultureInternal(cultureName, uiCultureName, displayName);
    }

    public virtual void ChangeCulture(string cultureName, string? uiCultureName = null, string? displayName = null)
    {
        ChangeCultureInternal(cultureName, uiCultureName, displayName);
    }

    private void ChangeCultureInternal(string cultureName, string? uiCultureName, string? displayName)
    {
        CultureName = Check.NotNullOrWhiteSpace(cultureName, nameof(cultureName));

        UiCultureName = (!uiCultureName.IsNullOrWhiteSpace()
            ? uiCultureName
            : cultureName)!;

        DisplayName = (!displayName.IsNullOrWhiteSpace()
            ? displayName
            : cultureName)!;
        
        TwoLetterISOLanguageName = new CultureInfo(cultureName)
            .TwoLetterISOLanguageName;
    }
}
