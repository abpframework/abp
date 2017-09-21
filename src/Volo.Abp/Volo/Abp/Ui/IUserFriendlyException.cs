namespace Volo.Abp.Ui
{
    public interface IUserFriendlyException
    {
        string Message { get; }

        string Details { get; set; }
    }
}