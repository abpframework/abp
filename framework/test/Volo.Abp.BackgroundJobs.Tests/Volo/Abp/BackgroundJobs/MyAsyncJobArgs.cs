namespace Volo.Abp.BackgroundJobs;

public class MyAsyncJobArgs
{
    public string Value { get; set; }

    public MyAsyncJobArgs()
    {

    }

    public MyAsyncJobArgs(string value)
    {
        Value = value;
    }
}
