using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LibGit2Sharp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.Build
{
    public class FileSystemDotNetProjectBuildConfigReader : IDotNetProjectBuildConfigReader, ITransientDependency
    {
        private readonly IJsonSerializer _jsonSerializer;
        private string _buildConfigName = "abp-build-config.json";

        public FileSystemDotNetProjectBuildConfigReader(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public DotNetProjectBuildConfig Read(string directoryPath)
        {
            var buildConfig = new DotNetProjectBuildConfig();
            var solutionFiles = Directory.GetFiles(directoryPath, "*.sln", SearchOption.TopDirectoryOnly);
            if (solutionFiles.Length == 1)
            {
                buildConfig.SlFilePath = solutionFiles.First();
                var configFile = GetClosestFile(directoryPath, _buildConfigName);
                if (configFile.IsNullOrEmpty())
                {
                    buildConfig.GitRepository = GetGitRepositoryUsingDirectory(directoryPath);
                }
                else
                {
                    var configFileContent = File.ReadAllText(configFile);
                    buildConfig.GitRepository = _jsonSerializer.Deserialize<GitRepository>(configFileContent);
                }

                SetBranchNames(buildConfig.GitRepository);

                return buildConfig;
            }

            var configFiles = Directory.GetFiles(directoryPath, _buildConfigName, SearchOption.TopDirectoryOnly);
            if (configFiles.Length == 1)
            {
                var configFile = configFiles.First();
                var configFileContent = File.ReadAllText(configFile);
                buildConfig.GitRepository = _jsonSerializer.Deserialize<GitRepository>(configFileContent);

                SetBranchNames(buildConfig.GitRepository);

                return buildConfig;
            }

            if (configFiles.Length > 1)
            {
                throw new Exception(
                    "There are more than 1 config (abp-build-config.json) file in the directory!"
                );
            }

            return new DotNetProjectBuildConfig
            {
                GitRepository = GetGitRepositoryUsingDirectory(directoryPath)
            };
        }

        private GitRepository GetGitRepositoryUsingDirectory(string directoryPath)
        {
            var directoryInfo = new DirectoryInfo(directoryPath);

            do
            {
                var gitFolderPath = string.Concat(directoryInfo.FullName, @"\.git");
                if (Directory.Exists(gitFolderPath))
                {
                    using (var repo = new Repository(string.Concat(directoryInfo.FullName, @"\.git")))
                    {
                        var repositoryName = GetRepositoryNameFromRepositoryInfo(repo);

                        return new GitRepository(repositoryName, repo.Head.FriendlyName, directoryInfo.FullName);
                    }
                }

                directoryInfo = directoryInfo.Parent;
            } while (directoryInfo?.Parent != null);

            throw new Exception("There is no solution file (*.sln) and " + _buildConfigName +
                                " in the working directory and working directory is not a GIT repository !");
        }

        private string GetRepositoryNameFromRepositoryInfo(Repository repository)
        {
            var remote = repository.Network.Remotes.FirstOrDefault(r => r.Name == "origin");
            if (remote == null)
            {
                throw new Exception("Remote origin is null for given repository !");
            }

            var remoteUrl = remote.Url;

            remoteUrl = Regex.Replace(remoteUrl, @"\.git$", "");

            remoteUrl = Regex.Replace(remoteUrl, "^git@", "https://");
            remoteUrl = Regex.Replace(remoteUrl, "^https:git@", "https://");
            remoteUrl = Regex.Replace(remoteUrl, ".com:", ".com/");

            var remoteUri = new Uri(remoteUrl);
            var pathSegments = remoteUri.AbsolutePath.Split("/", StringSplitOptions.RemoveEmptyEntries);

            if (pathSegments != null && pathSegments.Length >= 2)
            {
                var repo = pathSegments[1];
                return repo;
            }

            throw new Exception("Couldn't find repository name using remote origin url !");
        }

        private void SetBranchNames(GitRepository gitRepository)
        {
            using (var repo = new Repository(string.Concat(gitRepository.RootPath, @"\.git")))
            {
                gitRepository.BranchName = repo.Head.FriendlyName;
            }

            foreach (var dependingRepository in gitRepository.DependingRepositories)
            {
                SetBranchNames(dependingRepository);
            }
        }

        private string GetClosestFile(string directoryPath, string fileName)
        {
            var directory = new DirectoryInfo(directoryPath);
            var files = directory.GetFiles(fileName, SearchOption.TopDirectoryOnly);
            if (files.Any() && files.Length == 1)
            {
                return files.First().FullName;
            }

            do
            {
                directory = directory.Parent;
                if (directory == null)
                {
                    return string.Empty;
                }

                files = directory.GetFiles(fileName, SearchOption.TopDirectoryOnly);
                if (files.Any() && files.Length == 1)
                {
                    return files.First().FullName;
                }
            } while (true);
        }
    }
}
