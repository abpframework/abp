using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands;

public class GenerateJwksCommand : IConsoleCommand, ITransientDependency
{
    public const string Name = "generate-jwks";

    public ILogger<GenerateJwksCommand> Logger { get; set; }

    public GenerateJwksCommand()
    {
        Logger = NullLogger<GenerateJwksCommand>.Instance;
    }

    public Task ExecuteAsync(CommandLineArgs commandLineArgs)
    {
        var outputDir = commandLineArgs.Options.GetOrNull("output", "o")
                        ?? Directory.GetCurrentDirectory();
        var keySizeStr = commandLineArgs.Options.GetOrNull("key-size", "s") ?? "2048";
        var alg = commandLineArgs.Options.GetOrNull("alg") ?? "RS256";
        var kid = commandLineArgs.Options.GetOrNull("kid") ?? Guid.NewGuid().ToString("N");
        var filePrefix = commandLineArgs.Options.GetOrNull("file", "f") ?? "jwks";

        if (!int.TryParse(keySizeStr, out var keySize) || (keySize != 2048 && keySize != 4096))
        {
            Logger.LogError("Invalid key size '{0}'. Supported values: 2048, 4096.", keySizeStr);
            return Task.CompletedTask;
        }

        if (!IsValidAlgorithm(alg))
        {
            Logger.LogError("Invalid algorithm '{0}'. Supported values: RS256, RS384, RS512, PS256, PS384, PS512.", alg);
            return Task.CompletedTask;
        }

        if (!Directory.Exists(outputDir))
        {
            Directory.CreateDirectory(outputDir);
        }

        Logger.LogInformation("Generating RSA {0}-bit key pair (algorithm: {1})...", keySize, alg);

        using var rsa = RSA.Create();
        rsa.KeySize = keySize;

        var jwksJson = BuildJwksJson(rsa, alg, kid);
        var privateKeyPem = ExportPrivateKeyPem(rsa);

        var jwksFilePath = Path.Combine(outputDir, $"{filePrefix}.json");
        var privateKeyFilePath = Path.Combine(outputDir, $"{filePrefix}-private.pem");

        File.WriteAllText(jwksFilePath, jwksJson, Encoding.UTF8);
        File.WriteAllText(privateKeyFilePath, privateKeyPem, Encoding.UTF8);

        Logger.LogInformation("");
        Logger.LogInformation("Generated files:");
        Logger.LogInformation("  JWKS (public key)  : {0}", jwksFilePath);
        Logger.LogInformation("  Private key (PEM)  : {0}", privateKeyFilePath);
        Logger.LogInformation("");
        Logger.LogInformation("JWKS content (paste this into the ABP OpenIddict application's 'JSON Web Key Set' field):");
        Logger.LogInformation("");
        Logger.LogInformation("{0}", jwksJson);
        Logger.LogInformation("");
        Logger.LogInformation("IMPORTANT: Keep the private key file safe. Never share it or commit it to source control.");
        Logger.LogInformation("           The JWKS file contains only the public key and is safe to share.");

        return Task.CompletedTask;
    }

    private static string BuildJwksJson(RSA rsa, string alg, string kid)
    {
        var parameters = rsa.ExportParameters(false);

        var n = Base64UrlEncode(parameters.Modulus);
        var e = Base64UrlEncode(parameters.Exponent);

        using var stream = new System.IO.MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });

        writer.WriteStartObject();
        writer.WriteStartArray("keys");
        writer.WriteStartObject();
        writer.WriteString("kty", "RSA");
        writer.WriteString("use", "sig");
        writer.WriteString("kid", kid);
        writer.WriteString("alg", alg);
        writer.WriteString("n", n);
        writer.WriteString("e", e);
        writer.WriteEndObject();
        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Flush();

        return Encoding.UTF8.GetString(stream.ToArray());
    }

    private static string ExportPrivateKeyPem(RSA rsa)
    {
#if NET5_0_OR_GREATER
        return rsa.ExportPkcs8PrivateKeyPem();
#elif NETSTANDARD2_0
        // RSA.ExportPkcs8PrivateKey() was introduced in .NET Standard 2.1.
        // The ABP CLI always runs on .NET 5+, so this path is never reached at runtime.
        throw new PlatformNotSupportedException("Private key export requires .NET Standard 2.1 or later.");
#else
        var privateKeyBytes = rsa.ExportPkcs8PrivateKey();
        var base64 = Convert.ToBase64String(privateKeyBytes, Base64FormattingOptions.InsertLineBreaks);
        return $"-----BEGIN PRIVATE KEY-----\n{base64}\n-----END PRIVATE KEY-----";
#endif
    }

    private static string Base64UrlEncode(byte[] input)
    {
        return Convert.ToBase64String(input)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }

    private static bool IsValidAlgorithm(string alg)
    {
        return alg == "RS256" || alg == "RS384" || alg == "RS512" ||
               alg == "PS256" || alg == "PS384" || alg == "PS512";
    }

    public string GetUsageInfo()
    {
        var sb = new StringBuilder();

        sb.AppendLine("");
        sb.AppendLine("Usage:");
        sb.AppendLine("  abp generate-jwks [options]");
        sb.AppendLine("");
        sb.AppendLine("Options:");
        sb.AppendLine("  -o|--output <dir>         Output directory (default: current directory)");
        sb.AppendLine("  -s|--key-size <size>      RSA key size: 2048 or 4096 (default: 2048)");
        sb.AppendLine("  --alg <alg>               Algorithm: RS256, RS384, RS512, PS256, PS384, PS512 (default: RS256)");
        sb.AppendLine("  --kid <id>                Key ID (kid) - auto-generated if not specified");
        sb.AppendLine("  -f|--file <prefix>        Output file name prefix (default: jwks)");
        sb.AppendLine("                            Generates: <prefix>.json (JWKS) and <prefix>-private.pem (private key)");
        sb.AppendLine("");
        sb.AppendLine("Examples:");
        sb.AppendLine("  abp generate-jwks");
        sb.AppendLine("  abp generate-jwks --alg RS512 --key-size 4096");
        sb.AppendLine("  abp generate-jwks -o ./keys -f myapp");
        sb.AppendLine("");
        sb.AppendLine("Description:");
        sb.AppendLine("  Generates an RSA key pair for use with OpenIddict private_key_jwt client authentication.");
        sb.AppendLine("  The JWKS file (public key) should be pasted into the ABP OpenIddict application's");
        sb.AppendLine("  'JSON Web Key Set' field in the management UI.");
        sb.AppendLine("  The private key PEM file should be kept secure and used by the client application");
        sb.AppendLine("  to sign JWT assertions when authenticating to the token endpoint.");
        sb.AppendLine("");
        sb.AppendLine("See the documentation for more info: https://abp.io/docs/latest/cli");

        return sb.ToString();
    }

    public static string GetShortDescription()
    {
        return "Generates an RSA key pair (JWKS + private key) for OpenIddict private_key_jwt authentication.";
    }
}
