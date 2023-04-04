using System;
using System.IO;
using System.Linq;
using LocalizationKeySynchronizer;
using Spectre.Console;
using static LocalizationKeySynchronizer.Questions;

try
{
    // Do you want to find asynchronous keys, apply changes in the exported file or replace the keys?
    var option = AnsiConsole.Prompt(
        new SelectionPrompt<string>()
            .Title(_1.Question)
            .AddChoices(_1.Options.Find, _1.Options.Apply, _1.Options.Replace));

    switch (option)
    {
        case _1.Options.Apply:
        {
            // Enter the absolute path to the exported file:
            var path = AnsiConsole.Ask<string>(_2);

            if (!File.Exists(path))
            {
                AnsiConsole.MarkupLine("[red]The file does not exist![/]");
                Exit();
            }

            if (LocalizationHelper.ApplyChanges(path))
            {
                AnsiConsole.MarkupLine("[green]The changes have been applied successfully![/]");
                Exit();
            }

            AnsiConsole.MarkupLine("[red]An error occurred while applying changes![/]");
            Exit();
            break;
        }
        case _1.Options.Find:
        {
            // The default language path
            var path = AnsiConsole.Ask<string>(_3);

            if (!LocalizationHelper.TryGetLocalization(path, out var defaultLocalizationInfo))
            {
                AnsiConsole.MarkupLine("[red]The default language path is invalid![/]");
                Exit();
            }

            var defaultCulture = new AbpLocalization(path, defaultLocalizationInfo!);

// Get others cultures
            var paths = Directory.GetFiles(Path.GetDirectoryName(path)!, "*.json",
                SearchOption.TopDirectoryOnly);

            var otherCulturePaths = paths.Select(Path.GetFileNameWithoutExtension).Where(x=>!string.IsNullOrWhiteSpace(x)).Select(x=>x!).ToList();
            // select other cultures
            paths = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .Title("Select other cultures")
                        .PageSize(10).AddChoiceGroup("All", otherCulturePaths))
                .Select(x => Path.Combine(Path.GetDirectoryName(path)!, x + ".json"))
                .ToArray();

            var otherCultures = LocalizationHelper.GetLocalizations(paths);
            var asyncLocalizations = defaultCulture.GetAsynchronousLocalizations(otherCultures);

// Find keys that do not match the number of arguments, find missing keys, or both

            var options = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title(_4.Question)
                    .PageSize(10)
                    .AddChoiceGroup("All", _4.Options.ArgumentsCount, _4.Options.MissingKeys));

// For arguments
// Find keys that do not match the number of arguments

            string? exportPath = null;
            if (options.Contains(_4.Options.ArgumentsCount))
            {
                // Should the keys that do not match the number of arguments be deleted, exported or both?

                var options2 = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .Title(_5.Question)
                        .PageSize(10)
                        .AddChoiceGroup("All", _5.Options.Delete, _5.Options.Export));

                // Delete the keys that do not match the number of arguments
                if (options2.Contains(_5.Options.Delete))
                {
                    LocalizationHelper.DeleteKeysThatDoNotMatchTheNumberOfArguments(asyncLocalizations);
                }

                // Ask for the export path and export it
                if (options2.Contains(_5.Options.Export))
                {
                    if (options.Contains(_4.Options.MissingKeys))
                    {
                        exportPath = AnsiConsole.Ask<string>(_8);
                        LocalizationHelper.Export<AbpAsyncKey>(asyncLocalizations, exportPath);
                    }
                    else
                    {
                        exportPath = AnsiConsole.Ask<string>(_6);
                        LocalizationHelper.ExportKeysThatDoNotMatchTheNumberOfArguments(asyncLocalizations, exportPath);
                    }
                }
            }

// For missing keys
// Export missing keys
            if (options.Contains(_4.Options.MissingKeys))
            {
                if (string.IsNullOrEmpty(exportPath))
                {
                    exportPath = AnsiConsole.Ask<string>(_7);
                    LocalizationHelper.ExportMissingKeys(asyncLocalizations, exportPath);
                }
            }

            break;
        }
        case _1.Options.Replace:
        {
            // The localization folder path
            var path = AnsiConsole.Ask<string>(_9);

            // Old key
            var oldKey = AnsiConsole.Ask<string>(_10);

            // New key
            var newKey = AnsiConsole.Ask<string>(_11);

            // Localization paths
            var paths = Directory.GetFiles(path, "*.json",
                SearchOption.TopDirectoryOnly);

            // Select localizations

            var localizationPaths = paths.Select(Path.GetFileNameWithoutExtension).Where(x=>!string.IsNullOrWhiteSpace(x)).Select(x=>x!).ToList();
            paths = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                        .Title("Select localizations")
                        .PageSize(10)
                        .AddChoiceGroup("All", localizationPaths))
                .Select(x => Path.Combine(path, x + ".json"))
                .ToArray();

            var cultures = LocalizationHelper.GetLocalizations(paths);

            // Replace keys
            LocalizationHelper.ReplaceKey(oldKey, newKey, cultures);

            AnsiConsole.MarkupLine("[green]The keys have been replaced successfully![/]");
            break;
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e);
    AnsiConsole.MarkupLine($"[red]{e.Message}[/]");
    Exit();
}

void Exit()
{
    AnsiConsole.MarkupLine("[red]Press any key to exit...[/]");
    Console.ReadKey();
    Environment.Exit(0);
}