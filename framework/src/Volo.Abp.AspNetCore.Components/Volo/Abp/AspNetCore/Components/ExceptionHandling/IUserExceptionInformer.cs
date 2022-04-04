using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.ExceptionHandling;

public interface IUserExceptionInformer
{
    void Inform(UserExceptionInformerContext context);

    Task InformAsync(UserExceptionInformerContext context);
}
