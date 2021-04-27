namespace Volo.Abp.EventBus
{
    public class MyExceptionHandleEventData
    {
        public int RetryAttempts { get; set; }

        public MyExceptionHandleEventData(int retryAttempts)
        {
            RetryAttempts = retryAttempts;
        }
    }
}
