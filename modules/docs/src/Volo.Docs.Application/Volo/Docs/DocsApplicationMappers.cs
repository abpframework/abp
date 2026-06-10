using Riok.Mapperly.Abstractions;
using Volo.Docs.Documents;
using Volo.Abp.Mapperly;
using Volo.Docs.Common.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DocumentResourceToDocumentResourceDtoMapper : MapperBase<DocumentResource, DocumentResourceDto>
{
    public override partial DocumentResourceDto Map(DocumentResource source);

    public override partial void Map(DocumentResource source, DocumentResourceDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DocumentContributorToDocumentContributorDtoMapper : MapperBase<DocumentContributor, DocumentContributorDto>
{
    public override partial DocumentContributorDto Map(DocumentContributor source);

    public override partial void Map(DocumentContributor source, DocumentContributorDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class VersionInfoToVersionInfoDtoMapper : MapperBase<VersionInfo, VersionInfoDto>
{
    public override partial VersionInfoDto Map(VersionInfo source);

    public override partial void Map(VersionInfo source, VersionInfoDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectToProjectDtoMapper : MapperBase<Project, ProjectDto>
{
    public override partial ProjectDto Map(Project source);

    public override partial void Map(Project source, ProjectDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DocumentToDocumentWithDetailsDtoMapper : MapperBase<Document, DocumentWithDetailsDto>
{
    [MapperIgnoreTarget(nameof(DocumentWithDetailsDto.Project))]
    [MapperIgnoreTarget(nameof(DocumentWithDetailsDto.Contributors))]
    public override partial DocumentWithDetailsDto Map(Document source);

    [MapperIgnoreTarget(nameof(DocumentWithDetailsDto.Project))]
    [MapperIgnoreTarget(nameof(DocumentWithDetailsDto.Contributors))]
    public override partial void Map(Document source, DocumentWithDetailsDto destination);
}