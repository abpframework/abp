using Microsoft.Extensions.FileProviders;
using System;
using Volo.Abp.Localization.Json;

namespace Volo.Abp.Localization.VirtualFiles.Json
{
    public class JsonEmbeddedFileLocalizationDictionaryProvider : VirtualFileLocalizationDictionaryProviderBase
    {
        public JsonEmbeddedFileLocalizationDictionaryProvider(string virtualPath)
            : base(virtualPath)
        {

        }

        protected override bool CanParseFile(IFileInfo file)
        {
            return file.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase);
        }

        protected override ILocalizationDictionary CreateDictionary(string jsonString)
        {
            return JsonLocalizationDictionaryBuilder.BuildFromJsonString(jsonString); //TODO: Use composition over inheritance!
        }
    }
}