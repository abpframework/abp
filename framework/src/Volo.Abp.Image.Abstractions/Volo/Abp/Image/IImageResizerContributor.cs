using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Volo.Abp.Image;

public interface IImageResizerContributor
{
    Task<ImageContributorResult<Stream>> TryResizeAsync(Stream stream, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default);
    
    Task<ImageContributorResult<byte[]>> TryResizeAsync(byte[] bytes, ImageResizeArgs resizeArgs, string mimeType = null, CancellationToken cancellationToken = default);
}