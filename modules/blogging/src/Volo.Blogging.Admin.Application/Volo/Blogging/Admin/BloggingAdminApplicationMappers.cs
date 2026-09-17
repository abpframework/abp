using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.Blogging.Blogs;
using Volo.Blogging.Blogs.Dtos;

namespace Volo.Blogging.Admin;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class BlogToBlogDtoMapper : MapperBase<Blog, BlogDto>
{
    public override partial BlogDto Map(Blog source);

    public override partial void Map(Blog source, BlogDto destination);
}