using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Components.Web.Theming.Bundling;

public class ComponentBundleUrlBuilder : IComponentBundleUrlBuilder, ITransientDependency
{
    public virtual Task<string> BuildAsync(
        [NotNull] string fileName,
        [CanBeNull] string? appBasePath,
        [CanBeNull] string? navigationBaseUri)
    {
        Check.NotNull(fileName, nameof(fileName));

        if (IsExternalUrl(fileName))
        {
            return Task.FromResult(fileName);
        }

        var pathBase = !string.IsNullOrWhiteSpace(appBasePath)
            ? appBasePath
            : ExtractPathBaseFromNavigationBaseUri(navigationBaseUri);

        if (string.IsNullOrWhiteSpace(pathBase))
        {
            return Task.FromResult(fileName);
        }

        var normalized = pathBase.EnsureEndsWith('/');
        if (normalized == "/")
        {
            return Task.FromResult(fileName);
        }

        if (fileName.StartsWith(normalized, StringComparison.Ordinal))
        {
            return Task.FromResult(fileName);
        }

        return Task.FromResult(normalized + fileName.RemovePreFix("/"));
    }

    protected virtual bool IsExternalUrl([NotNull] string fileName)
    {
        return fileName.StartsWith("//", StringComparison.Ordinal) ||
               (fileName.Contains(':') && Uri.TryCreate(fileName, UriKind.Absolute, out _));
    }

    protected virtual string? ExtractPathBaseFromNavigationBaseUri([CanBeNull] string? navigationBaseUri)
    {
        if (string.IsNullOrWhiteSpace(navigationBaseUri))
        {
            return null;
        }

        return Uri.TryCreate(navigationBaseUri, UriKind.Absolute, out var uri)
            ? uri.AbsolutePath
            : null;
    }
}
