using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Volo.Abp.Internal;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Localization.VirtualFiles
{
    public abstract class VirtualFileLocalizationResourceContributorBase : LocalizationResourceContributorBase
    {
        private readonly string _virtualPath;

        private bool _subscribedForChanges;

        protected VirtualFileLocalizationResourceContributorBase(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public override List<ILocalizationDictionary> GetDictionaries(LocalizationResourceInitializationContext context)
        {
            var virtualFileProvider = context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();

            if (!_subscribedForChanges)
            {
                ChangeToken.OnChange(() => virtualFileProvider.Watch(_virtualPath.EnsureEndsWith('/') + "*.*"), () =>
                {
                    OnUpdated();
                });

                _subscribedForChanges = true;
            }

            return CreateDictionaries(virtualFileProvider);
        }

        private List<ILocalizationDictionary> CreateDictionaries(IFileProvider fileProvider)
        {
            var dictionaries = new Dictionary<string, ILocalizationDictionary>();

            foreach (var file in fileProvider.GetDirectoryContents(_virtualPath))
            {
                if (file.IsDirectory || !CanParseFile(file))
                {
                    continue;
                }

                var dictionary = CreateDictionaryFromFile(file);
                if (dictionaries.ContainsKey(dictionary.CultureName))
                {
                    throw new AbpException($"{file.PhysicalPath} dictionary has a culture name '{dictionary.CultureName}' which is already defined!");
                }
                
                dictionaries[dictionary.CultureName] = dictionary;
            }

            return dictionaries.Values.ToList();
        }

        protected abstract bool CanParseFile(IFileInfo file);

        protected virtual ILocalizationDictionary CreateDictionaryFromFile(IFileInfo file)
        {
            using (var stream = file.CreateReadStream())
            {
                return CreateDictionaryFromFileContent(Utf8Helper.ReadStringFromStream(stream));
            }
        }

        protected abstract ILocalizationDictionary CreateDictionaryFromFileContent(string fileContent);
    }
}