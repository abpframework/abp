/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    OrganizationService/OrganizationManager
* 创建者：天上有木月
* 创建时间：2019/5/6 13:22:44
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/

using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Users;

namespace OrganizationService
{
    public class OrganizationManager : IDomainService
    {
        private readonly IOrganizationRepository _organizationRepository;
        private readonly IRepository<UserOrganization, Guid> _userOrganizationRepository;
        public OrganizationManager(IOrganizationRepository organizationRepository, IRepository<UserOrganization, Guid> userOrganizationRepository)
        {
            _organizationRepository = organizationRepository;
            _userOrganizationRepository = userOrganizationRepository;
        }

        public  async Task RemoveFromOrganizationUnitAsync(Guid userId, Organization ou)
        {
            await _userOrganizationRepository.DeleteAsync(uou => uou.UserId == userId && uou.OrganizationId == ou.Id);
        }

        public  async Task SetOrganizationsAsync(Guid userId, params Guid[] organizationUnitIds)
        {
            if (organizationUnitIds == null)
            {
                organizationUnitIds = new Guid[0];
            }

            var currentOus = await _organizationRepository.GetOrganizationsAsync(userId);

            //Remove from removed OUs
            foreach (var currentOu in currentOus)
            {
                if (!organizationUnitIds.Contains(currentOu.Id))
                {
                    await RemoveFromOrganizationUnitAsync(userId, currentOu);
                }
            }

            //Add to added OUs
            foreach (var organizationId in organizationUnitIds)
            {
                if (currentOus.All(ou => ou.Id != organizationId))
                {
                    await _userOrganizationRepository.InsertAsync(new UserOrganization(userId, organizationId));

                }
            }
        }
    }
}
