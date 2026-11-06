using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.CmsKit.Public.RobotsTxts;

public class RobotsTxtDefaultContentProvider : IRobotsTxtDefaultContentProvider, ITransientDependency
{
    protected IWebHostEnvironment WebHostEnvironment { get; }

    protected CmsKitRobotsTxtOptions Options { get; }

    public RobotsTxtDefaultContentProvider(
        IWebHostEnvironment webHostEnvironment,
        IOptions<CmsKitRobotsTxtOptions> options)
    {
        WebHostEnvironment = webHostEnvironment;
        Options = options.Value;
    }

    public virtual async Task<string> GetStaticFileContentOrNullAsync()
    {
        var fileInfo = WebHostEnvironment.WebRootFileProvider?.GetFileInfo(Options.StaticFileName);
        if (fileInfo == null || !fileInfo.Exists || fileInfo.IsDirectory)
        {
            return null;
        }

        using (var stream = fileInfo.CreateReadStream())
        using (var reader = new StreamReader(stream))
        {
            return await reader.ReadToEndAsync();
        }
    }

    public virtual async Task<string> GetAsync()
    {
        return await GetStaticFileContentOrNullAsync() ?? Options.DefaultContent ?? string.Empty;
    }
}
