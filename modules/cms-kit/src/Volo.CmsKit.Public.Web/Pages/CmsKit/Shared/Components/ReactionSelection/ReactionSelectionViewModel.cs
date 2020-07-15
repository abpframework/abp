using System.Collections.Generic;

namespace Volo.CmsKit.Web.Pages.CmsKit.Shared.Components.ReactionSelection
{
    public class ReactionSelectionViewModel
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public List<ReactionViewModel> Reactions { get; set; }
    }
}
