namespace Volo.Abp.Cli.Build;

public interface IRepositoryBuildStatusStore
{
    GitRepositoryBuildStatus Get(string buildNamePrefix, GitRepository repository);

    void Set(string buildNamePrefix, GitRepository repository, GitRepositoryBuildStatus status);
}
