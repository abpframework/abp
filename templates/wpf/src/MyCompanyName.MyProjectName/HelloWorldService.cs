using Volo.Abp.DependencyInjection;

 namespace MyCompanyName.MyProjectName
{
    public class HelloWorldService : ITransientDependency
    {
        public string SayHello()
        {
            return "\tHello world!";
        }
    }
}
