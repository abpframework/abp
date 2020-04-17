using System;
using System.ComponentModel.DataAnnotations;
using Volo.Docs.Language;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class GetDocumentResourceInput
    {
        public Guid ProjectId { get; set; }

        [Required]
        [StringLength(DocumentConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(ProjectConsts.MaxVersionNameLength)]
        public string Version { get; set; }

        [Required]
        [StringLength(LanguageConsts.MaxLanguageCodeLength)]
        public string LanguageCode { get; set; }
    }
}