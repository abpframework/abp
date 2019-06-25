using Newtonsoft.Json;
using NuGet.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Cli.Http;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;
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

        public async Task<SemanticVersion> GetLatestVersionOrNullAsync(string packageId, bool includePreviews = false, bool includeNightly = false)
        {
            using (var client = new CliHttpClient())
            {
                var url = includeNightly ?
                    $"https://www.myget.org/F/abp-nightly/api/v3/flatcontainer/{packageId.ToLowerInvariant()}/index.json" :
                    $"https://api.nuget.org/v3-flatcontainer/{packageId.ToLowerInvariant()}/index.json";

                var responseMessage = await client.GetAsync(url, CancellationTokenProvider.Token);

                if (!responseMessage.IsSuccessStatusCode)
                {
                    throw new Exception("Remote server returns error! HTTP status code: " + responseMessage.StatusCode);
                }

                var result = await responseMessage.Content.ReadAsStringAsync();

                var versions = JsonSerializer.Deserialize<NuGetVersionResultDto>(result).Versions.Select(x => SemanticVersion.Parse(x));

                if (!includePreviews && !includeNightly)
                {
                    versions = versions.Where(x => !x.IsPrerelease);
                }

                return versions.Any() ? versions.Max() : null;
            }
        }

        public class NuGetVersionResultDto
        {
            [JsonProperty("versions")]
            public List<string> Versions { get; set; }
        }
    }
}
