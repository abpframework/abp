using System.Collections.Generic;

namespace Volo.Abp.UI.Navigation
{
    public interface IHasMenuItems
    {
        /// <summary>
        /// Menu items.
        /// </summary>
        IList<ApplicationMenuItem> Items { get; }
    }
}