using System;
using System.Buffers;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.WebUtilities;

namespace Volo.Abp.AspNetCore.Mvc.Infrastructure;

/// <summary>
/// https://github.com/dotnet/aspnetcore/issues/40928#issuecomment-1450063613
/// </summary>
public class AbpMemoryPoolHttpResponseStreamWriterFactory : IHttpResponseStreamWriterFactory
{
    public const int DefaultBufferSize = 32 * 1024;

    private readonly ArrayPool<byte> _bytePool;
    private readonly ArrayPool<char> _charPool;

    public AbpMemoryPoolHttpResponseStreamWriterFactory(ArrayPool<byte> bytePool, ArrayPool<char> charPool)
    {
        _bytePool = bytePool ?? throw new ArgumentNullException(nameof(bytePool));
        _charPool = charPool ?? throw new ArgumentNullException(nameof(charPool));
    }

    public TextWriter CreateWriter(Stream stream, Encoding encoding)
    {
        if (stream == null)
        {
            throw new ArgumentNullException(nameof(stream));
        }

        if (encoding == null)
        {
            throw new ArgumentNullException(nameof(encoding));
        }

        return new HttpResponseStreamWriter(stream, encoding, DefaultBufferSize, _bytePool, _charPool);
    }
}
