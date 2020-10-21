using System.Collections.Generic;
using System.Linq;
using Blazorise;

namespace Volo.Abp.Identity.Blazor
{
    public static class AbpIdentityBlazorMessageLocalizerExtensions
    {
        public static IEnumerable<string> Localize<T>(this AbpIdentityMessageLocalizer<T> abpIdentityMessageLocalizer,
            ValidationMessageLocalizerEventArgs eventArgs)
        {
            return abpIdentityMessageLocalizer.Localize(eventArgs.Messages?.Select(m => (m.Message, m.MessageArguments)))?.ToArray();
        }
    }
}
