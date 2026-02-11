using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Docs.Common.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs.Common;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectToProjectDtoMapper : MapperBase<Project, ProjectDto>
{
    public override partial ProjectDto Map(Project source);

    public override partial void Map(Project source, ProjectDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class VersionInfoToVersionInfoDtoMapper : MapperBase<VersionInfo, VersionInfoDto>
{
    public override partial VersionInfoDto Map(VersionInfo source);

    public override partial void Map(VersionInfo source, VersionInfoDto destination);
}
