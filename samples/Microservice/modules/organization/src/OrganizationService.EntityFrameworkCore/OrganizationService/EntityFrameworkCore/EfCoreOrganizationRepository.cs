/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    OrganizationService.EntityFrameworkCore/EfCoreOrganizationRepository
* 创建者：天上有木月
* 创建时间：2019/5/6 12:58:37
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Users;

namespace OrganizationService.EntityFrameworkCore
{
   public class EfCoreOrganizationRepository: EfCoreRepository<IOrganizationServiceDbContext, Organization, Guid>, IOrganizationRepository
    {
        public EfCoreOrganizationRepository(IDbContextProvider<IOrganizationServiceDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public Task<List<Organization>> GetOrganizationsAsync(Guid userId)
        {
            var query = from uou in DbContext.UserOrganizations
                join ou in DbContext.AbpOrganizations on uou.OrganizationId equals ou.Id
                where uou.UserId == userId
                select ou;

            return Task.FromResult(query.ToList());
        }
    }
}
