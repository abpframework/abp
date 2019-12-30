using Shouldly;
using Volo.Abp.Testing;
using Xunit;

namespace Volo.Abp.Security.Encryption
{
    public class StringEncryptionService_Tests : AbpIntegratedTest<AbpSecurityTestModule>
    {
        private readonly IStringEncryptionService _stringEncryptionService;

        public StringEncryptionService_Tests()
        {
            _stringEncryptionService = GetRequiredService<IStringEncryptionService>();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("This is a plain text!")]
        public void Should_Enrypt_And_Decrpyt_With_Default_Options(string plainText)
        {
            _stringEncryptionService
                .Decrypt(_stringEncryptionService.Encrypt(plainText))
                .ShouldBe(plainText);
        }
    }
}
