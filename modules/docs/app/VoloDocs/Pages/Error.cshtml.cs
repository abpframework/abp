using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using Volo.Abp.Domain.Entities;

namespace VoloDocs.Pages
{
    public class ErrorModel : AbpPageModel
    {
        public string ErrorMessage { get; set; }

        public async Task<ActionResult> OnGetAsync(string statusCode)
        {
            if (!int.TryParse(statusCode, out var errorStatusCode))
            {
                errorStatusCode = (int)HttpStatusCode.BadRequest;
            }

            var statusFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusFeature != null)
            {
                Logger.LogWarning("Handled {0} error for URL: {1}", statusCode, statusFeature.OriginalPath);
            }

            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            if (exceptionHandlerPathFeature != null)
            {
                var exception = exceptionHandlerPathFeature.Error;
                var path = exceptionHandlerPathFeature.Path;

                if (exception is EntityNotFoundException)
                {
                    ErrorMessage = exception.Message;
                    return Page();
                }
            }

            var isValidStatusCode = Enum.IsDefined(typeof(HttpStatusCode), errorStatusCode);
            if (!isValidStatusCode)
            {
                errorStatusCode = (int)HttpStatusCode.BadRequest;
            }

            ErrorMessage = _errorMessages.ContainsKey(errorStatusCode)
                ? _errorMessages[errorStatusCode]
                : "Looks like something went wrong!";

            return Page();
        }

        #region Error Messages
        /*For more ASCII arts http://patorjk.com/software/taag/#p=display&h=0&f=Big&t=400*/
        private readonly Dictionary<int, string> _errorMessages = new Dictionary<int, string>
        {
            {
                400, @"
  _  _      ___     ___  
 | || |    / _ \   / _ \ 
 | || |_  | | | | | | | |
 |__   _| | | | | | | | |
    | |   | |_| | | |_| |
    |_|    \___/   \___/ 

You've sent a bad request!"
            },
            {
                401, @"
  _  _      ___     __ 
 | || |    / _ \   /_ |
 | || |_  | | | |   | |
 |__   _| | | | |   | |
    | |   | |_| |   | |
    |_|    \___/    |_|

Authorization required!"
            },
            {
                403,
                @"
  _  _      ___    ____  
 | || |    / _ \  |___ \ 
 | || |_  | | | |   __) |
 |__   _| | | | |  |__ < 
    | |   | |_| |  ___) |
    |_|    \___/  |____/ 
                         
This is a forbidden area!"
            },
            {
                404, @"
  _  _      ___    _  _   
 | || |    / _ \  | || |  
 | || |_  | | | | | || |_ 
 |__   _| | | | | |__   _|
    | |   | |_| |    | |  
    |_|    \___/     |_|  
                          
Hmm, we couldn't find the page you're looking for..."
            },
            {
                500,
                @"
  _____    ___     ___  
 | ____|  / _ \   / _ \ 
 | |__   | | | | | | | |
 |___ \  | | | | | | | |
  ___) | | |_| | | |_| |
 |____/   \___/   \___/ 
                        
Houston, we have a problem. Internal server error!"
            },
            {
                502,
                @"
  _____    ___    ___  
 | ____|  / _ \  |__ \ 
 | |__   | | | |    ) |
 |___ \  | | | |   / / 
  ___) | | |_| |  / /_ 
 |____/   \___/  |____|

Ooops! Our server is experiencing a mild case of the hiccups."
            },
            {
                503,
                @"
  _____    ___    ____  
 | ____|  / _ \  |___ \ 
 | |__   | | | |   __) |
 |___ \  | | | |  |__ < 
  ___) | | |_| |  ___) |
 |____/   \___/  |____/ 

Looks like we're having some server issues."
            }
        };
        #endregion
    }
}