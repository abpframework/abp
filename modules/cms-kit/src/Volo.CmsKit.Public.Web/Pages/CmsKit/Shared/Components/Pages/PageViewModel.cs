using System;
using System.Collections.Generic;

namespace Volo.CmsKit.Public.Web.Pages.CmsKit.Shared.Components.Pages;

public class PageViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public List<ContentFragment> ContentFragments { get; set; }
}