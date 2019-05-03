using System;
using Newtonsoft.Json;

namespace Volo.Abp.SolutionTemplating.Github
{
    [JsonObject]
    public class GithubRelease
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("prerelease")]
        public bool IsPrerelease { get; set; }

        [JsonProperty("published_at")]
        public DateTime PublishTime { get; set; }
    }
}