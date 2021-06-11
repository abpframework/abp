# String Encryption

ABP Framework provides string encryption feature that allows to **Encrypt** and **Decrypt** strings.

##  Installation

> This package is already installed by default with the startup template. So, most of the time, you don't need to install it manually.

If installation is needed, it is suggested to use the [ABP CLI](https://docs.abp.io/en/abp/latest/CLI) to install this package.

### Using the ABP CLI

Open a command line window in the folder of the project (.csproj file) and type the following command:

```bash
abp add-package Volo.Abp.Security
```

### Manual Installation

If you want to manually install;

1. Add the [Volo.Abp.Security](https://www.nuget.org/packages/Volo.Abp.Security) NuGet package to your project:

   `Install-Package Volo.Abp.Security`

2. Add the `AbpSecurityModule` to the dependency list of your module:

   ```csharp
   [DependsOn(
       //...other dependencies
       typeof(AbpSecurityModule) // <-- Add module dependency like that
       )]
   public class YourModule : AbpModule
   {
   }
   ```

## Using String Encryption

All encryption operations are included in `IStringEncryptionService`. You can inject it and start to use.

```csharp
 public class MyService : DomainService
 {
     protected IStringEncryptionService StringEncryptionService { get; }

     public MyService(IStringEncryptionService stringEncryptionService)
     {
         StringEncryptionService = stringEncryptionService;
     }

     public string Encrypt(string value)
     {
         // To enrcypt a value
         return StringEncryptionService.Encrypt(value);
     }

     public string Decrpyt(string value)
     {
         // To decrypt a value
         return StringEncryptionService.Decrypt(value);
     }
 }
```

### Using Custom PassPhrase

`IStringEncryptionService` methods has **passPharase** parameter with default value and it uses default PassPhrase when you don't pass passPhrase parameter. 

```csharp
// Default Pass Phrase
var encryptedValue = StringEncryptionService.Encrypt(value);

// Custom Pass Phrase
var encryptedValue = StringEncryptionService.Encrypt(value, "MyCustomPassPhrase");

// Encrypt & Decrypt have same parameters.
var decryptedValue = StringEncryptionService.Decrypt(value, "MyCustomPassPhrase");
```

### Using Custom Salt

`IStringEncryptionService` methods has **salt** parameter with default value and it uses default Salt when you don't pass the parameter.

```csharp
// Default Salt
var encryptedValue = StringEncryptionService.Encrypt(value);

// Custom Salt
var encryptedValue = StringEncryptionService.Encrypt(value, salt: Encoding.UTF8.GetBytes("MyCustomSalt")); 

// Encrypt & Decrypt have same parameters.
var decryptedValue = StringEncryptionService.Decrypt(value,  salt: Encoding.UTF8.GetBytes("MyCustomSalt"));
```

***

## String Encryption Options

Default values can be configured with `AbpStringEncryptionOptions` type.

```csharp
Configure<AbpStringEncryptionOptions>(opts =>
{
    opts.DefaultPassPhrase = "MyStrongPassPhrase";
    opts.DefaultSalt = Encoding.UTF8.GetBytes("MyStrongSalt");
    opts.InitVectorBytes = Encoding.UTF8.GetBytes("YetAnotherStrongSalt");
    opts.Keysize = 512;
});
```

- **DefaultPassPhrase**: Default password to encrypt/decrypt texts. It's recommended to set to another value for security. Default value: `gsKnGZ041HLL4IM8`

- **DefaultSalt**: A value which is used as salt while  encrypting/decrypting.

  Default value: `Encoding.ASCII.GetBytes("hgt!16kl")`

- **InitVectorBytes:** This constant string is used as a "salt" value for the PasswordDeriveBytes function calls. This size of the IV (in bytes) must = (keysize / 8). Default keysize is 256, so the IV must be 32 bytes long. Using a 16 character string here gives us 32 bytes when converted to a byte array. 

  Default value: `Encoding.ASCII.GetBytes("jkE49230Tf093b42")`

- **Keysize:** This constant is used to determine the keysize of the encryption algorithm.

  Default value: `256`