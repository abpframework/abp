using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Volo.Abp.Http.Client.Authentication;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.WebApp;

public class PersistentComponentStateAbpAccessTokenProvider : IAbpAccessTokenProvider
{
    private string? AccessToken { get; set; }

    private readonly PersistentComponentState _persistentComponentState;

    public PersistentComponentStateAbpAccessTokenProvider(PersistentComponentState persistentComponentState)
    {
        _persistentComponentState = persistentComponentState;
        AccessToken = null;
    }

    public virtual Task<string?> GetTokenAsync()
    {
        if (AccessToken != null)
        {
            return Task.FromResult<string?>(AccessToken);
        }

        AccessToken = _persistentComponentState.TryTakeFromJson<PersistentAccessToken>(PersistentAccessToken.Key, out var token)
            ? token?.AccessToken
            : null;

        return Task.FromResult(AccessToken);
    }
}
