# Encryption and Decryption in ABP Framework

The ABP Framework provides various implementations of encryption and decryption to protect sensitive data. Here are three main encryption scenarios and their implementations:

## User Passwords

ABP's Identity module uses HMAC-SHA512 combined with PBKDF2 algorithm for password hashing. The process is as follows:

- Encryption process:
  - System generates a random 128-bit salt
  - Combines password and salt, performs 100,000 iterations using HMAC-SHA512 and PBKDF2 algorithms
  - Stores the final hash value combined with the salt (Note: stored ciphertext cannot be reversed to plaintext)

- Verification process:
  - System extracts the stored salt
  - Recalculates the hash value of the provided password using the same algorithm and iterations
  - Compares the results; verification succeeds if matched, fails if not

## String Encryption

ABP's `IStringEncryptionService` uses AES algorithm (CBC mode) for string encryption and decryption. It mainly encrypts and decrypts strings like settings and configuration information. The process is as follows:

- Encryption process:
  - Derives encryption key from passphrase and salt using Rfc2898DeriveBytes (PBKDF2) algorithm
  - Encrypts using AES algorithm with 256-bit key (controlled by Options.Keysize)
  - Uses initialization vector (Options.InitVectorBytes) to ensure encryption security

- Decryption process:
  - Uses the same passphrase and salt
  - Goes through the same key derivation process
  - Restores the encrypted content to original text

> Note: If you modify any encryption parameters like passphrase, salt, key size, etc., ensure all applications using encryption use the same parameters, otherwise decryption will fail. For example, encrypted settings in the database will become undecryptable.

## OAuth2/AuthServer Signing and Encryption

ABP uses the OpenIddict library for OAuth2 authentication server implementation, which uses two types of credentials to protect generated tokens:

- Credential types:
  - Signing credentials: Prevent token tampering, can be asymmetric (like RSA or ECDSA keys) or symmetric
  - Encryption credentials: Ensure token content confidentiality, prevent unauthorized access and reading

- Environment configuration:
  - Development environment:
    - Automatically creates two separate RSA certificates
    - One for signing, another for encryption
  - Production environment:
    - ABP Studio generates a single RSA certificate (`openiddict.pfx`) when creating project
    - This certificate is used for both signing and encryption operations

- Custom options:
  - Can replace default certificate with self-generated RSA certificate
  - Supports symmetric encryption (like AES), but not recommended for production

## Data Protection

Besides the above encryption and decryption features, ASP.NET Core's built-in components and services may use data protection, such as encrypting private data in cookies or generating links for email confirmation or password recovery. For details, refer to [ASP.NET Core Data Protection](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/introduction)

## Summary

The ABP Framework protects data security through various encryption mechanisms: from HMAC-SHA512 hashing for user passwords, to AES encryption for configuration information, and RSA certificate signing and encryption in OAuth2 authentication, while also integrating ASP.NET Core's data protection features.

For production environments, it's recommended to use strong passphrases and custom salt values, prioritize asymmetric encryption algorithms, and ensure proper management and backup of all encryption credentials.

## References

- [Hash passwords in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/consumer-apis/password-hashing)
- [String Encryption](https://abp.io/docs/latest/framework/infrastructure/string-encryption)
- [Encryption and signing credentials](https://documentation.openiddict.com/configuration/encryption-and-signing-credentials)
- [ASP.NET Core Data Protection](https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/introduction)
