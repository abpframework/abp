namespace Volo.CmsKit.Blogs;

public class BlogWithBlogPostCount
{
    public Blog Blog { get; set; }

    public int BlogPostCount { get; set; }
    
    public BlogWithBlogPostCount(Blog blog, int blogPostCount)
    {
        Blog = blog;
        BlogPostCount = blogPostCount;
    }
}