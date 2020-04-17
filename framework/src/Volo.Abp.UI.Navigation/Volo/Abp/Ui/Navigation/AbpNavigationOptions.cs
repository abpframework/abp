using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.UI.Navigation
{
    public class AbpNavigationOptions
    {
        [NotNull]
        public List<IMenuContributor> MenuContributors { get; }

        public AbpNavigationOptions()
        {
            MenuContributors = new List<IMenuContributor>();
        }
    }
}