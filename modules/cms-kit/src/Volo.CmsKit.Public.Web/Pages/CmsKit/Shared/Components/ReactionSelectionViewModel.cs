using System.Collections.Generic;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit.Web.Pages.CmsKit.Shared.Components
{
    public class ReactionSelectionViewModel
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public List<ReactionViewModel> Reactions { get; set; }
    }
}
