using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp.Domain.Entities;

namespace Volo.Abp.IdentityServer.ApiResources
{
    public class ApiScope : Entity
    {
        public virtual Guid ApiResourceId { get; protected set; }

        [NotNull]
        public virtual string Name { get; protected set; }

        public virtual string DisplayName { get; set; }

        public virtual string Description { get; set; }

        public virtual bool Required { get; set; }

        public virtual bool Emphasize { get; set; }

        public virtual bool ShowInDiscoveryDocument { get; set; }

        public virtual List<ApiScopeClaim> UserClaims { get; protected set; }

        protected ApiScope()
        {

        }

        public virtual bool Equals(Guid apiResourceId, [NotNull] string name)
        {
            return ApiResourceId == apiResourceId && Name == name;
        }

        protected internal ApiScope(
            Guid apiResourceId, 
            [NotNull] string name, 
            string displayName = null, 
            string description = null, 
            bool required = false, 
            bool emphasize = false, 
            bool showInDiscoveryDocument = true)
        {
            Check.NotNull(name, nameof(name));

            ApiResourceId = apiResourceId;
            Name = name;
            DisplayName = displayName ?? name;
            Description = description;
            Required = required;
            Emphasize = emphasize;
            ShowInDiscoveryDocument = showInDiscoveryDocument;

            UserClaims = new List<ApiScopeClaim>();
        }

        public virtual void AddUserClaim([NotNull] string type)
        {
            UserClaims.Add(new ApiScopeClaim(ApiResourceId, Name, type));
        }

        public virtual void RemoveAllUserClaims()
        {
            UserClaims.Clear();
        }

        public virtual void RemoveClaim(string type)
        {
            UserClaims.RemoveAll(r => r.Type == type);
        }

        public virtual ApiScopeClaim FindClaim(string type)
        {
            return UserClaims.FirstOrDefault(r => r.Name == Name && r.Type == type);
        }

        public override object[] GetKeys()
        {
            return new object[] { ApiResourceId, Name };
        }
    }
}