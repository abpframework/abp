using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Localization;

namespace Volo.Abp.Identity
{
    public class AbpIdentityMessageLocalizer<T>
    {
        private readonly IStringLocalizer<T> stringLocalizer;

        public AbpIdentityMessageLocalizer(IStringLocalizer<T> stringLocalizer)
        {
            this.stringLocalizer = stringLocalizer;
        }

        public virtual string Localize(string format, params string[] arguments)
        {
            try
            {
                return arguments?.Length > 0
                    ? string.Format(stringLocalizer[format], LocalizeArguments(arguments)?.ToArray())
                    : stringLocalizer[format];
            }
            catch
            {
                return stringLocalizer[format];
            }
        }

        public virtual IEnumerable<string> Localize(IEnumerable<(string format, string[] arguments)> messages)
        {
            return messages?.Select(m => Localize(m.format, m.arguments));
        }

        protected virtual IEnumerable<string> LocalizeArguments(IEnumerable<string> arguments)
        {
            foreach (var argument in arguments)
            {
                yield return stringLocalizer[$"DisplayName:{argument}"]
                    ?? stringLocalizer[argument]
                    ?? argument;
            }
        }
    }
}
