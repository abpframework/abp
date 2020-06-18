using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;
using Volo.Docs.Documents;

namespace Volo.Docs.Admin.Documents
{
    public class GetAllInput : PagedAndSortedResultRequestDto
    {
        public Guid? ProjectId { get; set; }

        [StringLength(DocumentConsts.MaxNameLength)]
        public string Name { get; set; }

        [StringLength(DocumentConsts.MaxLanguageCodeNameLength)]
        public string LanguageCode { get; set; }

        [StringLength(DocumentConsts.MaxVersionNameLength)]
        public string Version { get; set; }
    }
}