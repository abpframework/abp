﻿using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Volo.Abp.AuditLogging.EntityFrameworkCore
{
    [ConnectionStringName(AbpAuditLoggingConsts.ConnectionStringName)]
    public class AbpAuditLoggingDbContext : AbpDbContext<AbpAuditLoggingDbContext>, IAuditLoggingDbContext
    {
        public static string TablePrefix { get; set; } = AbpAuditLoggingConsts.DefaultDbTablePrefix;

        public static string Schema { get; set; } = AbpAuditLoggingConsts.DefaultDbSchema;

        public DbSet<AuditLog> AuditLogs { get; set; }

        public AbpAuditLoggingDbContext(DbContextOptions<AbpAuditLoggingDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureAuditLogging(TablePrefix, Schema);
        }
    }
}
