﻿using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;
using Volo.CmsKit.Reactions;

namespace Volo.CmsKit.EntityFrameworkCore
{
    [ConnectionStringName(CmsKitDbProperties.ConnectionStringName)]
    public class CmsKitDbContext : AbpDbContext<CmsKitDbContext>, ICmsKitDbContext
    {
        public DbSet<UserReaction> UserReactions { get; set; }

        public CmsKitDbContext(DbContextOptions<CmsKitDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureCmsKit();
        }
    }
}
