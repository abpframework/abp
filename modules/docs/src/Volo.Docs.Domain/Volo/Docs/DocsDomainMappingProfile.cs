using AutoMapper;
using Volo.Docs.Documents;
using Volo.Docs.Projects;

namespace Volo.Docs
{
    public class DocsDomainMappingProfile : Profile
    {
        public DocsDomainMappingProfile()
        {
            CreateMap<Document, DocumentEto>();
            CreateMap<Project, ProjectEto>();
        }
    }
}