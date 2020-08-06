using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace Volo.Abp.Identity
{
    public class RandomPasswordGenerator_Tests : AbpIdentityDomainTestBase
    {
        private readonly RandomPasswordGenerator _randomPasswordGenerator;

        public RandomPasswordGenerator_Tests()
        {
            _randomPasswordGenerator = GetRequiredService<RandomPasswordGenerator>();
        }

        [Fact]
        public async Task CreateAsync()
        {
            var password = await _randomPasswordGenerator.CreateAsync();
            password.Length.ShouldBeGreaterThanOrEqualTo(RandomPasswordGenerator.MinPasswordLength);
        }
    }
}
