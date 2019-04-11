using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using VoloDocs.EntityFrameworkCore;

namespace VoloDocs.Migrator
{
    class Program
    {
        private const string ScriptFile = "Script.txt";

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

                    if (args != null && args.Contains("-script"))
                    {
                        GenerateMigrationScript(dbContext);
                        return;
                    }

                    RunMigrations(connectionString, dbContext);
                }

                Console.WriteLine("\n\nPress ENTER to exit...");
                Console.ReadLine();
            }
        }

        private static void RunMigrations(string connectionString, VoloDocsDbContext dbContext)
        {
            Console.Write("\nThis program updates an existing database or creates a new one if not exists.\n" +
                          "The following connection string will be used:\n\n" +
                          connectionString + "\n\n" +
                          "Are you sure you want to run the migration? (y/n) ");

            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                Console.WriteLine("\n\nMigrating database...");

                try
                {
                    dbContext.Database.Migrate();

                    Console.WriteLine("Migration completed.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);

                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                        Console.WriteLine(ex.Message);
                    }

                    Console.Write("\nThere was problem while applying migrations. " +
                                  "Do you want to create the migration script? (y/n) ");

                    if (Console.ReadKey().Key == ConsoleKey.Y)
                    {
                        GenerateMigrationScript(dbContext);
                    }
                }
            }
        }

        private static void GenerateMigrationScript(VoloDocsDbContext dbContext)
        {
            if (File.Exists(ScriptFile))
            {
                File.Delete(ScriptFile);
            }

            Console.Write("\nGenerating migration scripts...");

            File.WriteAllText(ScriptFile, dbContext.Database.GenerateCreateScript());

            Console.Write("\nMigration script has been created to Script.txt file");
        }
    }
}