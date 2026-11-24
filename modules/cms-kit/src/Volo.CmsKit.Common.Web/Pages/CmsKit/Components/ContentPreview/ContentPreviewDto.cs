using System;

namespace Volo.CmsKit.Web.Pages.CmsKit.Components.ContentPreview;

[Serializable]
public class ContentPreviewDto
{
    public string Content { get; set; }

    public Guid? BlogId { get; set; }

    public bool PreventXSS { get; set; } = true;
}
