var __spreadArrays = (this && this.__spreadArrays) || function () {
    for (var s = 0, i = 0, il = arguments.length; i < il; i++) s += arguments[i].length;
    for (var r = Array(s), k = 0, i = 0; i < il; i++)
        for (var a = arguments[i], j = 0, jl = a.length; j < jl; j++, k++)
            r[k] = a[j];
    return r;
};
var abp;
(function (abp) {
    /* Application paths *****************************************/
    //Current application root path (including virtual directory if exists).
    abp.appPath = abp.appPath || "/";
    abp.pageLoadTime = new Date();
    //Converts given path to absolute path using abp.appPath variable.
    function toAbsAppPath(path) {
        if (path.indexOf("/") == 0) {
            path = path.substring(1);
        }
        return abp.appPath + path;
    }
    abp.toAbsAppPath = toAbsAppPath;
    /* LOGGING ***************************************************/
    //Implements Logging API that provides secure & controlled usage of console.log
    var log;
    (function (log_1) {
        var levels;
        (function (levels) {
            levels[levels["DEBUG"] = 1] = "DEBUG";
            levels[levels["INFO"] = 2] = "INFO";
            levels[levels["WARN"] = 3] = "WARN";
            levels[levels["ERROR"] = 4] = "ERROR";
            levels[levels["FATAL"] = 5] = "FATAL";
        })(levels = log_1.levels || (log_1.levels = {}));
        log_1.level = levels.DEBUG;
        function log(logObject, logLevel) {
            if (!window.console || !window.console.log) {
                return;
            }
            if (logLevel != undefined && logLevel < log_1.level) {
                return;
            }
            console.log(logObject);
        }
        log_1.log = log;
        function debug(logObject) {
            log("DEBUG: ", levels.DEBUG);
            log(logObject, levels.DEBUG);
        }
        log_1.debug = debug;
        function info(logObject) {
            log("INFO: ", levels.INFO);
            log(logObject, levels.INFO);
        }
        log_1.info = info;
        function warn(logObject) {
            log("WARN: ", levels.WARN);
            log(logObject, levels.WARN);
        }
        log_1.warn = warn;
        function error(logObject) {
            log("ERROR: ", levels.ERROR);
            log(logObject, levels.ERROR);
        }
        log_1.error = error;
        function fatal(logObject) {
            log("FATAL: ", levels.FATAL);
            log(logObject, levels.FATAL);
        }
        log_1.fatal = fatal;
    })(log = abp.log || (abp.log = {}));
    /* LOCALIZATION ***********************************************/
    var localization;
    (function (localization) {
        localization.values = {};
        localization.defaultResourceName = undefined;
        localization.currentCulture = {
            cultureName: undefined,
            name: undefined,
        };
        // TODO: TEST-REFACTORED
        function localize(key, sourceName) {
            var _a;
            var args = [];
            for (var _i = 2; _i < arguments.length; _i++) {
                args[_i - 2] = arguments[_i];
            }
            if (sourceName === "_") {
                //A convention to suppress the localization
                return key;
            }
            sourceName = sourceName || abp.localization.defaultResourceName;
            if (!sourceName) {
                abp.log.warn("Localization source name is not specified and the defaultResourceName was not defined!");
                return key;
            }
            var source = abp.localization.values[sourceName];
            if (!source) {
                abp.log.warn("Could not find localization source: " + sourceName);
                return key;
            }
            var value = source[key];
            if (value == undefined) {
                return key;
            }
            return (_a = abp.utils).formatString.apply(_a, __spreadArrays([value], args));
        }
        localization.localize = localize;
        function isLocalized(key, sourceName) {
            if (sourceName === "_") {
                //A convention to suppress the localization
                return true;
            }
            sourceName = sourceName || localization.defaultResourceName;
            if (!sourceName) {
                return false;
            }
            var source = localization.values[sourceName];
            if (!source) {
                return false;
            }
            var value = source[key];
            if (value === undefined) {
                return false;
            }
            return true;
        }
        localization.isLocalized = isLocalized;
        // TODO: TEST-REFACTORED
        function getResource(name) {
            return function (key) {
                var _a;
                var args = [];
                for (var _i = 1; _i < arguments.length; _i++) {
                    args[_i - 1] = arguments[_i];
                }
                return (_a = abp.localization).localize.apply(_a, __spreadArrays([key, name], args));
            };
        }
        localization.getResource = getResource;
        function getMapValue(packageMaps, packageName, language) {
            language = language || localization.currentCulture.name;
            if (!packageMaps || !packageName || !language) {
                return language;
            }
            var packageMap = packageMaps[packageName];
            if (!packageMap) {
                return language;
            }
            for (var i = 0; i < packageMap.length; i++) {
                var map = packageMap[i];
                if (map.name === language) {
                    return map.value;
                }
            }
            return language;
        }
        function getLanguagesMap(packageName, language) {
            return getMapValue(localization.languagesMap, packageName, language);
        }
        localization.getLanguagesMap = getLanguagesMap;
        function getLanguageFilesMap(packageName, language) {
            return getMapValue(localization.languageFilesMap, packageName, language);
        }
        localization.getLanguageFilesMap = getLanguageFilesMap;
    })(localization = abp.localization || (abp.localization = {}));
    /* AUTHORIZATION **********************************************/
    var auth;
    (function (auth) {
        auth.policies = abp.auth.policies || {};
        auth.grantedPolicies = abp.auth.grantedPolicies || {};
        auth.tokenCookieName = "Abp.AuthToken";
        function isGranted(policyName) {
            return !(auth.policies[policyName] && auth.grantedPolicies[policyName]);
        }
        auth.isGranted = isGranted;
        // TODO: TEST-REFACTORED
        function isAnyGranted() {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            if (!args || args.length <= 0) {
                return true;
            }
            return args.some(isGranted);
        }
        auth.isAnyGranted = isAnyGranted;
        // TODO: TEST-REFACTORED
        function areAllGranted() {
            var args = [];
            for (var _i = 0; _i < arguments.length; _i++) {
                args[_i] = arguments[_i];
            }
            if (!args || args.length <= 0) {
                return true;
            }
            return args.every(isGranted);
        }
        auth.areAllGranted = areAllGranted;
        function setToken(authToken, expireDate) {
            abp.utils.setCookieValue(abp.auth.tokenCookieName, authToken, expireDate, abp.appPath
            // TODO: DELETE-WITH-CARE following parameter existed before but it is not being used and its value is null
            // abp.domain
            );
        }
        auth.setToken = setToken;
        function getToken() {
            return abp.utils.getCookieValue(abp.auth.tokenCookieName);
        }
        auth.getToken = getToken;
        function clearToken() {
            setToken();
        }
        auth.clearToken = clearToken;
    })(auth = abp.auth || (abp.auth = {}));
    /* SETTINGS *************************************************/
    var setting;
    (function (setting) {
        setting.values = abp.setting.values || {};
        function get(name) {
            return setting.values[name];
        }
        setting.get = get;
        function getBoolean(name) {
            var value = get(name);
            return value == "true" || value == "True";
        }
        setting.getBoolean = getBoolean;
        function getInt(name) {
            return parseInt(setting.values[name]);
        }
        setting.getInt = getInt;
    })(setting = abp.setting || (abp.setting = {}));
    /* NOTIFICATION *********************************************/
    //Defines Notification API, not implements it
    var notify;
    (function (notify) {
        function success(message, title, options) {
            abp.log.warn("abp.notify.success is not implemented!");
        }
        notify.success = success;
        function info(message, title, options) {
            abp.log.warn("abp.notify.info is not implemented!");
        }
        notify.info = info;
        function warn(message, title, options) {
            abp.log.warn("abp.notify.warn is not implemented!");
        }
        notify.warn = warn;
        function error(message, title, options) {
            abp.log.warn("abp.notify.error is not implemented!");
        }
        notify.error = error;
    })(notify = abp.notify || (abp.notify = {}));
    /* MESSAGE **************************************************/
    //Defines Message API, not implements it
    var message;
    (function (message) {
        function _showMessage(msg, title) {
            if (title === void 0) { title = ""; }
            alert(title + " " + msg);
        }
        message._showMessage = _showMessage;
        function info(msg, title) {
            abp.log.warn("abp.message.info is not implemented!");
            return _showMessage(msg, title);
        }
        message.info = info;
        function success(msg, title) {
            abp.log.warn("abp.message.success is not implemented!");
            return _showMessage(msg, title);
        }
        message.success = success;
        function warn(msg, title) {
            abp.log.warn("abp.message.warn is not implemented!");
            return _showMessage(msg, title);
        }
        message.warn = warn;
        function error(msg, title) {
            abp.log.warn("abp.message.error is not implemented!");
            return _showMessage(msg, title);
        }
        message.error = error;
        function confirm(msg, titleOrCallback, callback) {
            abp.log.warn("abp.message.confirm is not properly implemented!");
            if (titleOrCallback && !(typeof titleOrCallback == "string")) {
                callback = titleOrCallback;
            }
            var result = confirm(msg);
            callback && callback(result);
        }
        message.confirm = confirm;
    })(message = abp.message || (abp.message = {}));
    /* UI *******************************************************/
    var ui;
    (function (ui) {
        /* opts: { //Can be an object with options or a string for query a selector
         *  elm: a query selector (optional - default: document.body)
         *  busy: boolean (optional - default: false)
         *  promise: A promise with always or finally handler (optional - auto unblocks the ui if provided)
         * }
         */
        function block(opts) {
            if (!opts) {
                opts = {};
            }
            else if (typeof opts == "string") {
                opts = {
                    elm: opts,
                };
            }
            var $elm = document.querySelector(opts.elm) || document.body;
            if (opts.busy) {
                $abpBlockArea.classList.add("abp-block-area-busy");
            }
            else {
                $abpBlockArea.classList.remove("abp-block-area-busy");
            }
            if (document.querySelector(opts.elm)) {
                $abpBlockArea.style.position = "absolute";
            }
            else {
                $abpBlockArea.style.position = "fixed";
            }
            $elm.appendChild($abpBlockArea);
            if (opts.promise) {
                if (opts.promise.always) {
                    //jQuery.Deferred style
                    opts.promise.always(function () {
                        unblock({
                            $elm: opts.elm,
                        });
                    });
                }
                else if (opts.promise["finally"]) {
                    //Q style
                    opts.promise["finally"](function () {
                        unblock({
                            $elm: opts.elm,
                        });
                    });
                }
            }
        }
        ui.block = block;
        /* opts: {
         *
         * }
         */
        function unblock(opts) {
            var element = document.querySelector(".abp-block-area");
            if (element) {
                element.classList.add("abp-block-area-disappearing");
                setTimeout(function () {
                    if (element) {
                        element.classList.remove("abp-block-area-disappearing");
                        element.parentElement.removeChild(element);
                    }
                }, 250);
            }
        }
        ui.unblock = unblock;
        /* UI BUSY */
        //Defines UI Busy API, not implements it
        function setBusy(opts) {
            if (!opts) {
                opts = {
                    busy: true,
                };
            }
            else if (typeof opts == "string") {
                opts = {
                    elm: opts,
                    busy: true,
                };
            }
            block(opts);
        }
        ui.setBusy = setBusy;
        function clearBusy(opts) {
            unblock(opts);
        }
        ui.clearBusy = clearBusy;
    })(ui = abp.ui || (abp.ui = {}));
    /* UI BLOCK */
    //Defines UI Block API and implements basically
    var $abpBlockArea = document.createElement("div");
    $abpBlockArea.classList.add("abp-block-area");
    // TODO: TEST-REFACTORED
    var EventBus = /** @class */ (function () {
        function EventBus() {
            this._callbacks = {};
        }
        EventBus.prototype.on = function (eventName, callback) {
            if (!this._callbacks[eventName]) {
                this._callbacks[eventName] = [];
            }
            this._callbacks[eventName].push(callback);
        };
        // TODO: TEST-REFACTORED
        EventBus.prototype.off = function (eventName, callback) {
            var callbacks = this._callbacks[eventName];
            if (!callbacks) {
                return;
            }
            this._callbacks[eventName] = callbacks.filter(function (c) { return c === callback; });
        };
        // TODO: TEST-REFACTORED
        EventBus.prototype.trigger = function (eventName) {
            var _this = this;
            var args = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                args[_i - 1] = arguments[_i];
            }
            var callbacks = this._callbacks[eventName];
            if (!callbacks || !callbacks.length) {
                return;
            }
            callbacks.forEach(function (callback) { return callback.apply(_this, args); });
        };
        return EventBus;
    }());
    abp.EventBus = EventBus;
    abp.event = new EventBus();
    /* UTILS ***************************************************/
    var utils;
    (function (utils) {
        /* Creates a name namespace.
         *  Example:
         *  var taskService = abp.utils.createNamespace(abp, 'services.task');
         *  taskService will be equal to abp.services.task
         *  first argument (root) must be defined first
         ************************************************************/
        function createNamespace(root, ns) {
            var parts = ns.split(".");
            for (var i = 0; i < parts.length; i++) {
                if (typeof root[parts[i]] == "undefined") {
                    root[parts[i]] = {};
                }
                root = root[parts[i]];
            }
            return root;
        }
        utils.createNamespace = createNamespace;
        /* Find and replaces a string (search) to another string (replacement) in
         *  given string (str).
         *  Example:
         *  abp.utils.replaceAll('This is a test string', 'is', 'X') = 'ThX X a test string'
         ************************************************************/
        function replaceAll(str, search, replacement) {
            var fix = search.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
            return str.replace(new RegExp(fix, "g"), replacement);
        }
        utils.replaceAll = replaceAll;
        // TODO: TEST-REFACTORED
        /* Formats a string just like string.format in C#.
         *  Example:
         *  abp.utils.formatString('Hello {0}','Tuana') = 'Hello Tuana'
         ************************************************************/
        function formatString(str) {
            var args = [];
            for (var _i = 1; _i < arguments.length; _i++) {
                args[_i - 1] = arguments[_i];
            }
            if (!args.length) {
                return str;
            }
            for (var i = 0; i < args.length; i++) {
                var placeHolder = "{" + i + "}";
                str = replaceAll(str, placeHolder, args[i]);
            }
            return str;
        }
        utils.formatString = formatString;
        function toPascalCase(str) {
            if (!str || !str.length) {
                return str;
            }
            if (str.length === 1) {
                return str.charAt(0).toUpperCase();
            }
            return str.charAt(0).toUpperCase() + str.substr(1);
        }
        utils.toPascalCase = toPascalCase;
        function toCamelCase(str) {
            if (!str || !str.length) {
                return str;
            }
            if (str.length === 1) {
                return str.charAt(0).toLowerCase();
            }
            return str.charAt(0).toLowerCase() + str.substr(1);
        }
        utils.toCamelCase = toCamelCase;
        function truncateString(str, maxLength) {
            if (!str || !str.length || str.length <= maxLength) {
                return str;
            }
            return str.substr(0, maxLength);
        }
        utils.truncateString = truncateString;
        function truncateStringWithPostfix(str, maxLength, postfix) {
            postfix = postfix || "...";
            if (!str || !str.length || str.length <= maxLength) {
                return str;
            }
            if (maxLength <= postfix.length) {
                return postfix.substr(0, maxLength);
            }
            return str.substr(0, maxLength - postfix.length) + postfix;
        }
        utils.truncateStringWithPostfix = truncateStringWithPostfix;
        function isFunction(obj) {
            return !!(obj && obj.constructor && obj.call && obj.apply);
        }
        utils.isFunction = isFunction;
        /**
         * parameterInfos should be an array of { name, value } objects
         * where name is query string parameter name and value is it's value.
         * includeQuestionMark is true by default.
         */
        function buildQueryString(parameterInfos, includeQuestionMark) {
            if (includeQuestionMark === undefined) {
                includeQuestionMark = true;
            }
            var qs = "";
            function addSeperator() {
                if (!qs.length) {
                    if (includeQuestionMark) {
                        qs = qs + "?";
                    }
                }
                else {
                    qs = qs + "&";
                }
            }
            for (var i = 0; i < parameterInfos.length; ++i) {
                var parameterInfo = parameterInfos[i];
                if (parameterInfo.value === undefined) {
                    continue;
                }
                if (parameterInfo.value === null) {
                    parameterInfo.value = "";
                }
                addSeperator();
                if (parameterInfo.value.toJSON &&
                    typeof parameterInfo.value.toJSON === "function") {
                    qs =
                        qs +
                            parameterInfo.name +
                            "=" +
                            encodeURIComponent(parameterInfo.value.toJSON());
                }
                else if (Array.isArray(parameterInfo.value) &&
                    parameterInfo.value.length) {
                    for (var j = 0; j < parameterInfo.value.length; j++) {
                        if (j > 0) {
                            addSeperator();
                        }
                        qs =
                            qs +
                                parameterInfo.name +
                                "[" +
                                j +
                                "]=" +
                                encodeURIComponent(parameterInfo.value[j]);
                    }
                }
                else {
                    qs =
                        qs +
                            parameterInfo.name +
                            "=" +
                            encodeURIComponent(parameterInfo.value);
                }
            }
            return qs;
        }
        utils.buildQueryString = buildQueryString;
        /**
         * Sets a cookie value for given key.
         * This is a simple implementation created to be used by ABP.
         * Please use a complete cookie library if you need.
         * @param {string} key
         * @param {string} value
         * @param {Date} expireDate (optional). If not specified the cookie will expire at the end of session.
         * @param {string} path (optional)
         */
        function setCookieValue(key, value, expireDate, path) {
            var cookieValue = encodeURIComponent(key) + "=";
            if (value) {
                cookieValue = cookieValue + encodeURIComponent(value);
            }
            if (expireDate) {
                cookieValue = cookieValue + "; expires=" + expireDate.toUTCString();
            }
            if (path) {
                cookieValue = cookieValue + "; path=" + path;
            }
            document.cookie = cookieValue;
        }
        utils.setCookieValue = setCookieValue;
        /**
         * Gets a cookie with given key.
         * This is a simple implementation created to be used by ABP.
         * Please use a complete cookie library if you need.
         * @param {string} key
         * @returns {string} Cookie value or null
         */
        function getCookieValue(key) {
            var equalities = document.cookie.split("; ");
            for (var i = 0; i < equalities.length; i++) {
                if (!equalities[i]) {
                    continue;
                }
                var splitted = equalities[i].split("=");
                if (splitted.length != 2) {
                    continue;
                }
                if (decodeURIComponent(splitted[0]) === key) {
                    return decodeURIComponent(splitted[1] || "");
                }
            }
            return null;
        }
        utils.getCookieValue = getCookieValue;
        /**
         * Deletes cookie for given key.
         * This is a simple implementation created to be used by ABP.
         * Please use a complete cookie library if you need.
         * @param {string} key
         * @param {string} path (optional)
         */
        function deleteCookie(key, path) {
            var cookieValue = encodeURIComponent(key) + "=";
            cookieValue =
                cookieValue +
                    "; expires=" +
                    new Date(new Date().getTime() - 86400000).toUTCString();
            if (path) {
                cookieValue = cookieValue + "; path=" + path;
            }
            document.cookie = cookieValue;
        }
        utils.deleteCookie = deleteCookie;
    })(utils = abp.utils || (abp.utils = {}));
    /* SECURITY ***************************************/
    var security;
    (function (security) {
        security.antiForgery = abp.security.antiForgery || {};
        security.antiForgery.tokenCookieName = "XSRF-TOKEN";
        security.antiForgery.tokenHeaderName = "RequestVerificationToken";
        security.antiForgery.getToken = function () {
            return abp.utils.getCookieValue(abp.security.antiForgery.tokenCookieName);
        };
    })(security = abp.security || (abp.security = {}));
    /* CLOCK *****************************************/
    var clock;
    (function (clock) {
        clock.kind = "Unspecified";
        function supportsMultipleTimezone() {
            return clock.kind === "Utc";
        }
        clock.supportsMultipleTimezone = supportsMultipleTimezone;
        function toLocal(date) {
            return new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds(), date.getMilliseconds());
        }
        function toUtc(date) {
            return Date.UTC(date.getUTCFullYear(), date.getUTCMonth(), date.getUTCDate(), date.getUTCHours(), date.getUTCMinutes(), date.getUTCSeconds(), date.getUTCMilliseconds());
        }
        function now() {
            if (clock.kind === "Utc") {
                return toUtc(new Date());
            }
            return new Date();
        }
        clock.now = now;
        function normalize(date) {
            if (clock.kind === "Unspecified") {
                return date;
            }
            if (clock.kind === "Local") {
                return toLocal(date);
            }
            if (clock.kind === "Utc") {
                return toUtc(date);
            }
        }
        clock.normalize = normalize;
    })(clock = abp.clock || (abp.clock = {}));
    /* FEATURES *************************************************/
    var features;
    (function (features) {
        features.values = abp.features.values || {};
        function isEnabled(name) {
            var value = get(name);
            return value == "true" || value == "True";
        }
        features.isEnabled = isEnabled;
        function get(name) {
            return features.values[name];
        }
        features.get = get;
    })(features = abp.features || (abp.features = {}));
})(abp || (abp = {}));
//# sourceMappingURL=abp.js.map