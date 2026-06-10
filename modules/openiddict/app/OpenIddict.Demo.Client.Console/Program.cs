using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;
using Duende.IdentityModel;
using Duende.IdentityModel.Client;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Volo.Abp.AspNetCore.Mvc.ApplicationConfigurations;

const string email = "admin@abp.io";
const string password = "1q2w3E*";
const string server = "https://localhost:44301/";
const string serverApi = "https://localhost:44301/api/abp/application-configuration";
const string api = "https://localhost:44303/api/claims";
const string clientId = "AbpApp";
const string clientSecret = "1q2w3e*";

var client = new HttpClient();

var configuration = await client.GetDiscoveryDocumentAsync(server);
if (configuration.IsError)
{
    throw new Exception(configuration.Error);
}

var passwordTokenRequest = new PasswordTokenRequest
{
    Address = configuration.TokenEndpoint,
    ClientId = clientId,
    ClientSecret = clientSecret,
    UserName = email,
    Password = password,
    Scope = "AbpAPI profile roles email phone offline_access",
};
passwordTokenRequest.Headers.Add("__tenant", "Default");
var tokenResponse = await client.RequestPasswordTokenAsync(passwordTokenRequest);

if (tokenResponse.IsError)
{
    throw new Exception(tokenResponse.Error);
}

Console.WriteLine("Access token: {0}", tokenResponse.AccessToken);
Console.WriteLine();
Console.WriteLine("Refresh token: {0}", tokenResponse.RefreshToken);
Console.WriteLine();

var refreshTokenResponse = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
{
    Address = configuration.TokenEndpoint,
    ClientId = clientId,
    ClientSecret = clientSecret,
    RefreshToken = tokenResponse.RefreshToken
});

if (refreshTokenResponse.IsError)
{
    throw new Exception(refreshTokenResponse.Error);
}

Console.WriteLine("New Access token: {0}", refreshTokenResponse.AccessToken);
Console.WriteLine();
Console.WriteLine("New Refresh token: {0}", refreshTokenResponse.RefreshToken);
Console.WriteLine();

var userinfo = await client.GetUserInfoAsync(new UserInfoRequest()
{
    Address = configuration.UserInfoEndpoint,
    Token = tokenResponse.AccessToken
});
if (userinfo.IsError)
{
    throw new Exception(userinfo.Error);
}

Console.WriteLine("UserInfo: {0}", JsonSerializer.Serialize(JsonDocument.Parse(userinfo.Raw), new JsonSerializerOptions
{
    WriteIndented = true
}));
Console.WriteLine();

var tokenExchangeResponse = await client.RequestTokenExchangeTokenAsync(new TokenExchangeTokenRequest()
{
    Address = configuration.TokenEndpoint,
    ClientId = clientId,
    ClientSecret = clientSecret,
    SubjectToken = refreshTokenResponse.AccessToken!,
    SubjectTokenType = "urn:ietf:params:oauth:token-type:access_token",
    Scope = "AbpAPI profile roles email phone offline_access",
});

if (tokenExchangeResponse.IsError)
{
    throw new Exception(tokenExchangeResponse.Error);
}

Console.WriteLine("Token Exchange token: {0}", tokenExchangeResponse.AccessToken);
Console.WriteLine();
Console.WriteLine("Token Exchange token: {0}", tokenExchangeResponse.RefreshToken);
Console.WriteLine();

userinfo = await client.GetUserInfoAsync(new UserInfoRequest()
{
    Address = configuration.UserInfoEndpoint,
    Token = tokenExchangeResponse.AccessToken
});
if (userinfo.IsError)
{
    throw new Exception(userinfo.Error);
}

Console.WriteLine("Token Exchange UserInfo: {0}", JsonSerializer.Serialize(JsonDocument.Parse(userinfo.Raw), new JsonSerializerOptions
{
    WriteIndented = true
}));
Console.WriteLine();

var introspectionResponse  = await client.IntrospectTokenAsync(new TokenIntrospectionRequest()
{
    Address = configuration.IntrospectionEndpoint,
    ClientId = clientId,
    ClientSecret = clientSecret,
    Token = tokenResponse.AccessToken,
    TokenTypeHint = "access_token"
});
if (introspectionResponse.IsError)
{
    throw new Exception(introspectionResponse.Error);
}

Console.WriteLine("Introspection : {0}", JsonSerializer.Serialize(JsonDocument.Parse(introspectionResponse.Raw), new JsonSerializerOptions
{
    WriteIndented = true
}));
Console.WriteLine();

var serverRequest = new HttpRequestMessage(HttpMethod.Get, serverApi);
serverRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

var serverResponse = await client.SendAsync(serverRequest);
serverResponse.EnsureSuccessStatusCode();

var dto = JsonSerializer.Deserialize<ApplicationConfigurationDto>(await serverResponse.Content.ReadAsStringAsync(), new JsonSerializerOptions()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
});
Console.WriteLine("Server API response: {0}", JsonSerializer.Serialize(dto.CurrentUser, new JsonSerializerOptions
{
    WriteIndented = true
}));

Console.WriteLine();

var request = new HttpRequestMessage(HttpMethod.Get, api);
request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();

Console.WriteLine("API response: {0}", JsonSerializer.Serialize(JsonDocument.Parse(await response.Content.ReadAsStringAsync()), new JsonSerializerOptions
{
    WriteIndented = true
}));

Console.WriteLine();

client = new HttpClient();

tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = configuration.TokenEndpoint,

    ClientId = clientId,
    ClientSecret = clientSecret,

    Scope = "AbpAPI offline_access",
});

if (tokenResponse.IsError)
{
    Console.WriteLine(tokenResponse.Error);
    return;
}

Console.WriteLine("Access token: {0}", tokenResponse.AccessToken);
Console.WriteLine();
Console.WriteLine("Refresh token: {0}", tokenResponse.RefreshToken);
Console.WriteLine();

serverRequest = new HttpRequestMessage(HttpMethod.Get, api);
serverRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

serverResponse = await client.SendAsync(serverRequest);
serverResponse.EnsureSuccessStatusCode();

Console.WriteLine("ClientCredentials API response: {0}", JsonSerializer.Serialize(JsonDocument.Parse(await serverResponse.Content.ReadAsStringAsync()), new JsonSerializerOptions
{
    WriteIndented = true
}));

Console.WriteLine();

// private_key_jwt client credentials flow
// The private key file is generated by `abp generate-jwks` and stored in the parent app/ directory,
// then copied to the output directory during build.
// The corresponding public key (jwks.json) is registered on the authorization server side.
var privateKeyPath = Path.Combine(AppContext.BaseDirectory, "jwks-private.pem");
if (!File.Exists(privateKeyPath))
{
    Console.WriteLine("private_key_jwt demo skipped: private key file not found at {0}.", privateKeyPath);
    return;
}

using var rsaKey = RSA.Create();
rsaKey.ImportFromPem(await File.ReadAllTextAsync(privateKeyPath));

// Read the kid dynamically from the JWKS file so it stays in sync with the server-registered JWKS.
string? signingKid = null;
var jwksForKidPath = Path.Combine(AppContext.BaseDirectory, "jwks.json");
if (File.Exists(jwksForKidPath))
{
    using var jwksDoc = JsonDocument.Parse(await File.ReadAllTextAsync(jwksForKidPath));
    if (jwksDoc.RootElement.TryGetProperty("keys", out var keysElem) &&
        keysElem.GetArrayLength() > 0 &&
        keysElem[0].TryGetProperty("kid", out var kidElem))
    {
        signingKid = kidElem.GetString();
    }
}
var signingKey = new RsaSecurityKey(rsaKey) { KeyId = signingKid };
var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.RsaSha256);

var now = DateTime.UtcNow;
var jwtHandler = new JsonWebTokenHandler();
var clientAssertionToken = jwtHandler.CreateToken(new SecurityTokenDescriptor
{
    // OpenIddict requires typ = "client-authentication+jwt" for client assertion JWTs.
    // The aud claim must equal the authorization server's issuer URI (Options.Issuer), not the token endpoint.
    TokenType = "client-authentication+jwt",
    Issuer = "AbpConsoleAppWithJwks",
    Audience = configuration.Issuer,
    Subject = new ClaimsIdentity(new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, "AbpConsoleAppWithJwks"),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
    }),
    IssuedAt = now,
    NotBefore = now,
    Expires = now.AddMinutes(5),
    SigningCredentials = signingCredentials,
});

client = new HttpClient();

var jwksTokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = configuration.TokenEndpoint,
    ClientId = "AbpConsoleAppWithJwks",
    ClientCredentialStyle = ClientCredentialStyle.PostBody,
    ClientAssertion = new ClientAssertion
    {
        Type = OidcConstants.ClientAssertionTypes.JwtBearer,
        Value = clientAssertionToken,
    },
    Scope = "AbpAPI",
});

if (jwksTokenResponse.IsError)
{
    Console.WriteLine("private_key_jwt error: {0}", jwksTokenResponse.Error);
    return;
}

Console.WriteLine("private_key_jwt Access token: {0}", jwksTokenResponse.AccessToken);
Console.WriteLine();

serverRequest = new HttpRequestMessage(HttpMethod.Get, api);
serverRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwksTokenResponse.AccessToken);
serverResponse = await client.SendAsync(serverRequest);
serverResponse.EnsureSuccessStatusCode();

Console.WriteLine("private_key_jwt API response: {0}", JsonSerializer.Serialize(JsonDocument.Parse(await serverResponse.Content.ReadAsStringAsync()), new JsonSerializerOptions
{
    WriteIndented = true
}));

Console.WriteLine();
