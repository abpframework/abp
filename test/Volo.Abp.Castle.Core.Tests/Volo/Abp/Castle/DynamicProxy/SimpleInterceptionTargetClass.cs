using System.Collections.Generic;
using System.Threading.Tasks;
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

        public virtual async Task<int> GetValueAsync()
        {
            Logs.Add("EnterGetValueAsync");
            await Task.Delay(1);
            Logs.Add("MiddleGetValueAsync");
            await Task.Delay(1);
            Logs.Add("ExitGetValueAsync");
            return 42;
        }
    }
}