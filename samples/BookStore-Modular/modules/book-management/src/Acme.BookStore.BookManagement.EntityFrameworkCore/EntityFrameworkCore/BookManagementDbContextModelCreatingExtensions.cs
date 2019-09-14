using System;
using Acme.BookStore.BookManagement.Books;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Acme.BookStore.BookManagement.EntityFrameworkCore
{
    public static class BookManagementDbContextModelCreatingExtensions
    {
        public static void ConfigureBookManagement(
            this ModelBuilder builder,
            Action<BookManagementModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new BookManagementModelBuilderConfigurationOptions();

            optionsAction?.Invoke(options);

            builder.Entity<Book>(b =>
               {
                   b.ToTable(BookManagementConsts.DefaultDbTablePrefix + "Books", BookManagementConsts.DefaultDbSchema);
                   b.ConfigureByConvention(); //auto configure for the base class props
                   b.Property(x => x.Name).IsRequired().HasMaxLength(128);
               });
        }
    }
}