using System;
using Volo.Abp.Auditing;
using Volo.Abp.Domain.Entities;

namespace Volo.Docs.Projects;

public class ProjectPdfFile : Entity, IHasCreationTime, IHasModificationTime
{
    public virtual Guid ProjectId { get; set; }
    public virtual string FileName { get; set; }
    public virtual string Version { get; set; }
    public virtual string LanguageCode { get; set; }
    public virtual DateTime CreationTime { get; set; }
    public virtual DateTime? LastModificationTime { get; set; }
    
    protected ProjectPdfFile()
    {

    }
    
    public ProjectPdfFile(Guid projectId, string fileName, string version, string languageCode)
    {
        ProjectId = projectId;
        FileName = fileName;
        Version = version;
        LanguageCode = languageCode;
    }
    
    public override object[] GetKeys()
    {
        return [ProjectId, FileName];
    }
    
    public virtual bool Equals(Guid projectId, string fileName)
    {
        return ProjectId == projectId && FileName == fileName;
    }
}