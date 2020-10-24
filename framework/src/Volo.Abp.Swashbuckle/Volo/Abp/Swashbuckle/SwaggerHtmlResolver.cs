using System.IO;
using System.Reflection;
using System.Text;
using Swashbuckle.AspNetCore.SwaggerUI;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Swashbuckle
{
    public class SwaggerHtmlResolver : ISwaggerHtmlResolver, ITransientDependency
    {
        public virtual Stream Resolver()
        {
            var stream = typeof(SwaggerUIOptions).GetTypeInfo().Assembly
                .GetManifestResourceStream("Swashbuckle.AspNetCore.SwaggerUI.index.html");

            var html = new StreamReader(stream)
                .ReadToEnd()
                .Replace("SwaggerUIBundle(configObject)", "abp.SwaggerUIBundle(configObject)");

            return new MemoryStream(Encoding.UTF8.GetBytes(html));
        }
    }
}
