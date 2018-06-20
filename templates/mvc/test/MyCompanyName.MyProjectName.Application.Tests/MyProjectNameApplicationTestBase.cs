using Volo.Abp;

namespace MyCompanyName.MyProjectName
{
    public abstract class MyProjectNameApplicationTestBase : AbpIntegratedTest<MyProjectNameApplicationTestModule>
    {
        protected override void SetAbpApplicationCreationOptions(AbpApplicationCreationOptions options)
        {
            options.UseAutofac();
        }
    }
}
