using System.Threading.Tasks;

namespace Volo.CmsKit.Public.RobotsTxts;

public interface IRobotsTxtDefaultContentProvider
{
    /// <summary>
    /// Gets the content of the static robots.txt file in the web root folder, or null if there is no such file.
    /// </summary>
    Task<string> GetStaticFileContentOrNullAsync();

    /// <summary>
    /// Gets the static robots.txt file content if exists, otherwise <see cref="CmsKitRobotsTxtOptions.DefaultContent"/>.
    /// </summary>
    Task<string> GetAsync();
}
