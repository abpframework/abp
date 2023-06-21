var abp = abp || {};

(function () {

    abp.SwaggerUIBundle = function (configObject) {
        const wellKnownOpenIdConfiguration = ".well-known/openid-configuration"; 
        var excludeUrl = ["swagger.json", "connect/token"];
        var csrfExecludeUrl = [wellKnownOpenIdConfiguration];
        var firstRequest = true;
        var oidcSupportedFlows = configObject.oidcSupportedFlows || [];
        var oidcSupportedScopes = configObject.oidcSupportedScopes || [];
        var oidcDiscoveryEndpoint = configObject.oidcDiscoveryEndpoint || [];
        abp.appPath = configObject.baseUrl || abp.appPath;

        var requestInterceptor = configObject.requestInterceptor;
        var responseInterceptor = configObject.responseInterceptor;

        configObject.requestInterceptor = async function (request) {

            if (request.url.includes(excludeUrl[1])) {
                firstRequest = true;
            }

            if (firstRequest && !excludeUrl.some(url => request.url.includes(url))) {
                await fetch(`${abp.appPath}abp/Swashbuckle/SetCsrfCookie`, {
                    headers: request.headers
                });
                firstRequest = false;
            }
            // Intercept .well-known request when the discoveryEndpoint is provided
            if (!firstRequest && oidcDiscoveryEndpoint.length !== 0 && request.url.includes(wellKnownOpenIdConfiguration)) {
                request.url = oidcDiscoveryEndpoint;
            }

            var antiForgeryToken = abp.security.antiForgery.getToken();
            //don't send anti forgery tokens to auth server (like keycloak)
            if (antiForgeryToken && !csrfExecludeUrl.some(url => request.url.includes(url)) && !(configObject.resolvedIssuer && request.url.includes(configObject.resolvedIssuer))) {
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

        configObject.responseInterceptor = async function (response) {
            if (response.url.endsWith(wellKnownOpenIdConfiguration) && response.status === 200) {
                var openIdConnectData = JSON.parse(response.text);

                if (oidcDiscoveryEndpoint.length > 0) {
                    openIdConnectData.grant_types_supported = oidcSupportedFlows;
                }

                if (oidcSupportedFlows.length > 0) {
                    openIdConnectData.grant_types_supported = oidcSupportedFlows;
                }

                if (oidcSupportedScopes.length > 0) {
                    openIdConnectData.scopes_supported = oidcSupportedScopes;
                }

                if (openIdConnectData.issuer) {
                    configObject.resolvedIssuer = openIdConnectData.issuer;
                }
                
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
