using Shouldly;
using System.IO;
using System.Text;
using Xunit;

namespace Volo.Abp.BlobStoring.Aliyun
{
    //public class AliyunBlobContainer_Tests : BlobContainer_Tests<AbpBlobStoringAliyunTestModule>
    //{
    //    private readonly IBlobContainer<MyTestContainer> _blobContainer;
    //    private readonly string _blobname = "my-blob";
    //    private readonly string _stringContent = "my-blob-content";
    //    public AliyunBlobContainer_Tests()
    //    {
    //        _blobContainer = GetRequiredService<IBlobContainer<MyTestContainer>>();
    //    }

    //    [Fact]
    //    public async void BlobContainer_Test()
    //    {
    //        var streamContent = Encoding.Default.GetBytes(_stringContent);
    //        await _blobContainer.SaveAsync(_blobname, streamContent, true);
    //        var streamData = await _blobContainer.GetAsync(_blobname);
    //        var tfExist = await _blobContainer.ExistsAsync(_blobname);
    //        if (tfExist)
    //        {
    //            await _blobContainer.DeleteAsync(_blobname);
    //        }
    //        StreamReader reader = new StreamReader(streamData);
    //        var stringData = reader.ReadToEnd();// Encoding.Default.GetString(bytes);
    //        stringData.ShouldBe(_stringContent);
    //    }
    //}
}
