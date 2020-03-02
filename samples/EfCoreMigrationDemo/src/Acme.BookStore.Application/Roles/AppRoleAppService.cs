using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace Acme.BookStore.Roles
{
    public class AppRoleAppService : ApplicationService, IAppRoleAppService
    {
        private readonly IRepository<AppRole, Guid> _appRoleRepository;

        public AppRoleAppService(IRepository<AppRole, Guid> appRoleRepository)
        {
            _appRoleRepository = appRoleRepository;
        }

        public async Task<List<AppRoleDto>> GetListAsync()
        {
            var roles = await _appRoleRepository.GetListAsync();

            return roles
                .Select(r => new AppRoleDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Title = r.Title
                })
                .ToList();
        }

        public async Task UpdateTitleAsync(Guid id, string title)
        {
            var role = await _appRoleRepository.GetAsync(id);
            
            role.Title = title;
            
            await _appRoleRepository.UpdateAsync(role);
        }
    }
}
