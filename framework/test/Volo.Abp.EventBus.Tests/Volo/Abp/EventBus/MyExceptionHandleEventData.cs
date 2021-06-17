namespace Volo.Abp.EventBus
{
    public class MyExceptionHandleEventData
    {
        public int Value { get; set; }

        public MyExceptionHandleEventData(int value)
        {
            Value = value;
        }
    }
}
