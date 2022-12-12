using System;

namespace Volo.CmsKit.Public.Web.Security.Captcha;

public class CaptchaOutput
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public byte[] ImageBytes { get; set; }
    public int Result { get; set; }
}

public class CaptchaInput
{
    public int Number1 { get; set; }
    public int Number2 { get; set; }
}

public class CaptchaRequest
{
    public CaptchaInput Input { get; set; }
    public CaptchaOutput Output { get; set; }

    public CaptchaRequest()
    {
        Input = new CaptchaInput();
        Output = new CaptchaOutput
        {
            Id = Guid.NewGuid()
        };
    }
}