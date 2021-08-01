using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace Volo.Abp.Account.Web.AbpGrantTypes
{
    public class AbpGrantTypeOptions
    {
        public Dictionary<string, Type> GrantTypeProviders { get; protected set; }

        public AbpGrantTypeOptions()
        {
            GrantTypeProviders = new Dictionary<string, Type>();

            Add<AbpPasswordGrantTypeProvider>(GrantTypes.Password);
            Add<AbpClientCredentialsGrantTypeProvider>(GrantTypes.ClientCredentials);
        }

        public AbpGrantTypeOptions Add<TGrantTypeProvider>([NotNull] string grantType) where TGrantTypeProvider : IGrantTypeProvider
        {
            return Add(grantType, typeof(TGrantTypeProvider));
        }

        public AbpGrantTypeOptions Add([NotNull] string grantType, [NotNull] Type grantTypeProviderType)
        {
            GrantTypeProviders[grantType] = grantTypeProviderType;
            return this;
        }
    }
}
