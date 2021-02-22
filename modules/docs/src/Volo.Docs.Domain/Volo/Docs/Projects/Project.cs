using System;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Volo.Docs.Projects
{
    public class Project : AggregateRoot<Guid>
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

        /// <summary>
        /// The document to be used for the parameters file (index).
        /// </summary>
        public virtual string ParametersDocumentName { get; protected set; }

        public virtual string MinimumVersion { get; set; }

        /// <summary>
        /// The source of the documents (e.g. Github).
        /// </summary>
        public virtual string DocumentStoreType { get; protected set; }

        public virtual string MainWebsiteUrl { get; set; }

        public virtual string LatestVersionBranchName { get; set; }

        protected Project()
        {
        }

        public Project(
            Guid id,
            [NotNull] string name,
            [NotNull] string shortName,
            [NotNull] string documentStoreType,
            [NotNull] string format,
            [NotNull] string defaultDocumentName = "Index",
            [NotNull] string navigationDocumentName = "docs-nav.json",
            [NotNull] string parametersDocumentName = "docs-params.json")
            : base(id)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            ShortName = Check.NotNullOrWhiteSpace(shortName, nameof(shortName));
            DocumentStoreType = Check.NotNullOrWhiteSpace(documentStoreType, nameof(documentStoreType));
            Format = Check.NotNullOrWhiteSpace(format, nameof(format));
            DefaultDocumentName = Check.NotNullOrWhiteSpace(defaultDocumentName, nameof(defaultDocumentName));
            NavigationDocumentName = Check.NotNullOrWhiteSpace(navigationDocumentName, nameof(navigationDocumentName));
            ParametersDocumentName = Check.NotNullOrWhiteSpace(parametersDocumentName, nameof(parametersDocumentName));

            NormalizeShortName();
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }

        public void SetFormat(string format)
        {
            Format = Check.NotNullOrWhiteSpace(format, nameof(format));
        }

        public void SetNavigationDocumentName(string navigationDocumentName)
        {
            NavigationDocumentName = Check.NotNullOrWhiteSpace(navigationDocumentName, nameof(navigationDocumentName));
        }

        public void SetParametersDocumentName(string parametersDocumentName)
        {
            ParametersDocumentName = Check.NotNullOrWhiteSpace(parametersDocumentName, nameof(parametersDocumentName));
        }

        public void SetDefaultDocumentName(string defaultDocumentName)
        {
            DefaultDocumentName = Check.NotNullOrWhiteSpace(defaultDocumentName, nameof(defaultDocumentName));
        }

        private void NormalizeShortName()
        {
            ShortName = ShortName.ToLower();
        }
    }
}
