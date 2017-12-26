using System.Collections.Generic;

namespace Volo.Abp.Ui.Navigation
{
    public interface IHasMenuItems
    {
        /// <summary>
        /// Menu items.
        /// </summary>
        IList<ApplicationMenuItem> Items { get; }
    }
}