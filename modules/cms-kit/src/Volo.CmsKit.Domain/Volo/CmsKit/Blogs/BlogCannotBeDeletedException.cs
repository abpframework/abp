using System;
using System.Runtime.Serialization;
using Volo.Abp;

namespace Volo.CmsKit.Blogs
{
    [Serializable]
    public class BlogCannotBeDeletedException : BusinessException
    {
        public BlogCannotBeDeletedException(Guid id)
        {
            Code = CmsKitErrorCodes.Blogs.SlugAlreadyExist;
            WithData("Id", id);
        }
        
        public BlogCannotBeDeletedException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}