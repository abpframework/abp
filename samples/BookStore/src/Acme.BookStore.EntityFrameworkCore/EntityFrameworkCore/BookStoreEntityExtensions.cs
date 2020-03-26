using Volo.Abp.EntityFrameworkCore.Extensions;
using Volo.Abp.Identity;
using Volo.Abp.Threading;

namespace Acme.BookStore.EntityFrameworkCore
{
    public static class BookStoreEntityExtensions
    {
        private static readonly OneTimeRunner OneTimeRunner = new OneTimeRunner();

        public static void Configure()
        {
            OneTimeRunner.Run(() =>
            {
                /* You can configure entity extension properties for the
                 * entities defined in the used modules.
                 *
                 * Example:
                 *
                 * EntityExtensionManager.AddProperty<IdentityUser, string>(
                 *     "MyProperty",
                 *     b =>
                 *     {
                 *         b.HasMaxLength(128);
                 *     });
                 *
                 * See the documentation for more:
                 * https://docs.abp.io/en/abp/latest/Customizing-Application-Modules-Extending-Entities
                 */
            });
        }
    }
}