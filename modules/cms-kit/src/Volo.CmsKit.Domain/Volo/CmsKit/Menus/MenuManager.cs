using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
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

        /// <summary>
        /// Creates a new instance of <see cref="Menu"/> without inserting into database.
        /// </summary>
        /// <param name="tenantId">Tenant Id of Menu.</param>
        /// <param name="name">Name of Menu.</param>
        /// <returns>Created instance.</returns>
        public virtual async Task<Menu> CreateAsync(Guid? tenantId, [NotNull] string name)
        {
            var menu = new Menu(
                GuidGenerator.Create(), 
                tenantId, 
                Check.NotNullOrEmpty(name, nameof(name), MenuConsts.MaxNameLength));
            
            var existingMenuCount = await MenuRepository.GetCountAsync();

            if (existingMenuCount == 0)
            {
                menu.IsMainMenu = true;
            }

            return menu;
        }

        public virtual void SetPageUrl(MenuItem menuItem, Page page)
        {
            menuItem.SetPageId(page.Id);
            menuItem.SetUrl(PageConsts.UrlPrefix + page.Slug);
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

        public virtual void OrganizeTreeOrderForMenuItem(Menu menu, MenuItem menuItem)
        {
            var sameTree = menu.Items.Where(x => x.ParentId == menuItem.ParentId).OrderBy(x => x.Order).ToList();

            sameTree.Remove(menuItem); // Remove if exists to prevent misordering with same order number

            sameTree.Insert(menuItem.Order, menuItem);

            for (int i = 0; i < sameTree.Count; i++)
            {
                sameTree[i].Order = i;
            }
        }

        public virtual async Task SetMainMenuAsync(Guid menuId)
        {
            var menus = await MenuRepository.GetCurrentAndNextMainMenusAsync(menuId ,includeDetails: false);

            foreach (var menu in menus)
            {
                menu.IsMainMenu = menuId == menu.Id;
            }

            await MenuRepository.UpdateManyAsync(menus);
        }

        public virtual async Task UnSetMainMenuAsync(Guid menuId)
        {
            var menu = await MenuRepository.GetAsync(menuId, includeDetails: false);

            menu.IsMainMenu = false;

            await MenuRepository.UpdateAsync(menu);
        }
    }
}