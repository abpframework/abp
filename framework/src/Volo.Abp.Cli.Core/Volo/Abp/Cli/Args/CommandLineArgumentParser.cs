using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Args;

public class CommandLineArgumentParser : ICommandLineArgumentParser, ITransientDependency
{
    public CommandLineArgs Parse(string[] args)
    {
        if (args.IsNullOrEmpty())
        {
            return CommandLineArgs.Empty();
        }

        var argumentList = args.ToList();

        //Command

        var command = argumentList[0];
        argumentList.RemoveAt(0);

        if (!argumentList.Any())
        {
            return new CommandLineArgs(command);
        }

        //Target

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

        //Options

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

    public CommandLineArgs Parse(string lineText)
    {
        return Parse(GetArgsArrayFromLine(lineText));
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

    private static string[] GetArgsArrayFromLine(string lineText)
    {
        var args = new List<string>();
        var currentArgBuilder = new StringBuilder();
        string currentArg = null;
        bool isInQuotes = false;
        for (int i = 0; i < lineText.Length; i++)
        {
            var c = lineText[i];
            if (c == ' ' && !isInQuotes)
            {
                currentArg = currentArgBuilder.ToString();
                if (!currentArg.IsNullOrWhiteSpace())
                {
                    args.Add(currentArg);
                }

                currentArgBuilder = new StringBuilder();
            }
            else
            {
                if (c == '\"')
                {
                    isInQuotes = !isInQuotes;
                }
                else
                {
                    currentArgBuilder.Append(c);
                }
            }
        }

        currentArg = currentArgBuilder.ToString();
        if (!currentArg.IsNullOrWhiteSpace())
        {
            args.Add(currentArg);
        }

        return args.ToArray();
    }
}
