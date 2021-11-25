using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Validation;
using Volo.Docs.Documents;

namespace Volo.Docs.Admin.Documents
{
    public class PullDocumentInput : PullAllDocumentInput
    {
        [DynamicStringLength(typeof(DocumentConsts), nameof(DocumentConsts.MaxNameLength))]
        public string Name { get; set; }
    }
}
