using System;
using System.ComponentModel.DataAnnotations;
using Volo.Docs.Projects;

namespace Volo.Docs.Documents
{
    public class GetNavigationDocumentInput
    {
        public Guid ProjectId { get; set; }

        [StringLength(ProjectConsts.MaxVersionNameLength)]
        public string Version { get; set; }
    }
}