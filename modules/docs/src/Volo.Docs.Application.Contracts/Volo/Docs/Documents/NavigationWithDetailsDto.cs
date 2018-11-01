using System;
using Newtonsoft.Json;

namespace Volo.Docs.Documents
{
    public class NavigationWithDetailsDto : DocumentWithDetailsDto
    {
        [JsonProperty("items")]
        public NavigationNode RootNode { get; set; }

        public void ConvertItems()
        {
            if (!SuccessfullyRetrieved || Content.IsNullOrEmpty())
            {
                RootNode = new NavigationNode();
                return;
            }

            try
            {
                RootNode = JsonConvert.DeserializeObject<NavigationNode>(Content);
            }
            catch (JsonException ex)
            {
                //todo: should log the exception?
                RootNode = new NavigationNode();
            }
        }
    }
}