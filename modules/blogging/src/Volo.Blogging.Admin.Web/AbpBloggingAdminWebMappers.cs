using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Blogging.Admin.Blogs;
using Volo.Blogging.Admin.Pages.Blogging.Admin.Blogs;
using Volo.Blogging.Blogs.Dtos;
using EditModel = Volo.Blogging.Admin.Pages.Blogging.Admin.Blogs.EditModel;

namespace Volo.Blogging.Admin;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BlogDtoToBlogEditViewModelMapper : MapperBase<BlogDto, EditModel.BlogEditViewModel>
{
    public override partial EditModel.BlogEditViewModel Map(BlogDto source);

    public override partial void Map(BlogDto source, EditModel.BlogEditViewModel destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BlogCreateModalViewToCreateBlogDtoMapper : MapperBase<CreateModel.BlogCreateModalView, CreateBlogDto>
{
    public override partial CreateBlogDto Map(CreateModel.BlogCreateModalView source);

    public override partial void Map(CreateModel.BlogCreateModalView source, CreateBlogDto destination);
}
