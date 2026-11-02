using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Shouldly;
using Volo.Abp.Data;
using Volo.Abp.Mapperly;
using Volo.Abp.ObjectExtending;
using Volo.Abp.ObjectMapping;
using Volo.Docs.Common;
using Volo.Docs.Common.Projects;
using Volo.Docs.Projects;
using Xunit;

namespace Volo.Docs
{
    public class ProjectMapping_Tests : DocsApplicationTestBase
    {
        static ProjectMapping_Tests()
        {
            ObjectExtensionManager.Instance.AddOrUpdateProperty<Project, string>("RegisteredSecret");
            ObjectExtensionManager.Instance.AddOrUpdateProperty<ProjectDto, string>("RegisteredSecret");
        }

        public static IEnumerable<object[]> MappingCases()
        {
            foreach (var documentMapper in new[] { false, true })
            {
                foreach (var mode in new[]
                {
                    "DirectNew", "DirectExisting", "DirectAliased", "ObjectNew", "ObjectExisting",
                    "ObjectAliased", "ListNew", "ListExisting", "ArrayNew", "ArrayExisting"
                })
                {
                    foreach (var properties in new[] { "Public", "PrivateOnly", "Null" })
                    {
                        yield return new object[] { documentMapper, mode, properties };
                    }
                }
            }
        }

        [Theory]
        [MemberData(nameof(MappingCases))]
        public void Should_Only_Map_Public_Extra_Properties(bool documentMapper, string mode, string properties)
        {
            Project source = new MappingProject();
            source.SetProperty("GitHubAccessToken", "SOURCE_TOKEN_MARKER");
            source.SetProperty("GitHubUserAgent", "SOURCE_USER_AGENT_MARKER");
            source.SetProperty("CustomSecret", "SOURCE_CUSTOM_SECRET_MARKER");
            source.SetProperty("RegisteredSecret", "SOURCE_REGISTERED_SECRET_MARKER");
            source.SetProperty("Path", "SOURCE_PATH_MARKER");
            source.SetProperty("NestedSecret", new Dictionary<string, object> { ["value"] = "SOURCE_NESTED_SECRET_MARKER" });
            if (properties == "Public")
            {
                source.SetProperty("GithubVersionProviderSource", GithubVersionProviderSource.Branches);
                source.SetProperty("VersionBranchPrefix", "rel-");
            }
            if (properties == "Null")
            {
                ((MappingProject)source).ClearExtraProperties();
            }

            var sourceProperties = source.ExtraProperties;
            var sourceJson = JsonSerializer.Serialize(sourceProperties);
            var destination = new ProjectDto
            {
                ExtraProperties = new ExtraPropertyDictionary
                {
                    ["TargetOnlySecret"] = "TARGET_SECRET_MARKER",
                    ["VersionBranchPrefix"] = "STALE_PREFIX_MARKER"
                }
            };
            if (mode.EndsWith("Aliased", StringComparison.Ordinal) && sourceProperties != null)
            {
                destination.ExtraProperties = sourceProperties;
            }

            MapperBase<Project, ProjectDto> directMapper = documentMapper
                ? new ProjectToProjectDtoMapper()
                : new Common.ProjectToProjectDtoMapper();
            IObjectMapper objectMapper = documentMapper
                ? GetRequiredService<IObjectMapper<DocsApplicationModule>>()
                : GetRequiredService<IObjectMapper<DocsCommonApplicationModule>>();
            ProjectDto result;
            switch (mode)
            {
                case "DirectNew":
                    result = directMapper.Map(source);
                    break;
                case "DirectExisting":
                case "DirectAliased":
                    directMapper.Map(source, destination);
                    result = destination;
                    break;
                case "ObjectNew":
                    result = objectMapper.Map<Project, ProjectDto>(source);
                    break;
                case "ObjectExisting":
                case "ObjectAliased":
                    result = objectMapper.Map(source, destination);
                    break;
                case "ListNew":
                    result = objectMapper.Map<List<Project>, List<ProjectDto>>(new List<Project> { source }).Single();
                    break;
                case "ListExisting":
                    result = objectMapper.Map(new List<Project> { source }, new List<ProjectDto> { destination, new ProjectDto() }).Single();
                    break;
                case "ArrayNew":
                    result = objectMapper.Map<Project[], ProjectDto[]>(new[] { source }).Single();
                    break;
                case "ArrayExisting":
                    result = objectMapper.Map(new[] { source }, new[] { destination, new ProjectDto() }).Single();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode));
            }

            result.Id.ShouldBe(source.Id);
            result.Name.ShouldBe(source.Name);
            result.ExtraProperties.ShouldNotBeSameAs(sourceProperties);
            if (properties == "Public")
            {
                result.ExtraProperties.Keys.ShouldBe(new[] { "GithubVersionProviderSource", "VersionBranchPrefix" }, ignoreOrder: true);
                result.GetProperty<GithubVersionProviderSource>("GithubVersionProviderSource").ShouldBe(GithubVersionProviderSource.Branches);
                result.GetProperty<string>("VersionBranchPrefix").ShouldBe("rel-");
            }
            else
            {
                result.ExtraProperties.ShouldBeEmpty();
            }

            var json = JsonSerializer.Serialize(result);
            json.ShouldNotContain("SOURCE_");
            json.ShouldNotContain("TARGET_SECRET_MARKER");
            json.ShouldNotContain("STALE_PREFIX_MARKER");
            source.ExtraProperties.ShouldBeSameAs(sourceProperties);
            JsonSerializer.Serialize(sourceProperties).ShouldBe(sourceJson);
            result.SetProperty("VersionBranchPrefix", "changed-");
            JsonSerializer.Serialize(sourceProperties).ShouldBe(sourceJson);
        }

        private sealed class MappingProject : Project
        {
            public MappingProject() : base(Guid.NewGuid(), "Mapping test", "mapping-test", "GitHub", "md")
            {
            }

            public void ClearExtraProperties()
            {
                ExtraProperties = null;
            }
        }
    }
}
