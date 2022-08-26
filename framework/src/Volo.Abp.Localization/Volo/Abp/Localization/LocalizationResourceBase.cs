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

    public LocalizationResourceBase([NotNull] string resourceName)
    {
        ResourceName = Check.NotNullOrWhiteSpace(resourceName, nameof(resourceName));
        Contributors = new LocalizationResourceContributorList();
        BaseResourceNames = new();
    }
}