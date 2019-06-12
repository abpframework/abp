# Volo.Abp.Ldap

# Only Authenticate(not read/write AD)

## Configure

add section in `appsettings.json`

### use SSL

```json
"LDAP": {
    "ServerHost": "192.168.101.54", 
    "ServerPort": 636,
    "UseSsl": true
}
```

### not use SSL

```json
"LDAP": {
    "ServerHost": "192.168.101.54", 
    "ServerPort": 389,
    "UseSsl": false
}
```

## Authenticate

 Injecting `ILdapManager` into a class. For example:

```csharp
public class TaxAppService : ApplicationService
{
    private readonly ILdapManager _ldapManager;

    public TaxAppService(ILdapManager ldapManager)
    {
        _ldapManager = ldapManager;
    }

    public void Authenticate(string userName, string password)
    { 
        var result = _ldapManager.Authenticate(userName, password);
    }
}
```

- `userName` must be full domain name. E.g abc@abc.com 

# Read/Write AD

## Configure

### use SSL

```json
"LDAP": {
    "ServerHost": "192.168.101.54",
    "ServerPort": 636,
    "UseSsl": true,
    "Credentials": {
        "DomainUserName": "administrator@yourdomain.com.cn",
        "Password": "yH.20190528"
    },
    "SearchBase": "DC=yourdomain,DC=com,DC=cn",
    "DomainName": "yourdomain.com.cn",
    "DomainDistinguishedName": "DC=yourdomain,DC=com,DC=cn"
}
```

### not use SSL

```json
"LDAP": {
    "ServerHost": "192.168.101.54",
    "ServerPort": 389,
    "UseSsl": false,
    "Credentials": {
        "DomainUserName": "administrator@yourdomain.com.cn",
        "Password": "yH.20190528"
    },
    "SearchBase": "DC=yourdomain,DC=com,DC=cn",
    "DomainName": "yourdomain.com.cn",
    "DomainDistinguishedName": "DC=yourdomain,DC=com,DC=cn"
}
```

- `Credentials:DomainUserName` a administrator of AD.

- `Credentials:Password` the password for the administrator.
- `SearchBase`:  where search from AD.
- `DomainName`: name of you domain. no need `www`.
- `DomainDistinguishedName`: distinguished name of root domain.

## Query Organizations

```cs
// query all organizations
// filter: (&(objectClass=organizationalUnit)) 
_ldapManager.GetOrganizations();

// query organizations by name
// filter: (&(name=abc)(objectClass=organizationalUnit))
_ldapManager.GetOrganizations("abc");

```

## Query Organization

```csharp
// query organization by distinguished name
// filter: (&(distinguishedName=abc)(objectClass=organizationalUnit))
_ldapManager.GetOrganization("abc");

```

## Add Organization

```csharp
// use LdapOrganization
_ldapManager.AddSubOrganization("nameA", parentOrganization);

// or use OrganizationDistinguishedName
_ldapManager.AddSubOrganization("nameA", "OU=Domain Controllers,DC=yourdomain,DC=com,DC=cn");
```

## Query Users

```cs
// query all users
// filter: (&(objectCategory=person)(objectClass=user))
_ldapManager.GetUsers();

// query organizations by name
// filter: (&(name=abc)(objectCategory=person)(objectClass=user))
_ldapManager.GetUsers(name : "abc");

// query organizations by displayName
// filter: (&(displayName=abc)(objectCategory=person)(objectClass=user))
_ldapManager.GetUsers(displayName : "abc");

// query organization by commonName
// filter: (&(cn=abc)(objectCategory=person)(objectClass=user))
_ldapManager.GetUsers(commonName : "abc");

```

## Query User

```csharp
// query a user by distinguished name
// filter: (&(distinguishedName=abc)(objectCategory=person)(objectClass=user))
_ldapManager.GetUser("abc");

```

## Add User

```csharp
// use LdapOrganization
_ldapManager.AddUserToOrganization("nameA", "passwordA", parentOrganization);

// or use OrganizationDistinguishedName
_ldapManager.AddUserToOrganization("nameA", "passwordA", "OU=Domain Controllers,DC=yourdomain,DC=com,DC=cn");
```

# More

See [unit test](../../test/Volo.Abp.Ldap.Tests)