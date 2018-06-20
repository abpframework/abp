namespace Volo.Abp.Application.Dtos
{
    /// <summary>
    /// This interface is defined to standardize to request a limited result.
    /// </summary>
    public interface ILimitedResultRequest
    {
        /// <summary>
        /// Max expected result count.
        /// </summary>
        int MaxResultCount { get; set; }
    }
}