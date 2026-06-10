using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectToProjectEtoMapper : MapperBase<Project, ProjectEto>
{
    public override partial ProjectEto Map(Project source);

    public override partial void Map(Project source, ProjectEto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class DocumentToDocumentEtoMapper : MapperBase<Document, DocumentEto>
{
    public override partial DocumentEto Map(Document source);

    public override partial void Map(Document source, DocumentEto destination);
}