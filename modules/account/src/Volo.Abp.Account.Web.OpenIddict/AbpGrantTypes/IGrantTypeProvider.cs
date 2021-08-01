using OpenIddict.Abstractions;
using System.Threading.Tasks;

namespace Volo.Abp.Account.Web.AbpGrantTypes
{
    public interface IGrantTypeProvider
    {
        string GrantType { get; }

        Task<GrantTypeResult> HandleAsync(OpenIddictRequest request);
    }
}