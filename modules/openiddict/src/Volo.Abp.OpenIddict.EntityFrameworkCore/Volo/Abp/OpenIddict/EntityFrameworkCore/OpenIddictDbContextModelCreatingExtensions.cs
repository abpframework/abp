using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text.Json;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.EntityFrameworkCore.ValueConverters;
using Volo.Abp.OpenIddict.Applications;
using Volo.Abp.OpenIddict.Authorizations;
using Volo.Abp.OpenIddict.EntityFrameworkCore.ValueConverters;
using Volo.Abp.OpenIddict.OpenIddictApplications;
using Volo.Abp.OpenIddict.Scopes;
using Volo.Abp.OpenIddict.Tokens;

namespace Volo.Abp.OpenIddict.EntityFrameworkCore
{
    public static class OpenIddictDbContextModelCreatingExtensions
    {
        public static void ConfigureOpenIddict(
            this ModelBuilder builder,
            Action<OpenIddictModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new OpenIddictModelBuilderConfigurationOptions(
                AbpOpenIddictDbProperties.DbTablePrefix,
                AbpOpenIddictDbProperties.DbSchema
            );

            optionsAction?.Invoke(options);

            builder.Entity<OpenIddictApplication>(b =>
            {
                b.ToTable(options.TablePrefix + "OpenIddictApplications", options.Schema);
                b.ConfigureByConvention();


                /* Configure more properties here */
                b.Property(x => x.ClientId)
                    .HasMaxLength(OpenIddictApplicationConst.ClientIdMaxLength)
                    .IsRequired();
                b.Property(x => x.ClientSecret)
                    .HasMaxLength(OpenIddictApplicationConst.ClientSecretMaxLength);
                b.Property(x => x.ConsentType)
                    .HasMaxLength(OpenIddictApplicationConst.ConsentTypeMaxLength);
                b.Property(x => x.DisplayName)
                    .HasMaxLength(OpenIddictApplicationConst.DisplayNameMaxLength);
                b.Property(x => x.PostLogoutRedirectUris)
                    .HasConversion(new AbpJsonValueConverter<HashSet<string>>())
                    .Metadata.SetValueComparer(new HashSetStringValueComparter());
                b.Property(x => x.RedirectUris)
                    .HasConversion(new AbpJsonValueConverter<HashSet<string>>())
                    .Metadata.SetValueComparer(new HashSetStringValueComparter());
                b.Property(x => x.DisplayNames)
                    .HasConversion(new CultureInfoStringDictionaryValueConverter())
                    .Metadata.SetValueComparer(new CultureInfoStringDictionaryValueComparter());
                b.Property(x => x.Requirements)
                    .HasConversion(new AbpJsonValueConverter<HashSet<string>>())
                    .Metadata.SetValueComparer(new HashSetStringValueComparter());
                b.Property(x => x.Permissions)
                    .HasConversion(new AbpJsonValueConverter<HashSet<string>>())
                    .Metadata.SetValueComparer(new HashSetStringValueComparter());
                b.Property(x => x.Properties)
                    .HasConversion(new AbpJsonValueConverter<Dictionary<string, JsonElement>>())
                    .Metadata.SetValueComparer(new StringJsonElementDictionaryValueComparter());
                b.Property(x => x.Type)
                    .HasMaxLength(OpenIddictApplicationConst.TypeMaxLength);


                b.HasIndex(x => x.ClientId);

                b.ConfigureObjectExtensions();
            });


            builder.Entity<OpenIddictAuthorization>(b =>
            {
                b.ToTable(options.TablePrefix + "OpenIddictAuthorizations", options.Schema);
                b.ConfigureByConvention();


                /* Configure more properties here */
                b.Property(x => x.Properties)
                    .HasConversion(new AbpJsonValueConverter<Dictionary<string, JsonElement>>())
                    .Metadata.SetValueComparer(new StringJsonElementDictionaryValueComparter());
                b.Property(x => x.Scopes)
                    .HasConversion(new AbpJsonValueConverter<HashSet<string>>())
                    .Metadata.SetValueComparer(new HashSetStringValueComparter());
                b.Property(x => x.Status)
                    .HasMaxLength(OpenIddictAuthorizationConst.StatusMaxLength);
                b.Property(x => x.Subject)
                    .HasMaxLength(OpenIddictAuthorizationConst.SubjectMaxLength);
                b.Property(x => x.Type)
                    .HasMaxLength(OpenIddictAuthorizationConst.TypeMaxLength)
                    .IsRequired();

                b.HasIndex(x => new { x.ApplicationId, x.Status, x.Subject, x.Type });

                b.HasOne<OpenIddictApplication>()
                    .WithMany()
                    .HasForeignKey(x => x.ApplicationId)
                    .IsRequired(false);

                b.ConfigureObjectExtensions();
            });


            builder.Entity<OpenIddictScope>(b =>
            {
                b.ToTable(options.TablePrefix + "OpenIddictScopes", options.Schema);
                b.ConfigureByConvention();


                /* Configure more properties here */
                b.Property(x => x.Description)
                    .HasMaxLength(OpenIddictScopeConst.DescriptionMaxLength);
                b.Property(x => x.Descriptions)
                    .HasConversion(new CultureInfoStringDictionaryValueConverter())
                    .Metadata.SetValueComparer(new CultureInfoStringDictionaryValueComparter());
                b.Property(x => x.DisplayName)
                    .HasMaxLength(OpenIddictScopeConst.DisplayNameMaxLength);
                b.Property(x => x.DisplayNames)
                    .HasConversion(new CultureInfoStringDictionaryValueConverter())
                    .Metadata.SetValueComparer(new CultureInfoStringDictionaryValueComparter());
                b.Property(x => x.Name)
                    .HasMaxLength(OpenIddictScopeConst.NameMaxLength);
                b.Property(x => x.Properties)
                    .HasConversion(new AbpJsonValueConverter<Dictionary<string, JsonElement>>())
                    .Metadata.SetValueComparer(new StringJsonElementDictionaryValueComparter());
                b.Property(x => x.Resources)
                    .HasConversion(new AbpJsonValueConverter<HashSet<string>>())
                    .Metadata.SetValueComparer(new HashSetStringValueComparter());

                b.ConfigureObjectExtensions();
            });


            builder.Entity<OpenIddictToken>(b =>
            {
                b.ToTable(options.TablePrefix + "OpenIddictTokens", options.Schema);
                b.ConfigureByConvention();


                /* Configure more properties here */
                b.Property(x => x.Properties)
                    .HasConversion(new AbpJsonValueConverter<Dictionary<string, JsonElement>>())
                    .Metadata.SetValueComparer(new StringJsonElementDictionaryValueComparter());
                b.Property(x => x.ReferenceId)
                    .HasMaxLength(OpenIddictTokenConst.ReferenceIdMaxLength);
                b.Property(x => x.Status)
                                .HasMaxLength(OpenIddictTokenConst.StatusMaxLength);
                b.Property(x => x.Subject)
                                .HasMaxLength(OpenIddictTokenConst.SubjectMaxLength);
                b.Property(x => x.Type)
                                .HasMaxLength(OpenIddictTokenConst.TypeMaxLength)
                                .IsRequired();

                b.HasIndex(x => x.ReferenceId)
                               .IsUnique();

                b.HasOne<OpenIddictApplication>()
                                .WithMany()
                                .HasForeignKey(x => x.ApplicationId)
                                .IsRequired(false);
                b.HasOne<OpenIddictAuthorization>()
                                .WithMany()
                                .HasForeignKey(x => x.AuthorizationId)
                                .IsRequired(false);

                b.ConfigureObjectExtensions();
            });
        }
    }
}
