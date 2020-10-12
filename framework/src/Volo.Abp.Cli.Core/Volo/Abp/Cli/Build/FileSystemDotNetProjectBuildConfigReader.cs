using System;
using System.IO;
using System.Linq;
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
                var configFileContent = File.ReadAllText(configFile);
                buildConfig.GitRepository = _jsonSerializer.Deserialize<GitRepository>(configFileContent);

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

            Console.WriteLine(
                "There are more than 1 config (abp-build-config.json) file in the directory!"
            );

            throw new Exception("There is no solution file (*.sln) or " + _buildConfigName + " in the working directory !");
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
