using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using Volo.Abp.Domain.Entities.Auditing;

namespace Volo.Abp.OpenIddict.Applications
{
    public class OpenIddictApplication : FullAuditedAggregateRoot<Guid>
    {
        public string ClientId { get; protected set; }

        public string ClientSecret { get; protected set; }

        public string ConsentType { get; protected set; }

        public string DisplayName { get; protected set; }

        //JSON Object
        public Dictionary<CultureInfo, string> DisplayNames { get; protected set; }

        //JSON Array
        public HashSet<string> Permissions { get; protected set; }

        //JSON Array
        public HashSet<string> PostLogoutRedirectUris { get; protected set; }

        //JSON Object
        public Dictionary<string, JsonElement> Properties { get; protected set; }

        //JSON Array
        public HashSet<string> RedirectUris { get; protected set; }

        //JSON Array
        public HashSet<string> Requirements { get; protected set; }

        public string Type { get; protected set; }

        protected OpenIddictApplication() { }

        public OpenIddictApplication(Guid id, string clientId)
        : base(id)
        {
            ClientId = clientId;
            DisplayNames = new();
            Permissions = new();
            PostLogoutRedirectUris = new();
            Properties = new();
            RedirectUris = new();
            Requirements = new();
        }

        public void SetClientId(string identifier)
        {
            ClientId = identifier;
        }

        public void SetClientSecret(string secret)
        {
            ClientSecret = secret;
        }

        public void SetClientType(string type)
        {
            Type = type;
        }

        public void SetConsentType(string type)
        {
            ConsentType = type;
        }

        public void SetDisplayName(string name)
        {
            DisplayName = name;
        }

        public void SetDisplayNames(Dictionary<CultureInfo, string> displayNames)
        {
            DisplayNames = new Dictionary<CultureInfo, string>(displayNames);
        }

        public void SetPermissions(HashSet<string> permissions)
        {
            Permissions = permissions;
        }

        public void SetPostLogoutRedirectUris(HashSet<string> addresses)
        {
            PostLogoutRedirectUris = addresses;
        }

        public void SetProperties(Dictionary<string, JsonElement> properties)
        {
            Properties = new Dictionary<string, JsonElement>(properties);
        }

        public void SetRedirectUris(HashSet<string> addresses)
        {
            RedirectUris = addresses;
        }

        public void SetRequirements(HashSet<string> requirements)
        {
            Requirements = requirements;
        }
    }
}
