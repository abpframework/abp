using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
namespace LocalizationKeySynchronizer;

public static partial class LocalizationHelper
{
    public static bool TryGetLocalization(string path, out AbpLocalizationInfo? localizationInfo)
    {
        if (File.Exists(path) == false)
        {
            localizationInfo = default;
            return false;
        }

        var json = File.ReadAllTextAsync(path).GetAwaiter().GetResult();
        return AbpLocalizationInfo.TryDeserialize(json, out localizationInfo);
    }

    public static List<AbpLocalization> GetLocalizations(IEnumerable<string> paths)
    {
        var results = new List<AbpLocalization>();
        foreach (var path in paths)
        {
            if (TryGetLocalization(path, out var localizationInfo))
            {
                results.Add(new AbpLocalization(path, localizationInfo!));
            }
        }

        return results;
    }

    private static Dictionary<string, int> GetKeysAndArgCount(this AbpLocalizationInfo localizationInfo)
    {
        return localizationInfo.Texts.ToDictionary(k => k.Key, v => GetArgCount(v.Value));
    }

    private static int GetArgCount(string value)
    {
        var matches = MyRegex().Matches(value);
        return matches.Count;
    }

    public static List<AbpAsyncLocalization> GetAsynchronousLocalizations(this AbpLocalization defaultLocalization,
        IEnumerable<AbpLocalization> otherLocalizations)
    {
        var results = new List<AbpAsyncLocalization>();
        var defaultCultureKeysAndArgCount = defaultLocalization.LocalizationInfo.GetKeysAndArgCount();
        foreach (var localization in otherLocalizations)
        {
            var keysAndArgCount = localization.LocalizationInfo.GetKeysAndArgCount();
            var asynchronousResource =
                new AbpAsyncLocalization(localization, defaultLocalization, new List<AbpAsyncKey>());
            foreach (var (key, defaultCultureArgCount) in defaultCultureKeysAndArgCount)
            {
                if (keysAndArgCount.TryGetValue(key, out var value))
                {
                    if (value != defaultCultureArgCount)
                    {
                        asynchronousResource.AsyncKeys.Add(new ArgumentCountMismatch(key,
                            defaultLocalization.LocalizationInfo.Texts[key], defaultCultureArgCount, value,
                            localization.LocalizationInfo.Texts[key]));
                    }
                }
                else
                {
                    asynchronousResource.AsyncKeys.Add(new MissingKey(key,
                        defaultLocalization.LocalizationInfo.Texts[key]));
                }
            }

            if (asynchronousResource.AsyncKeys.Any())
            {
                results.Add(asynchronousResource);
            }
        }

        return results;
    }

    public static void DeleteKeysThatDoNotMatchTheNumberOfArguments(
        IEnumerable<AbpAsyncLocalization> asynchronousResources)
    {
        foreach (var resource in asynchronousResources)
        {
            foreach (var key in resource.AsyncKeys.Select(x => x.Key))
            {
                resource.Localization.LocalizationInfo.Texts.Remove(key);
            }

            File.WriteAllTextAsync(resource.Localization.FilePath,
                JsonHelper.Serialize(resource.Localization.LocalizationInfo)).GetAwaiter().GetResult();
        }
    }

    public static void ExportKeysThatDoNotMatchTheNumberOfArguments(
        IEnumerable<AbpAsyncLocalization> asynchronousResources, string? exportPath)
    {
        Export<ArgumentCountMismatch>(asynchronousResources, exportPath);
    }

    public static void Export<T>(IEnumerable<AbpAsyncLocalization> asynchronousResources, string? exportPath)
        where T : AbpAsyncKey
    {
        var asyncLocalizationViewModels = asynchronousResources.Select(x =>
            new AbpAsyncLocalizationViewModel(x.Reference.LocalizationInfo.Culture,
                x.Localization.LocalizationInfo.Culture, x.Localization.FilePath,
                x.AsyncKeys.Where(k => k is T).ToList())).ToList();

        if (exportPath != null)
        {
            File.WriteAllTextAsync(exportPath,
                    JsonHelper.Serialize(asyncLocalizationViewModels))
                .GetAwaiter().GetResult();
        }
    }

    public static void ExportMissingKeys(IEnumerable<AbpAsyncLocalization> asyncLocalizations, string? exportPath)
    {
        Export<MissingKey>(asyncLocalizations, exportPath);
    }

    public static bool ApplyChanges(string path)
    {
        var json = File.ReadAllTextAsync(path).GetAwaiter().GetResult();

        if (JsonHelper.TryDeserialize(json, out List<AbpAsyncLocalizationViewModel>? asyncLocalizationViewModels) ==
            false)
        {
            return false;
        }

        foreach (var asyncLocalizationViewModel in asyncLocalizationViewModels!)
        {
            if (TryGetLocalization(asyncLocalizationViewModel.Path, out var localizationInfo) == false)
            {
                return false;
            }

            foreach (var asyncKey in asyncLocalizationViewModel.AsyncKeys.Where(asyncKey =>
                         !string.IsNullOrWhiteSpace(asyncKey.NewValue)))
            {
                localizationInfo!.Texts[asyncKey.Key] = asyncKey.NewValue;
            }

            File.WriteAllTextAsync(asyncLocalizationViewModel.Path,
                JsonHelper.Serialize(localizationInfo)).GetAwaiter().GetResult();
        }

        return true;
    }

    public static void ReplaceKey(string oldKey, string newKey, List<AbpLocalization> localizations)
    {
        foreach (var localization in localizations)
        {
            if (!localization.LocalizationInfo.Texts.TryGetValue(oldKey, out var value))
            {
                continue;
            }

            localization.LocalizationInfo.Texts.Remove(oldKey);
            localization.LocalizationInfo.Texts.Add(newKey, value);
            File.WriteAllTextAsync(localization.FilePath,
                JsonHelper.Serialize(localization.LocalizationInfo)).GetAwaiter().GetResult();
        }
    }

    [GeneratedRegex("{(\\d+)}")]
    private static partial Regex MyRegex();
}