using OpenIddict.Abstractions;
using System.Collections.Generic;

namespace Volo.Abp.OpenIddict
{
    public class AbpOpenIddictOptions
    {
        public List<OpenIddictApplicationDescriptor> Applications { get; set; } = new();

        public List<OpenIddictScopeDescriptor> Scopes { get; set; } = new();
    }
}
