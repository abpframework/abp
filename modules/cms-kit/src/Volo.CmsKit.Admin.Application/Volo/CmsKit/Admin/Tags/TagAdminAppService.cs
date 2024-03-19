using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Data;
using Volo.Abp.Features;
using Volo.Abp.GlobalFeatures;
using Volo.Abp.ObjectExtending;
using Volo.CmsKit.Features;
using Volo.CmsKit.GlobalFeatures;
using Volo.CmsKit.Permissions;
using Volo.CmsKit.Tags;

namespace Volo.CmsKit.Admin.Tags;

[RequiresFeature(CmsKitFeatures.TagEnable)]
[Authorize(CmsKitAdminPermissions.Tags.Default)]
[RequiresGlobalFeature(typeof(TagsFeature))]
public class TagAdminAppService : CmsKitAppServiceBase, ITagAdminAppService
{
    protected ITagRepository Repository { get; }
    protected TagManager TagManager { get; }
    protected ITagDefinitionStore TagDefinitionStore { get; }
    protected IStringLocalizerFactory StringLocalizerFactory { get; }

    public TagAdminAppService(
        ITagRepository repository,
        TagManager tagManager,
        ITagDefinitionStore tagDefinitionStore,
        IStringLocalizerFactory stringLocalizerFactory)
    {
        Repository = repository;
        TagManager = tagManager;
        TagDefinitionStore = tagDefinitionStore;
        StringLocalizerFactory = stringLocalizerFactory;
    }

    [Authorize(CmsKitAdminPermissions.Tags.Create)]
    public virtual async Task<TagDto> CreateAsync(TagCreateDto input)
    {
        var tag = await TagManager.CreateAsync(
            GuidGenerator.Create(),
            input.EntityType,
            input.Name);
        input.MapExtraPropertiesTo(tag);
        await Repository.InsertAsync(tag);

        return ObjectMapper.Map<Tag, TagDto>(tag);
    }

    [Authorize(CmsKitAdminPermissions.Tags.Update)]
    public virtual async Task<TagDto> UpdateAsync(Guid id, TagUpdateDto input)
    {
        var tag = await TagManager.UpdateAsync(
            id,
            input.Name);
        input.MapExtraPropertiesTo(tag);

        tag.SetConcurrencyStampIfNotNull(input.ConcurrencyStamp);

        await Repository.UpdateAsync(tag);

        return ObjectMapper.Map<Tag, TagDto>(tag);
    }

    [Authorize(CmsKitAdminPermissions.Tags.Default)]
    public virtual async Task<List<TagDefinitionDto>> GetTagDefinitionsAsync()
    {
        var definitions = await TagDefinitionStore.GetTagEntityTypeDefinitionListAsync();

        return definitions
                    .Select(s =>
                        new TagDefinitionDto
                        {
                            EntityType = s.EntityType,
                            DisplayName = s.DisplayName?.Localize(StringLocalizerFactory) ?? s.EntityType
                        })
                    .ToList();
    }

    [Authorize(CmsKitAdminPermissions.Tags.Default)]
    public virtual async Task<TagDto> GetAsync(Guid id)
    {
        var tag = await Repository.GetAsync(id);

        return ObjectMapper.Map<Tag, TagDto>(tag);
    }

    [Authorize(CmsKitAdminPermissions.Tags.Default)]
    public virtual async Task<PagedResultDto<TagDto>> GetListAsync(TagGetListInput input)
    {
        var tags = await Repository.GetListAsync(input.Filter, input.MaxResultCount, input.SkipCount, input.Sorting);
        var count = await Repository.GetCountAsync(input.Filter);

        return new PagedResultDto<TagDto>(
            count,
            ObjectMapper.Map<List<Tag>, List<TagDto>>(tags)
            );
    }

    [Authorize(CmsKitAdminPermissions.Tags.Delete)]
    public virtual async Task DeleteAsync(Guid id)
    {
        await Repository.DeleteAsync(id);
    }
}
