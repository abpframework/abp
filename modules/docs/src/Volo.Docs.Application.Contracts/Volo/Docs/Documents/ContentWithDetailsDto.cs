using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class DocumentWithDetailsDto
    {
        public string Title { get; set; }

        public string Content { get; set; }

        public string Format { get; set; }

        public string EditLink { get; set; }

        public string RootUrl { get; set; }

        public string RawRootUrl { get; set; }

        public string Version { get; set; }

        public string LocalDirectory { get; set; }

        public string FileName { get; set; }

        public ProjectDto Project { get; set; }

        public bool SuccessfullyRetrieved { get; set; }
    }

    public class NavigationNode
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("items")]
        public List<NavigationNode> Items { get; set; }

        public bool HasChildItems => Items != null && Items.Any();

        public bool IsEmpty => Text == null && Path == null;

        public bool IsSelected(string documentName)
        {
            if (documentName == null)
            {
                return false;
            }

            if (string.Equals(documentName, Path, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            if (Items == null)
            {
                return false;
            }

            foreach (var childItem in Items)
            {
                if (childItem.IsSelected(documentName))
                {
                    return true;
                }
            }

            return false;
        }
    }

    public class NavigationWithDetailsDto : DocumentWithDetailsDto
    {
        [JsonProperty("items")]
        public NavigationNode RootNode { get; set; }

        public void ConvertItems()
        {
            try
            {
                RootNode = JsonConvert.DeserializeObject<NavigationNode>(Content);
            }
            catch (JsonException)
            {
                //todo: should log the exception?
                RootNode = new NavigationNode();
            }
        }
    }
}