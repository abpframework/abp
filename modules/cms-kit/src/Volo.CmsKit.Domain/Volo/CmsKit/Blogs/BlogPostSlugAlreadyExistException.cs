using System;
using Volo.Abp;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostSlugAlreadyExistException : BusinessException
    {
        internal BlogPostSlugAlreadyExistException(string code = null, string message = null, string details = null, Exception innerException = null, Microsoft.Extensions.Logging.LogLevel logLevel = Microsoft.Extensions.Logging.LogLevel.Warning) : base(code, message, details, innerException, logLevel)
        {
        }

        internal BlogPostSlugAlreadyExistException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext context) : base(serializationInfo, context)
        {
        }

        public BlogPostSlugAlreadyExistException(Guid blogId, string slug)
        {
            Slug = slug;
            BlogId = blogId;

            Code = CmsKitErrorCodes.Blogs.SlugAlreadyExist;

            WithData(nameof(Slug), Slug);
            WithData(nameof(BlogId), BlogId);
        }

        public string Slug { get; }

        public Guid BlogId { get; }
    }
}
