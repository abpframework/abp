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
            try
            {
                RunDemo();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR: " + ex.Message);
                Console.ReadLine();
            }
        }

        private static void RunDemo()
        {
            var services = new ServiceCollection();

            using (var application = services.AddApplication<AbpDeskConsoleDemoModule>(options =>
            {
                options.UseAutofac();
                AddPlugIns(options);
            }))
            {
                using (var scope = services.BuildServiceProviderFromFactory().CreateScope())
                {
                    application.Initialize(scope.ServiceProvider);

                    RunListers(application);

                    Console.WriteLine("Press ENTER to exit...");
                    Console.ReadLine();

                    application.Shutdown();
                }
            }
        }

        private static void RunListers(IAbpApplication application)
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
