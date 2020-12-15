var abp = abp || {};

(function () {
    abp.SwaggerUIBundle = function (configObject) {

        abp.setAntiForgeryToken();

        configObject.requestInterceptor = function (request) {

            var antiForgeryToken = abp.security.antiForgery.getToken();
            if (antiForgeryToken) {
                request.headers[abp.security.antiForgery.tokenHeaderName] = antiForgeryToken;
            }
            return request;
        };

        return SwaggerUIBundle(configObject);
    }

    abp.setAntiForgeryToken = function(){
        fetch("/api/abp/application-configuration");
    }
})();
