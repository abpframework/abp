using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Newtonsoft.Json.Linq;

namespace Volo.Abp.AspNetCore.Authentication.OAuth.Claims
{
    public class MultipleClaimAction : ClaimAction
    {
        public MultipleClaimAction(string claimType, string jsonKey)
            : base(claimType, jsonKey)
        {
        }

        public override void Run(JObject userData, ClaimsIdentity identity, string issuer)
        {
            var prop = userData?.Property(ValueType);
            if (prop == null)
            {
                return;
            }

            var propValue = prop.Value;

            switch (propValue.Type)
            {
                case JTokenType.String:
                    identity.AddClaim(new Claim(ClaimType, propValue.Value<string>(), ValueType, issuer));
                    break;
                case JTokenType.Array:
                    foreach (var innterValue in propValue.Values<string>())
                    {
                        identity.AddClaim(new Claim(ClaimType, innterValue, ValueType, issuer));
                    }
                    break;
                default:
                    throw new AbpException("Unhandled JTokenType: " + propValue.Type);
            }
        }
    }
}