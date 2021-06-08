using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Domain.Services;
using Volo.Abp.Uow;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Menus
{
    public class MenuManager : CmsKitDomainServiceBase
    {
        protected IMenuRepository MenuRepository { get; }

        public MenuManager(IMenuRepository menuRepository)
        {
            MenuRepository = menuRepository;
        }

        public virtual void SetPageUrl(MenuItem menuItem, Page page)
        {
            menuItem.SetPageId(page.Id);
            menuItem.SetUrl(page.Slug.EnsureStartsWith('/'));
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(Guid menuId, Guid menuItemId, Guid newParentId)
        {
            var menu = await MenuRepository.GetAsync(menuId, includeDetails: true);

            var menuItem = menu.Items.FirstOrDefault(x => x.Id == menuItemId)
                ?? throw new EntityNotFoundException(typeof(MenuItem), menuItemId);

            if (!menu.Items.Any(a => a.Id == newParentId))
            {
                throw new EntityNotFoundException(typeof(MenuItem), newParentId);
            }

            menuItem.ParentId = newParentId;

            await MenuRepository.UpdateAsync(menu);
        }
    }
}
