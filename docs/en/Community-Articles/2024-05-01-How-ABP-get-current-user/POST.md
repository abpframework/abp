# How claim type works in ASP NET Core and ABP Framework

## The Claim Type

A web application may use one or more authentication schemes to obtain the current user's information, such as `Cookies`, `JwtBearer`, `OpenID Connect`, `Google` etc.

After authentication, we get a set of claims that can be issued using a trusted identity provider. A claim is a type/name-value pair representing the subject. The type property provides the semantic content of the claim, that is, it states what the claim is about. 

The [`ICurrentUser`](https://docs.abp.io/en/abp/latest/CurrentUser) service of the ABP Framework provides a convenient way to access the current user's information from the claims.

The claim type is the key to getting the correct value of the current user, and we have a static `AbpClaimTypes` class that defines the names of the standard claims in the ABP Framework:

```cs
public static class AbpClaimTypes
{
    public static string UserId { get; set; } = ClaimTypes.NameIdentifier;
    public static string UserName { get; set; } = ClaimTypes.Name;
    public static string Role { get; set; } = ClaimTypes.Role;
    public static string Email { get; set; } = ClaimTypes.Email;
    //...
}
```

As you can see, the default claim type of `AbpClaimTypes` comes from the [`System.Security.Claims.ClaimTypes`](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes) class, which is the recommended practice in NET.

## Claim type in different authentication schemes

We usually see two types of claim types in our daily development. One of them is the [`System.Security.Claims.ClaimTypes`](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes) and the other one is the `OpenId Connect` [standard claims](https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims).

### ASP NET Core Identity

There is a [`ClaimsIdentityOptions`](https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.claimsidentityoptions) property in the `IdentityOptions`, which can be used to configure the claim type:

| Property             | Description                                                                                                   |
|----------------------|---------------------------------------------------------------------------------------------------------------|
| EmailClaimType       | Gets or sets the ClaimType used for the user email claim. Defaults to Email.                                  |
| RoleClaimType        | Gets or sets the ClaimType used for a Role claim. Defaults to Role.                                           |
| SecurityStampClaimType | Gets or sets the ClaimType used for the security stamp claim. Defaults to "AspNet.Identity.SecurityStamp".  |
| UserIdClaimType      | Gets or sets the ClaimType used for the user identifier claim. Defaults to NameIdentifier.                    |
| UserNameClaimType    | Gets or sets the ClaimType used for the user name claim. Defaults to Name.                                    |

* The Identity creates a `ClaimsIdentity` object with the claim type that you have configured in the `ClaimsIdentityOptions` class. 
* The ABP Framework configures it based on `AbpClaimTypes,` so usually you don't need to worry about it.

### JwtBearer/OpenID Connect Client

The `JwtBearer/OpenID Connect` gets claims from `id_token` or fetches user information from the `AuthServer`, and then maps/adds it to the current `ClaimsIdentity`.

To map the [standard claim](https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims) type to the [`System.Security.Claims.ClaimTypes`](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes) via [azure-activedirectory-identitymodel-extensions-for-dotnet](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet) library by default, which is maintained by the Microsoft team:

```cs
Dictionary<string, string> ClaimTypeMapping = new Dictionary<string, string>
{
    { "actort", ClaimTypes.Actor },
    { "birthdate", ClaimTypes.DateOfBirth },
    { "email", ClaimTypes.Email },
    { "family_name", ClaimTypes.Surname },
    { "gender", ClaimTypes.Gender },
    { "given_name", ClaimTypes.GivenName },
    { "nameid", ClaimTypes.NameIdentifier },
    { "sub", ClaimTypes.NameIdentifier },
    { "website", ClaimTypes.Webpage },
    { "unique_name", ClaimTypes.Name },
    { "oid", "http://schemas.microsoft.com/identity/claims/objectidentifier" },
    { "scp", "http://schemas.microsoft.com/identity/claims/scope" },
    { "tid", "http://schemas.microsoft.com/identity/claims/tenantid" },
    { "acr", "http://schemas.microsoft.com/claims/authnclassreference" },
    { "adfs1email", "http://schemas.xmlsoap.org/claims/EmailAddress" },
    { "adfs1upn", "http://schemas.xmlsoap.org/claims/UPN" },
    { "amr", "http://schemas.microsoft.com/claims/authnmethodsreferences" },
    { "authmethod", ClaimTypes.AuthenticationMethod },
    { "certapppolicy", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/applicationpolicy" },
    { "certauthoritykeyidentifier", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/authoritykeyidentifier" },
    { "certbasicconstraints", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/basicconstraints" },
    { "certeku", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/eku" },
    { "certissuer", "http://schemas.microsoft.com/2012/12/certificatecontext/field/issuer" },
    { "certissuername", "http://schemas.microsoft.com/2012/12/certificatecontext/field/issuername" },
    { "certkeyusage", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/keyusage" },
    { "certnotafter", "http://schemas.microsoft.com/2012/12/certificatecontext/field/notafter" },
    { "certnotbefore", "http://schemas.microsoft.com/2012/12/certificatecontext/field/notbefore" },
    { "certpolicy", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/certificatepolicy" },
    { "certpublickey", ClaimTypes.Rsa },
    { "certrawdata", "http://schemas.microsoft.com/2012/12/certificatecontext/field/rawdata" },
    { "certserialnumber", ClaimTypes.SerialNumber },
    { "certsignaturealgorithm", "http://schemas.microsoft.com/2012/12/certificatecontext/field/signaturealgorithm" },
    { "certsubject", "http://schemas.microsoft.com/2012/12/certificatecontext/field/subject" },
    { "certsubjectaltname", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/san" },
    { "certsubjectkeyidentifier", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/subjectkeyidentifier" },
    { "certsubjectname", "http://schemas.microsoft.com/2012/12/certificatecontext/field/subjectname" },
    { "certtemplateinformation", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/certificatetemplateinformation" },
    { "certtemplatename", "http://schemas.microsoft.com/2012/12/certificatecontext/extension/certificatetemplatename" },
    { "certthumbprint", ClaimTypes.Thumbprint },
    { "certx509version", "http://schemas.microsoft.com/2012/12/certificatecontext/field/x509version" },
    { "clientapplication", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-client-application" },
    { "clientip", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-client-ip" },
    { "clientuseragent", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-client-user-agent" },
    { "commonname", "http://schemas.xmlsoap.org/claims/CommonName" },
    { "denyonlyprimarygroupsid", ClaimTypes.DenyOnlyPrimaryGroupSid },
    { "denyonlyprimarysid", ClaimTypes.DenyOnlyPrimarySid },
    { "denyonlysid", ClaimTypes.DenyOnlySid },
    { "devicedispname", "http://schemas.microsoft.com/2012/01/devicecontext/claims/displayname" },
    { "deviceid", "http://schemas.microsoft.com/2012/01/devicecontext/claims/identifier" },
    { "deviceismanaged", "http://schemas.microsoft.com/2012/01/devicecontext/claims/ismanaged" },
    { "deviceostype", "http://schemas.microsoft.com/2012/01/devicecontext/claims/ostype" },
    { "deviceosver", "http://schemas.microsoft.com/2012/01/devicecontext/claims/osversion" },
    { "deviceowner", "http://schemas.microsoft.com/2012/01/devicecontext/claims/userowner" },
    { "deviceregid", "http://schemas.microsoft.com/2012/01/devicecontext/claims/registrationid" },
    { "endpointpath", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-endpoint-absolute-path" },
    { "forwardedclientip", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-forwarded-client-ip" },
    { "group", "http://schemas.xmlsoap.org/claims/Group" },
    { "groupsid", ClaimTypes.GroupSid },
    { "idp", "http://schemas.microsoft.com/identity/claims/identityprovider" },
    { "insidecorporatenetwork", "http://schemas.microsoft.com/ws/2012/01/insidecorporatenetwork" },
    { "isregistereduser", "http://schemas.microsoft.com/2012/01/devicecontext/claims/isregistereduser" },
    { "ppid", "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/privatepersonalidentifier" },
    { "primarygroupsid", ClaimTypes.PrimaryGroupSid },
    { "primarysid", ClaimTypes.PrimarySid },
    { "proxy", "http://schemas.microsoft.com/2012/01/requestcontext/claims/x-ms-proxy" },
    { "pwdchgurl", "http://schemas.microsoft.com/ws/2012/01/passwordchangeurl" },
    { "pwdexpdays", "http://schemas.microsoft.com/ws/2012/01/passwordexpirationdays" },
    { "pwdexptime", "http://schemas.microsoft.com/ws/2012/01/passwordexpirationtime" },
    { "relyingpartytrustid", "http://schemas.microsoft.com/2012/01/requestcontext/claims/relyingpartytrustid" },
    { "role", ClaimTypes.Role },
    { "roles", ClaimTypes.Role },
    { "upn", ClaimTypes.Upn },
    { "winaccountname", ClaimTypes.WindowsAccountName },
};
```

#### Disable JwtBearer/OpenID Connect Client Claim Type Mapping

To turn off the claim type mapping, you can set the `MapInboundClaims` property of `JwtBearerOptions` or `OpenIdConnectOptions` to `false`. Then, you can get the original claim types from the token(`access_token` or `id_token`):

JWT Example:

```json
{
  "iss": "https://localhost:44305/",
  "exp": 1714466127,
  "iat": 1714466127,
  "aud": "MyProjectName",
  "scope": "MyProjectName offline_access",
  "sub": "ed7f5cfd-7311-0402-245c-3a123ff787f9",
  "unique_name": "admin",
  "preferred_username": "admin",
  "given_name": "admin",
  "role": "admin",
  "email": "admin@abp.io",
  "email_verified": "False",
  "phone_number_verified": "False",
}
```

### OAuth2(Google, Facebook, Twitter, Microsoft) Extenal Login Client

The `OAuth2 handler` fetchs a JSON containing user information from the `OAuth2` server. The third-party provider issues the claim type based on their standard server and then maps/adds it to the current `ClaimsIdentity`. The ASP NET Core provides some built-in claim-type mappings for different providers as can be seen below examples:

**Example**: The `ClaimActions` property of the `GoogleOptions` maps the Google's claim types to [`System.Security.Claims.ClaimTypes`](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes):

```cs
ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id"); // v2
ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "sub"); // v3
ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
ClaimActions.MapJsonKey("urn:google:profile", "link");
ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
```

**Example**: The `ClaimActions` property of the `FacebookOptions` maps the Facebook's claim types to [`System.Security.Claims.ClaimTypes`](https://learn.microsoft.com/en-us/dotnet/api/system.security.claims.claimtypes):

```cs
ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
ClaimActions.MapJsonSubKey("urn:facebook:age_range_min", "age_range", "min");
ClaimActions.MapJsonSubKey("urn:facebook:age_range_max", "age_range", "max");
ClaimActions.MapJsonKey(ClaimTypes.DateOfBirth, "birthday");
ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
ClaimActions.MapJsonKey(ClaimTypes.Name, "name");
ClaimActions.MapJsonKey(ClaimTypes.GivenName, "first_name");
ClaimActions.MapJsonKey("urn:facebook:middle_name", "middle_name");
ClaimActions.MapJsonKey(ClaimTypes.Surname, "last_name");
ClaimActions.MapJsonKey(ClaimTypes.Gender, "gender");
ClaimActions.MapJsonKey("urn:facebook:link", "link");
ClaimActions.MapJsonSubKey("urn:facebook:location", "location", "name");
ClaimActions.MapJsonKey(ClaimTypes.Locality, "locale");
ClaimActions.MapJsonKey("urn:facebook:timezone", "timezone");
```

### OpenIddict AuthServer

The `OpenIddict` uses the [standard claims](https://openid.net/specs/openid-connect-core-1_0.html#StandardClaims) as the claim type of the `id_token` or `access_token` and `UserInfo` endpoint response, etc.

* For JWT token, it also uses the [azure-activedirectory-identitymodel-extensions-for-dotnet](https://github.com/AzureAD/azure-activedirectory-identitymodel-extensions-for-dotnet) to get the claims from the `id_token` or `access_token`.
* For reference token, it gets the claims from the `database`.

## Summary

Once you find the claims you received do not meet your expectations, follow the instructions above to troubleshoot the problem. 

This article can help you understand the claim type in the ABP Framework and ASP NET Core.

