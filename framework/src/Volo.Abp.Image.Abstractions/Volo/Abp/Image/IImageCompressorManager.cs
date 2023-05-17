using System.IO;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Volo.Abp.Image;

public interface IImageCompressorManager // TODO: Rename to IImageCompressor 
{
    Task<Stream> CompressAsync(
        Stream stream,
        [CanBeNull] IImageFormat imageFormat = null,
        CancellationToken cancellationToken = default);
}