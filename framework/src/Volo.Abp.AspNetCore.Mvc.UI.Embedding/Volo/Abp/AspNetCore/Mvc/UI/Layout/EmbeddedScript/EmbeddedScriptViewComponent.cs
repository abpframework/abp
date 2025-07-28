using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Volo.Abp.AspNetCore.Mvc.UI.Embedding.Volo.Abp.AspNetCore.Mvc.UI.Layout.EmbeddedScript;

public class EmbeddedScriptViewComponent : AbpViewComponent
{
    public virtual IViewComponentResult Invoke()
    {
        return View("~/Volo/Abp/AspNetCore/Mvc/UI/Layout/EmbeddedScript/Default.cshtml");
    }
}