using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.UI.Navigation;

namespace Volo.Abp.UI.Navigation
{
    public class ApplicationMenuItemList : List<ApplicationMenuItem>
    {
        public ApplicationMenuItemList()
        {

        }

        public ApplicationMenuItemList(int capacity)
            : base(capacity)
        {

        }

        public ApplicationMenuItemList(IEnumerable<ApplicationMenuItem> collection)
            : base(collection)
        {

        }

        public void Normalize()
        {
            RemoveEmptyItems();
            Order();
        }

        private void RemoveEmptyItems()
        {
            RemoveAll(item => item.IsLeaf && item.Url.IsNullOrEmpty());
        }

        private void Order()
        {
            //TODO: Is there any way that is more performant?
            var orderedItems = this.OrderBy(item => item.Order).ToArray();
            Clear();
            AddRange(orderedItems);
        }
    }
}
