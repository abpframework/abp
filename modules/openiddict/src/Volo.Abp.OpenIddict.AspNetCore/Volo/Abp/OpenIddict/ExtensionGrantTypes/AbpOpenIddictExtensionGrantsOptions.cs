using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.OpenIddict.ExtensionGrantTypes;

public class AbpOpenIddictExtensionGrantsOptions
{
    public Dictionary<string, IExtensionGrant> Grants { get; }

    public AbpOpenIddictExtensionGrantsOptions()
    {
        Grants = new Dictionary<string, IExtensionGrant>();
    }

    public TExtensionGrantType Find<TExtensionGrantType>(string name)
        where TExtensionGrantType : IExtensionGrant
    {
        return (TExtensionGrantType)Grants.FirstOrDefault(x => x.Key == name && x.Value is TExtensionGrantType).Value;
    }
}
