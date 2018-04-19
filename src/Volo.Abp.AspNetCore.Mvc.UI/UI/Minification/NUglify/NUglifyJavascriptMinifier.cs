using NUglify;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.AspNetCore.Mvc.UI.Minification.NUglify
{
    public class NUglifyJavascriptMinifier : IJavascriptMinifier, ITransientDependency
    {
        public string Minify(string source, string fileName = null)
        {
            var result = Uglify.Js(source, fileName);
            CheckErrors(result);
            return result.Code;
        }

        private static void CheckErrors(UglifyResult result)
        {
            if (result.HasErrors)
            {
                throw new NUglifyException(
                    "There are some errors on uglifying the given source code!",
                    result.Errors
                );
            }
        }
    }
}