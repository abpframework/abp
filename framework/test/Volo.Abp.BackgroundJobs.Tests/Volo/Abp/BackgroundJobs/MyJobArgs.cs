namespace Volo.Abp.BackgroundJobs
{
    [BackgroundJobName(Name)]
    public class MyJobArgs
    {
        public const string Name = "TestJobs.MyJob";

        public string Value { get; set; }

        public MyJobArgs()
        {

        }

        public MyJobArgs(string value)
        {
            Value = value;
        }
    }
}