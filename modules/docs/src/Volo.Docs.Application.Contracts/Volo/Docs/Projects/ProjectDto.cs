using System;
using System.Collections.Generic;
using Volo.Abp.Application.Dtos;

namespace Volo.Docs.Projects
{
    public class ProjectDto : EntityDto<Guid>
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string DefaultDocumentName { get; set; }

        public string NavigationDocumentName { get; set; }

        public string GoogleCustomSearchId { get; set; }

        public Dictionary<string, object> ExtraProperties { get; protected set; }

        public string MainWebsiteUrl { get; set; }
    }
}