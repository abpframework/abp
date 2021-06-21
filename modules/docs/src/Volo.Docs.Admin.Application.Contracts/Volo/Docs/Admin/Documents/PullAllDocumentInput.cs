using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.Docs.Documents;

namespace Volo.Docs.Admin.Documents
{
    public class PullAllDocumentInput
    {
        public Guid ProjectId { get; set; }

        [DynamicStringLength(typeof(DocumentConsts), nameof(DocumentConsts.MaxLanguageCodeNameLength))]
        public string LanguageCode { get; set; }

        [DynamicStringLength(typeof(DocumentConsts), nameof(DocumentConsts.MaxVersionNameLength))]
        public string Version { get; set; }
    }
}