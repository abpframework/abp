using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Volo.CmsKit.Pages;

public class PageManager : DomainService
{
    protected IPageRepository PageRepository { get; }

    public PageManager(IPageRepository pageRepository)
    {
        PageRepository = pageRepository;
    }

    public virtual async Task<Page> CreateAsync(
        [NotNull] string title,
        [NotNull] string slug,
        [CanBeNull] string content = null,
        [CanBeNull] string script = null,
        [CanBeNull] string style = null)
    {
        Check.NotNullOrEmpty(title, nameof(title));
        Check.NotNullOrEmpty(slug, nameof(slug));

        await CheckPageSlugAsync(slug);

        return new Page(
            GuidGenerator.Create(),
            title,
            slug,
            content,
            script,
            style,
            CurrentTenant.Id);
    }

    public virtual async Task SetSlugAsync(Page page, string newSlug)
    {
        if (page.Slug != newSlug)
        {
            await CheckPageSlugAsync(newSlug);
            page.SetSlug(newSlug);
        }
    }

    protected virtual async Task CheckPageSlugAsync(string slug)
    {
        if (await PageRepository.ExistsAsync(slug))
        {
            throw new PageSlugAlreadyExistsException(slug);
        }
    }
}
