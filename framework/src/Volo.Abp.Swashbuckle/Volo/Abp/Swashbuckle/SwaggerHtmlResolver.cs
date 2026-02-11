using System.IO;
using System.Reflection;
using System.Text;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Swashbuckle;

public class SwaggerHtmlResolver : ISwaggerHtmlResolver, ITransientDependency
{
    public virtual Stream Resolver()
    {
        var scriptBundleScript = "<script src=\"%(ScriptBundlePath)\" charset=\"utf-8\"></script>";
        var abpSwaggerScript = "<script src=\"ui/abp.swagger.js\" charset=\"utf-8\"></script>";
        var stream = typeof(SwaggerUIOptions).GetTypeInfo().Assembly
            .GetManifestResourceStream("Swashbuckle.AspNetCore.SwaggerUI.index.html");

        var html = new StreamReader(stream!)
            .ReadToEnd()
            .Replace(scriptBundleScript, $"{scriptBundleScript}\n{abpSwaggerScript}");

        return new MemoryStream(Encoding.UTF8.GetBytes(html));
    }
}
