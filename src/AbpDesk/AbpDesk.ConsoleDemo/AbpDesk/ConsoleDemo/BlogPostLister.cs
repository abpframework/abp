using System;
using AbpDesk.Blogging;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Volo.Abp.Uow;
using Volo.Abp.DependencyInjection;

namespace AbpDesk.ConsoleDemo
{
    public class BlogPostLister : ITransientDependency
    {
        private readonly IQueryableRepository<BlogPost> _blogPostRepository;
        private readonly IGuidGenerator _guidGenerator;

        public BlogPostLister(
            IQueryableRepository<BlogPost> blogPostRepository,
            IGuidGenerator guidGenerator)
        {
            _blogPostRepository = blogPostRepository;
            _guidGenerator = guidGenerator;
        }

        [UnitOfWork]
        public virtual void List()
        {
            Console.WriteLine();
            Console.WriteLine("List of blog posts:");

            //var blog = _blogPostRepository.FirstOrDefault(b => b.Title.StartsWith("Hello World 3!"));
            //blog.SetTitle(blog.Title + "'");
            //blog.Comments.Add(new BlogPostComment("@john", "good post! " + DateTime.Now.ToString(CultureInfo.InvariantCulture), star: (byte)RandomHelper.GetRandom(1, 6)));
            //_blogPostRepository.Update(blog);

            //_blogPostRepository.Insert(new BlogPost(_guidGenerator.Create(), "Hello World 1!", DateTime.Now.ToString(CultureInfo.InvariantCulture)));

            foreach (var blogPost in _blogPostRepository)
            {
                Console.WriteLine("# " + blogPost);

                foreach (var comment in blogPost.Comments)
                {
                    Console.WriteLine("  - " + comment);
                }
            }
        }
    }
}
