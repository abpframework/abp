using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Blogging.Pages.Blogs.Posts;
using Volo.Blogging.Posts;

namespace Volo.Blogging;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PostWithDetailsDtoToEditPostViewModelMapper : MapperBase<PostWithDetailsDto, EditPostViewModel>
{
    [MapperIgnoreTarget(nameof(EditPostViewModel.Tags))]
    public override partial EditPostViewModel Map(PostWithDetailsDto source);

    [MapperIgnoreTarget(nameof(EditPostViewModel.Tags))]
    public override partial void Map(PostWithDetailsDto source, EditPostViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class CreatePostViewModelToCreatePostDtoMapper : MapperBase<NewModel.CreatePostViewModel, CreatePostDto>
{
    public override partial CreatePostDto Map(NewModel.CreatePostViewModel source);

    public override partial void Map(NewModel.CreatePostViewModel source, CreatePostDto destination);
}
