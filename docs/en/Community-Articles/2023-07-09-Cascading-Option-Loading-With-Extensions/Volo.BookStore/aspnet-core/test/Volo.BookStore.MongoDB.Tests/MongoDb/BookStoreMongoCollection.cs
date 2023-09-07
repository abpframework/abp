using Xunit;

namespace Volo.BookStore.MongoDB;

[CollectionDefinition(BookStoreTestConsts.CollectionDefinitionName)]
public class BookStoreMongoCollection : BookStoreMongoDbCollectionFixtureBase
{

}
