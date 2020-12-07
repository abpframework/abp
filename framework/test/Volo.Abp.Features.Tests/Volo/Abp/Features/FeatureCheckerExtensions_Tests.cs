using Shouldly;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.Features
{
    public class FeatureCheckerExtensions_Tests : FeatureTestBase
    {
        private readonly IExceptionToErrorInfoConverter _exceptionToErrorInfoConverter;

        public FeatureCheckerExtensions_Tests()
        {
            _exceptionToErrorInfoConverter = GetRequiredService<IExceptionToErrorInfoConverter>();
        }

        [Fact]
        public void Test_AbpAuthorizationException_Localization()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var exception = new AbpAuthorizationException(code: AbpFeatureErrorCodes.FeatureIsNotEnabled)
                    .WithData("FeatureName", "my_feature_name");
                var errorInfo = _exceptionToErrorInfoConverter.Convert(exception, false);
                errorInfo.Message.ShouldBe("功能未启用: my_feature_name");

                exception = new AbpAuthorizationException(code: AbpFeatureErrorCodes.AllOfTheseFeaturesMustBeEnabled)
                    .WithData("FeatureNames", "my_feature_name, my_feature_name2");
                errorInfo = _exceptionToErrorInfoConverter.Convert(exception, false);
                errorInfo.Message.ShouldBe("必要的功能未启用. 这些功能需要启用: my_feature_name, my_feature_name2");

                exception = new AbpAuthorizationException(code: AbpFeatureErrorCodes.AtLeastOneOfTheseFeaturesMustBeEnabled)
                    .WithData("FeatureNames", "my_feature_name, my_feature_name2");
                errorInfo = _exceptionToErrorInfoConverter.Convert(exception, false);
                errorInfo.Message.ShouldBe("必要的功能未启用. 需要启用这些功能中的一项：my_feature_name, my_feature_name2");
            }
        }
    }
}
