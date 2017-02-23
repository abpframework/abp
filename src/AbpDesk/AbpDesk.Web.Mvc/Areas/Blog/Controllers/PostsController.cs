using System.Threading.Tasks;
using AbpDesk.Blogging;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;

namespace AbpDesk.Web.Mvc.Areas.Blog.Controllers
{
    //TODO: Move this controller to a plug-in
    [Area("Blog")]
    public class PostsController : AbpController
    {
        private readonly IQueryableRepository<BlogPost> _blogPostRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PostsController(IQueryableRepository<BlogPost> blogPostRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _blogPostRepository = blogPostRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public async Task<ActionResult> Index()
        {
            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                var posts = await _blogPostRepository.GetListAsync(HttpContext.RequestAborted);

                await unitOfWork.CompleteAsync(HttpContext.RequestAborted);

                return View(posts);
            }
        }
    }
}
