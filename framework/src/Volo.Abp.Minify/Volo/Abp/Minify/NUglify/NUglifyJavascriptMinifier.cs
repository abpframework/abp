using NUglify;
using Volo.Abp.Minify.Scripts;

namespace Volo.Abp.Minify.NUglify;

public class NUglifyJavascriptMinifier : NUglifyMinifierBase, IJavascriptMinifier
{
    protected override UglifyResult UglifySource(string source, string fileName)
    {
        return Uglify.Js(source, fileName);
    }
}
