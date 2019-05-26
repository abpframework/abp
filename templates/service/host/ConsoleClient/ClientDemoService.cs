using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace ConsoleClient
{
    public class ClientDemoService : ITransientDependency
    {
        //private readonly ISampleAppService _sampleAppService;

        public ClientDemoService(
            //ISampleAppService sampleAppService
            )
        {
            //_sampleAppService = sampleAppService;
        }

        public async Task RunAsync()
        {
            //var output = await _sampleAppService.Method1Async();
        }
    }
}