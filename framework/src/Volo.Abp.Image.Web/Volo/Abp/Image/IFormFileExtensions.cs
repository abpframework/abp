using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Image;

public static class IFormFileExtensions //TODO: Remove
{
    public async static Task<IFormFile> CompressImageAsync(
        this IFormFile formFile,
        IImageFormat imageFormat,
        IImageCompressor imageCompressor,
        CancellationToken cancellationToken = default)
    {
        if (!formFile.ContentType.StartsWith("image"))
        {
            return formFile;
        }

        if (!imageCompressor.CanCompress(imageFormat))
        {
            return formFile;
        }

        var compressedImageStream = await formFile.OpenReadStream().CompressImageAsync(imageFormat, imageCompressor, cancellationToken);

        var newFormFile = new FormFile(compressedImageStream, 0, compressedImageStream.Length, formFile.Name,
            formFile.FileName) { Headers = formFile.Headers };

        return newFormFile;
    }

    public async static Task<IFormFile> CompressImageAsync(this IFormFile formFile, IServiceProvider serviceProvider)
    {
        var imageFormatDetector = serviceProvider.GetRequiredService<IImageFormatDetector>();
        var imageCompressorSelector = serviceProvider.GetRequiredService<IImageCompressorSelector>();
        var format = imageFormatDetector.FindFormat(formFile.OpenReadStream());
        return await formFile.CompressImageAsync(format, imageCompressorSelector.FindCompressor(format));
    }

    public static async Task<IFormFile> ResizeImageAsync(
        this IFormFile formFile,
        IImageResizeParameter imageResizeParameter,
        IImageFormat imageFormat,
        IImageResizer imageResizer,
        CancellationToken cancellationToken = default)
    {
        if (!formFile.ContentType.StartsWith("image"))
        {
            return formFile;
        }

        if (!imageResizer.CanResize(imageFormat))
        {
            return formFile;
        }

        var resizedImageStream =
            await formFile.OpenReadStream().ResizeImageAsync(imageResizeParameter, imageFormat, imageResizer, cancellationToken);

        var newFormFile = new FormFile(resizedImageStream, 0, resizedImageStream.Length, formFile.Name,
            formFile.FileName) { Headers = formFile.Headers };

        return newFormFile;
    }

    public static Task<IFormFile> ResizeImageAsync(this IFormFile formFile,
        IImageResizeParameter imageResizeParameter, IServiceProvider serviceProvider)
    {
        var imageFormatDetector = serviceProvider.GetRequiredService<IImageFormatDetector>();
        var imageResizerSelector = serviceProvider.GetRequiredService<IImageResizerSelector>();
        var format = imageFormatDetector.FindFormat(formFile.OpenReadStream());
        return formFile.ResizeImageAsync(imageResizeParameter, format, imageResizerSelector.FindResizer(format));
    }
}