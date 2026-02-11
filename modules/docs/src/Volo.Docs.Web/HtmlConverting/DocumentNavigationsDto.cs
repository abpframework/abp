using System;

namespace Volo.Docs.HtmlConverting;

public class DocumentNavigationsDto
{
    public DocumentNavigationDto Previous { get; set; }
    public DocumentNavigationDto Next { get; set; }
    
    public bool HasPrevious => Previous?.Path.IsNullOrEmpty() == false;
    public bool HasNext => Next?.Path.IsNullOrEmpty() == false;
    public bool HasValues => HasPrevious || HasNext;
}