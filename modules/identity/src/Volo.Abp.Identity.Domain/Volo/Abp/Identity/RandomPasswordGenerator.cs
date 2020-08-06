using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.Identity
{
    /// <summary>
    /// This class can be used to generate random password
    /// based on the rules defined in the <see cref="IdentityOptions"/>.
    /// </summary>
    public class RandomPasswordGenerator : ITransientDependency
    {
        public const int MinPasswordLength = 32;

        protected IdentityOptions Options { get; }
        protected Random Random { get; }

        public RandomPasswordGenerator(IOptions<IdentityOptions> options)
        {
            Options = options.Value;
            Random = new Random();
        }

        public virtual Task<string> CreateAsync()
        {
            var nonAlphanumeric = Options.Password.RequireNonAlphanumeric;
            var digit = Options.Password.RequireDigit;
            var lowercase = Options.Password.RequireLowercase;
            var uppercase = Options.Password.RequireUppercase;

            var passwordBuilder = new StringBuilder();

            var length = Math.Max(Options.Password.RequiredLength, MinPasswordLength);
            while (passwordBuilder.Length < length)
            {
                var nextChar = (char)Random.Next(32, 126);

                passwordBuilder.Append(nextChar);

                if (char.IsDigit(nextChar))
                {
                    digit = false;
                }
                else if (char.IsLower(nextChar))
                {
                    lowercase = false;
                }
                else if (char.IsUpper(nextChar))
                {
                    uppercase = false;
                }
                else if (!char.IsLetterOrDigit(nextChar))
                {
                    nonAlphanumeric = false;
                }
            }

            if (nonAlphanumeric)
            {
                passwordBuilder.Append((char)Random.Next(33, 48));
            }

            if (digit)
            {
                passwordBuilder.Append((char)Random.Next(48, 58));
            }

            if (lowercase)
            {
                passwordBuilder.Append((char)Random.Next(97, 123));
            }

            if (uppercase)
            {
                passwordBuilder.Append((char)Random.Next(65, 91));
            }

            return Task.FromResult(passwordBuilder.ToString());
        }
    }
}
