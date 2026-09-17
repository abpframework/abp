using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.TestApp.Application.Dto;

namespace Volo.Abp.TestApp.Application;

/// <summary>
/// A documented application service for testing API descriptions.
/// </summary>
/// <remarks>
/// This service is used in integration tests to verify XML doc extraction.
/// </remarks>
public interface IDocumentedAppService : IApplicationService
{
    /// <summary>
    /// Gets a greeting message for the specified name.
    /// </summary>
    /// <param name="name">The name of the person to greet.</param>
    /// <returns>A personalized greeting message.</returns>
    Task<string> GetGreetingAsync(string name);

    /// <summary>
    /// Creates a documented item.
    /// </summary>
    /// <param name="input">The input for creating a documented item.</param>
    /// <returns>The created documented item.</returns>
    Task<DocumentedDto> CreateAsync(DocumentedDto input);

    /// <summary>
    /// Searches for items matching the query.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <param name="maxResults">The maximum number of results to return.</param>
    /// <returns>A list of matching item names.</returns>
    Task<string> SearchAsync(string query, int maxResults);

    Task DeleteAsync(int id);
}
