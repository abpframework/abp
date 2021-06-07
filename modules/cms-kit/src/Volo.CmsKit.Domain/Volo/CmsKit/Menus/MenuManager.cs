using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Services;
using Volo.CmsKit.Pages;

namespace Volo.CmsKit.Menus
{
    public class MenuManager : DomainService
    {
        public virtual void SetPageUrl(MenuItem menuItem, Page page)
        {
            menuItem.SetPageId(page.Id);
            menuItem.SetUrl(page.Slug.EnsureStartsWith('/'));
        }

        public virtual Task MoveAsync(Guid menuId, Guid menuItemId, Guid newParentId)
        {
            // TODO: Implement move method
            throw new NotImplementedException();
        }
    }
}
