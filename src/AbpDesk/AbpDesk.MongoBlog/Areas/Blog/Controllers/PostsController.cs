using System;
using System.Threading.Tasks;
using AbpDesk.Blogging;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Domain.Repositories;

namespace Areas.Blog.Controllers
{
    [Area("Blog")]
    public class PostsController : AbpController
    {
        private readonly IQueryableRepository<BlogPost, Guid> _blogPostRepository;

        public PostsController(IQueryableRepository<BlogPost, Guid> blogPostRepository)
        {
            _blogPostRepository = blogPostRepository;
        }

        public async Task<ActionResult> Index()
        {
            var posts = await _blogPostRepository.GetListAsync(HttpContext.RequestAborted);
            return View(posts);
        }
    }
}
