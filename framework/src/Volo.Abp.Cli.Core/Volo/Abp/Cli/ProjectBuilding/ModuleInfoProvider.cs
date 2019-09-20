using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Json;
using Volo.Abp.Cli.Http;
using Volo.Abp.Cli.ProjectBuilding.Building;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Http;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.ProjectBuilding
{
    public class ModuleInfoProvider: IModuleInfoProvider, ITransientDependency
    {
        public IJsonSerializer JsonSerializer { get; }
        public ICancellationTokenProvider CancellationTokenProvider { get; }

        public ModuleInfoProvider(IJsonSerializer jsonSerializer, ICancellationTokenProvider cancellationTokenProvider)
        {
            JsonSerializer = jsonSerializer;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task<ModuleInfo> GetAsync(string name)
        {
            var moduleList = await GetModuleListAsync();

            var module = moduleList.FirstOrDefault(m => m.Name == name);

            if (module == null)
            {
                throw new Exception("Module not found!");
            }

            return module;
        }

        private async Task<List<ModuleInfo>> GetModuleListAsync()
        {
            using (var client = new CliHttpClient())
            {
                var responseMessage = await client.GetAsync(
                    $"{CliUrls.WwwAbpIo}api/download/modules/",
                    CancellationTokenProvider.Token
                );

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception("Remote server returns error! HTTP status code: " + responseMessage.StatusCode);
                }

                var result = await responseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<ModuleInfo>>(result);
            }
        }
    }
}
