using System.Net.Http.Headers;
using System.Text.Json;
using IdentityModel.Client;

const string email = "admin@abp.io";
const string password = "1q2w3E*";
const string server = "https://localhost:44301/";
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


var request = new HttpRequestMessage(HttpMethod.Get, api);
request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();

Console.WriteLine("API response: {0}", JsonSerializer.Serialize(JsonDocument.Parse(await response.Content.ReadAsStringAsync()), new JsonSerializerOptions
{
    WriteIndented = true
}));

Console.WriteLine();


tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
{
    Address = configuration.TokenEndpoint,

    ClientId = clientId,
    ClientSecret = clientSecret,

    Scope = "AbpAPI profile roles email phone offline_access",
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
