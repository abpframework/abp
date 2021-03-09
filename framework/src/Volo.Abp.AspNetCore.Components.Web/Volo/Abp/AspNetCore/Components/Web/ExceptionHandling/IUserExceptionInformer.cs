namespace Volo.Abp.AspNetCore.Components.Web.ExceptionHandling
{
    public interface IUserExceptionInformer
    {
        void Inform(UserExceptionInformerContext context);
    }
}
