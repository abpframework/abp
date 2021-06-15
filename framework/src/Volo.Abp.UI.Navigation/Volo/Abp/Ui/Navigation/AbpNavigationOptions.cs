using System.Collections.Generic;
using JetBrains.Annotations;

namespace Volo.Abp.UI.Navigation
{
    public class AbpNavigationOptions
    {
        [NotNull]
        public List<IMenuContributor> MenuContributors { get; }
        
        /// <summary>
        /// Includes the <see cref="StandardMenus.Main"/> by default.
        /// </summary>
        public List<string> MainMenuNames { get; }

        public AbpNavigationOptions()
        {
            MenuContributors = new List<IMenuContributor>();
            
            MainMenuNames = new List<string>
            {
                StandardMenus.Main
            };
        }
    }
}