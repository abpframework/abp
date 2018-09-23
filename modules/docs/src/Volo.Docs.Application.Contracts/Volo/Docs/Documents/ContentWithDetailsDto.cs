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

        public bool IsOpened(string documentName)
        {
            if (documentName == null)
            {
                return false;
            }

            if (!HasChildItems)
            {
                return documentName == Path;
            }

            var isOpened = false;
            foreach (var n in Items)
            {
                if (n.IsOpened(documentName))
                {
                    isOpened = true;
                    break;
                }
            }

            return isOpened;
        }
    }

    public class NavigationWithDetailsDto : DocumentWithDetailsDto
    {
        [JsonProperty("items")]
        public NavigationNode RootNode { get; set; }

        public void ConvertItems()
        {
            RootNode = string.IsNullOrWhiteSpace(Content) ?
                new NavigationNode() :
                JsonConvert.DeserializeObject<NavigationNode>(Content);
        }
    }
}