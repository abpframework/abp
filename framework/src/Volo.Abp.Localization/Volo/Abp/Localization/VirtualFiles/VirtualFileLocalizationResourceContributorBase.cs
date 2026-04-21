using System;
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
    private IVirtualFileProvider _virtualFileProvider = default!;
    private Dictionary<string, ILocalizationDictionary>? _dictionaries;
    private bool _subscribedForChanges;
    private readonly object _syncObj = new object();

    protected VirtualFileLocalizationResourceContributorBase(string virtualPath)
    {
        _virtualPath = virtualPath;
    }

    public virtual void Initialize(LocalizationResourceInitializationContext context)
    {
        _virtualFileProvider = context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();
    }

    public virtual LocalizedString? GetOrNull(string cultureName, string name)
    {
        return GetDictionaries().GetOrDefault(cultureName)?.GetOrNull(name);
    }

    public virtual void Fill(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        GetDictionaries().GetOrDefault(cultureName)?.Fill(dictionary);
    }

    public Task FillAsync(string cultureName, Dictionary<string, LocalizedString> dictionary)
    {
        Fill(cultureName, dictionary);
        return Task.CompletedTask;
    }

    public Task<IEnumerable<string>> GetSupportedCulturesAsync()
    {
        return Task.FromResult((IEnumerable<string>)GetDictionaries().Keys);
    }

    private Dictionary<string, ILocalizationDictionary> GetDictionaries()
    {
        var dictionaries = _dictionaries;
        if (dictionaries != null)
        {
            return dictionaries;
        }

        lock (_syncObj)
        {
            dictionaries = _dictionaries;
            if (dictionaries != null)
            {
                return dictionaries;
            }

            if (!_subscribedForChanges)
            {
                ChangeToken.OnChange(() => _virtualFileProvider.Watch(_virtualPath.EnsureEndsWith('/') + "*.*"),
                    () =>
                    {
                        _dictionaries = null;
                    });

                _subscribedForChanges = true;
            }

            dictionaries = _dictionaries = CreateDictionaries();
        }

        return dictionaries;
    }

    private Dictionary<string, ILocalizationDictionary> CreateDictionaries()
    {
        var rawDictionaries = new Dictionary<string, Dictionary<string, LocalizedString>>();

        foreach (var file in _virtualFileProvider.GetDirectoryContents(_virtualPath)
                     .Where(f => !f.IsDirectory && CanParseFile(f))
                     .OrderBy(f => f.Name, StringComparer.Ordinal))
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

        return rawDictionaries.ToDictionary(
            kvp => kvp.Key,
            kvp => (ILocalizationDictionary)new StaticLocalizationDictionary(kvp.Key, kvp.Value)
        );
    }

    protected abstract bool CanParseFile(IFileInfo file);

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
}
