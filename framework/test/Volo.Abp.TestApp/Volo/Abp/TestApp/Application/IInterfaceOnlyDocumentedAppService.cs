using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Volo.Abp.TestApp.Application;

/// <summary>
/// A service documented only on the interface to test XML doc fallback.
/// </summary>
/// <remarks>
/// Used to verify that documentation is resolved from the interface when the implementation has none.
/// </remarks>
public interface IInterfaceOnlyDocumentedAppService : IApplicationService
{
    /// <summary>
    /// Gets a message documented only on the interface.
    /// </summary>
    /// <param name="key">The message key.</param>
    /// <returns>The resolved message.</returns>
    Task<string> GetMessageAsync(string key);
}
