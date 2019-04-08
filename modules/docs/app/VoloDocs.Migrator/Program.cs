using System;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using VoloDocs.EntityFrameworkCore;

namespace VoloDocs.Migrator
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Initializing VoloDocs Migrator ... ");

            using (var app = AbpApplicationFactory.Create<VoloDocsMigratorModule>())
            {
                app.Initialize();

                using (var dbContext = app.Resolve<VoloDocsDbContext>())
                {
                    var connectionString = dbContext.Database.GetDbConnection().ConnectionString;

                    Console.Clear();

                    Console.Write("\nThis program updates an existing database or creates a new one if not exists.\n" +
                                      "The following connection string will be used:\n\n" +
                                      new string('=', 70) + "\n" +
                                      connectionString + "\n" +
                                      new string('=', 70) + "\n\n" +
                                      "Do you confirm to run the migration? (y/n) ");

                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        try
                        {
                            Console.WriteLine("\nMigrating...");

                            dbContext.Database.Migrate();

                            Console.WriteLine("\nCompleted.");
                        }
                        catch (Exception ex)
                        {
                            Console.Write("\nAn error occured while migrating the database:\n");
                            Console.Write(ex.ToString());
                        }
                    }
                }

                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
            }
        }
    }
}
