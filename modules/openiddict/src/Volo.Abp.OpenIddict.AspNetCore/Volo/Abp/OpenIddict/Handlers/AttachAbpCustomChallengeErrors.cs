using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenIddict.Server;

namespace Volo.Abp.OpenIddict.Handlers;

public class AttachAbpCustomChallengeErrors : IOpenIddictServerHandler<OpenIddictServerEvents.ProcessChallengeContext>
{
    private static readonly List<string> CustomChallengeErrors = new List<string>()
    {
        "userId",
        "twoFactorToken"
    };

    public static OpenIddictServerHandlerDescriptor Descriptor { get; }
        = OpenIddictServerHandlerDescriptor.CreateBuilder<OpenIddictServerEvents.ProcessChallengeContext>()
            .UseSingletonHandler<AttachAbpCustomChallengeErrors>()
            .SetOrder(OpenIddictServerHandlers.AttachDefaultChallengeError.Descriptor.Order + 1)
            .SetType(OpenIddictServerHandlerType.Custom)
            .Build();

    public ValueTask HandleAsync(OpenIddictServerEvents.ProcessChallengeContext context)
    {
        Check.NotNull(context, nameof(context));

        foreach (var property in context.Properties.Where(x => CustomChallengeErrors.Contains(x.Key)))
        {
            context.Response.SetParameter(property.Key, property.Value);
        }

        return default;
    }
}
