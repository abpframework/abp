using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.OpenIddict.Scopes
{
    public class OpenIddictScope : AggregateRoot<Guid>
    {
        public string Description { get; protected set; }

        //JSON Object
        public Dictionary<CultureInfo, string> Descriptions { get; protected set; }

        public string DisplayName { get; protected set; }

        //JSON Object
        public Dictionary<CultureInfo, string> DisplayNames { get; protected set; }

        public string Name { get; protected set; }

        //JSON Object
        public Dictionary<string, JsonElement> Properties { get; protected set; }

        //JSON array
        public HashSet<string> Resources { get; protected set; }

        protected OpenIddictScope() { }

        public OpenIddictScope(Guid id) : base(id)
        {
            Descriptions = new();
            DisplayNames = new();
            Properties = new();
        }

        public void SetDescription(string description)
        {
            Description = description;
        }

        public void SetDisplayName(string displayName)
        {
            DisplayName = displayName;
        }

        public void SetName(string name)
        {
            Name = name;
        }

        public void SetDisplayNames(Dictionary<CultureInfo, string> names)
        {
            DisplayNames = names;
        }

        public void SetDescriptions(Dictionary<CultureInfo, string> descriptions)
        {
            Descriptions = descriptions;
        }

        public void SetResources(HashSet<string> resources)
        {
            Resources = resources;
        }

        public void SetProperties(Dictionary<string, JsonElement> properties)
        {
            Properties = properties;
        }
    }
}
