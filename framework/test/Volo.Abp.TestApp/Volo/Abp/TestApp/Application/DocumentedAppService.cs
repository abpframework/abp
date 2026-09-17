using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
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
[Description("Documented service description from attribute")]
[Display(Name = "Documented Service")]
public class DocumentedAppService : ApplicationService, IDocumentedAppService
{
    /// <summary>
    /// Gets a greeting message for the specified name.
    /// </summary>
    /// <param name="name">The name of the person to greet.</param>
    /// <returns>A personalized greeting message.</returns>
    [Description("Get greeting description from attribute")]
    [Display(Name = "Get Greeting")]
    public async Task<string> GetGreetingAsync(string name)
    {
        return await Task.FromResult($"Hello, {name}!");
    }

    /// <summary>
    /// Creates a documented item.
    /// </summary>
    /// <param name="input">The input for creating a documented item.</param>
    /// <returns>The created documented item.</returns>
    public async Task<DocumentedDto> CreateAsync(DocumentedDto input)
    {
        return await Task.FromResult(input);
    }

    /// <summary>
    /// Searches for items matching the query.
    /// </summary>
    /// <param name="query">The search query string.</param>
    /// <param name="maxResults">The maximum number of results to return.</param>
    /// <returns>A list of matching item names.</returns>
    public async Task<string> SearchAsync(
        [Description("Query param description from attribute")] [Display(Name = "Search Query")] string query,
        int maxResults)
    {
        return await Task.FromResult($"Results for {query}");
    }

    public async Task DeleteAsync(int id)
    {
        await Task.CompletedTask;
    }
}
