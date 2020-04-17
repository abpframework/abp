using AutoMapper;
using Volo.Docs.Admin.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs.Admin
{
    public class DocsAdminApplicationAutoMapperProfile : Profile
    {
        public DocsAdminApplicationAutoMapperProfile()
        {
            CreateMap<Project, ProjectDto>();
        }
    }
}