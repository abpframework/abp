namespace Volo.Abp.DynamicProxy
{
    public interface IAbpMethodInvocation: IAbpMethodInvocationCore
    {
        void Proceed();
    }
}