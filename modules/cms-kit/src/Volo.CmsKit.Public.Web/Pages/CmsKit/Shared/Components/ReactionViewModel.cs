using JetBrains.Annotations;

namespace Volo.CmsKit.Web.Pages.CmsKit.Shared.Components
{
    public class ReactionViewModel
    {
        [NotNull]
        public string Name { get; set; }

        [CanBeNull]
        public string DisplayName { get; set; }

        [NotNull]
        public string Icon { get; set; }
    }
}