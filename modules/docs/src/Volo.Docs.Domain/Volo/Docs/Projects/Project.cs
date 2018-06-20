using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.Domain.Entities;

namespace Volo.Docs.Projects
{
    public class Project : AggregateRoot<Guid>, IHasExtraProperties
    {
        public virtual string Name { get; protected set; }

        public virtual string ShortName { get; protected set; }

        public virtual string Format { get; protected set; }

        public virtual string DefaultDocumentName { get; protected set; }

        public virtual string NavigationDocumentName { get; protected set; }

        public virtual string DocumentStoreType { get; protected set; }

        public virtual string GoogleCustomSearchId { get; set; }

        public Dictionary<string, object> ExtraProperties { get; protected set; }

        protected Project()
        {
            ExtraProperties = new Dictionary<string, object>(); //TODO: Needed (by mongodb)..?
        }

        public Project(Guid id, [NotNull] string name, [NotNull] string shortName, [NotNull] string defaultDocumentName, [NotNull] string navigationDocumentName, string googleCustomSearchId)
        {
            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ShortName = Check.NotNullOrWhiteSpace(shortName, nameof(shortName));
            DefaultDocumentName = Check.NotNullOrWhiteSpace(defaultDocumentName, nameof(defaultDocumentName));
            NavigationDocumentName = Check.NotNullOrWhiteSpace(navigationDocumentName, nameof(navigationDocumentName));
            GoogleCustomSearchId = Check.NotNullOrWhiteSpace(googleCustomSearchId, nameof(googleCustomSearchId));
            ExtraProperties = new Dictionary<string, object>();
        }
    }
}