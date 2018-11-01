using System;
using Volo.Abp.Application.Dtos;

namespace Volo.Docs.Projects
{
    public class ProjectDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string DefaultDocumentName { get; set; }

        public string MainWebsiteUrl { get; set; }

        public virtual string DocumentStoreType { get; protected set; }

        public virtual string Format { get; protected set; }
    }
}