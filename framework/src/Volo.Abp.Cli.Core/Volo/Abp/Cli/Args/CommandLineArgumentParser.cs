using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Args
{
    public class CommandLineArgumentParser : ICommandLineArgumentParser, ITransientDependency
    {
        public CommandLineArgs Parse(string[] args)
        {
            if (args.IsNullOrEmpty())
            {
                return CommandLineArgs.Empty();
            }

            var argumentList = args.ToList();

            //Get command

            var command = argumentList[0];
            argumentList.RemoveAt(0);

            if (!argumentList.Any())
            {
                return new CommandLineArgs(command);
            }

            //Get target

            var target = argumentList[0];
            if (target.StartsWith("-"))
            {
                target = null;
            }
            else
            {
                argumentList.RemoveAt(0);
            }

            if (!argumentList.Any())
            {
                return new CommandLineArgs(command, target);
            }

            //Get options

            var commandLineArgs = new CommandLineArgs(command, target);

            while (argumentList.Any())
            {
                var optionName = ParseOptionName(argumentList[0]);
                argumentList.RemoveAt(0);

                if (!argumentList.Any())
                {
                    commandLineArgs.Options[optionName] = null;
                    break;
                }

                if (IsOptionName(argumentList[0]))
                {
                    commandLineArgs.Options[optionName] = null;
                    continue;
                }

                commandLineArgs.Options[optionName] = argumentList[0];
                argumentList.RemoveAt(0);
            }

            return commandLineArgs;
        }

        private static bool IsOptionName(string argument)
        {
            return argument.StartsWith("-") || argument.StartsWith("--");
        }

        private static string ParseOptionName(string argument)
        {
            if (argument.StartsWith("--"))
            {
                if (argument.Length <= 2)
                {
                    throw new ArgumentException("Should specify an option name after '--' prefix!");
                }

                return argument.RemovePreFix("--");
            }

            if (argument.StartsWith("-"))
            {
                if (argument.Length <= 1)
                {
                    throw new ArgumentException("Should specify an option name after '--' prefix!");
                }

                return argument.RemovePreFix("-");
            }

            throw new ArgumentException("Option names should start with '-' or '--'.");
        }
    }
}