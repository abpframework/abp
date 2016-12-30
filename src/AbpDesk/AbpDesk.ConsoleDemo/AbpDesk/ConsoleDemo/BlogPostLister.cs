using System;
using System.Globalization;
using System.Linq;
using AbpDesk.Blogging;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Uow;
using Volo.DependencyInjection;

namespace AbpDesk.ConsoleDemo
{
    public class BlogPostLister : ITransientDependency
    {
        private readonly IQueryableRepository<BlogPost, string> _blogPostRepository; //TODO: Should not be needed to string
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public BlogPostLister(IQueryableRepository<BlogPost, string> blogPostRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _blogPostRepository = blogPostRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public void List()
        {
            Console.WriteLine();
            Console.WriteLine("List of blog posts:");

            using (var unitOfWork = _unitOfWorkManager.Begin())
            {
                //var blog = _blogPostRepository.FirstOrDefault(b => b.Title.StartsWith("Hello World 3!"));
                //blog.SetTitle(blog.Title + "'");
                //blog.Comments.Add(new BlogPostComment("@john", "good post! " + DateTime.Now.ToString(CultureInfo.InvariantCulture), star: (byte)RandomHelper.GetRandom(1, 6)));
                //_blogPostRepository.Update(blog);

                //_blogPostRepository.Insert(new BlogPost("Hello World 3!", DateTime.Now.ToString(CultureInfo.InvariantCulture)));

                foreach (var blogPost in _blogPostRepository)
                {
                    Console.WriteLine("# " + blogPost);

                    foreach (var comment in blogPost.Comments)
                    {
                        Console.WriteLine("  - " + comment);
                    }
                }

                unitOfWork.Complete();
            }
        }
    }
}
