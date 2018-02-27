using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Volo.Abp.DynamicProxy;
using Volo.Abp.Threading;

namespace Volo.Abp.Castle.DynamicProxy
{
    public class CastleAbpMethodInvocationAdapter : IAbpMethodInvocation
    {
        public object[] Arguments => Invocation.Arguments;

        public IReadOnlyDictionary<string, object> ArgumentsDictionary => _lazyArgumentsDictionary.Value;
        private readonly Lazy<IReadOnlyDictionary<string, object>> _lazyArgumentsDictionary;

        public Type[] GenericArguments => Invocation.GenericArguments;

        public object TargetObject => Invocation.InvocationTarget ?? Invocation.MethodInvocationTarget;

        public MethodInfo Method => Invocation.MethodInvocationTarget ?? Invocation.Method;

        public object ReturnValue
        {
            get => _actualReturnValue ?? Invocation.ReturnValue;
            set => Invocation.ReturnValue = value;
        }

        private object _actualReturnValue;

        protected IInvocation Invocation { get; }

        public CastleAbpMethodInvocationAdapter(IInvocation invocation)
        {
            Invocation = invocation;

            _lazyArgumentsDictionary = new Lazy<IReadOnlyDictionary<string, object>>(GetArgumentsDictionary);
        }

        public void Proceed()
        {
            Invocation.Proceed();

            if (Invocation.Method.IsAsync())
            {
                AsyncHelper.RunSync(() => (Task)Invocation.ReturnValue);
            }
        }

        public Task ProceedAsync()
        {
            Invocation.Proceed();
            _actualReturnValue = Invocation.ReturnValue;
            return Invocation.Method.IsAsync()
                ? (Task)_actualReturnValue
                : Task.FromResult(_actualReturnValue);
        }

        private IReadOnlyDictionary<string, object> GetArgumentsDictionary()
        {
            var dict = new Dictionary<string, object>();

            var methodParameters = Method.GetParameters();
            for (int i = 0; i < methodParameters.Length; i++)
            {
                dict[methodParameters[i].Name] = Invocation.Arguments[i];
            }

            return dict;
        }
    }
}