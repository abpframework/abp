using Shouldly;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.GlobalFeatures
{
    public class AbpGlobalFeatureNotEnableException_Localization_Test : GlobalFeatureTestBase
    {
        private readonly IExceptionToErrorInfoConverter _exceptionToErrorInfoConverter;

        public AbpGlobalFeatureNotEnableException_Localization_Test()
        {
            _exceptionToErrorInfoConverter = GetRequiredService<IExceptionToErrorInfoConverter>();
        }

        [Fact]
        public void AbpAuthorizationException_Localization()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var exception = new AbpGlobalFeatureNotEnabledException(code: AbpGlobalFeatureErrorCodes.GlobalFeatureIsNotEnabled)
                    .WithData("ServiceName", "MyService")
                    .WithData("GlobalFeatureName", "TestFeature");;
                var errorInfo = _exceptionToErrorInfoConverter.Convert(exception, false);
                errorInfo.Message.ShouldBe("'MyService'服务需要启用'TestFeature'功能.");
            }
        }
    }
}
