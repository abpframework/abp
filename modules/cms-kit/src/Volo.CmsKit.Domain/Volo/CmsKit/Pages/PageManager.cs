using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
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

	public virtual async Task SetHomePageAsync(Page page)
	{
		var currentHomePages = await PageRepository.GetListAsync();

		foreach (var homePage in currentHomePages)
		{
			homePage.SetIsHomePage(false);
		}

		await PageRepository.UpdateManyAsync(currentHomePages);
		await PageRepository.UpdateAsync(page);
		
		page.SetIsHomePage(true);
	}

	public virtual async Task<Page> GetHomePageAsync()
	{
		var currentHomePages = await PageRepository.GetListAsync();

		if (currentHomePages.Count > 1)
		{
			throw new BusinessException("There can be only one home page.");
		}

		return currentHomePages.FirstOrDefault();
	}

	protected virtual async Task CheckPageSlugAsync(string slug)
	{
		if (await PageRepository.ExistsAsync(slug))
		{
			throw new PageSlugAlreadyExistsException(slug);
		}
	}
}
