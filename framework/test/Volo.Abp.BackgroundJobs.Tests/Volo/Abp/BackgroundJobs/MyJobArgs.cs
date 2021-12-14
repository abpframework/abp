namespace Volo.Abp.BackgroundJobs;

public class MyJobArgs
{
    public string Value { get; set; }

    public MyJobArgs()
    {

    }

    public MyJobArgs(string value)
    {
        Value = value;
    }
}
