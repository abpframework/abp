using System;
using System.Collections.Concurrent;

namespace Volo.Abp.AspNetCore.Components.WebAssembly;

/// <summary>
/// SHA-256 hashes of the access tokens that this client revoked itself after getting a newer one.
/// </summary>
internal static class WebAssemblyRevokedAccessTokens
{
    private static readonly ConcurrentDictionary<string, byte> Hashes = new();

    public static bool TryAdd(string accessToken)
    {
        return Hashes.TryAdd(accessToken.ToSha256(), 0);
    }

    public static void Remove(string accessToken)
    {
        Hashes.TryRemove(accessToken.ToSha256(), out _);
    }

    public static bool ContainsHash(string? accessTokenHash)
    {
        return !accessTokenHash.IsNullOrWhiteSpace() && Hashes.ContainsKey(accessTokenHash);
    }
}
