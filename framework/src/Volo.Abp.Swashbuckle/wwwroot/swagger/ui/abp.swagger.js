var abp = abp || {};

(function() {

    abp.SwaggerUIBundle = function(configObject) {

        var excludeUrl = ["swagger.json", "connect/token"]
        var firstRequest = true;
        var oidcSupportedFlows = configObject.oidcSupportedFlows || [];
        abp.appPath = configObject.baseUrl || abp.appPath;

        var requestInterceptor = configObject.requestInterceptor;
        var responseInterceptor = configObject.responseInterceptor;

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

        configObject.responseInterceptor = async function(response) {
            if(response.url.endsWith(".well-known/openid-configuration") && response.status === 200 && oidcSupportedFlows.length > 0) {
                var openIdConnectData = JSON.parse(response.text);
                openIdConnectData.grant_types_supported = oidcSupportedFlows;
                response.text = JSON.stringify(openIdConnectData);
            }

            if (responseInterceptor) {
                responseInterceptor(response);
            }
            return response;
        };

        return SwaggerUIBundle(configObject);
    }
})();
