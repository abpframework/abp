using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Volo.Abp.AspNetCore.Authentication.OAuth.Claims;

public class RemoveDuplicateClaimAction : ClaimAction
{
    public RemoveDuplicateClaimAction(string claimType)
        : base(claimType, ClaimValueTypes.String)
    {
    }

    /// <inheritdoc />
    public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
    {
        var claims = identity.Claims.Where(c => c.Type == ClaimType).ToArray();
        if (claims.Length < 2)
        {
            return;
        }

        var previousValues = new List<string>();
        foreach (var claim in claims)
        {
            if (claim.Value.IsIn(previousValues))
            {
                identity.RemoveClaim(claim);
            }
            else
            {
                previousValues.Add(claim.Value);
            }
        }
    }
}
