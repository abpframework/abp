using System;
using Volo.Abp;

namespace Volo.CmsKit.Blogs
{
    public class BlogPostUrlSlugAlreadyExistException : BusinessException
    {
        internal BlogPostUrlSlugAlreadyExistException(string code = null, string message = null, string details = null, Exception innerException = null, Microsoft.Extensions.Logging.LogLevel logLevel = Microsoft.Extensions.Logging.LogLevel.Warning) : base(code, message, details, innerException, logLevel)
        {
        }

        internal BlogPostUrlSlugAlreadyExistException(System.Runtime.Serialization.SerializationInfo serializationInfo, System.Runtime.Serialization.StreamingContext context) : base(serializationInfo, context)
        {
        }

        public BlogPostUrlSlugAlreadyExistException(Guid blogId, string urlSlug)
        {
            UrlSlug = urlSlug;
            BlogId = blogId;

            Code = CmsKitErrorCodes.Blogs.UrlSlugAlreadyExist;

            WithData(nameof(UrlSlug), UrlSlug);
            WithData(nameof(BlogId), BlogId);
        }

        public string UrlSlug { get; }

        public Guid BlogId { get; }
    }
}
