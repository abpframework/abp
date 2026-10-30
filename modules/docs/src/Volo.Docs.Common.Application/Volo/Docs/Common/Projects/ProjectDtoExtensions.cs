using Volo.Abp.Data;

namespace Volo.Docs.Common.Projects;

public static class ProjectDtoExtensions
{
    public static ExtraPropertyDictionary ToPublicProjectExtraProperties(this ExtraPropertyDictionary source)
    {
        var extraProperties = new ExtraPropertyDictionary();

        foreach (var propertyName in new[] { "GithubVersionProviderSource", "VersionBranchPrefix" })
        {
            if (source != null && source.TryGetValue(propertyName, out var value))
            {
                extraProperties[propertyName] = value;
            }
        }

        return extraProperties;
    }
}
