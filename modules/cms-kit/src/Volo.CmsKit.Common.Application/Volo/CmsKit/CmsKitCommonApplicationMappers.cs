using Riok.Mapperly.Abstractions;
using Volo.Abp.Mapperly;
using Volo.CmsKit.Blogs;
using Volo.CmsKit.Tags;
using Volo.CmsKit.Users;

namespace Volo.CmsKit;

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class OrganizationUnitRoleToOrganizationUnitRoleDtoMapper : TwoWayMapperBase<BlogFeatureCacheItem, BlogFeatureDto>
{
    public override partial BlogFeatureDto Map(BlogFeatureCacheItem source);
    public override partial void Map(BlogFeatureCacheItem source, BlogFeatureDto destination);

    public override partial BlogFeatureCacheItem ReverseMap(BlogFeatureDto destination);
    public override partial void ReverseMap(BlogFeatureDto destination, BlogFeatureCacheItem source);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class BlogFeatureToBlogFeatureDtoMapper : MapperBase<BlogFeature, BlogFeatureDto>
{
    public override partial BlogFeatureDto Map(BlogFeature source);

    public override partial void Map(BlogFeature source, BlogFeatureDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class BlogFeatureToBlogFeatureCacheItemMapper : MapperBase<BlogFeature, BlogFeatureCacheItem>
{
    public override partial BlogFeatureCacheItem Map(BlogFeature source);

    public override partial void Map(BlogFeature source, BlogFeatureCacheItem destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
public partial class PopularTagToPopularTagDtoMapper : MapperBase<PopularTag, PopularTagDto>
{
    public override partial PopularTagDto Map(PopularTag source);

    public override partial void Map(PopularTag source, PopularTagDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class CmsUserToCmsUserDtoMapper : MapperBase<CmsUser, CmsUserDto>
{
    public override partial CmsUserDto Map(CmsUser source);

    public override partial void Map(CmsUser source, CmsUserDto destination);
}

[Mapper(RequiredMappingStrategy = RequiredMappingStrategy.Target)]
[MapExtraProperties]
public partial class TagToTagDtoMapper : MapperBase<Tag, TagDto>
{
    public override partial TagDto Map(Tag source);

    public override partial void Map(Tag source, TagDto destination);
}