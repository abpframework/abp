using NUglify;
using Volo.Abp.Minify.Html;

namespace Volo.Abp.Minify.NUglify
{
    public class NUglifyHtmlMinifier : NUglifyMinifierBase, IHtmlMinifier
    {
        protected override UglifyResult UglifySource(string source, string fileName)
        {
            return Uglify.Html(source, sourceFileName: fileName);
        }
    }
}