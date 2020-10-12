using System;
using System.Threading.Tasks;

namespace Volo.Abp.AspNetCore.Components.WebAssembly.ExceptionHandling
{
    public interface IUserExceptionInformer
    {
        Task InformAsync(UserExceptionInformerContext context);
    }
}
