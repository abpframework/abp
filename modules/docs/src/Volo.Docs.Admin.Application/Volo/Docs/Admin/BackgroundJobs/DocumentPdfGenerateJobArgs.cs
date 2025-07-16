using System;
using Volo.Abp.BackgroundJobs;

namespace Volo.Docs.Admin.BackgroundJobs;

public class DocumentPdfGenerateJobArgs
{
    public Guid ProjectId { get; set; }

    public string Version { get; set; }

    public string LanguageCode { get; set; }
}