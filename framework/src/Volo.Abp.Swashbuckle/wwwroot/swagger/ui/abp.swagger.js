var abp = abp || {};

(function () {

    var oldSwaggerUIBundle = SwaggerUIBundle;

    SwaggerUIBundle = function (configObject) {
        var excludeUrl = ["swagger.json", "connect/token"]
        var firstRequest = true;
        var oidcSupportedFlows = configObject.oidcSupportedFlows || [];
        var oidcSupportedScopes = configObject.oidcSupportedScopes || [];
        var oidcDiscoveryEndpoint = configObject.oidcDiscoveryEndpoint || [];
        var tenantPlaceHolders = ["{{tenantId}}", "{{tenantName}}", "{0}"]
        abp.appPath = abp.appPath || "/";

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

            // Intercept .well-known request when the discoveryEndpoint is provided
            if (response.url.endsWith("swagger.json") && response.status === 200 && oidcDiscoveryEndpoint.length !== 0) {
                var swaggerData = JSON.parse(response.text);

                if (swaggerData.components.securitySchemes && swaggerData.components.securitySchemes.oidc) {
                    swaggerData.components.securitySchemes.oidc.openIdConnectUrl = await replaceTenantPlaceHolder(oidcDiscoveryEndpoint);
                }

                response.text = JSON.stringify(swaggerData);
            }

            if (responseInterceptor) {
                responseInterceptor(response);
            }
            return response;
        };

        async function replaceTenantPlaceHolder(url) {

            if (!abp.currentTenant) {
                await getAbpApplicationConfiguration();
            }

            if (abp.currentTenant.id == null && abp.currentTenant.name == null) {
                return url
                    .replace(tenantPlaceHolders[0] + ".", "")
                    .replace(tenantPlaceHolders[1] + ".", "")
                    .replace(tenantPlaceHolders[2] + ".", "");
            }

            url = url.replace(tenantPlaceHolders[0], abp.currentTenant.id).replace(tenantPlaceHolders[1], abp.currentTenant.name);

            if (abp.currentTenant.name != null) {
                url = url.replace(tenantPlaceHolders[2], abp.currentTenant.name);
            } else if (abp.currentTenant.id != null) {
                url = url.replace(tenantPlaceHolders[2], abp.currentTenant.id);
            }

            return url;
        }

        function getAbpApplicationConfiguration() {
            return fetch(`${abp.appPath}api/abp/application-configuration`).then(response => response.json()).then(data => {
                abp.currentTenant = data.currentTenant;
            });
        }

        return oldSwaggerUIBundle(configObject);
    }

    SwaggerUIBundle = Object.assign(SwaggerUIBundle, oldSwaggerUIBundle);

    window.addEventListener("storage", function (event) {
        if (event.key !== "abp_swagger_oauth2" || !event.newValue) {
            return;
        }

        var qp = JSON.parse(event.newValue || "{}");
        localStorage.removeItem("abp_swagger_oauth2");
        var oauth2 = window.swaggerUIRedirectOauth2;
        var sentState = oauth2.state;
        var redirectUrl = oauth2.redirectUrl;
        var isValid = qp.state === sentState;

        if ((
          oauth2.auth.schema.get("flow") === "accessCode" ||
          oauth2.auth.schema.get("flow") === "authorizationCode" ||
          oauth2.auth.schema.get("flow") === "authorization_code"
        ) && !oauth2.auth.code) {
            if (!isValid) {
                oauth2.errCb({
                    authId: oauth2.auth.name,
                    source: "auth",
                    level: "warning",
                    message: "Authorization may be unsafe, passed state was changed in server. The passed state wasn't returned from auth server."
                });
            }

            if (qp.code) {
                delete oauth2.state;
                oauth2.auth.code = qp.code;
                oauth2.callback({auth: oauth2.auth, redirectUrl: redirectUrl});
            } else {
                let oauthErrorMsg;
                if (qp.error) {
                    oauthErrorMsg = "["+qp.error+"]: " +
                        (qp.error_description ? qp.error_description+ ". " : "no accessCode received from the server. ") +
                        (qp.error_uri ? "More info: "+qp.error_uri : "");
                }

                oauth2.errCb({
                    authId: oauth2.auth.name,
                    source: "auth",
                    level: "error",
                    message: oauthErrorMsg || "[Authorization failed]: no accessCode received from the server."
                });
            }
        } else {
            oauth2.callback({auth: oauth2.auth, token: qp, isValid: isValid, redirectUrl: redirectUrl});
        }
    });

})();
