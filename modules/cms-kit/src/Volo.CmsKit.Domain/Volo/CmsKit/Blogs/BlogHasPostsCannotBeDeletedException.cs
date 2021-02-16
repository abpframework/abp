using System;
using System.Runtime.Serialization;
using Volo.Abp;

namespace Volo.CmsKit.Blogs
{
    [Serializable]
    public class BlogHasPostsCannotBeDeletedException : BusinessException
    {
        public BlogHasPostsCannotBeDeletedException(Guid id)
        {
            Code = CmsKitErrorCodes.Blogs.SlugAlreadyExist;
            WithData("Id", id);
        }
        
        public BlogHasPostsCannotBeDeletedException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}