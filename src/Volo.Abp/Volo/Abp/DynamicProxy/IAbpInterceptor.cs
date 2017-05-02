namespace Volo.Abp.DynamicProxy
{
    public interface IAbpInterceptor
    {
        void Intercept(IAbpMethodInvocation invocation);
    }
}
