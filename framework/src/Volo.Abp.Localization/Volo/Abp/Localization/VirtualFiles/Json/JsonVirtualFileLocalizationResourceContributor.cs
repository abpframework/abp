using Microsoft.Extensions.FileProviders;
using System;
using Volo.Abp.Localization.Json;

namespace Volo.Abp.Localization.VirtualFiles.Json;

//TODO: Use composition over inheritance..?

public class JsonVirtualFileLocalizationResourceContributor : VirtualFileLocalizationResourceContributorBase
{
    public JsonVirtualFileLocalizationResourceContributor(string virtualPath)
        : base(virtualPath)
    {

    }

    protected override bool CanParseFile(IFileInfo file)
    {
        return file.Name.EndsWith(".json", StringComparison.OrdinalIgnoreCase);
    }

    protected override string? GetCultureNameOrNull(IFileInfo file)
    {
        try
        {
            using (var stream = file.CreateReadStream())
            {
                return JsonLocalizationDictionaryBuilder.GetCultureNameOrNull(stream);
            }
        }
        catch (Exception)
        {
            // All files are parsed as before, which reports the error if there is one.
            return null;
        }
    }

    protected override ILocalizationDictionary? CreateDictionaryFromFileContent(string jsonString)
    {
        return JsonLocalizationDictionaryBuilder.BuildFromJsonString(jsonString);
    }
}
