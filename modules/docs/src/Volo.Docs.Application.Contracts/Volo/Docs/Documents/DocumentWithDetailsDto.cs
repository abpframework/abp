using System;
using System.Collections.Generic;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    [Serializable]
    public class DocumentWithDetailsDto
    {
        public virtual string Name { get; set; }

        public virtual string Version { get; set; }

        public virtual string LanguageCode { get; set; }

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

        public ProjectDto Project { get; set; }

        public List<DocumentContributorDto> Contributors { get; set; }
    }
}