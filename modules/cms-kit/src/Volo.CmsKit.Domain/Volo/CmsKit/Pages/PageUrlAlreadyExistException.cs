using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;
using Volo.Abp;

namespace Volo.CmsKit.Pages
{
    [Serializable]
    public class PageUrlAlreadyExistException : BusinessException
    {
        public PageUrlAlreadyExistException([NotNull] string url)
        {
            Code = CmsKitErrorCodes.Pages.UrlAlreadyExist;
            WithData(nameof(Page.Url), url);
        }
        
        public PageUrlAlreadyExistException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}