using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Imaging;

public interface IImageResizerContributor
{
    Task<ImageResizeResult<Stream>> TryResizeAsync(
        Stream stream, 
        ImageResizeArgs resizeArgs, 
        [CanBeNull] string mimeType = null, 
        CancellationToken cancellationToken = default);
    
    Task<ImageResizeResult<byte[]>> TryResizeAsync(
        byte[] bytes, 
        ImageResizeArgs resizeArgs, 
        [CanBeNull] string mimeType = null, 
        CancellationToken cancellationToken = default);
}