using System;
using System.Collections.Generic;

namespace Volo.Docs.Documents
{
    [Serializable]
    public class DocumentEto
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Name { get; set; }

        public string Version { get; set; }

        public string LanguageCode { get; set; }

        public string FileName { get; set; }

        public string Content { get; set; }

        public string Format { get; set; }

        public string EditLink { get; set; }

        public string RootUrl { get; set; }

        public string RawRootUrl { get; set; }

        public string LocalDirectory { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdatedTime { get; set; }

        public DateTime? LastSignificantUpdateTime { get; set; }

        public DateTime LastCachedTime { get; set; }
    }
}