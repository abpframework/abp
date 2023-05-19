using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Localization;

public abstract class LocalizationResourceBase
{
    [NotNull] 
    public string ResourceName { get; }
    
    public List<string> BaseResourceNames { get; }
    
    [CanBeNull]
    public string DefaultCultureName { get; set; }
    
    [NotNull]
    public LocalizationResourceContributorList Contributors { get; }

    public LocalizationResourceBase(
        [NotNull] string resourceName,
        [CanBeNull] string defaultCultureName = null,
        [CanBeNull] ILocalizationResourceContributor initialContributor = null)
    {
        ResourceName = Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName));
        DefaultCultureName = defaultCultureName;
        
        Contributors = new LocalizationResourceContributorList();
        BaseResourceNames = new();
        
        if (initialContributor != null)
        {
            Contributors.Add(initialContributor);
        }
    }
}