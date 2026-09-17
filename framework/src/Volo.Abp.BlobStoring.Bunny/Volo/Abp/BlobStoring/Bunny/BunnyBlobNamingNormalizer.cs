using System.Globalization;
using System.Text.RegularExpressions;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Localization;

namespace Volo.Abp.BlobStoring.Bunny;

public class BunnyBlobNamingNormalizer : IBlobNamingNormalizer, ITransientDependency
{
    private readonly static Regex ValidCharactersRegex =
        new Regex(@"^[a-z0-9-]*$", RegexOptions.Compiled);

    private const int MinLength = 4;
    private const int MaxLength = 64;

    public virtual string NormalizeBlobName(string blobName) => blobName;

    public virtual string NormalizeContainerName(string containerName)
    {
        Check.NotNullOrWhiteSpace(containerName, nameof(containerName));

        using (CultureHelper.Use(CultureInfo.InvariantCulture))
        {
            // Trim whitespace and convert to lowercase
            var normalizedName = containerName
                .Trim()
                .ToLowerInvariant();

            // Remove any invalid characters
            normalizedName = Regex.Replace(normalizedName, "[^a-z0-9-]", string.Empty);

            // Validate structure
            if (!ValidCharactersRegex.IsMatch(normalizedName))
            {
                throw new AbpException(
                    $"Container name contains invalid characters: {containerName}. " +
                    "Only lowercase letters, numbers, and hyphens are allowed.");
            }

            // Validate length
            if (normalizedName.Length < MinLength || normalizedName.Length > MaxLength)
            {
                throw new AbpException(
                    $"Container name must be between {MinLength} and {MaxLength} characters. " +
                    $"Current length: {normalizedName.Length}");
            }

            return normalizedName;
        }
    }
}
