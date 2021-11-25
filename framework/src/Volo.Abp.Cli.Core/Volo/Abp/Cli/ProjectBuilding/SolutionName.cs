using System;

namespace Volo.Abp.Cli.ProjectBuilding;

public class SolutionName
{
    public string FullName { get; }

    public string CompanyName { get; }

    public string ProjectName { get; }

    private SolutionName(string fullName, string companyName, string projectName)
    {
        FullName = fullName;
        CompanyName = companyName;
        ProjectName = projectName;
    }

    public static SolutionName Parse(string fullName, string microserviceName)
    {
        return new SolutionName(fullName + "." + microserviceName, fullName, microserviceName);
    }

    public static SolutionName Parse(string fullName)
    {
        if (fullName.Length < 1)
        {
            throw new UserFriendlyException("Solution name is not valid. It should be 1 character length at minimum.");
        }

        string companyName = null;
        var projectName = fullName;

        if (fullName.Contains("."))
        {
            var lastDotIndex = fullName.LastIndexOf(".", StringComparison.OrdinalIgnoreCase);
            companyName = fullName.Substring(0, lastDotIndex);
            projectName = fullName.Substring(lastDotIndex + 1);

            if (companyName.Length < 1)
            {
                throw new UserFriendlyException("Solution name is not valid. Company name should be 1 character length at minimum.");
            }

            if (projectName.Length < 1)
            {
                throw new UserFriendlyException("Solution name is not valid. Project name should be 1 character length at minimum.");
            }
        }

        return new SolutionName(fullName, companyName, projectName);
    }
}
