using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Primitives;
using Volo.Abp.Internal;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Localization.VirtualFiles;

public abstract class VirtualFileLocalizationResourceContributorBase : ILocalizationResourceContributor
{
    public bool IsDynamic => false;

    private readonly string _virtualPath;
    private IServiceProvider _serviceProvider = default!;
    private IVirtualFileProvider? _virtualFileProvider;
    private volatile LocalizationFiles? _files;
    private bool _subscribedForChanges;
    private readonly object _syncObj = new object();

    protected VirtualFileLocalizationResourceContributorBase(string virtualPath)
    {
        _virtualPath = virtualPath;
    }

    public virtual void Initialize(LocalizationResourceInitializationContext context)
    {
        // The provider is resolved on first use, outside any lock: Initialize runs while the localizer factory holds
        // its lock, and resolving a singleton under a lock deadlocks with a thread that creates that singleton and
        // reads localized texts.
        _serviceProvider = context.ServiceProvider;
    }

    private IVirtualFileProvider VirtualFileProvider =>
        _virtualFileProvider ??= _serviceProvider.GetRequiredService<IVirtualFileProvider>();

    public virtual LocalizedString? GetOrNull(string cultureName, string name)
    {
        return GetDictionaryOrNull(cultureName)?.GetOrNull(name);
    }

    public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        GetDictionaryOrNull(cultureName)?.Fill(dictionary);
    }

    public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        Fill(cultureName, dictionary);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        return Task.FromResult((IEnumerable<string>)GetFiles().CultureNames);
    }

    private ILocalizationDictionary? GetDictionaryOrNull(string cultureName)
    {
        var files = GetFiles();
        if (files.Dictionaries.TryGetValue(cultureName, out var dictionary))
        {
            return dictionary;
        }

        if (files.FilesByCulture == null || !files.FilesByCulture.TryGetValue(cultureName, out var cultureFiles))
        {
            return null;
        }

        lock (_syncObj)
        {
            if (files.Dictionaries.TryGetValue(cultureName, out dictionary))
            {
                return dictionary;
            }

            dictionary = CreateDictionaryOrNull(cultureName, cultureFiles);
            if (dictionary != null)
            {
                files.Dictionaries[cultureName] = dictionary;
                return dictionary;
            }

            // A file did not contain the culture it was indexed with, so the index can not be trusted.
            var allDictionaries = CreateAllDictionaries(files.AllFiles);
            if (ReferenceEquals(_files, files))
            {
                _files = allDictionaries;
            }

            return allDictionaries.Dictionaries.GetOrDefault(cultureName);
        }
    }

    private LocalizationFiles GetFiles()
    {
        var files = _files;
        if (files != null)
        {
            return files;
        }

        var virtualFileProvider = VirtualFileProvider;

        lock (_syncObj)
        {
            files = _files;
            if (files != null)
            {
                return files;
            }

            if (!_subscribedForChanges)
            {
                ChangeToken.OnChange(() => virtualFileProvider.Watch(_virtualPath.EnsureEndsWith('/') + "*.*"),
                    () =>
                    {
                        _files = null;
                    });

                _subscribedForChanges = true;
            }

            _files = files = CreateFiles(virtualFileProvider);
        }

        return files;
    }

    private LocalizationFiles CreateFiles(IVirtualFileProvider virtualFileProvider)
    {
        var allFiles = virtualFileProvider.GetDirectoryContents(_virtualPath)
            .Where(f => !f.IsDirectory && CanParseFile(f))
            .OrderBy(f => f.Name, StringComparer.Ordinal)
            .ToList();

        var filesByCulture = new Dictionary<string, List<IFileInfo>>();

        foreach (var file in allFiles)
        {
            var cultureName = GetCultureNameOrNull(file);
            if (cultureName.IsNullOrEmpty())
            {
                return CreateAllDictionaries(allFiles);
            }

            if (!filesByCulture.TryGetValue(cultureName!, out var cultureFiles))
            {
                cultureFiles = new List<IFileInfo>();
                filesByCulture[cultureName!] = cultureFiles;
            }

            cultureFiles.Add(file);
        }

        return new LocalizationFiles(allFiles, filesByCulture, filesByCulture.Keys.ToList().AsReadOnly());
    }

    private ILocalizationDictionary? CreateDictionaryOrNull(string cultureName, List<IFileInfo> cultureFiles)
    {
        var rawDictionary = new Dictionary<string, LocalizedString>();

        foreach (var file in cultureFiles)
        {
            var dictionary = CreateDictionaryFromFile(file);
            if (dictionary == null || dictionary.CultureName != cultureName)
            {
                return null;
            }

            dictionary.Fill(rawDictionary);
        }

        return new StaticLocalizationDictionary(cultureName, rawDictionary);
    }

    private LocalizationFiles CreateAllDictionaries(List<IFileInfo> allFiles)
    {
        var rawDictionaries = new Dictionary<string, Dictionary<string, LocalizedString>>();

        foreach (var file in allFiles)
        {
            var dictionary = CreateDictionaryFromFile(file);

            if (dictionary == null)
            {
                continue;
            }

            if (!rawDictionaries.TryGetValue(dictionary.CultureName, out var raw))
            {
                raw = new Dictionary<string, LocalizedString>();
                rawDictionaries[dictionary.CultureName] = raw;
            }

            dictionary.Fill(raw);
        }

        var dictionaries = new ConcurrentDictionary<string, ILocalizationDictionary>();
        foreach (var raw in rawDictionaries)
        {
            dictionaries[raw.Key] = new StaticLocalizationDictionary(raw.Key, raw.Value);
        }

        return new LocalizationFiles(allFiles, null, rawDictionaries.Keys.ToList().AsReadOnly(), dictionaries);
    }

    protected abstract bool CanParseFile(IFileInfo file);

    /// <summary>
    /// Returns the culture of the given file without building its texts, so only the files of the requested cultures
    /// need to be parsed. Returns null if it can not be determined; then all files are parsed at once.
    /// </summary>
    protected virtual string? GetCultureNameOrNull(IFileInfo file)
    {
        return null;
    }

    protected virtual ILocalizationDictionary? CreateDictionaryFromFile(IFileInfo file)
    {
        using (var stream = file.CreateReadStream())
        {
            try
            {
                return CreateDictionaryFromFileContent(Utf8Helper.ReadStringFromStream(stream));
            }
            catch (Exception e)
            {
                throw new AbpException("Invalid localization file format: " + (file.GetVirtualOrPhysicalPathOrNull() ?? file.Name), e);
            }
        }
    }

    protected abstract ILocalizationDictionary? CreateDictionaryFromFileContent(string fileContent);

    private class LocalizationFiles
    {
        public List<IFileInfo> AllFiles { get; }

        /// <summary>
        /// Null when all dictionaries are created at once.
        /// </summary>
        public Dictionary<string, List<IFileInfo>>? FilesByCulture { get; }

        public IReadOnlyList<string> CultureNames { get; }

        public ConcurrentDictionary<string, ILocalizationDictionary> Dictionaries { get; }

        public LocalizationFiles(
            List<IFileInfo> allFiles,
            Dictionary<string, List<IFileInfo>>? filesByCulture,
            IReadOnlyList<string> cultureNames,
            ConcurrentDictionary<string, ILocalizationDictionary>? dictionaries = null)
        {
            AllFiles = allFiles;
            FilesByCulture = filesByCulture;
            CultureNames = cultureNames;
            Dictionaries = dictionaries ?? new ConcurrentDictionary<string, ILocalizationDictionary>();
        }
    }
}
