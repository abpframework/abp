using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.Ui.Navigation
{
    public class NavigationOptions
    {
        [NotNull]
        public List<IMenuContributor> MenuContributors { get; }

        public NavigationOptions()
        {
            MenuContributors = new List<IMenuContributor>();
        }
    }
}