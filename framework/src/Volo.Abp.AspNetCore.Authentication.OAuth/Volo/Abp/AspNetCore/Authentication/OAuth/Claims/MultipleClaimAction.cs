using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;

namespace Volo.Abp.AspNetCore.Authentication.OAuth.Claims
{
    public class MultipleClaimAction : ClaimAction
    {
        public MultipleClaimAction(string claimType, string jsonKey)
            : base(claimType, jsonKey)
        {

        }

        public override void Run(JsonElement userData, ClaimsIdentity identity, string issuer)
        {            
            var prop = userData.GetProperty(ValueType);
            if (prop.ValueKind == JsonValueKind.Null)
            {
                return;
            }

            switch (prop.ValueKind)
            {
                case JsonValueKind.String:
                    identity.AddClaim(new Claim(ClaimType, prop.GetString(), ValueType, issuer));
                    break;
                case JsonValueKind.Array:
                    foreach (var arramItem in prop.EnumerateArray())
                    {
                        identity.AddClaim(new Claim(ClaimType, arramItem.GetString(), ValueType, issuer));
                    }
                    break;
                default:
                    throw new AbpException("Unhandled JsonValueKind: " + prop.ValueKind);
            }
        }
    }
}