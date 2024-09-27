using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.AspNetCore.Mvc.UI.Theming;

namespace Volo.CmsKit.Admin.Web.Layouts;

public static class LayoutConstants
{
    public const string Account = StandardLayouts.Account;
    public const string Public = StandardLayouts.Public;
    public const string Empty = StandardLayouts.Empty;
    public const string Application = StandardLayouts.Application;
    public static SelectList GetLayoutsSelectList()
    {
        return new SelectList(new List<string> { Account, Public, Empty, Application }, Application);
    }
}
