using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Docs.Admin.Documents
{
    public class DocumentWithoutDetailsDto : EntityDto<Guid>
    {
        public virtual string Version { get; set; }

        public virtual string LanguageCode { get; set; }
        
        public virtual string Format { get; set; }
    }
}