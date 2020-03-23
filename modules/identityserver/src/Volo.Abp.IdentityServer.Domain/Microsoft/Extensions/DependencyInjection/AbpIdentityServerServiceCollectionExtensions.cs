using IdentityServer4;
using IdentityServer4.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;

public static class IdentityServerBuilderExtensionsCrypto
{
    public static IIdentityServerBuilder AddInMemoryDeveloperSigningCredential(this IIdentityServerBuilder builder)
    {
        var key = CryptoHelper.CreateRsaSecurityKey();

        return builder.AddSigningCredential(key, IdentityServerConstants.RsaSigningAlgorithm.RS256);
    }
    public static IIdentityServerBuilder AddInMemoryDeveloperSigningCredentialStatic(this IIdentityServerBuilder builder)
    {
        const String json =
                    @"{
                ""alg"" : ""RS256"",
                ""kty"" : ""RSA"",
                ""use"" : ""sig"",
                ""d"" : ""KGGNkbbgm2hNMqW6fP1fmcWwEBy77WOJIPAXnDJ0KxNTtqDF8K5ULj7EElHO1A8ZnNl1Ey/x//G9lJCOQUU9wmj010dOSsW0NBbR5NtRtLLuVbkVdyft53PGeTQs+1S3c51fz9jojtNqmlfXSANPFOH6QhxmzpTx3KLsf/TpCzblkSrEGOOqCCvVdl7ybTcB230jNhh3JoL7po1rvxKtoOM4a/Bs0NtKj7e+VaHcf0GLnBPJYetsHu43ZfNejJeDoouaXZzeVEklY3B0pe10OTCIOu0JUKGZxNekklRIo1WSEYdL+CJfrSKWIv8bLj6xSr5zrASvWODyH443LN6ZvQ=="",
                ""e"" : ""AQAB"",
                ""n"" : ""q7mZfquRq8tzg/5slbNdQmrosNN/mFXS25dbSPm11qEDCgZa452KkO8+hvMtqa92QaqdlmalSF8+FRDOz3grDR5NtmnXZxuKnp+raKfzpC6hCvh2JSIe/J9enmsMM4YeI4d1FOSDwhJlZIYMdMnqG/VJtO1LSHjOaF3XN31ANKF0nPAsmr2/WysiQlxnxxiikLEnsFuNdS615ODDXFGTQ1E+zc4zVur4/Ox0cllPwHPA4PqoIgdPJPL+xM9IOIXuAGtsp4CYoxT6VWaRrALIZXXDY806WGTuctq4KKot6FGL9HQte2hRLl4E/r8SzIK86U3wRwrBe7saK+XUXoP0gQ="",
		        ""p"" :	""25dkucyCSqxRcJpRrhl7PXqw7wqBZeLQgYlZLpK493PdM8pFfq+/LK1hFtxIjdFKqXS/TOikB4YCBMEH0Im3HZ8Lo0dub3SWNhdegJyRjMbcoO+A9YSODEj7DFaNpZtdmtDi1n6etJm66ctPSR20NNpzoYZuaJ92fVQiKiOh6Qs="",
                ""q"" : ""yDKBrS8l1DOx4dwP9hdwhqZJ3XahidiIZSL7m46I/6+cjaki/1mtNiA60MOgqTKegP7Fo7jAYvliqQwnvVGmQvLv19cfKywlIuKN9DdkLHnKh75hfo7aakEbO7GJ5zVgsNnKOdf8wvpclfvIuRDEVva4cksPzsJy6K7C8ENCSCM="",
                ""dp"" :  ""GlYJ6o6wgawxCEQ5z5uWwETau5CS/Fk7kI2ceI14SZVHzlJQC2WglAcnQcqhmQCk57Xsy5iLM6vKyi8sdMJPh+nvR2HlyNA+w7YBy4L7odqn01VmLgv7zVVjZpNq4ZXEoDC1Q+xjtF1LoYaUt7wsRLp+a7znuPyHBXj1sAAeBwk="",
                ""dq"" :  ""W8OK3S83T8VCTBzq1Ap6cb3XLcQq11yBaJpYaj0zXr/IKsbUW+dnFeBAFWEWS3gAX3Bod1tAFB3rs0D3FjhO1XE1ruHUT520iAEAwGiDaj+JLh994NzqELo3GW2PoIM/BtFNeKYgHd9UgQsgPnQJCzOb6Aev/z3yHeW9RRQPVbE="",
                ""qi"" :  ""w4KdmiDN1GtK71JxaasqmEKPNfV3v2KZDXKnfyhUsdx/idKbdTVjvMOkxFPJ4FqV4yIVn06f3QHTm4NEG18Diqxsrzd6kXQIHOa858tLsCcmt9FoGfrgCFgVceh3K/Zah/r8rl9Y61u0Z1kZumwMvFpFE+mVU01t9HgTEAVkHTc="",
            }";

        JsonWebKey jsonWebKey = new JsonWebKey(json);
        SigningCredentials credentials = new SigningCredentials(jsonWebKey, jsonWebKey.Alg);
        return builder.AddSigningCredential(credentials);
    }
}