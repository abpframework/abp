using System.Threading;
using System.Threading.Tasks;

namespace System.IO;

public static class AbpStreamExtensions
{
    public static byte[] GetAllBytes(this Stream stream)
    {
        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using (var ms = stream.CreateMemoryStream())
        {
            return ms.ToArray();
        }
    }

    public static async Task<byte[]> GetAllBytesAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream is MemoryStream memoryStream)
        {
            return memoryStream.ToArray();
        }

        using (var ms = await stream.CreateMemoryStreamAsync(cancellationToken))
        {
            return ms.ToArray();
        }
    }

    public static Task CopyToAsync(this Stream stream, Stream destination, CancellationToken cancellationToken)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        
        return stream.CopyToAsync(
            destination,
            81920, //this is already the default value, but needed to set to be able to pass the cancellationToken
            cancellationToken
        );
    }
    
    public async static Task<MemoryStream> CreateMemoryStreamAsync(this Stream stream, CancellationToken cancellationToken = default)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream, cancellationToken);
        
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        
        memoryStream.Position = 0;
        return memoryStream;
    }

    public static MemoryStream CreateMemoryStream(this Stream stream)
    {
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        
        var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);
        
        if (stream.CanSeek)
        {
            stream.Position = 0;
        }
        
        memoryStream.Position = 0;
        return memoryStream;
    }
}
