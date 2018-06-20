using NUglify;
using NUglify.JavaScript;
using Volo.Abp.AspNetCore.Mvc.UI.Minification.Scripts;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Minification.NUglify
{
    public class NUglifyJavascriptMinifier : NUglifyMinifierBase, IJavascriptMinifier
    {
        protected override UglifyResult UglifySource(string source, string fileName)
        {
            return Uglify.Js(source, fileName);
        }
    }
}