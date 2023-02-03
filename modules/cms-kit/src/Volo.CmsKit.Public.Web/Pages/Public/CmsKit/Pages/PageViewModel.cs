using System;
using System.Collections.Generic;
using AutoMapper;
using Volo.CmsKit.Contents;
using Volo.CmsKit.Public.Pages;

namespace Volo.CmsKit.Public.Web.Pages.Public.CmsKit.Pages;

[AutoMap(typeof(PageDto), ReverseMap = true)]
public class PageViewModel
{
    public Guid Id { get; set; }

    public string Title { get; set; }

    public List<ContentFragment> ContentFragments { get; set; }

    public string Script { get; set; }

    public string Style { get; set; }
}