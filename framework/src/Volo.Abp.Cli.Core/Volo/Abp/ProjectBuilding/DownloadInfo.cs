using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.ProjectBuilding.Building;

namespace Volo.Abp.ProjectBuilding
{
    [Table("Downloads")]
    public class DownloadInfo : CreationAuditedAggregateRoot<int>
    {
        [Required]
        [StringLength(128)]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(42)]
        public string TemplateName { get; set; }

        [Required]
        public DatabaseProvider DatabaseProvider { get; set; }

        [Required]
        [StringLength(20)]
        public string Version { get; set; }

        public int CreationDuration { get; set; }

        public DownloadInfo(string projectName, string templateName, DatabaseProvider databaseProvider, string version, int creationDuration)
        {
            ProjectName = projectName;
            TemplateName = templateName;
            DatabaseProvider = databaseProvider;
            Version = version;
            CreationDuration = creationDuration;
        }
    }
}
