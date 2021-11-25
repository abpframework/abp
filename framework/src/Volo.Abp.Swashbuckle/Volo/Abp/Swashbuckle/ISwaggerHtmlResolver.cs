using System.IO;

namespace Volo.Abp.Swashbuckle;

public interface ISwaggerHtmlResolver
{
    Stream Resolver();
}
