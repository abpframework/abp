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
        /// <summary>
        /// Name of the project for display purposes.
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// A short name of the project to be seen in URLs.
        /// </summary>
        public virtual string ShortName { get; protected set; }

        /// <summary>
        /// The format of the document (e.g. "md" for Markdown, "html" for HTML).
        /// </summary>
        public virtual string Format { get; protected set; }

        /// <summary>
        /// The document for the initial page.
        /// </summary>
        public virtual string DefaultDocumentName { get; protected set; }

        /// <summary>
        /// The document to be used for the navigation menu (index).
        /// </summary>
        public virtual string NavigationDocumentName { get; protected set; }

        public virtual string MinimumVersion { get; set; }

        /// <summary>
        /// The source of the documents (e.g. Github).
        /// </summary>
        public virtual string DocumentStoreType { get; protected set; }

        public virtual string GoogleCustomSearchId { get; set; }

        public virtual Dictionary<string, object> ExtraProperties { get; protected set; }

        public virtual string MainWebsiteUrl { get; protected set; }

        public virtual string LatestVersionBranchName { get; set; }

        protected Project()
        {
            ExtraProperties = new Dictionary<string, object>();
        }

        public Project(Guid id, [NotNull] string name, [NotNull] string shortName, [NotNull] string defaultDocumentName, [NotNull] string navigationDocumentName, string googleCustomSearchId, string mainWebsiteUrl)
        {
            Id = id;
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ShortName = Check.NotNullOrWhiteSpace(shortName, nameof(shortName));
            DefaultDocumentName = Check.NotNullOrWhiteSpace(defaultDocumentName, nameof(defaultDocumentName));
            NavigationDocumentName = Check.NotNullOrWhiteSpace(navigationDocumentName, nameof(navigationDocumentName));
            GoogleCustomSearchId = Check.NotNullOrWhiteSpace(googleCustomSearchId, nameof(googleCustomSearchId));
            ExtraProperties = new Dictionary<string, object>();
            MainWebsiteUrl = mainWebsiteUrl;
        }
    }
}