using AutoMapper;
using Volo.Docs.Documents;
using Volo.Abp.AutoMapper;
using Volo.Docs.Common.Projects;
using Volo.Docs.Projects;

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
