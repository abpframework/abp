```json
//[doc-seo]
{
    "Description": "Configure ABP IdentityModel clients for server-to-server access tokens, tenant-aware client selection, and request customization."
}
```

# IdentityModel Clients

The `Volo.Abp.IdentityModel` package obtains access tokens for server-to-server HTTP calls. `AbpIdentityModelModule` binds the `IdentityClients` configuration section to `AbpIdentityClientOptions`.

## Installation

Install the `Volo.Abp.IdentityModel` NuGet package in the project that obtains the tokens:

````shell
abp add-package Volo.Abp.IdentityModel
````

The command adds the package and the `AbpIdentityModelModule` dependency to the module class.

## Configure Identity Clients

Define a `Default` client and any named clients in the application configuration:

````json
{
  "IdentityClients": {
    "Default": {
      "GrantType": "client_credentials",
      "ClientId": "MyProject_Backend",
      "ClientSecret": "your-client-secret",
      "Authority": "https://localhost:44301/",
      "Scope": "MyProject"
    },
    "Reporting": {
      "GrantType": "client_credentials",
      "ClientId": "MyProject_Reporting",
      "ClientSecret": "your-reporting-client-secret",
      "Authority": "https://localhost:44301/",
      "Scope": "Reporting"
    }
  }
}
````

When a client name is requested for the current tenant, ABP selects the first available configuration in this order:

1. `<client-name>.<tenant-id>`
2. `<client-name>.<tenant-name>`
3. `<client-name>`
4. `Default`

If no client name is supplied, ABP uses `Default` as the client name. Tenant-specific entries let a tenant use different credentials without changing the consuming service. For example, `Reporting.8e6fcd0a-75ab-4d94-90f4-9a2503d0e70c` overrides the `Reporting` client for that tenant ID.

Pass the client name to `TryAuthenticateAsync` when you authenticate an `HttpClient` directly:

````csharp
var client = _httpClientFactory.CreateClient();

if (!await _authenticationService.TryAuthenticateAsync(client, "Reporting"))
{
    throw new InvalidOperationException(
        "The Reporting identity client is not configured."
    );
}

var response = await client.GetAsync("https://reporting.example.com/api/reports");
````

Install `Volo.Abp.Http.Client.IdentityModel` when dynamic HTTP client proxies should obtain tokens automatically. Its `AbpHttpClientIdentityModelModule` integration uses the remote service's `IdentityClient` value when configured, then the remote service name, and finally the `Default` identity client fallback described above:

````json
{
  "RemoteServices": {
    "Reporting": {
      "BaseUrl": "https://reporting.example.com/",
      "IdentityClient": "Reporting"
    }
  }
}
````

## Customize Discovery and Token Requests

Use `IdentityModelHttpRequestMessageOptions.ConfigureHttpRequestMessage` to add headers or otherwise customize the request messages:

````csharp
Configure<IdentityModelHttpRequestMessageOptions>(options =>
{
    options.ConfigureHttpRequestMessage = request =>
    {
        request.Headers.TryAddWithoutValidation(
            "X-Internal-Client",
            "MyProject"
        );
    };
});
````

The callback runs for discovery, client-credentials, password and device-authorization request messages created by the default authentication service.
