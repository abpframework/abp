using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public BlogPostUrlSlugAlreadyExistException(string urlSlug)
        {
            UrlSlug = urlSlug;

            Code = CmsKitErrorCodes.Blogs.UrlSlugAlreadyExist;

            WithData(nameof(UrlSlug), UrlSlug);
        }

        public string UrlSlug { get; }
    }
}
