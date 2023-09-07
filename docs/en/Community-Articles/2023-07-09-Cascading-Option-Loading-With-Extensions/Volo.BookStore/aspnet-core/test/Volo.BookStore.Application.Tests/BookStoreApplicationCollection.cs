using Volo.BookStore.MongoDB;
using Xunit;

namespace Volo.BookStore;

[CollectionDefinition(BookStoreTestConsts.CollectionDefinitionName)]
public class BookStoreApplicationCollection : BookStoreMongoDbCollectionFixtureBase
{

}
