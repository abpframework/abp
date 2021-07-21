using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Uow;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Menus
{
    public class MenuItemManager : CmsKitDomainServiceBase
    {
        protected IMenuItemRepository MenuItemRepository { get; }

        public MenuItemManager(IMenuItemRepository menuRepository)
        {
            MenuItemRepository = menuRepository;
        }

        public virtual void SetPageUrl(MenuItem menuItem, Page page)
        {
            menuItem.SetPageId(page.Id);
            menuItem.SetUrl(PageConsts.UrlPrefix + page.Slug);
        }

        [UnitOfWork]
        public virtual async Task MoveAsync(Guid menuItemId, Guid? newParentMenuItemId, int position = 0)
        {
            var menuItems = await MenuItemRepository.GetListAsync();

            var movedMenuItem = menuItems.FirstOrDefault(x => x.Id == menuItemId)
                           ?? throw new EntityNotFoundException(typeof(MenuItem), menuItemId);

            if (newParentMenuItemId.HasValue && !menuItems.Any(a => a.Id == newParentMenuItemId.Value))
            {
                throw new EntityNotFoundException(typeof(MenuItem), newParentMenuItemId);
            }

            movedMenuItem.ParentId = newParentMenuItemId;
            movedMenuItem.Order = position;

            OrganizeTreeOrderForMenuItem(menuItems, movedMenuItem);

            await MenuItemRepository.UpdateManyAsync(menuItems);
        }

        public virtual void OrganizeTreeOrderForMenuItem(List<MenuItem> items, MenuItem menuItem)
        {
            var sameTree = items.Where(x => x.ParentId == menuItem.ParentId).OrderBy(x => x.Order).ToList();

            sameTree.Remove(menuItem); // Remove if exists to prevent misordering with same order number

            sameTree.Insert(menuItem.Order, menuItem);

            for (int i = 0; i < sameTree.Count; i++)
            {
                sameTree[i].Order = i;
            }
        }
    }
}