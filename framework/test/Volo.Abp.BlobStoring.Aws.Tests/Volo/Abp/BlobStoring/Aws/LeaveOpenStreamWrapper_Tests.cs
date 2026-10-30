using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.BlobStoring.Fakes;
using Xunit;

namespace Volo.Abp.BlobStoring.Aws;

public class LeaveOpenStreamWrapper_Tests
{
    [Fact]
    public async Task Should_Bridge_The_Old_Read_Overload_To_The_Modern_One_And_Leave_The_Stream_Open()
    {
        var content = new byte[1000];
        new Random(42).NextBytes(content);
        using var inner = new FakeModernAsyncOnlyStream(new MemoryStream(content));
        var wrapper = new LeaveOpenStreamWrapper(inner);

        // The SDK reads over the old overload; the inner stream only supports the modern one
        var buffer = new byte[content.Length];
        var totalReadCount = 0;
        while (totalReadCount < buffer.Length)
        {
            var readCount = await wrapper.ReadAsync(buffer, totalReadCount, buffer.Length - totalReadCount, default);
            if (readCount == 0)
            {
                break;
            }

            totalReadCount += readCount;
        }

        totalReadCount.ShouldBe(content.Length);
        buffer.SequenceEqual(content).ShouldBeTrue();

        wrapper.Dispose();

        // The wrapped stream stays open (a disposed one would throw here)
        (await inner.ReadAsync(new byte[1].AsMemory(), default)).ShouldBe(0);
    }

    [Fact]
    public void Should_Translate_A_Failing_Length_Probe_For_The_Sdk()
    {
        // The SDK only handles NotSupportedException while probing the content length
        using var inner = new FakeIoFailingLengthStream(new MemoryStream(new byte[10]));
        var wrapper = new LeaveOpenStreamWrapper(inner);

        Should.Throw<NotSupportedException>(() => wrapper.Length);
        Should.Throw<NotSupportedException>(() => wrapper.Position);
    }
}
