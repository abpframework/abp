namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This interface is defined to standardize to request a paged result.
    /// </summary>
    public interface IPagedResultRequest : ILimitedResultRequest
    {
        /// <summary>
        /// Skip count (beginning of the page).
        /// </summary>
        int SkipCount { get; set; }

        /// <summary>
        /// How many items in a page.
        /// </summary>
        int PageSize { get; set; }

        /// <summary>
        /// Which Page
        /// </summary>
        int PageIndex { get; set; }
    }
}