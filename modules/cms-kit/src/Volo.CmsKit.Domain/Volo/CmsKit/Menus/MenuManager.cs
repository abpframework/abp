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
        public virtual async Task MoveAsync(Guid menuId, Guid menuItemId, Guid? newParentId, int position = 0)
        {
            var menu = await MenuRepository.GetAsync(menuId, includeDetails: true);

            var menuItem = menu.Items.FirstOrDefault(x => x.Id == menuItemId)
                ?? throw new EntityNotFoundException(typeof(MenuItem), menuItemId);

            if (newParentId.HasValue && !menu.Items.Any(a => a.Id == newParentId.Value))
            {
                throw new EntityNotFoundException(typeof(MenuItem), newParentId);
            }

            menuItem.ParentId = newParentId;
            menuItem.Order = position;
            
            OrganizeTreeOrderForMenuItem(menu, menuItem);

            await MenuRepository.UpdateAsync(menu);
        }

        public void OrganizeTreeOrderForMenuItem(Menu menu, MenuItem menuItem)
        {
            var sameTree = menu.Items.Where(x => x.ParentId == menuItem.ParentId).OrderBy(x => x.Order).ToList();

            sameTree.Remove(menuItem); // Remove if exists
            
            sameTree.Insert(menuItem.Order, menuItem);

            for (int i = 0; i < sameTree.Count; i++)
            {
                sameTree[i].Order = i;
            }
        }
    }
}
