using System;
using System.Text;
using System.Text.Json;
using JetBrains.Annotations;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp;

namespace Microsoft.Extensions.DependencyInjection;

public static class AbpSwaggerUIOptionsExtensions
{
    /// <summary>
    /// Sets the abp.appPath used by the Swagger UI scripts.
    /// </summary>
    /// <param name="options">The Swagger UI options.</param>
    /// <param name="appPath">The application base path.</param>
    public static void AbpAppPath([NotNull] this SwaggerUIOptions options, [NotNull] string appPath)
    {
        Check.NotNull(options, nameof(options));
        Check.NotNull(appPath, nameof(appPath));

        var normalizedAppPath = NormalizeAppPath(appPath);
        options.HeadContent = BuildAppPathScript(normalizedAppPath, options.HeadContent ?? string.Empty);
    }

    private static string NormalizeAppPath(string appPath)
    {
        return string.IsNullOrWhiteSpace(appPath)
            ? "/"
            : appPath.Trim().EnsureStartsWith('/').EnsureEndsWith('/');
    }

    private static string BuildAppPathScript(string normalizedAppPath, string headContent)
    {
        var builder = new StringBuilder(headContent);
        if (builder.Length > 0)
        {
            builder.AppendLine();
        }

        builder.AppendLine("<script>");
        builder.AppendLine("    var abp = abp || {};");
        builder.AppendLine($"    abp.appPath = {JsonSerializer.Serialize(normalizedAppPath)};");
        builder.AppendLine("</script>");
        return builder.ToString();
    }
}
