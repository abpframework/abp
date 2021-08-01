using System;
using System.Collections.Generic;
using System.Text.Json;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.OpenIddict.Authorizations
{
    public class OpenIddictAuthorization : AggregateRoot<Guid>
    {
        public Guid? ApplicationId { get; protected set; }

        //JSON Object
        public Dictionary<string, JsonElement> Properties { get; protected set; }

        //JSON Array
        public HashSet<string> Scopes { get; protected set; }

        public string Status { get; protected set; }

        public string Subject { get; protected set; }

        public string Type { get; protected set; }

        public DateTime? CreationDate { get; protected set; }

        protected OpenIddictAuthorization() { }

        public OpenIddictAuthorization(Guid id)
        : base(id)
        {
            Properties = new();
            Scopes = new();
        }

        public void SetApplicationId(Guid? applicationId)
        {
            ApplicationId = applicationId;
        }

        public void SetProperties(Dictionary<string, JsonElement> properties)
        {
            Properties = properties;
        }

        public void SetScopes(HashSet<string> scopes)
        {
            Scopes = scopes;
        }

        public void SetStatus(string status)
        {
            Status = status;
        }

        public void SetSubject(string subject)
        {
            Subject = subject;
        }

        public void SetType(string type)
        {
            Type = type;
        }

        public void SetCreationDate(DateTime? date)
        {
            CreationDate = date;
        }
    }
}
