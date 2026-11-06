using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Abp.Data;
using Volo.Docs.Common.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs.Common;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectToProjectDtoMapper : MapperBase<Project, ProjectDto>
{
    [MapProperty(nameof(Project.ExtraProperties), nameof(ProjectDto.ExtraProperties), Use = nameof(MapPublicExtraProperties))]
    public override partial ProjectDto Map(Project source);

    [MapProperty(nameof(Project.ExtraProperties), nameof(ProjectDto.ExtraProperties), Use = nameof(MapPublicExtraProperties))]
    public override partial void Map(Project source, ProjectDto destination);

    private static ExtraPropertyDictionary MapPublicExtraProperties(ExtraPropertyDictionary source)
    {
        return source.ToPublicProjectExtraProperties();
    }
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class VersionInfoToVersionInfoDtoMapper : MapperBase<VersionInfo, VersionInfoDto>
{
    public override partial VersionInfoDto Map(VersionInfo source);

    public override partial void Map(VersionInfo source, VersionInfoDto destination);
}
