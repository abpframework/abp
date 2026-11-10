using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity;

public class IdentityPasskeyAuthenticatorNameProvider : ITransientDependency
{
    protected AbpIdentityPasskeyOptions Options { get; }

    public IdentityPasskeyAuthenticatorNameProvider(IOptions<AbpIdentityPasskeyOptions> options)
    {
        Options = options.Value;
    }

    public virtual Task<string?> GetNameOrNullAsync(byte[]? aaguid)
    {
        if (aaguid is not { Length: 16 })
        {
            return Task.FromResult<string?>(null);
        }

        // AAGUID is stored in the authenticator data as big-endian bytes.
        return Task.FromResult(Options.KnownAuthenticators.TryGetValue(new Guid(aaguid, bigEndian: true), out var name)
            ? name
            : null);
    }
}
