using AutoMapper;
using Volo.Docs.Common.Projects;
using Volo.Docs.Projects;

namespace Volo.Docs.Common
{
    public class DocsCommonApplicationAutoMapperProfile : Profile
    {
        public DocsCommonApplicationAutoMapperProfile()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<VersionInfo, VersionInfoDto>();
        }
    }
}
