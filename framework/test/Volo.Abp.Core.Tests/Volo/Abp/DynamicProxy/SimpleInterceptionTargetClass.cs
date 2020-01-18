using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.TestBase.Logging;

namespace Volo.Abp.DynamicProxy
{
    public class SimpleInterceptionTargetClass : ICanLogOnObject
    {
        public List<string> Logs { get; } = new List<string>();

        public virtual void DoIt()
        {
            Logs.Add("ExecutingDoIt");
        }

        public virtual int GetValue()
        {
            Logs.Add("ExecutingGetValue");
            return 42;
        }

        public virtual async Task<int> GetValueAsync()
        {
            Logs.Add("EnterGetValueAsync");
            await Task.Delay(5).ConfigureAwait(false);
            Logs.Add("MiddleGetValueAsync");
            await Task.Delay(5).ConfigureAwait(false);
            Logs.Add("ExitGetValueAsync");
            return 42;
        }

        public virtual async Task DoItAsync()
        {
            Logs.Add("EnterDoItAsync");
            await Task.Delay(5).ConfigureAwait(false);
            Logs.Add("MiddleDoItAsync");
            await Task.Delay(5).ConfigureAwait(false);
            Logs.Add("ExitDoItAsync");
        }
    }
}