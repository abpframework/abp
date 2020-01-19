using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Acme.BookStore
{
    [ConnectionStringName("MongoDBConnString")]
    public class BookStoreMongoDBDbContext: AbpMongoDbContext
    {
        public IMongoCollection<Book> Questions => Collection<Book>();
        
        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.Entity<Book>(b =>
            {
                b.CollectionName = "Book";
            });
        }
    }
}
