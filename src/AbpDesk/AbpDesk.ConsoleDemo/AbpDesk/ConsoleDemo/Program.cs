using System;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Modularity.PlugIns;

namespace AbpDesk.ConsoleDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var application = services.AddApplication<AbpDeskConsoleDemoModule>(options =>
            {
                AddPlugIns(options);
            });

            using (var scope = services.BuildServiceProvider().CreateScope())
            {
                application.Initialize(scope.ServiceProvider);

                RunDemo(application);

                Console.WriteLine("Press ENTER to run again...");
                Console.ReadLine();

                application.Shutdown();
            }
        }

        private static void RunDemo(AbpApplication application)
        {
            application
                .ServiceProvider
                .GetRequiredService<TicketLister>()
                .List();

            application
                .ServiceProvider
                .GetRequiredService<UserLister>()
                .List();

            application
                .ServiceProvider
                .GetRequiredService<BlogPostLister>()
                .List();
        }

        private static void AddPlugIns(AbpApplicationCreationOptions options)
        {
            options.PlugInSources.Add(
                new FolderPlugInSource(
                    Path.Combine(
                        Directory.GetCurrentDirectory(),
                        @"..\AbpDesk.SamplePlugInModule\bin\Debug\netstandard1.6\"
                    )
                )
                {
                    Filter = filePath => filePath.EndsWith("AbpDesk.SamplePlugInModule.dll")
                }
            );
        }
    }
}
