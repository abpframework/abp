using System;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Comments
{
    public class IndexModel : CmsKitAdminPageModel
    {
        public string EntityType { get; set; }

        public string EntityId { get; set; }

        public string Author { get; set; }

        public DateTime? CreationStartDate { get; set; }
        
        public DateTime? CreationEndDate { get; set; }
    }
}
