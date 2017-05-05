using System.Collections.Generic;
using Volo.Abp.TestBase.Logging;
using Volo.DependencyInjection;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class SimpleInterceptionTargetClass : ITransientDependency, ICanLogOnObject
    {
        public List<string> Logs { get; } = new List<string>();

        public virtual int GetValue()
        {
            Logs.Add("ExecutingGetValue");
            return 42;
        }
    }
}