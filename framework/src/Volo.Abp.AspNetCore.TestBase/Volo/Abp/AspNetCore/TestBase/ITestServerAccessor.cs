using Microsoft.AspNetCore.TestHost;

namespace Volo.Abp.AspNetCore.TestBase;

public interface ITestServerAccessor
{
    TestServer Server { get; set; }
}
