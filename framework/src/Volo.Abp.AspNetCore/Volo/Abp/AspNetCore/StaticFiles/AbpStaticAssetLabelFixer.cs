using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.StaticAssets;

namespace Volo.Abp.AspNetCore.StaticFiles;

/// <summary>
/// The 11.0.1xx SDKs leave the package base path out of the labels of compressed package assets, so two packages with
/// the same asset path produce duplicate labels, and Razor Pages and Razor Components fail on startup when they read
/// the asset collection. This adds the missing base path to the duplicate labels before the endpoints are built.
/// Remove it once the SDK fixes the labels.
/// </summary>
internal static class AbpStaticAssetLabelFixer
{
    private const string LabelPropertyName = "label";

    public static void FixDuplicateLabels(IReadOnlyList<StaticAssetDescriptor> descriptors)
    {
        // A fixed label can collide with another label that was unique, so repeat until nothing changes.
        // Every change adds a missing base path, so a label can only grow up to its full path and the loop ends.
        while (FixDuplicateLabelsOnce(descriptors))
        {
        }

        // The route of an endpoint is unique, so it is used for the labels whose base path couldn't be found.
        foreach (var (descriptor, _) in GetDuplicates(descriptors))
        {
            SetLabel(descriptor, descriptor.Route);
        }
    }

    private static bool FixDuplicateLabelsOnce(IReadOnlyList<StaticAssetDescriptor> descriptors)
    {
        var duplicates = GetDuplicates(descriptors);
        if (duplicates.Count == 0)
        {
            return false;
        }

        var labelsByAssetPath = descriptors
            .Where(descriptor => descriptor.Selectors.Count > 0)
            .Select(descriptor => (descriptor.AssetPath, Label: GetLabel(descriptor)))
            .Where(x => x.Label != null)
            .GroupBy(x => x.AssetPath, StringComparer.Ordinal)
            .ToDictionary(group => group.Key, group => group.Select(x => x.Label!).ToList(), StringComparer.Ordinal);

        var changed = false;
        foreach (var (descriptor, label) in duplicates)
        {
            var basePath = GetBasePathFromUncompressedLabel(descriptor, label, labelsByAssetPath);
            if (basePath.IsNullOrEmpty())
            {
                basePath = GetBasePathFromRoute(descriptor.Route, label);
            }

            if (basePath.IsNullOrEmpty())
            {
                continue;
            }

            SetLabel(descriptor, basePath + label);
            changed = true;
        }

        return changed;
    }

    private static List<(StaticAssetDescriptor Descriptor, string Label)> GetDuplicates(IReadOnlyList<StaticAssetDescriptor> descriptors)
    {
        return descriptors
            .Where(descriptor => descriptor.Selectors.Count == 0)
            .Select(descriptor => (Descriptor: descriptor, Label: GetLabel(descriptor)))
            .Where(x => x.Label != null)
            .GroupBy(x => x.Label!, StringComparer.OrdinalIgnoreCase)
            .Where(group => group.Count() > 1)
            .SelectMany(group => group)
            .Select(x => (x.Descriptor, x.Label!))
            .ToList();
    }

    /// <summary>
    /// The endpoint that serves the same compressed file through content negotiation has the full label of the
    /// uncompressed asset, such as "_content/Package/scripts/app.js" for "scripts/app.js.gz".
    /// </summary>
    private static string GetBasePathFromUncompressedLabel(
        StaticAssetDescriptor descriptor,
        string label,
        Dictionary<string, List<string>> labelsByAssetPath)
    {
        var extensionIndex = label.LastIndexOf('.');
        if (extensionIndex <= 0 || !labelsByAssetPath.TryGetValue(descriptor.AssetPath, out var uncompressedLabels))
        {
            return string.Empty;
        }

        var uncompressedLabel = label.Substring(0, extensionIndex);
        foreach (var fullLabel in uncompressedLabels)
        {
            if (fullLabel.Length > uncompressedLabel.Length &&
                fullLabel.EndsWith("/" + uncompressedLabel, StringComparison.OrdinalIgnoreCase))
            {
                return fullLabel.Substring(0, fullLabel.Length - uncompressedLabel.Length);
            }
        }

        return string.Empty;
    }

    private static string GetBasePathFromRoute(string route, string label)
    {
        var routeDirectory = GetDirectory(route);
        var labelDirectory = GetDirectory(label);
        if (!routeDirectory.EndsWith(labelDirectory, StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }

        return routeDirectory.Substring(0, routeDirectory.Length - labelDirectory.Length);
    }

    private static string GetDirectory(string path)
    {
        var index = path.LastIndexOf('/');
        return index < 0 ? string.Empty : path.Substring(0, index + 1);
    }

    private static string? GetLabel(StaticAssetDescriptor descriptor)
    {
        return descriptor.Properties.FirstOrDefault(IsLabel)?.Value;
    }

    private static void SetLabel(StaticAssetDescriptor descriptor, string label)
    {
        descriptor.Properties = descriptor.Properties
            .Select(property => IsLabel(property) ? new StaticAssetProperty(property.Name, label) : property)
            .ToList();
    }

    private static bool IsLabel(StaticAssetProperty property)
    {
        return property.Name.Equals(LabelPropertyName, StringComparison.OrdinalIgnoreCase);
    }
}
