using System;
using System.Collections.Generic;

namespace Volo.Abp.Cli.Build;

/// <summary>
/// Represents a source code repository
/// </summary>
public class GitRepository
{
    /// <summary>
    /// Name of the repository
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Branch of the repository
    /// </summary>
    public string BranchName { get; set; }

    /// <summary>
    /// Root path of the repository which contains .git folder
    /// </summary>
    public string RootPath { get; set; }

    public List<GitRepository> DependingRepositories { get; set; }

    public List<string> IgnoredDirectories { get; set; }

    public GitRepository(string name, string branchName, string rootPath)
    {
        Name = name;
        BranchName = branchName;
        RootPath = rootPath;
        DependingRepositories = new List<GitRepository>();
        IgnoredDirectories = new List<string>();
    }

    public string GetUniqueName(string prefix)
    {
        var name = Name + "_" + BranchName;
        foreach (var dependingRepository in DependingRepositories)
        {
            AddToUniqueName(dependingRepository, name);
        }

        return (prefix.IsNullOrEmpty() ? "" : prefix + "_") + name.ToMd5();
    }

    private void AddToUniqueName(GitRepository gitRepository, string name)
    {
        name += "_" + gitRepository.Name + "_" + gitRepository.BranchName;

        foreach (var dependingRepository in gitRepository.DependingRepositories)
        {
            AddToUniqueName(dependingRepository, name);
        }
    }

    public string FindRepositoryOf(string csProjFilePath)
    {
        if (csProjFilePath.StartsWith(RootPath))
        {
            return Name;
        }

        foreach (var dependingRepository in DependingRepositories)
        {
            var name = FindRepositoryOfInternal(dependingRepository, csProjFilePath);
            if (!string.IsNullOrEmpty(name))
            {
                return name;
            }
        }

        return null;
    }

    private string FindRepositoryOfInternal(GitRepository repository, string csProjFilePath)
    {
        if (csProjFilePath.StartsWith(repository.RootPath))
        {
            return repository.Name;
        }

        foreach (var dependingRepository in repository.DependingRepositories)
        {
            return FindRepositoryOfInternal(dependingRepository, csProjFilePath);
        }

        return null;
    }
}
