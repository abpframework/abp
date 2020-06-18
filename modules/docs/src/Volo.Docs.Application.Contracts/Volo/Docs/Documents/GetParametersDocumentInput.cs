using System;
using System.ComponentModel.DataAnnotations;
using Volo.Docs.Language;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class GetParametersDocumentInput
    {
        public Guid ProjectId { get; set; }

        [StringLength(ProjectConsts.MaxVersionNameLength)]
        public string Version { get; set; }

        [Required]
        [StringLength(LanguageConsts.MaxLanguageCodeLength)]
        public string LanguageCode { get; set; }
    }
}