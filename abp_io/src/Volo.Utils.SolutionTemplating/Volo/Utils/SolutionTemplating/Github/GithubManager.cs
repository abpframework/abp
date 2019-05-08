using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Volo.Abp.Threading;
using Volo.Utils.SolutionTemplating.Building;

namespace Volo.Utils.SolutionTemplating.Github
{
    public class GithubManager
    {
        private static readonly object LockObject = new object();

        public string GetVersion(ProjectBuildContext context)
        {
            if (context.Request.Version.StartsWith("Branch:", StringComparison.OrdinalIgnoreCase))
            {
                //TODO: Should not cache branch files!
                return context.Request.Version.Substring("Branch:".Length);
            }

            var url = "https://api.github.com/repos/" + context.Template.GithubRepository.RepositoryNameWithOrganization + "/releases";

            using (var client = new System.Net.Http.HttpClient())
            {
                var credentials = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:", context.Template.GithubRepository.AccessToken);
                credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                client.Timeout = TimeSpan.FromMinutes(30);

                client.DefaultRequestHeaders.UserAgent.ParseAdd("MyAgent/1.0");
                var releases = JsonConvert.DeserializeObject<List<GithubRelease>>(client.GetStringAsync(url).Result);

                if (context.Request.Version == StandardVersions.LatestUnstable)
                {
                    return releases.FirstOrDefault(r => !r.IsPrerelease)?.Name;
                }

                return releases.FirstOrDefault()?.Name;
            }
        }

        public void DownloadIfNotExist(ProjectBuildContext context)
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                if (File.Exists(context.Template.FilePath))
                {
                    return;
                }

                lock (LockObject)
                {
                    if (File.Exists(context.Template.FilePath))
                    {
                        return;
                    }

                    var downloadUrl = "https://github.com/" + context.Template.GithubRepository.RepositoryNameWithOrganization + "/archive/" + context.Template.Version + ".zip";

                    var credentials = string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:", context.Template.GithubRepository.AccessToken);
                    credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(credentials));

                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                    client.Timeout = TimeSpan.FromMinutes(30);

                    var fileContents = AsyncHelper.RunSync(() => client.GetByteArrayAsync(downloadUrl));

                    File.WriteAllBytes(context.Template.FilePath, fileContents);
                }
            }
        }

        //public string GetExistingVersion(ProjectBuildContext context)
        //{
        //    var directoryInfo = new DirectoryInfo(templateRootPath);
        //    var existingFileName = directoryInfo.GetFiles().FirstOrDefault(f => f.Name.StartsWith(repositoryName))?.Name;
        //    var latestVersion = "";

        //    if (existingFileName != null)
        //    {
        //        var pFrom = existingFileName.IndexOf(repositoryName + "-", StringComparison.Ordinal) + (repositoryName + "-").Length;
        //        var pTo = existingFileName.LastIndexOf(".zip", StringComparison.Ordinal);

        //        latestVersion = existingFileName.Substring(pFrom, pTo - pFrom);
        //    }

        //    return latestVersion;
        //}
    }
}
