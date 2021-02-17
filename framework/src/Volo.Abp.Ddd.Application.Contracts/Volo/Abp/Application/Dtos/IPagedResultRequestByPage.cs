namespace Volo.Abp.Application.Dtos
{

    /// <summary>
    /// This interface is defined to standardize to request a paged result.
    /// </summary>
    public interface IPagedResultRequestByPage : ILimitedResultRequest
    {
        /// <summary>
        /// Page (beginning of the page).
        /// </summary>
        int Page { get; set; }
    }
}
