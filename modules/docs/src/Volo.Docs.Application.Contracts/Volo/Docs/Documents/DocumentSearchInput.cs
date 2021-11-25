using System;

namespace Volo.Docs.Documents
{
    public class DocumentSearchInput
    {
        public string Context { get; set; }

        public Guid ProjectId { get; set; }

        public string LanguageCode { get; set; }

        public string Version { get; set; }

        public int? SkipCount { get; set; }

        public int? MaxResultCount { get; set; }
    }
}