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

        public virtual DateTime? CreationTime { get; set; }

        public virtual DateTime? LastUpdatedTime { get; set; }

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

    public static class NavigationNodeExtension
    {
        public static IEnumerable<NavigationNode> GetAllNodes(this IEnumerable<NavigationNode> source, Func<NavigationNode, IEnumerable<NavigationNode>> selector)
        {
            if (source == null)
            {
                yield break;
            }

            foreach (var item in source)
            {
                yield return item;
                foreach (var subItem in GetAllNodes(selector(item), selector))
                {
                    yield return subItem;
                }
            }
        }
    }
}