using System;
using AbpDesk.Blogging;
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
                //_blogPostRepository.Insert(new BlogPost("Hello World 3!", "Hello World 3......"));

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
