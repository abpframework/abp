var abp = abp || {};

(function () {
    abp.SwaggerUIBundle = function (configObject) {
        configObject.requestInterceptor = function (request) {
            var token = abp.auth.getToken();
            request.headers.Authorization = token ? "Bearer " + token : null;
            var antiForgeryToken = abp.security.antiForgery.getToken();
            if (antiForgeryToken) {
                request.headers[abp.security.antiForgery.tokenHeaderName] = antiForgeryToken;
            }
            return request;
        };

        return SwaggerUIBundle(configObject);
    }
})();
