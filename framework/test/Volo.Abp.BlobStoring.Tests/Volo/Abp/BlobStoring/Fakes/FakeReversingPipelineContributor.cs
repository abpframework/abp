using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BlobStoring.Fakes;

/// <summary>
/// A test contributor that reverses the BLOB bytes on both save and get.
/// </summary>
public class FakeReversingPipelineContributor : IBlobPipelineContributor, ITransientDependency
{
    public Task<Stream> OnSaveAsync(BlobPipelineSaveArgs args)
    {
        return Task.FromResult(Reverse(args.BlobStream));
    }

    public Task<Stream> OnGetAsync(BlobPipelineGetArgs args)
    {
        return Task.FromResult(Reverse(args.BlobStream));
    }

    private static Stream Reverse(Stream stream)
    {
        using (var memoryStream = new MemoryStream())
        {
            stream.CopyTo(memoryStream);
            var bytes = memoryStream.ToArray();
            Array.Reverse(bytes);
            return new MemoryStream(bytes);
        }
    }
}
