﻿using Microsoft.EntityFrameworkCore;
using MyCompanyName.MyProjectName.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace MyCompanyName.MyProjectName.Host
{
    public class DemoAppDbContext : AbpDbContext<DemoAppDbContext>
    {
        public DemoAppDbContext(DbContextOptions<DemoAppDbContext> options)
            : base(options)
        {
        
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigurePermissionManagement();
            modelBuilder.ConfigureSettingManagement();
            modelBuilder.ConfigureAuditLogging();

            modelBuilder.ConfigureMyProjectName();
        }
    }
}
