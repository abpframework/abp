using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.CmsKit.Localization;
using Microsoft.Extensions.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using ImageMagick;

namespace Volo.CmsKit.Public.Web.Security.Captcha;

public class SimpleMathsCaptchaGenerator : ITransientDependency
{
    protected IStringLocalizer<CmsKitResource> Localizer { get; }
    protected IDistributedCache<CaptchaOutput> Cache { get; }

    public SimpleMathsCaptchaGenerator(IStringLocalizer<CmsKitResource> localizer, IDistributedCache<CaptchaOutput> cache)
    {
        Localizer = localizer;
        Cache = cache;
    }

    public virtual Task<CaptchaOutput> GenerateAsync()
    {
        return GenerateAsync(options: null, number1: null, number2: null);
    }

    public virtual Task<CaptchaOutput> GenerateAsync(CaptchaOptions options)
    {
        return GenerateAsync(options, number1: null, number2: null);
    }

    /// <summary>
    /// Creates a simple captcha code.
    /// </summary>
    /// <param name="options">Options for captcha generation</param>
    /// <param name="number1">First number for maths operation</param>
    /// <param name="number2">Second number for maths operation</param>
    /// <returns></returns>
    public virtual async Task<CaptchaOutput> GenerateAsync(CaptchaOptions options, int? number1, int? number2)
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
                ImageBytes = GenerateInternal(text, options)
            }
        };

        await Cache.SetAsync(request.Output.Id.ToString("N"), request.Output, new DistributedCacheEntryOptions 
        {
            AbsoluteExpiration = DateTimeOffset.Now.Add(options.DurationOfValidity)
        });

        return request.Output;
    }

    private static int Calculate(int number1, int number2)
    {
        return number1 + number2;
    }

    public virtual async Task ValidateAsync(Guid requestId, int value)
    {
        var request = await Cache.GetAsync(requestId.ToString("N"));
        
        if(request == null || request.Result != value) 
        {
            throw new UserFriendlyException(Localizer["CaptchaCodeErrorMessage"]);
        }
    }

    public virtual async Task ValidateAsync(Guid requestId, string value)
    {
        if (int.TryParse(value, out var captchaInput))
        {
            await ValidateAsync(requestId, captchaInput);
        }
        else
        {
            throw new UserFriendlyException(Localizer["CaptchaCodeMissingMessage"]);
        }
    }

    private byte[] GenerateInternal(string stringText, CaptchaOptions options)
    {
        try
        {
            var random = new Random();
        
            var drawables = new Drawables()
                .FontPointSize(options.FontSize)
                .StrokeColor(MagickColors.Transparent);
            
            var family = MagickNET.FontFamilies.FirstOrDefault();
            if (!family.IsNullOrWhiteSpace())
            {
                drawables = drawables.Font(family, options.FontStyle, FontWeight.Normal, FontStretch.Normal);
            }

            var size = (ushort)(drawables.FontTypeMetrics(stringText)?.TextWidth ?? 0);
            using var image = new MagickImage(MagickColors.White, size + 15, options.Height);

            double position = 0;
            var startWith = (byte)random.Next(5, 10);

            foreach (var character in stringText)
            {
                var text = character.ToString();
                var color = options.TextColor[random.Next(0, options.TextColor.Length)];
                drawables.FillColor(new MagickColor(color.R, color.G, color.B, color.A))
                    .Text(startWith + position,
                        RandomTextGenerator.GenerateNextFloat(image.BaseHeight / 2.3, image.BaseHeight / 1.7), text);

                position += drawables.FontTypeMetrics(text)?.TextWidth ?? 0;
            }

            // add rotation
            var rotation = GetRotation(options);
            drawables.Rotation(rotation);

            drawables.Draw(image);

            Parallel.For(0, options.DrawLines, _ =>
            {
                // ReSharper disable once AccessToDisposedClosure
                if (image is { IsDisposed: false })
                {
                    var x0 = random.Next(0, random.Next(0, 30));
                    var y0 = random.Next(10, image.Height);

                    var x1 = random.Next(30, image.Width);
                    var y1 = random.Next(0, image.Height);

                    image.Draw(new Drawables()
                        .StrokeColor(options.DrawLinesColor[random.Next(0, options.DrawLinesColor.Length)])
                        .StrokeWidth(RandomTextGenerator.GenerateNextFloat(options.MinLineThickness,
                            options.MaxLineThickness))
                        .Line(x0, y0, x1, y1));
                }
            });

            Parallel.For(0, options.NoiseRate, _ =>
            {
                if (image is { IsDisposed: false })
                {
                    var x = random.Next(0, image.Width);
                    var y = random.Next(0, image.Height);
                    image.Draw(new Drawables()
                        .FillColor(options.NoiseRateColor[random.Next(0, options.NoiseRateColor.Length)])
                        .Point(x, y)
                    );
                }
            });

            image.Resize(new MagickGeometry(options.Width, options.Height) { IgnoreAspectRatio = true });

            return image.ToByteArray(options.Encoder);
        }
        catch (Exception e)
        {
            return Array.Empty<byte>();
        }
    }

    private double GetRotation(CaptchaOptions options)
    {
        var random = new Random();
        var rotationDegrees = random.Next(0, options.MaxRotationDegrees);
        return rotationDegrees;
    }

}