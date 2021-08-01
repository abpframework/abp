using System;
using System.Collections.Generic;
using System.Text.Json;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.OpenIddict.Tokens
{
    public class OpenIddictToken : AggregateRoot<Guid>
    {
        public Guid? AuthorizationId { get; protected set; }

        public Guid? ApplicationId { get; protected set; }

        public DateTime? CreationDate { get; protected set; }

        public DateTime? ExpirationDate { get; protected set; }

        public string Payload { get; protected set; }

        //JSON object
        public Dictionary<string, JsonElement> Properties { get; protected set; }

        public DateTime? RedemptionDate { get; protected set; }

        public string ReferenceId { get; protected set; }

        public string Status { get; protected set; }

        public string Subject { get; protected set; }

        public string Type { get; protected set; }

        protected OpenIddictToken() { }

        public OpenIddictToken(Guid id) : base(id)
        {
            Properties = new();
        }

        public void SetAuthorizationId(Guid? authorizationId)
        {
            AuthorizationId = authorizationId;
        }

        public void SetApplicationId(Guid? applicationId)
        {
            ApplicationId = applicationId;
        }

        public void SetCreationDate(DateTime? date)
        {
            CreationDate = date;
        }

        public void SetExpirationDate(DateTime? date)
        {
            ExpirationDate = date;
        }

        public void SetPayload(string payload)
        {
            Payload = payload;
        }

        public void SetProperties(Dictionary<string, JsonElement> properties)
        {
            Properties = properties;
        }

        public void SetRedemptionDate(DateTime? date)
        {
            RedemptionDate = date;
        }

        public void SetReferenceId(string referenceId)
        {
            ReferenceId = referenceId;
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
    }
}
