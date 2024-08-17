using System;
using System.Collections.Generic;
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
    private LocalizationResourceBase _resource = default!;

    protected VirtualFileLocalizationResourceContributorBase(string virtualPath)
    {
        _virtualPath = virtualPath;
    }

    public virtual void Initialize(LocalizationResourceInitializationContext context)
    {
        _resource = context.Resource;
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
        var dictionaries = new Dictionary<string, ILocalizationDictionary>();

        foreach (var file in _virtualFileProvider.GetDirectoryContents(_virtualPath))
        {
            if (file.IsDirectory || !CanParseFile(file))
            {
                continue;
            }

            var dictionary = CreateDictionaryFromFile(file);

            if (dictionary == null)
            {
                continue;
            }
            
            if (dictionaries.ContainsKey(dictionary.CultureName))
            {
                throw new AbpException($"{file.GetVirtualOrPhysicalPathOrNull()} dictionary has a culture name '{dictionary.CultureName}' which is already defined! Localization resource: {_resource.ResourceName}");
            }

            dictionaries[dictionary.CultureName] = dictionary;
        }

        return dictionaries;
    }

    protected abstract bool CanParseFile(IFileInfo file);

    protected virtual ILocalizationDictionary? CreateDictionaryFromFile(IFileInfo file)
    {
        using (var stream = file.CreateReadStream())
        {
            return CreateDictionaryFromFileContent(Utf8Helper.ReadStringFromStream(stream));
        }
    }

    protected abstract ILocalizationDictionary? CreateDictionaryFromFileContent(string fileContent);
}
