using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Volo.Abp.DependencyInjection;
using Color = SixLabors.ImageSharp.Color;
using PointF = SixLabors.ImageSharp.PointF;

namespace Volo.CmsKit.Public.Web.Security.Captcha;

public class SimpleMathsCaptchaGenerator : ISingletonDependency
{
    private static Dictionary<Guid, CaptchaRequest> Session { get; set; } = new Dictionary<Guid, CaptchaRequest>();

    public CaptchaOutput Generate()
    {
        return Generate(options: null, number1: null, number2: null);
    }

    public CaptchaOutput Generate(CaptchaOptions options)
    {
        return Generate(options, number1: null, number2: null);
    }

    /// <summary>
    /// Creates a simple captcha code.
    /// </summary>
    /// <param name="options">Options for captcha generation</param>
    /// <param name="number1">First number for maths operation</param>
    /// <param name="number2">Second number for maths operation</param>
    /// <returns></returns>
    public CaptchaOutput Generate(CaptchaOptions options, int? number1, int? number2)
    {
        var random = new Random();
        options ??= new CaptchaOptions();

        number1 ??= random.Next(options.Number1MinValue, options.Number1MaxValue);
        number2 ??= random.Next(options.Number2MinValue, options.Number2MaxValue);

        var text = number1 + "+" + number2;
        var request = new CaptchaRequest
        {
            Input =
            {
                Number1 = number1.Value,
                Number2 = number2.Value
            },
            Output =
            {
                Text = text,
                Result = Calculate(number1.Value, number2.Value),
                ImageBytes =  GenerateInternal(text, options)
            }
        };

        Session[request.Output.Id] = request;
        return request.Output;
    }

    private static int Calculate(int number1, int number2)
    {
        return number1 + number2;
    }

    public void Validate(Guid requestId, int value)
    {
        var request = Session[requestId];
        if (request.Output.Result != value)
        {
            throw new CaptchaException("The captcha code doesn't match text on the picture! Please try again.");
        }
    }

    public void Validate(Guid requestId, string value)
    {
        if (int.TryParse(value, out var captchaInput))
        {
            Validate(requestId, captchaInput);
        }
        else
        {
            throw new CaptchaException("The captcha code is missing!");
        }
    }

    private byte[] GenerateInternal(string stringText, CaptchaOptions options)
    {
        byte[] result;

        using (var image = new Image<Rgba32>(options.Width, options.Height))
        {
            float position = 0;
            var random = new Random();
            var startWith = (byte)random.Next(5, 10);
            image.Mutate(ctx => ctx.BackgroundColor(Color.Transparent));
            var fontName = options.FontFamilies[random.Next(0, options.FontFamilies.Length)];
            var font = SystemFonts.CreateFont(fontName, options.FontSize, options.FontStyle);

            foreach (var character in stringText)
            {
                var text = character.ToString();
                var color = options.TextColor[random.Next(0, options.TextColor.Length)];
                var location = new PointF(startWith + position, random.Next(6, 13));
                image.Mutate(ctx => ctx.DrawText(text, font, color, location));
                position += TextMeasurer.Measure(character.ToString(), new RendererOptions(font, location)).Width;
            }

            //add rotation
            var rotation = GetRotation(options);
            image.Mutate(ctx => ctx.Transform(rotation));

            // add the dynamic image to original image
            var size = (ushort)TextMeasurer.Measure(stringText, new RendererOptions(font)).Width;
            var img = new Image<Rgba32>(size + 15, options.Height);
            img.Mutate(ctx => ctx.BackgroundColor(Color.White));

            Parallel.For(0, options.DrawLines, i =>
            {
                var x0 = random.Next(0, random.Next(0, 30));
                var y0 = random.Next(10, img.Height);

                var x1 = random.Next(30, img.Width);
                var y1 = random.Next(0, img.Height);

                img.Mutate(ctx =>
                    ctx.DrawLines(options.TextColor[random.Next(0, options.TextColor.Length)],
                                  RandomTextGenerator.GenerateNextFloat(options.MinLineThickness, options.MaxLineThickness),
                                  new PointF[] { new PointF(x0, y0), new PointF(x1, y1) })
                    );
            });

            img.Mutate(ctx => ctx.DrawImage(image, 0.80f));

            Parallel.For(0, options.NoiseRate, i =>
            {
                var x0 = random.Next(0, img.Width);
                var y0 = random.Next(0, img.Height);
                img.Mutate(
                        ctx => ctx
                            .DrawLines(options.NoiseRateColor[random.Next(0, options.NoiseRateColor.Length)],
                            RandomTextGenerator.GenerateNextFloat(0.5, 1.5), new PointF[] { new Vector2(x0, y0), new Vector2(x0, y0) })
                    );
            });

            img.Mutate(x =>
            {
                x.Resize(options.Width, options.Height);
            });

            using (var ms = new MemoryStream())
            {
                img.Save(ms, options.Encoder);
                result = ms.ToArray();
            }
        }

        return result;
    }

    private static AffineTransformBuilder GetRotation(CaptchaOptions options)
    {
        var random = new Random();
        var width = random.Next(10, options.Width);
        var height = random.Next(10, options.Height);
        var pointF = new PointF(width, height);
        var rotationDegrees = random.Next(0, options.MaxRotationDegrees);
        return new AffineTransformBuilder().PrependRotationDegrees(rotationDegrees, pointF);
    }
}