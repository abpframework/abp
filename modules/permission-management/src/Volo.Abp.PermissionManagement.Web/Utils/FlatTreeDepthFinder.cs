using System.Collections.Generic;

namespace Volo.Abp.PermissionManagement.Web.Utils
{
    public class FlatTreeDepthFinder<T>
        where T : class, IFlatTreeItem
    {
        public virtual void SetDepths(List<T> items)
        {
            SetDepths(items, null, 0);
        }

        private static void SetDepths(List<T> items, string currentParent, int currentDepth)
        {
            foreach (var item in items)
            {
                if (item.ParentName == currentParent)
                {
                    item.Depth = currentDepth;
                    SetDepths(items, item.Name, currentDepth + 1);
                }
            }
        }
    }
}
