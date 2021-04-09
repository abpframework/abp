using System;
using Volo.Abp.DependencyInjection;

namespace MyCompanyName.MyProjectName
{
    public class HelloWorldService : ITransientDependency
    {
        public void SayHello()
        {
            Console.WriteLine("\tHello World!");
        }
    }
}
