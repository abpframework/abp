using System.Net.Http.Headers;
using IdentityModel.Client;

const string email = "admin@abp.io";
const string password = "1q2w3E*";
const string server = "https://localhost:44301/";
const string api = "https://localhost:44303/WeatherForecast";
const string clientId = "MyClient";
const string clientSecret = "901564A5-E7FE-42CB-B10D-61EF6A8F3654";
            
var client = new HttpClient();

var configuration = await client.GetDiscoveryDocumentAsync(server);
if (configuration.IsError)
{
    throw new Exception($"An error occurred while retrieving the configuration document: {configuration.Error}");
}

var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
{
    Address = configuration.TokenEndpoint,
    ClientId = clientId,
    ClientSecret = clientSecret,
    UserName = email,
    Password = password
});

if (tokenResponse.IsError)
{
    throw new Exception(tokenResponse.Error);
}

Console.WriteLine("Access token: {0}", tokenResponse.AccessToken);
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

Console.WriteLine("UserInfo: {0}", userinfo.Raw);
Console.WriteLine();

var request = new HttpRequestMessage(HttpMethod.Get, api);
request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

var response = await client.SendAsync(request);
response.EnsureSuccessStatusCode();

Console.WriteLine("API response: {0}", await response.Content.ReadAsStringAsync());
Console.WriteLine();