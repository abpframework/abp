using System.Runtime.Serialization;
using Volo.Abp;

namespace Volo.CmsKit.Public.Web.Security.Captcha;

public class CaptchaException : UserFriendlyException
{
    public CaptchaException(string message) : base(message)
    {
    }

    public CaptchaException(SerializationInfo serializationInfo, StreamingContext context) : base(serializationInfo, context)
    {
    }
}