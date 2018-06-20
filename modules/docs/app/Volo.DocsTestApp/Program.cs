using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Volo.DocsTestApp
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
