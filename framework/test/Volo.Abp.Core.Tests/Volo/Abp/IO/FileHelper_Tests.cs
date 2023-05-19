using Shouldly;
using Xunit;

namespace Volo.Abp.IO;

public class FileHelper_Tests
{
    [Fact]
    public void GetExtension()
    {
        FileHelper.GetExtension("test").ShouldBeNull();
        FileHelper.GetExtension("te.st").ShouldBe("st");
    }
}
