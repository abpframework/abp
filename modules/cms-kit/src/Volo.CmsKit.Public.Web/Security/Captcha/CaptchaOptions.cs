using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace Volo.CmsKit.Public.Web.Security.Captcha;

public class CaptchaOptions
{
    public Color[] TextColor { get; set; } = new Color[]
    {
        Color.Blue, Color.Black, Color.Black, Color.Brown, Color.Gray, Color.Green
    };
    public Color[] DrawLinesColor { get; set; } = new Color[]
    {
        Color.Blue, Color.Black, Color.Black, Color.Brown, Color.Gray, Color.Green
    };

    public float MinLineThickness { get; set; } = 0.7f;

    public float MaxLineThickness { get; set; } = 2.0f;

    public ushort Width { get; set; } = 180;

    public ushort Height { get; set; } = 70;

    public ushort NoiseRate { get; set; } = 500;

    public Color[] NoiseRateColor { get; set; } = new Color[] { Color.Gray };

    public byte FontSize { get; set; } = 32;

    public FontStyle FontStyle { get; set; } = FontStyle.Regular;

    public EncoderTypes EncoderType { get; set; } = EncoderTypes.Png;

    public IImageEncoder Encoder => RandomTextGenerator.GetEncoder(EncoderType);

    public byte DrawLines { get; set; } = 2;

    public byte MaxRotationDegrees { get; set; } = 4;

    public int Number1MinValue { get; set; } = 1;

    public int Number1MaxValue { get; set; } = 99;

    public int Number2MinValue { get; set; } = 1;

    public int Number2MaxValue { get; set; } = 99;

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