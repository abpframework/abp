using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Volo.Abp.Internal;
using Volo.Abp.VirtualFileSystem;

namespace Volo.Abp.Localization.VirtualFiles
{
    public abstract class VirtualFileLocalizationDictionaryProviderBase : LocalizationDictionaryProviderBase
    {
        private readonly string _virtualPath;

        protected VirtualFileLocalizationDictionaryProviderBase(string virtualPath)
        {
            _virtualPath = virtualPath;
        }

        public override void Initialize(LocalizationResourceInitializationContext context) //TODO: Extract initialization to a factory..?
        {
            var virtualFileProvider = context.ServiceProvider.GetRequiredService<IVirtualFileProvider>();
            var directoryContents = virtualFileProvider.GetDirectoryContents(_virtualPath);

            foreach (var file in directoryContents)
            {
                if (file.IsDirectory || !CanParseFile(file))
                {
                    continue;
                }

                using (var stream = file.CreateReadStream())
                {
                    var fileContent = Utf8Helper.ReadStringFromStream(stream);

                    var dictionary = CreateDictionary(fileContent);
                    if (Dictionaries.ContainsKey(dictionary.CultureName))
                    {
                        throw new AbpException($"{file.PhysicalPath} dictionary has a culture name '{dictionary.CultureName}' which is already defined!");
                    }

                    Dictionaries[dictionary.CultureName] = dictionary;
                }
            }
        }

        protected abstract bool CanParseFile(IFileInfo file);

        protected abstract ILocalizationDictionary CreateDictionary(string fileContent);
    }
}