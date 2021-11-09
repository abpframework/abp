using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Cli.Build;

public class DefaultBuildStatusGenerator : IBuildStatusGenerator, ITransientDependency
{
    private readonly IGitRepositoryHelper _gitRepositoryHelper;

    public DefaultBuildStatusGenerator(IGitRepositoryHelper gitRepositoryHelper)
    {
        _gitRepositoryHelper = gitRepositoryHelper;
    }

    public GitRepositoryBuildStatus Generate(
        DotNetProjectBuildConfig buildConfig,
        List<DotNetProjectInfo> changedProjects,
        List<string> buildSucceededProjects)
    {
        var lastCommitId = _gitRepositoryHelper.GetLastCommitId(buildConfig.GitRepository);
        var repoFriendlyName = _gitRepositoryHelper.GetFriendlyName(buildConfig.GitRepository);

        var status = new GitRepositoryBuildStatus(
            buildConfig.GitRepository.Name,
            repoFriendlyName
        );

        if (ShouldUpdateRepositoryCommitId(buildConfig, changedProjects, buildSucceededProjects))
        {
            status.CommitId = lastCommitId;
        }

        status.SucceedProjects = changedProjects.Where(p =>
                p.RepositoryName == buildConfig.GitRepository.Name &&
                buildSucceededProjects.Contains(p.CsProjPath)
            )
            .Select(e => new DotNetProjectBuildStatus
            {
                CsProjPath = e.CsProjPath,
                CommitId = lastCommitId
            }).ToList();

        foreach (var dependingRepository in buildConfig.GitRepository.DependingRepositories)
        {
            GenerateBuildStatusInternal(
                buildConfig,
                dependingRepository,
                changedProjects,
                buildSucceededProjects,
                status
            );
        }

        return status;
    }

    private bool ShouldUpdateRepositoryCommitId(
        DotNetProjectBuildConfig buildConfig,
        List<DotNetProjectInfo> changedProjects,
        List<string> buildSucceededProjects)
    {
        if (!buildConfig.SlFilePath.IsNullOrEmpty())
        {
            return false;
        }

        if (changedProjects.Count == 0 || buildSucceededProjects.Count == 0)
        {
            return false;
        }

        return changedProjects.Count == buildSucceededProjects.Count;
    }

    private void GenerateBuildStatusInternal(
        DotNetProjectBuildConfig buildConfig,
        GitRepository gitRepository,
        List<DotNetProjectInfo> changedProjects,
        List<string> buildSucceededProjects,
        GitRepositoryBuildStatus status)
    {
        var lastCommitId = _gitRepositoryHelper.GetLastCommitId(gitRepository);
        var repoFriendlyName = _gitRepositoryHelper.GetFriendlyName(gitRepository);

        var dependingRepositoryStatus = new GitRepositoryBuildStatus(
            gitRepository.Name,
            repoFriendlyName
        );

        if (ShouldUpdateRepositoryCommitId(buildConfig, changedProjects, buildSucceededProjects))
        {
            dependingRepositoryStatus.CommitId = lastCommitId;
        }

        dependingRepositoryStatus.SucceedProjects = changedProjects.Where(p =>
                p.RepositoryName == gitRepository.Name &&
                buildSucceededProjects.Contains(p.CsProjPath)
            )
            .Select(e => new DotNetProjectBuildStatus()
            {
                CsProjPath = e.CsProjPath,
                CommitId = lastCommitId
            }).ToList();

        foreach (var dependingRepository in gitRepository.DependingRepositories)
        {
            GenerateBuildStatusInternal(
                buildConfig,
                dependingRepository,
                changedProjects,
                buildSucceededProjects,
                dependingRepositoryStatus
            );
        }

        status.DependingRepositories.Add(dependingRepositoryStatus);
    }
}
