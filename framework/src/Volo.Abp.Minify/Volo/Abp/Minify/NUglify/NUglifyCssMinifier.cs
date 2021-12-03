using NUglify;
using Volo.Abp.Minify.Styles;

namespace Volo.Abp.Minify.NUglify;

public class NUglifyCssMinifier : NUglifyMinifierBase, ICssMinifier
{
    protected override UglifyResult UglifySource(string source, string fileName)
    {
        return Uglify.Css(source, fileName);
    }
}
