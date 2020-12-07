using System.Threading.Tasks;
using Shouldly;
using Volo.Abp.AspNetCore.ExceptionHandling;
using Volo.Abp.Authorization;
using Volo.Abp.Localization;
using Xunit;

namespace Volo.Abp.Features
{
    public class FeatureCheckerExtensions_Tests : FeatureTestBase
    {
        private readonly IFeatureChecker _featureChecker;
        private readonly IExceptionToErrorInfoConverter _exceptionToErrorInfoConverter;

        public FeatureCheckerExtensions_Tests()
        {
            _featureChecker = GetRequiredService<IFeatureChecker>();
            _exceptionToErrorInfoConverter = GetRequiredService<IExceptionToErrorInfoConverter>();
        }

        [Fact]
        public async Task CheckEnabledAsync()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var ex = await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
                    await _featureChecker.CheckEnabledAsync("BooleanTestFeature1"));

                var errorInfo = _exceptionToErrorInfoConverter.Convert(ex, false);
                errorInfo.Message.ShouldBe("功能未启用: BooleanTestFeature1");
            }
        }

        [Fact]
        public async Task CheckEnabled_RequiresAll()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var ex = await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
                    await _featureChecker.CheckEnabledAsync(true, "BooleanTestFeature1", "BooleanTestFeature2"));

                var errorInfo = _exceptionToErrorInfoConverter.Convert(ex, false);
                errorInfo.Message.ShouldBe("必要的功能未启用. 这些功能需要启用: BooleanTestFeature1, BooleanTestFeature2");
            }
        }

        [Fact]
        public async Task CheckEnabled_Not_RequiresAll()
        {
            using (CultureHelper.Use("zh-Hans"))
            {
                var ex = await Assert.ThrowsAsync<AbpAuthorizationException>(async () =>
                    await _featureChecker.CheckEnabledAsync(false, "BooleanTestFeature1", "BooleanTestFeature2"));

                var errorInfo = _exceptionToErrorInfoConverter.Convert(ex, false);
                errorInfo.Message.ShouldBe("必要的功能未启用. 需要启用这些功能中的一项：BooleanTestFeature1, BooleanTestFeature2");
            }
        }
    }
}
