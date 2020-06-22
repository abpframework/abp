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

        [StringLength(DocumentConsts.MaxVersionNameLength)]
        public string Version { get; set; }
 
        [StringLength(DocumentConsts.MaxLanguageCodeNameLength)]
        public string LanguageCode { get; set; }

        [StringLength(DocumentConsts.MaxFileNameNameLength)]
        public string FileName { get; set; }

        [StringLength(DocumentConsts.MaxFormatNameLength)]
        public string Format { get; set; }

        public DateTime? CreationTimeMin { get; set; }
        public DateTime? CreationTimeMax { get; set; }

        public DateTime? LastUpdatedTimeMin { get; set; }
        public DateTime? LastUpdatedTimeMax { get; set; }
        
        public DateTime? LastSignificantUpdateTimeMin { get; set; }
        public DateTime? LastSignificantUpdateTimeMax { get; set; }
        
        public DateTime? LastCachedTimeMin { get; set; }
        public DateTime? LastCachedTimeMax { get; set; }
    }
}