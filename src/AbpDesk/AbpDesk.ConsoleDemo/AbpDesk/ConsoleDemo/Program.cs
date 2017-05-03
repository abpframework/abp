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

            var application = services.AddApplication<AbpDeskConsoleDemoModule>(options =>
            {
                options.UseAutofac();
                AddPlugIns(options);
            });
            
            using (var scope = services.BuildAutofacServiceProvider().CreateScope())
            {
                application.Initialize(scope.ServiceProvider);

                RunListers(application);

                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();

                application.Shutdown();
            }
        }

        private static void RunListers(AbpApplication application)
        {
            application
                .ServiceProvider
                .GetRequiredService<TicketLister2>()
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
