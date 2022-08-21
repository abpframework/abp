var abp = abp || {};

(function() {

    abp.SwaggerUIBundle = function(configObject) {

        var excludeUrl = ["swagger.json", "connect/token"]
        var firstRequest = true;
        abp.appPath = configObject.baseUrl || abp.appPath;

        var requestInterceptor = configObject.requestInterceptor;

        configObject.requestInterceptor = async function(request) {

            if (request.url.includes(excludeUrl[1])) {
                firstRequest = true;
            }

            if (firstRequest && !excludeUrl.some(url => request.url.includes(url))) {
                await fetch(`${abp.appPath}abp/Swashbuckle/SetCsrfCookie`, {
                    headers: request.headers
                });
                firstRequest = false;
            }

            var antiForgeryToken = abp.security.antiForgery.getToken();
            if (antiForgeryToken) {
                request.headers[abp.security.antiForgery.tokenHeaderName] = antiForgeryToken;
            }

            if (!request.headers["X-Requested-With"]) {
                request.headers["X-Requested-With"] = "XMLHttpRequest";
            }

            if (requestInterceptor) {
                requestInterceptor(request);
            }
            return request;
        };

        return SwaggerUIBundle(configObject);
    }
})();
