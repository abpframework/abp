namespace Volo.Abp.UI
{
    /* TODO:
     * - define an interface to split Details, like IHasExceptionDetails
     */

    public interface IUserFriendlyException
    {
        string Message { get; }

        string Details { get; set; }
    }
}