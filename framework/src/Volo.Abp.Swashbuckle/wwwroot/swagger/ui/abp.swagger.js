var abp = abp || {};

(function () {

    abp.SwaggerUIBundle = function (configObject) {

        fetch("/abp/Swashbuckle/SetCsrfCookie");

        configObject.requestInterceptor = function (request) {

            var antiForgeryToken = abp.security.antiForgery.getToken();
            if (antiForgeryToken) {
                request.headers[abp.security.antiForgery.tokenHeaderName] = antiForgeryToken;
            }
            return request;
        };

        return SwaggerUIBundle(configObject);
    }
})();
