using Volo.Abp;

namespace Acme.BookStore
{
    public abstract class BookStoreApplicationTestBase : AbpIntegratedTest<BookStoreApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
