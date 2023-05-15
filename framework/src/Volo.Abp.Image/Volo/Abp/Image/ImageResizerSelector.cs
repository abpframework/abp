using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Image;

public class ImageResizerSelector : IImageResizerSelector, ITransientDependency
{
    private readonly IEnumerable<IImageResizer> _imageResizers;

    public ImageResizerSelector(IEnumerable<IImageResizer> imageResizers)
    {
        _imageResizers = imageResizers;
    }

    public IImageResizer FindResizer(IImageFormat imageFormat)
    {
        return _imageResizers.FirstOrDefault(x => x.CanResize(imageFormat));
    }
}