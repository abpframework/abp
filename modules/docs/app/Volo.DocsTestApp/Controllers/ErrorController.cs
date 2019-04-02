using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Volo.Abp.AspNetCore.Mvc;

namespace Volo.DocsTestApp.Controllers
{
    public class ErrorController : AbpController
    {
        [Route("error/{statusCode}")]
        [HttpGet]
        public IActionResult Index(int statusCode = 0)
        {
            var statusFeature = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            if (statusFeature != null)
            {
                Log.Warning("Handled {0} error for URL: {1}", statusCode, statusFeature.OriginalPath);
            }

            var isValidStatusCode = Enum.IsDefined(typeof(HttpStatusCode), statusCode);
            if (!isValidStatusCode)
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }

            return new ContentResult
            {
                ContentType = System.Net.Mime.MediaTypeNames.Text.Html,
                StatusCode = statusCode,
                Content = string.Format(HtmlBody, _errorMessages.ContainsKey(statusCode)
                    ? _errorMessages[statusCode]
                    : "Looks like something went wrong!")
            };
        }

        private const string HtmlBody = "<html><body><div style='text-align:center'><pre>{0}</pre><a href='/'>Go to home page</a></div></body></html>";

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
                          
We can't find the page you're looking for..."
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
    }
}
