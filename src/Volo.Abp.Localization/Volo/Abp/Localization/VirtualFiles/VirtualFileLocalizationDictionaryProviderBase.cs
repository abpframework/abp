using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Volo.Abp.Internal;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Localization.VirtualFiles
{
    public abstract class VirtualFileLocalizationDictionaryProviderBase : LocalizationDictionaryProviderBase
    {
        private readonly string _virtualPath;

        private bool _subscribedForChanges;

        protected VirtualFileLocalizationDictionaryProviderBase(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public override void Initialize(LocalizationResourceInitializationContext context) //TODO: Extract initialization to a factory..?
        {
            var virtualFileProvider = context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();

            CreateDictionaries(virtualFileProvider);

            if (!_subscribedForChanges)
            {
                ChangeToken.OnChange(() => virtualFileProvider.Watch(_virtualPath.EnsureEndsWith('/') + "**/*.*"), () =>
                {
                    CreateDictionaries(virtualFileProvider);
                    OnUpdated();
                });

                _subscribedForChanges = true;
            }
        }

        private void CreateDictionaries(IFileProvider fileProvider)
        {
            Dictionaries.Clear();

            foreach (var file in fileProvider.GetDirectoryContents(_virtualPath))
            {
                if (file.IsDirectory || !CanParseFile(file))
                {
                    continue;
                }

                var dictionary = CreateDictionaryFromFile(file);
                if (Dictionaries.ContainsKey(dictionary.CultureName))
                {
                    throw new AbpException($"{file.PhysicalPath} dictionary has a culture name '{dictionary.CultureName}' which is already defined!");
                }
                
                Dictionaries[dictionary.CultureName] = dictionary;
            }
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