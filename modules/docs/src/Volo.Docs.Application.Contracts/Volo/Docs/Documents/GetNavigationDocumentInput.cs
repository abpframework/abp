using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.Docs.Language;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class GetNavigationDocumentInput
    {
        public Guid ProjectId { get; set; }

        [DynamicStringLength(typeof(ProjectConsts), nameof(ProjectConsts.MaxVersionNameLength))]
        public string Version { get; set; }

        [Required]
        [DynamicStringLength(typeof(LanguageConsts), nameof(LanguageConsts.MaxLanguageCodeLength))]
        public string LanguageCode { get; set; }
    }
}