using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;

namespace Volo.Abp.OpenIddict.Controllers;

[Route("connect/token")]
public partial class TokenController : OpenIdDictControllerBase
{
    [HttpGet, HttpPost, Produces("application/json")]
    public virtual async Task<IActionResult> HandleAsync()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ??
                      //TODO: L
                      throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (request.IsPasswordGrantType())
        {
            return await HandlePasswordAsync(request);
        }

        if (request.IsAuthorizationCodeGrantType() )
        {
            return await HandleAuthorizationCodeAsync(request);
        }
        
        if (request.IsRefreshTokenGrantType() )
        {
            return await HandleRefreshTokenAsync(request);
        }
        
        if (request.IsDeviceCodeGrantType() )
        {
            return await HandleDeviceCodeAsync(request);
        }
        
        if (request.IsClientCredentialsGrantType())
        {
            return await HandleClientCredentialsAsync(request);
        }

        //TODO: L
        throw new NotImplementedException("The specified grant type is not implemented.");
    }
}
