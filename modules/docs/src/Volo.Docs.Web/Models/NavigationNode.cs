using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Volo.Docs.Documents
{
    public class NavigationNode
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("items")]
        public List<NavigationNode> Items { get; set; }

        public bool IsLeaf => !HasChildItems;

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
}