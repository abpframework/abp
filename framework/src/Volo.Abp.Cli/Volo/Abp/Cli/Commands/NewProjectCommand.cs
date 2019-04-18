using System;
using System.Threading.Tasks;
using Volo.Abp.Cli.Args;
using Volo.Abp.DependencyInjection;
using Volo.Abp.SolutionTemplating;
using Volo.Abp.SolutionTemplating.Building;

namespace Volo.Abp.Cli.Commands
{
    public class NewProjectCommand : IConsoleCommand, ITransientDependency
    {
        protected SolutionBuilder SolutionBuilder { get; }

        public NewProjectCommand(SolutionBuilder solutionBuilder)
        {
            SolutionBuilder = solutionBuilder;
        }

        public async Task ExecuteAsync(CommandLineArgs commandLineArgs)
        {
            if (commandLineArgs.Target == null)
            {
                Console.WriteLine("Project name is missing.");
                Console.WriteLine("Usage:");
                Console.WriteLine("  abp new <project-name>");
                Console.WriteLine("Example:");
                Console.WriteLine("  abp new Acme.BookStore");
            }

            Console.WriteLine("TODO: Create new project");

            //await SolutionBuilder.BuildAsync(
            //    null,
            //    commandLineArgs.Target,
            //    DatabaseProvider.EntityFrameworkCore,
            //    "...",
            //    true
            //);
        }
    }
}