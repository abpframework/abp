using System;
using ImageMagick;

namespace Volo.CmsKit.Public.Web.Security.Captcha;

public class CaptchaOptions
{
    public MagickColor[] TextColor { get; set; } = new MagickColor[]
    {
        MagickColors.Blue, MagickColors.Black, MagickColors.Black, MagickColors.Brown, MagickColors.Gray, MagickColors.Green
    };
    public MagickColor[] DrawLinesColor { get; set; } = new MagickColor[]
    {
        MagickColors.Blue, MagickColors.Black, MagickColors.Black, MagickColors.Brown, MagickColors.Gray, MagickColors.Green
    };

    public float MinLineThickness { get; set; } = 0.7f;

    public float MaxLineThickness { get; set; } = 2.0f;

    public ushort Width { get; set; } = 180;

    public ushort Height { get; set; } = 70;

    public ushort NoiseRate { get; set; } = 500;

    public MagickColor[] NoiseRateColor { get; set; } = new MagickColor[] { MagickColors.Gray };

    public byte FontSize { get; set; } = 32;

    public FontStyleType FontStyle { get; set; } = FontStyleType.Normal;

    public EncoderTypes EncoderType { get; set; } = EncoderTypes.Png;

    public MagickFormat Encoder => RandomTextGenerator.GetEncoder(EncoderType);

    public byte DrawLines { get; set; } = 2;

    public byte MaxRotationDegrees { get; set; } = 4;

    public int Number1MinValue { get; set; } = 1;

    public int Number1MaxValue { get; set; } = 99;

    public int Number2MinValue { get; set; } = 1;

    public int Number2MaxValue { get; set; } = 99;

    public TimeSpan DurationOfValidity { get; set; } = TimeSpan.FromMinutes(10);

    public CaptchaOptions()
    {

    }
    public CaptchaOptions(int number1MinValue, int number1MaxValue, int number2MinValue, int number2MaxValue)
    {
        Number1MinValue = number1MinValue;
        Number1MaxValue = number1MaxValue;
        Number2MinValue = number2MinValue;
        Number1MaxValue = number2MaxValue;
    }
}