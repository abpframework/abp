using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Volo.Abp.Json;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Threading;

namespace Volo.Abp.Cli.NuGet
{
    public class NuGetService : ISingletonDependency
    {
        protected IJsonSerializer JsonSerializer { get; }
        protected ICancellationTokenProvider CancellationTokenProvider { get; }


        public NuGetService(
            IJsonSerializer jsonSerializer,
            ICancellationTokenProvider cancellationTokenProvider)
        {
            JsonSerializer = jsonSerializer;
            CancellationTokenProvider = cancellationTokenProvider;
        }

        public async Task<string> GetLatestVersionAsync(string packageId,bool includePreviews = false)
        {
            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                
                var responseMessage = await client.GetAsync(
                    $"https://api.nuget.org/v3-flatcontainer/{packageId.ToLowerInvariant()}/index.json",
                    CancellationTokenProvider.Token);
                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception("Remote server returns error! HTTP status code: " + responseMessage.StatusCode);
                }

                var result = await responseMessage.Content.ReadAsStringAsync();

                var versions = JsonSerializer.Deserialize<NuGetVersionResultDto>(result).Versions;
                if (!includePreviews)
                {
                    versions = versions
                        .Where(x => !x.Contains("beta") && !x.Contains("preview") && !x.Contains("alpha") && !x.Contains("rc"))
                        .ToList();
                }

                return versions.Count > 0 ? versions.Last() : null;
            }
        }

        public class NuGetVersionResultDto
        {
            [JsonProperty("versions")]
            public List<string> Versions { get; set; }
        }
    }
}