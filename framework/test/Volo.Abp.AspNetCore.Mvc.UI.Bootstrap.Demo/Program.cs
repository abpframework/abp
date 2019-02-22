using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHostInternal(args).Run();
        }

        public static IWebHost BuildWebHostInternal(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
