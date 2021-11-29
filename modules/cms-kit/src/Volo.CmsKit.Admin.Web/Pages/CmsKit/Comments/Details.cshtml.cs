using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.CmsKit.Admin.Comments;

namespace Volo.CmsKit.Admin.Web.Pages.CmsKit.Comments
{
    public class DetailsModel : CmsKitAdminPageModel
    {
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }
        
        public string Author { get; set; }

        public DateTime? CreationStartDate { get; set; }
        
        public DateTime? CreationEndDate { get; set; }

        public CommentWithAuthorDto CommentWithAuthorDto { get; protected set; }
        
        protected readonly ICommentAdminAppService CommentAdminAppService;

        public DetailsModel(ICommentAdminAppService commentAdminAppService)
        {
            CommentAdminAppService = commentAdminAppService;
        }

        public async Task OnGetAsync()
        {
            CommentWithAuthorDto = await CommentAdminAppService.GetAsync(Id);
        }
    }
}
