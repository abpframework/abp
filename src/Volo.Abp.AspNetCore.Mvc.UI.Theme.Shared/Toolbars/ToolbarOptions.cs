using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.AspNetCore.Mvc.UI.Theme.Shared.Toolbars
{
    public class ToolbarOptions
    {
        [NotNull]
        public List<IToolbarContributor> Contributors { get; }

        public ToolbarOptions()
        {
            Contributors = new List<IToolbarContributor>();
        }
    }
}
