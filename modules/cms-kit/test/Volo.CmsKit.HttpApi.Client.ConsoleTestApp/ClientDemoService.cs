using System;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit;

public class ClientDemoService : ITransientDependency
{
    public async Task RunAsync()
    {
        await TestWithDynamicProxiesAsync();
    }

    private async Task TestWithDynamicProxiesAsync()
    {
        Console.WriteLine();
        Console.WriteLine($"***** {nameof(TestWithDynamicProxiesAsync)} *****");
    }
}
