using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Identity;

public class ExternalLoginProviderDictionary : Dictionary<string, ExternalLoginProviderInfo>
{
    /// <summary>
    /// Adds or replaces a provider.
    /// </summary>
    public void Add<TProvider>([NotNull] string name)
        where TProvider : IExternalLoginProvider
    {
        this[name] = new ExternalLoginProviderInfo(name, typeof(TProvider));
    }
}
