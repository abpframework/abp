using AutoMapper;
using Volo.Docs.Documents;
using Volo.Docs.Projects;
using Volo.Abp.AutoMapper;

namespace Volo.Docs
{
    public class DocsApplicationAutoMapperProfile : Profile
    {
        public DocsApplicationAutoMapperProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<VersionInfo, VersionInfoDto>();
            CreateMap<Document, DocumentWithDetailsDto>().Ignore(x => x.Project).Ignore(x => x.Contributors);
            CreateMap<DocumentContributor, DocumentContributorDto>();
            CreateMap<DocumentResource, DocumentResourceDto>();
        }
    }
}
