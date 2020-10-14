namespace Volo.Abp.AspNetCore.Components.WebAssembly.ExceptionHandling
{
    public interface IUserExceptionInformer
    {
        void Inform(UserExceptionInformerContext context);
    }
}
