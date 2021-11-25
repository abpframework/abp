using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Tags;

public class TagManager : DomainService
{
    protected ITagRepository TagRepository { get; }
    protected ITagDefinitionStore TagDefinitionStore { get; }

    public TagManager(ITagRepository tagRepository, ITagDefinitionStore tagDefinitionStore)
    {
        TagRepository = tagRepository;
        TagDefinitionStore = tagDefinitionStore;
    }

    public virtual async Task<Tag> GetOrAddAsync([NotNull] string entityType, [NotNull] string name)
    {
        var tag = await TagRepository.FindAsync(entityType, name);

        if (tag == null)
        {
            tag = await CreateAsync(GuidGenerator.Create(), entityType, name);
            await TagRepository.InsertAsync(tag);
        }

        return tag;
    }

    public virtual async Task<Tag> CreateAsync(Guid id,
                                               [NotNull] string entityType,
                                               [NotNull] string name)
    {
        if (!await TagDefinitionStore.IsDefinedAsync(entityType))
        {
            throw new EntityNotTaggableException(entityType);
        }

        if (await TagRepository.AnyAsync(entityType, name))
        {
            throw new TagAlreadyExistException(entityType, name);
        }

        return
            new Tag(id, entityType, name, CurrentTenant.Id);
    }

    public virtual async Task<Tag> UpdateAsync(Guid id, [NotNull] string name)
    {
        Check.NotNullOrEmpty(name, nameof(name));

        var tag = await TagRepository.GetAsync(id);

        if (name != tag.Name &&
            await TagRepository.AnyAsync(tag.EntityType, name))
        {
            throw new TagAlreadyExistException(tag.EntityType, name);
        }

        tag.SetName(name);

        return tag;
    }
}
