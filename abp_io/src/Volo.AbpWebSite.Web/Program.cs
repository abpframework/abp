using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Volo.AbpWebSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHostInternal(args).Run();
        }

        internal static IWebHost BuildWebHostInternal(string[] args) =>
            new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
    }
}
