using AutoMapper;
using Volo.Abp.AutoMapper;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Admin.Pages.Docs.Admin.Projects;
using Volo.Docs.Admin.Projects;

namespace Volo.Docs.Admin
{
    public class DocsAdminWebAutoMapperProfile : Profile
    {
        public DocsAdminWebAutoMapperProfile()
        {
            CreateMap<CreateModel.CreateGithubProjectViewModel, CreateProjectDto>().Ignore(x => x.ExtraProperties);

            CreateMap<EditModel.EditGithubProjectViewModel, UpdateProjectDto>().Ignore(x => x.ExtraProperties);

            CreateMap<ProjectDto, EditModel.EditGithubProjectViewModel > ()
                .Ignore(x => x.GitHubAccessToken).Ignore(x => x.GitHubRootUrl).Ignore(x => x.GitHubUserAgent);

            CreateMap<PullModel.PullDocumentViewModel, PullAllDocumentInput>();
            CreateMap<PullModel.PullDocumentViewModel, PullDocumentInput>();
        }
    }
}
