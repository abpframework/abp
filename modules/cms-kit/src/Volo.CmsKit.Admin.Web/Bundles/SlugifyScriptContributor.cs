using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc.UI.Bundling;

namespace Volo.CmsKit.Admin.Web.Bundles
{
    public class SlugifyScriptContributor : BundleContributor
    {
        public override void ConfigureBundle(BundleConfigurationContext context)
        {
            context.Files.Add("/libs/slugify/slugify.js");
        }
    }
}
