using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Docs.Admin.Documents;
using Volo.Docs.Admin.Pages.Docs.Admin.Projects;
using Volo.Docs.Admin.Projects;

namespace Volo.Docs.Admin;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PullDocumentViewModelToPullAllDocumentInputMapper : MapperBase<PullModel.PullDocumentViewModel, PullAllDocumentInput>
{
    public override partial PullAllDocumentInput Map(PullModel.PullDocumentViewModel source);

    public override partial void Map(PullModel.PullDocumentViewModel source, PullAllDocumentInput destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PullDocumentViewModelToPullDocumentInputMapper : MapperBase<PullModel.PullDocumentViewModel, PullDocumentInput>
{
    public override partial PullDocumentInput Map(PullModel.PullDocumentViewModel source);

    public override partial void Map(PullModel.PullDocumentViewModel source, PullDocumentInput destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class ProjectDtoToEditGithubProjectViewModelMapper : MapperBase<ProjectDto, EditModel.EditGithubProjectViewModel>
{
    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.GitHubAccessToken))]
    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.GitHubRootUrl))]
    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.GitHubUserAgent))]
    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.GithubVersionProviderSource))]
    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.VersionBranchPrefix))]
    public override partial EditModel.EditGithubProjectViewModel Map(ProjectDto source);

    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.GitHubAccessToken))]
    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.GitHubRootUrl))]
    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.GitHubUserAgent))]
    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.GithubVersionProviderSource))]
    [MapperIgnoreTarget(nameof(EditModel.EditGithubProjectViewModel.VersionBranchPrefix))]
    public override partial void Map(ProjectDto source, EditModel.EditGithubProjectViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CreateGithubProjectViewModelToCreateProjectDtoMapper : MapperBase<CreateModel.CreateGithubProjectViewModel, CreateProjectDto>
{
    public override partial CreateProjectDto Map(CreateModel.CreateGithubProjectViewModel source);

    public override partial void Map(CreateModel.CreateGithubProjectViewModel source, CreateProjectDto destination);
}


[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class EditGithubProjectViewModelToUpdateProjectDtoMyClassMapper : MapperBase<EditModel.EditGithubProjectViewModel, UpdateProjectDto>
{
    public override partial UpdateProjectDto Map(EditModel.EditGithubProjectViewModel source);

    public override partial void Map(EditModel.EditGithubProjectViewModel source, UpdateProjectDto destination);
}
