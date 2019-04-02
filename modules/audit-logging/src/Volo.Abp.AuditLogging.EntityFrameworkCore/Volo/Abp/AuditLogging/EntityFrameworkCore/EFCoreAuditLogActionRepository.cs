/*
* CLR版本:          4.0.30319.42000
* 命名空间名称/文件名:    Volo.Abp.AuditLogging.EntityFrameworkCore/EFCoreAuditLogActionRepository
* 创建者：天上有木月
* 创建时间：2019/4/2 3:01:42
* 邮箱：igeekfan@foxmail.com
* 文件功能描述： 
* 
* 修改人： 
* 时间：
* 修改说明：
*/
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    public class EfCoreAuditLogActionRepository : EfCoreRepository<IAuditLoggingDbContext, AuditLogAction, Guid>, IAuditLogActionRepository
    {
        public EfCoreAuditLogActionRepository(IDbContextProvider<IAuditLoggingDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<AuditLogAction>> GetListAsync(Guid auditLogId)
        {
            return await DbSet.AsNoTracking().Where(r => r.AuditLogId == auditLogId).ToListAsync();
        }
    }
}
