/**
 * abp.ResourceLoader can be used to load a script/style file from a URL on demand.
 * It ensures that a script/style is only loaded once.
 */
var abp = abp || {};
(function ($) {

    /* UrlStates enum */
    var UrlStates = {
        LOADING: 'LOADING',
        LOADED: 'LOADED',
        FAILED: 'FAILED'
    };

    /* UrlInfo class */
    function UrlInfo(url) {
        this.url = url;
        this.state = UrlStates.LOADING;
        this.loadCallbacks = [];
        this.failCallbacks = [];
    }

    UrlInfo.prototype.succeed = function () {
        this.state = UrlStates.LOADED;
        for (var i = 0; i < this.loadCallbacks.length; i++) {
            this.loadCallbacks[i]();
        }
    };

    UrlInfo.prototype.failed = function () {
        this.state = UrlStates.FAILED;
        for (var i = 0; i < this.failCallbacks.length; i++) {
            this.failCallbacks[i]();
        }
    };

    UrlInfo.prototype.handleCallbacks = function (loadCallback, failCallback) {
        switch (this.state) {
            case UrlStates.LOADED:
                loadCallback && loadCallback();
                break;
            case UrlStates.FAILED:
                failCallback && failCallback();
                break;
            case UrlStates.LOADING:
                this.addCallbacks(loadCallback, failCallback);
                break;
        }
    };

    UrlInfo.prototype.addCallbacks = function (loadCallback, failCallback) {
        loadCallback && this.loadCallbacks.push(loadCallback);
        failCallback && this.failCallbacks.push(failCallback);
    };

    /* ResourceLoader API */

    abp.ResourceLoader = (function () {

        var _urlInfos = {};

        function getCacheKey(url) {
            return url;
        }

        function appendTimeToUrl(url) {

            if (url.indexOf('?') < 0) {
                url += '?';
            } else {
                url += '&';
            }

            url += '_=' + new Date().getTime();

            return url;
        }

        var _loadFromUrl = function (url, loadCallback, failCallback, serverLoader) {

            var cacheKey = getCacheKey(url);

            var urlInfo = _urlInfos[cacheKey];

            if (urlInfo) {
                urlInfo.handleCallbacks(loadCallback, failCallback);
                return;
            }

            _urlInfos[cacheKey] = urlInfo = new UrlInfo(url);
            urlInfo.addCallbacks(loadCallback, failCallback);

            serverLoader(urlInfo);
        };

        var _loadScript = function (url, loadCallback, failCallback) {
            _loadFromUrl(url, loadCallback, failCallback, function(urlInfo) {
                $.getScript(url)
                    .done(function () {
                        urlInfo.succeed();
                    })
                    .fail(function () {
                        urlInfo.failed();
                    });
            });
        };

        var _loadStyle = function (url) {
            _loadFromUrl(url, undefined, undefined, function (urlInfo) {
                
                $('<link/>', {
                    rel: 'stylesheet',
                    type: 'text/css',
                    href: appendTimeToUrl(url)
                }).appendTo('head');
            });
        };

        return {
            loadScript: _loadScript,
            loadStyle: _loadStyle
        }
    })();

})(jQuery);