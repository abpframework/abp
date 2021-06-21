using Shouldly;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Xunit;

namespace Microsoft.AspNetCore.Authorization
{
    public class AbpAuthorizationServiceExtensions_Tests : AuthorizationTestBase
    {
        private readonly IExceptionToErrorInfoConverter _exceptionToErrorInfoConverter;

        public AbpAuthorizationServiceExtensions_Tests()
        {
            _exceptionToErrorInfoConverter = GetRequiredService<IExceptionToErrorInfoConverter>();
        }

        [Fact]
        public void Test_AbpAuthorizationException_Localization()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var exception = new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGranted);
                var errorInfo = _exceptionToErrorInfoConverter.Convert(exception, false);
                errorInfo.Message.ShouldBe("授权失败! 提供的策略尚未授予.");

                exception = new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedWithPolicyName)
                    .WithData("PolicyName", "my_policy_name");
                errorInfo = _exceptionToErrorInfoConverter.Convert(exception, false);
                errorInfo.Message.ShouldBe("授权失败! 提供的策略尚未授予: my_policy_name");

                exception =  new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenPolicyHasNotGrantedForGivenResource)
                    .WithData("ResourceName", "my_resource_name");
                errorInfo = _exceptionToErrorInfoConverter.Convert(exception, false);
                errorInfo.Message.ShouldBe("授权失败! 提供的策略未授予提供的资源: my_resource_name");

                exception = new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenRequirementHasNotGrantedForGivenResource)
                    .WithData("ResourceName", "my_resource_name");
                errorInfo = _exceptionToErrorInfoConverter.Convert(exception, false);
                errorInfo.Message.ShouldBe("授权失败! 提供的要求未授予提供的资源: my_resource_name");

                exception =  new AbpAuthorizationException(code: AbpAuthorizationErrorCodes.GivenRequirementsHasNotGrantedForGivenResource)
                    .WithData("ResourceName", "my_resource_name");
                errorInfo = _exceptionToErrorInfoConverter.Convert(exception, false);
                errorInfo.Message.ShouldBe("授权失败! 提供的要求未授予提供的资源: my_resource_name");
            }
        }
    }
}
