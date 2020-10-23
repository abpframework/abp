using System.IO;

namespace Volo.Abp
{
    public interface ISwaggerHtmlResolver
    {
        Stream Resolver();
    }
}
