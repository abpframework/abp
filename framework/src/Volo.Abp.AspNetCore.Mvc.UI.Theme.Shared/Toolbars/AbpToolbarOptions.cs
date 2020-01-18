using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class AbpToolbarOptions
    {
        [NotNull]
        public List<IToolbarContributor> Contributors { get; }

        public AbpToolbarOptions()
        {
            Contributors = new List<IToolbarContributor>();
        }
    }
}
