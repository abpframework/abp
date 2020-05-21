using System;
using Volo.Abp.DependencyInjection;

namespace MyCompanyName.MyProjectName.ConsoleApp
{
    public class HelloWorldService : ITransientDependency
    {
        public void SayHello()
        {
            Console.WriteLine("Hello World!");
        }
    }
}
