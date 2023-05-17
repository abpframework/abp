using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Volo.Abp.Image;

public static class StreamExtensions //TODO: Remove
{
    public static Stream CompressImage(this Stream stream, IImageFormat imageFormat, IImageCompressor imageCompressor)
    {
        if (!imageCompressor.CanCompress(imageFormat))
        {
            return stream;
        }

        return imageCompressor.Compress(stream);
    }
    
    public static Stream CompressImage(this Stream stream, IServiceProvider serviceProvider)
    {
        var imageFormatDetector = serviceProvider.GetRequiredService<IImageFormatDetector>();
        var imageCompressorSelector = serviceProvider.GetRequiredService<IImageCompressorSelector>();
        var format = imageFormatDetector.FindFormat(stream);
        return stream.CompressImage(format, imageCompressorSelector.FindCompressor(format));
    }
    
    public static Task<Stream> CompressImageAsync(this Stream stream, IImageFormat imageFormat, IImageCompressor imageCompressor, CancellationToken cancellationToken = default)
    {
        if (!imageCompressor.CanCompress(imageFormat))
        {
            return Task.FromResult(stream);
        }

        return imageCompressor.CompressAsync(stream, cancellationToken);
    }
    
    public static Task<Stream> CompressImageAsync(this Stream stream, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var imageFormatDetector = serviceProvider.GetRequiredService<IImageFormatDetector>();
        var imageCompressorSelector = serviceProvider.GetRequiredService<IImageCompressorSelector>();
        var format = imageFormatDetector.FindFormat(stream);
        return stream.CompressImageAsync(format, imageCompressorSelector.FindCompressor(format), cancellationToken);
    }
    
    public static Stream ResizeImage(this Stream stream, IImageResizeParameter imageResizeParameter, IImageFormat imageFormat, IImageResizer imageResizer)
    {
        if (!imageResizer.CanResize(imageFormat))
        {
            return stream;
        }

        return imageResizer.Resize(stream, imageResizeParameter);
    }
    
    public static Stream ResizeImage(this Stream stream, IImageResizeParameter imageResizeParameter, IServiceProvider serviceProvider)
    {
        var imageFormatDetector = serviceProvider.GetRequiredService<IImageFormatDetector>();
        var imageResizerSelector = serviceProvider.GetRequiredService<IImageResizerSelector>();
        var format = imageFormatDetector.FindFormat(stream);
        return stream.ResizeImage(imageResizeParameter, format, imageResizerSelector.FindResizer(format));
    }
    
    public static Task<Stream> ResizeImageAsync(this Stream stream, IImageResizeParameter imageResizeParameter, IImageFormat imageFormat, IImageResizer imageResizer, CancellationToken cancellationToken = default)
    {
        if (!imageResizer.CanResize(imageFormat))
        {
            return Task.FromResult(stream);
        }

        return imageResizer.ResizeAsync(stream, imageResizeParameter, cancellationToken);
    }
    
    public static Task<Stream> ResizeImageAsync(this Stream stream, IImageResizeParameter imageResizeParameter, IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var imageFormatDetector = serviceProvider.GetRequiredService<IImageFormatDetector>();
        var imageResizerSelector = serviceProvider.GetRequiredService<IImageResizerSelector>();
        var format = imageFormatDetector.FindFormat(stream);
        return stream.ResizeImageAsync(imageResizeParameter, format, imageResizerSelector.FindResizer(format), cancellationToken);
    }
}