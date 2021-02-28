using System;
using System.Runtime.Serialization;
using Volo.Abp;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostSlugAlreadyExistException : BusinessException
    {
        internal BlogPostSlugAlreadyExistException(string code = null, string message = null, string details = null, Exception innerException = null, Microsoft.Extensions.Logging.LogLevel logLevel = Microsoft.Extensions.Logging.LogLevel.Warning) : base(code, message, details, innerException, logLevel)
        {
        }

        internal BlogPostSlugAlreadyExistException(SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext context) : base(serializationInfo, context)
        {
        }

        public BlogPostSlugAlreadyExistException(Guid blogId, string slug)
        {
            Slug = slug;
            BlogId = blogId;

            Code = CmsKitErrorCodes.BlogPosts.SlugAlreadyExist;

            WithData(nameof(Slug), Slug);
            WithData(nameof(BlogId), BlogId);
        }

        public virtual string Slug { get; }

        public virtual Guid BlogId { get; }
    }
}
