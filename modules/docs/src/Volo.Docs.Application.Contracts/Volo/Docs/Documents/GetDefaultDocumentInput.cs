using System;
using System.ComponentModel.DataAnnotations;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class GetDefaultDocumentInput
    {
        public Guid ProjectId { get; set; }

        [StringLength(ProjectConsts.MaxVersionNameLength)]
        public string Version { get; set; }

        [StringLength(ProjectConsts.MaxLanguageCodeLength)]
        public string LanguageCode { get; set; }
    }
}