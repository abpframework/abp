using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;

namespace Acme.BookStore.Roles
{
    public interface IAppRoleAppService : IApplicationService
    {
        Task<List<AppRoleDto>> GetListAsync();

        Task UpdateTitleAsync(Guid id, string title);
    }
}