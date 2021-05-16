using System;
using System.Collections.Generic;
using System.Linq;

namespace Volo.Abp.Cli.Utils
{
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

        private static readonly char[] IllegalChars = new[]
        {
            '/',
            '?',
            ':',
            '&',
            '\\',
            '*',
            '\'',
            '<',
            '>',
            '|',
            '#',
            '%',
        };

        private static bool HasParentDirectoryString(string projectName)
        {
            return projectName.Contains("..");
        }

        private static bool HasIllegalChar(string projectName)
        {
            foreach (var illegalChar in IllegalChars)
            {
                if (projectName.Contains(illegalChar))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool HasSurrogateOrControlChar(string projectName)
        {
            return projectName.Any(chr => char.IsControl(chr) || char.IsSurrogate(chr));
        }

        private static bool IsIllegalProjectName(string projectName)
        {
            foreach (var illegalProjectName in IllegalProjectNames)
            {
                if (projectName.Equals(illegalProjectName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        public static bool IsValid(string projectName)
        {
            if (projectName == null)
            {
                throw new CliUsageException("Project name cannot be empty!");
            }

            if (HasIllegalChar(projectName))
            {
                return false;
            }

            if (HasSurrogateOrControlChar(projectName))
            {
                return false;
            }

            if (HasParentDirectoryString(projectName))
            {
                return false;
            }

            if (IsIllegalProjectName(projectName))
            {
                return false;
            }

            return true;
        }
    }
}
