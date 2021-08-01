using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict
{
    public interface IOpenIddictMemoryStore
    {
        ConcurrentDictionary<Guid, OpenIddictApplication> Applications { get; }

        ConcurrentDictionary<Guid, OpenIddictAuthorization> Authorizations { get; }

        ConcurrentDictionary<Guid, OpenIddictScope> Scopes { get; }

        ConcurrentDictionary<Guid, OpenIddictToken> Tokens { get; }
    }
}