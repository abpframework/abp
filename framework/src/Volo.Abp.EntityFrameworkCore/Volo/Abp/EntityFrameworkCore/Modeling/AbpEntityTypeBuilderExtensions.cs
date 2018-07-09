using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Volo.Abp.Auditing;
using Volo.Abp.Data;

namespace Volo.Abp.EntityFrameworkCore.Modeling
{
    public static class AbpEntityTypeBuilderExtensions
    {
        public static void ConfigureExtraProperties<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasExtraProperties
        {
            b.Property(x => x.ExtraProperties)
                .HasConversion(
                    d => JsonConvert.SerializeObject(d, Formatting.None),
                    s => JsonConvert.DeserializeObject<Dictionary<string, object>>(s)
                )
                .HasColumnName(nameof(IHasExtraProperties.ExtraProperties));
        }

        public static void ConfigureSoftDelete<T>(this EntityTypeBuilder<T> b)
            where T : class, ISoftDelete
        {
            b.Property(x => x.IsDeleted).IsRequired().HasDefaultValue(false).HasColumnName(nameof(ISoftDelete.IsDeleted));
        }

        public static void ConfigureDeletionTime<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasDeletionTime
        {
            b.ConfigureSoftDelete();
            b.Property(x => x.DeletionTime).IsRequired(false).HasColumnName(nameof(IHasDeletionTime.DeletionTime));
        }

        public static void ConfigureMayHaveCreator<T>(this EntityTypeBuilder<T> b)
            where T : class, IMayHaveCreator
        {
            b.Property(x => x.CreatorId).IsRequired(false).HasColumnName(nameof(IMayHaveCreator.CreatorId));
        }

        public static void ConfigureMustHaveCreator<T>(this EntityTypeBuilder<T> b)
            where T : class, IMustHaveCreator
        {
            b.Property(x => x.CreatorId).IsRequired().HasColumnName(nameof(IMustHaveCreator.CreatorId));
        }

        public static void ConfigureDeletionAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IDeletionAuditedObject
        {
            b.ConfigureDeletionTime();
            b.Property(x => x.DeleterId).IsRequired(false).HasColumnName(nameof(IDeletionAuditedObject.DeleterId));
        }

        public static void ConfigureCreationTime<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasCreationTime
        {
            b.Property(x => x.CreationTime).IsRequired().HasColumnName(nameof(IHasCreationTime.CreationTime));
        }

        public static void ConfigureCreationAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, ICreationAuditedObject
        {
            b.ConfigureCreationTime();
            b.Property(x => x.CreatorId).IsRequired(false).HasColumnName(nameof(ICreationAuditedObject.CreatorId));
        }

        public static void ConfigureLastModificationTime<T>(this EntityTypeBuilder<T> b)
            where T : class, IHasModificationTime
        {
            b.Property(x => x.LastModificationTime).IsRequired(false).HasColumnName(nameof(IHasModificationTime.LastModificationTime));
        }

        public static void ConfigureModificationAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IModificationAuditedObject
        {
            b.ConfigureLastModificationTime();
            b.Property(x => x.LastModifierId).IsRequired(false).HasColumnName(nameof(IModificationAuditedObject.LastModifierId));
        }

        public static void ConfigureAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IAuditedObject
        {
            b.ConfigureCreationAudited();
            b.ConfigureModificationAudited();
        }

        public static void ConfigureFullAudited<T>(this EntityTypeBuilder<T> b)
            where T : class, IFullAuditedObject
        {
            b.ConfigureAudited();
            b.ConfigureDeletionAudited();
        }

        //TODO: Add other interfaces (IMultiTenant, IAuditedObject<TUser>...)
    }
}
