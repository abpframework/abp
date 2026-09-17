using System;

namespace Volo.Docs.Admin.Projects;

[Serializable]
public class ProjectPdfFileDto
{
    public virtual Guid ProjectId { get; set; }
    public virtual string FileName { get; set; }
    public virtual string Version { get; set; }
    public virtual string LanguageCode { get; set; }
    public virtual DateTime CreationTime { get; set; }
    public virtual DateTime? LastModificationTime { get; set; }
}