using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace Volo.Docs.Documents
{
    public class Document : AggregateRoot<Guid>
    {
        public virtual Guid ProjectId { get; protected set; }

        public virtual string Name { get; protected set; }

        public virtual string Version { get; protected set; }

        public virtual string LanguageCode { get; protected set; }

        public virtual string FileName { get; set; }

        public virtual string Content { get; set; }

        public virtual string Format { get; set; }

        public virtual string EditLink { get; set; }

        public virtual string RootUrl { get; set; }

        public virtual string RawRootUrl { get; set; }

        public virtual string LocalDirectory { get; set; }

        public virtual DateTime CreationTime { get; set; }
        
        public virtual DateTime LastUpdatedTime { get; set; }

        public virtual DateTime LastCachedTime { get; set; }

        public virtual List<DocumentContributor> Contributors { get; set; }

        protected Document()
        {
            Contributors = new List<DocumentContributor>();
            ExtraProperties = new Dictionary<string, object>();
        }

        public Document(
            Guid id,
            Guid projectId,
            [NotNull] string name,
            [NotNull] string version,
            [NotNull] string languageCode,
            [NotNull] string fileName,
            [NotNull] string content,
            [NotNull] string format,
            [NotNull] string editLink,
            [NotNull] string rootUrl,
            [NotNull] string rawRootUrl,
            [NotNull] string localDirectory,
            DateTime creationTime,
            DateTime lastUpdatedTime,
            DateTime lastCachedTime
        )
        {
            Id = id;
            ProjectId = projectId;

            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
            Version = Check.NotNullOrWhiteSpace(version, nameof(version));
            LanguageCode = Check.NotNullOrWhiteSpace(languageCode, nameof(languageCode));
            FileName = Check.NotNullOrWhiteSpace(fileName, nameof(fileName));
            Content = Check.NotNullOrWhiteSpace(content, nameof(content));
            Format = Check.NotNullOrWhiteSpace(format, nameof(format));
            EditLink = Check.NotNullOrWhiteSpace(editLink, nameof(editLink));
            RootUrl = Check.NotNullOrWhiteSpace(rootUrl, nameof(rootUrl));
            RawRootUrl = Check.NotNullOrWhiteSpace(rawRootUrl, nameof(rawRootUrl));
            LocalDirectory = Check.NotNull(localDirectory, nameof(localDirectory));

            CreationTime = creationTime;
            LastUpdatedTime = lastUpdatedTime;
            LastCachedTime = lastCachedTime;

            Contributors = new List<DocumentContributor>();
            ExtraProperties = new Dictionary<string, object>();
        }

        public virtual void AddContributor(string username, string userProfileUrl, string avatarUrl)
        {
            Contributors.AddIfNotContains(new DocumentContributor(Id, username, userProfileUrl, avatarUrl));
        }

        public virtual void RemoveAllContributors()
        {
            Contributors.Clear();
        }

        public virtual void RemoveContributor(string username, string userProfileUrl, string avatarUrl)
        {
            Contributors.RemoveAll(r =>
                r.Username == username && r.UserProfileUrl == userProfileUrl && r.AvatarUrl == avatarUrl);
        }

        public virtual DocumentContributor FindContributor(string username, string userProfileUrl, string avatarUrl)
        {
            return Contributors.FirstOrDefault(r =>
                r.Username == username && r.UserProfileUrl == userProfileUrl && r.AvatarUrl == avatarUrl);
        }
    }
}