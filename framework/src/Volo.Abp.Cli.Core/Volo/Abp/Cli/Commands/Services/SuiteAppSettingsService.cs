using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Volo.Abp.Cli.Utils;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Commands.Services;

public class SuiteAppSettingsService : ITransientDependency
{
    private const int DefaultPort = 3000;
    
    public CmdHelper CmdHelper { get; }

    public SuiteAppSettingsService(CmdHelper cmdHelper)
    {
        CmdHelper = cmdHelper;
    }
    public async Task<int> GetSuitePortAsync(string version)
    {
        var filePath = GetFilePathOrNull(version);

        if (filePath == null)
        {
            return DefaultPort;
        }

        var content = File.ReadAllText(filePath);

        var contentAsJson = JObject.Parse(content);

        var url = contentAsJson["AbpSuite"]?["ApplicationUrl"]?.ToString();

        if (url == null)
        {
            return DefaultPort;
        }
        
        return Convert.ToInt32(url.Split(":").Last());
    }

    private string GetFilePathOrNull(string version)
    {
        var suiteVersion = version;

        if (suiteVersion == null)
        {
            return null;
        }
        
        var path = Path.Combine(
            "%USERPROFILE%",
            ".dotnet",
            "tools",
            ".store",
            "volo.abp.suite",
            suiteVersion,
            "volo.abp.suite",
            suiteVersion,
            "content",
            "appsettings.json"
            );

        if (!File.Exists(path))
        {
            return null;
        }

        return path;
    }
}