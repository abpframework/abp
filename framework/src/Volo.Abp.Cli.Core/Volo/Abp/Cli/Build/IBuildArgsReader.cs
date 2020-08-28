using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Volo.Abp.Json;

namespace Volo.Abp.Cli.Build
{
    public interface IDotNetProjectBuildConfigReader
    {
        DotNetProjectBuildConfig Read(string directoryPath);
    }

    public class FileSystemDotNetProjectBuildConfigReader : IDotNetProjectBuildConfigReader
    {
        private IJsonSerializer _jsonSerializer;

        public FileSystemDotNetProjectBuildConfigReader(IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public DotNetProjectBuildConfig Read(string directoryPath)
        {
            var buildArgs = new DotNetProjectBuildConfig();
            var solutionFiles = Directory.GetFiles(directoryPath, "*.sln", SearchOption.TopDirectoryOnly);
            if (solutionFiles.Length == 1)
            {
                buildArgs.SlFilePath = solutionFiles.First();
            }
            else
            {
                Console.WriteLine(
                    "There are more than 1 solution (*.sln) file in the directory. Searching for build-config.json file."
                );
            }

            var configFiles = Directory.GetFiles(directoryPath, "abp-build-config.json", SearchOption.TopDirectoryOnly);
            if (configFiles.Length == 1)
            {
                var configFile = configFiles.First();
                buildArgs.GitRepository = _jsonSerializer.Deserialize<GitRepository>(configFile);
            }
            else
            {
                Console.WriteLine(
                    "There are more than 1 config (abp-build-config.json) file in the directory!"
                );
            }

            SetBranchNames(buildArgs.GitRepository);

            return buildArgs;
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
    }
}
