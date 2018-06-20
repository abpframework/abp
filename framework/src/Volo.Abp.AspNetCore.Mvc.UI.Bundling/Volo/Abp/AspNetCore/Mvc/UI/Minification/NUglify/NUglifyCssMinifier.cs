using NUglify;
using Volo.Abp.AspNetCore.Mvc.UI.Minification.Styles;

namespace Volo.Abp.AspNetCore.Mvc.UI.Minification.NUglify
{
    public class NUglifyCssMinifier : NUglifyMinifierBase, ICssMinifier
    {
        protected override UglifyResult UglifySource(string source, string fileName)
        {
            return Uglify.Css(source, fileName);
        }
    }
}