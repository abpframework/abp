var abp = abp || {};

(function () {

    abp.SwaggerUIBundle = function (configObject) {

        var excludeUrl = ["swagger.json", "connect/token"]
        var firstRequest = true;
        var oidcSupportedFlows = configObject.oidcSupportedFlows || [];
        var oidcSupportedScopes = configObject.oidcSupportedScopes || [];
        var oidcDiscoveryEndpoint = configObject.oidcDiscoveryEndpoint || [];
        var tenantPlaceHolders = ["{{tenantId}}", "{{tenantName}}" , "{0}"]
        abp.appPath = configObject.baseUrl || abp.appPath;

        var requestInterceptor = configObject.requestInterceptor;
        var responseInterceptor = configObject.responseInterceptor;

        getAbpApplicationConfiguration();
        
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
            if (!firstRequest && oidcDiscoveryEndpoint.length !== 0 && request.url.includes(".well-known/openid-configuration")) {
                if (oidcDiscoveryEndpoint.endsWith(".well-known/openid-configuration")) {
                    request.url = replaceTenantPlaceHolder(oidcDiscoveryEndpoint);
                    console.log(request.url);
                    return;
                }
                if (!oidcDiscoveryEndpoint.endsWith("/")) {
                    oidcDiscoveryEndpoint += "/"
                }
                request.url = replaceTenantPlaceHolder(oidcDiscoveryEndpoint) + ".well-known/openid-configuration";
                
                console.log(request.url);
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

        configObject.responseInterceptor = async function (response) {
            if (response.url.endsWith(".well-known/openid-configuration") && response.status === 200) {
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

                response.text = JSON.stringify(openIdConnectData);
            }

            if (responseInterceptor) {
                responseInterceptor(response);
            }
            return response;
        };
        
        function replaceTenantPlaceHolder(url) {
            
            url.replace(tenantPlaceHolders[0], abp.currentTenant.id);
            url.replace(tenantPlaceHolders[1], abp.currentTenant.name);
            
            if(abp.currentTenant.name != null){
                url.replace(tenantPlaceHolders[2], abp.currentTenant.name);
            }else if (abp.currentTenant.id != null){
                url.replace(tenantPlaceHolders[2], abp.currentTenant.id);
            }
            
            return url;
        }
        
        function getAbpApplicationConfiguration() {
            fetch(`${abp.appPath}api/abp/application-configuration`).then(response => response.json()).then(data => {
                abp.currentTenant = data.currentTenant; 
            });
        }

        return SwaggerUIBundle(configObject);
    }
})();
