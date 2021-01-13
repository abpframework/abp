namespace Volo.Abp.AspNetCore.Components.UI.ExceptionHandling
{
    public interface IUserExceptionInformer
    {
        void Inform(UserExceptionInformerContext context);
    }
}
