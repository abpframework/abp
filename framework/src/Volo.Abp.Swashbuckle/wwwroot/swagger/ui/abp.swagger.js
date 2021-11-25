var abp = abp || {};

(function () {

    abp.SwaggerUIBundle = function (configObject) {

        var excludeUrl = ["swagger.json", "connect/token"]
        var firstRequest = true;
        abp.appPath = configObject.baseUrl || abp.appPath;

        configObject.requestInterceptor = async function (request) {

            if(request.url.includes(excludeUrl[1])){
                firstRequest = true;
            }

            if(firstRequest && !excludeUrl.some(url => request.url.includes(url)))
            {
                await fetch(`${abp.appPath}abp/Swashbuckle/SetCsrfCookie`,{
                    headers: request.headers
                });
                firstRequest = false;
            }

            var antiForgeryToken = abp.security.antiForgery.getToken();
            if (antiForgeryToken) {
                request.headers[abp.security.antiForgery.tokenHeaderName] = antiForgeryToken;
            }
            return request;
        };

        return SwaggerUIBundle(configObject);
    }
})();
