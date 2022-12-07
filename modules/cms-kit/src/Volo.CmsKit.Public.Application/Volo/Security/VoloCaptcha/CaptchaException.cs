using System;

namespace Volo.CmsKit.Public.Application.Security.VoloCaptcha;

public class CaptchaException : Exception
{
    public CaptchaException(string message) : base(message)
    {
    }
}