using System.Collections.Generic;

namespace Volo.Abp.IdentityServer
{
    public class AbpRedirectUriValidatorOptions
    {
        public List<string> DomainFormats { get; }

        public AbpRedirectUriValidatorOptions()
        {
            DomainFormats = new List<string>();
        }
    }
}
