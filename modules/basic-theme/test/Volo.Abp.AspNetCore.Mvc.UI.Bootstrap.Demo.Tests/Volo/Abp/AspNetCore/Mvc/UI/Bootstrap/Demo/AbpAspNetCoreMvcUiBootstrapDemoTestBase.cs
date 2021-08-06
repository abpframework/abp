using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Shouldly;
using Volo.Abp.AspNetCore.TestBase;

namespace Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo
{
    public abstract class AbpAspNetCoreMvcUiBootstrapDemoTestBase : AbpAspNetCoreIntegratedTestBase<TestStartup>
    {
        protected override IHostBuilder CreateHostBuilder()
        {
            return base.CreateHostBuilder()
                .UseContentRoot(CalculateContentRootPath());
        }

        private static string CalculateContentRootPath()
        {
            return CalculateContentRootPath(
                "Volo.Abp.sln",
                $"test{Path.DirectorySeparatorChar}Volo.Abp.AspNetCore.Mvc.UI.Bootstrap.Demo"
            );
        }

        private static string CalculateContentRootPath(
            string testFileNameInTheRootFolder,
            string relativeContentPath)
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            while (!ContainsFile(currentDirectory, testFileNameInTheRootFolder))
            {
                currentDirectory = new DirectoryInfo(currentDirectory).Parent.FullName;
            }

            if (!relativeContentPath.IsNullOrWhiteSpace())
            {
                currentDirectory = Path.Combine(currentDirectory, relativeContentPath);
            }

            return currentDirectory;
        }

        private static bool ContainsFile(string currentDirectory, string projectFileName)
        {
            return Directory
                .GetFiles(currentDirectory, "*.*", SearchOption.TopDirectoryOnly)
                .Any(f => Path.GetFileName(f) == projectFileName);
        }


        protected virtual async Task<string> GetResponseAsStringAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            using (var response = await GetResponseAsync(url, expectedStatusCode))
            {
                return await response.Content.ReadAsStringAsync();
            }
        }

        protected virtual async Task<HttpResponseMessage> GetResponseAsync(string url, HttpStatusCode expectedStatusCode = HttpStatusCode.OK)
        {
            using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
            {
                requestMessage.Headers.Add("Accept-Language", CultureInfo.CurrentUICulture.Name);
                var response = await Client.SendAsync(requestMessage);
                response.StatusCode.ShouldBe(expectedStatusCode);
                return response;
            }
        }
    }
}
