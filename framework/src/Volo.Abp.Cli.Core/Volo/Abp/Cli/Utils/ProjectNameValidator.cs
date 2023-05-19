using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.Utils;

public static class ProjectNameValidator
{
    private static readonly string[] IllegalProjectNames = new[]
    {
            "MyCompanyName.MyProjectName",
            "MyProjectName",
            "CON", //Windows doesn't accept these names as file name
            "AUX",
            "PRN",
            "COM1",
            "LPT2"
        };

    private static readonly string[] IllegalKeywords = new[]
    {
            "Blazor"
        };

    private static void ValidateParentDirectoryString(string projectName)
    {
        if (projectName.Contains(".."))
        {
            throw new CliUsageException("Project name cannot contain \"..\"! Specify a different name.");
        }
    }

    private static void ValidateSurrogateOrControlChar(string projectName)
    {
        if (projectName.Any(chr => char.IsControl(chr) || char.IsSurrogate(chr)))
        {
            throw new CliUsageException("Project name cannot contain surrogate or control characters! Specify a different name.");
        }
    }

    private static void ValidateIllegalProjectName(string projectName)
    {
        foreach (var illegalProjectName in IllegalProjectNames)
        {
            if (projectName.Equals(illegalProjectName, StringComparison.OrdinalIgnoreCase))
            {
                throw new CliUsageException("Project name cannot be \"" + illegalProjectName + "\"! Specify a different name.");
            }
        }
    }

    private static void ValidateIllegalKeywords(string projectName)
    {
        foreach (var illegalKeyword in IllegalKeywords)
        {
            if (projectName.Split(".").Contains(illegalKeyword))
            {
                throw new CliUsageException("Project name cannot contain the word \"" + illegalKeyword + "\". Specify a different name.");
            }
        }
    }

    public static void Validate(string projectName)
    {
        ValidateSurrogateOrControlChar(projectName);
        ValidateParentDirectoryString(projectName);
        ValidateIllegalProjectName(projectName);
        ValidateIllegalKeywords(projectName);
    }
}
