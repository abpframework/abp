var abp = abp || {};
(function () {

    /* Application paths *****************************************/

    //Current application root path (including virtual directory if exists).
    abp.appPath = abp.appPath || '/';

    abp.pageLoadTime = new Date();

    //Converts given path to absolute path using abp.appPath variable.
    abp.toAbsAppPath = function (path) {
        if (path.indexOf('/') == 0) {
            path = path.substring(1);
        }

        return abp.appPath + path;
    };

    /* LOGGING ***************************************************/
    //Implements Logging API that provides secure & controlled usage of console.log

    abp.log = abp.log || {};

    abp.log.levels = {
        DEBUG: 1,
        INFO: 2,
        WARN: 3,
        ERROR: 4,
        FATAL: 5
    };

    abp.log.level = abp.log.levels.DEBUG;

    abp.log.log = function (logObject, logLevel) {
        if (!window.console || !window.console.log) {
            return;
        }

        if (logLevel != undefined && logLevel < abp.log.level) {
            return;
        }

        console.log(logObject);
    };

    abp.log.debug = function (logObject) {
        abp.log.log("DEBUG: ", abp.log.levels.DEBUG);
        abp.log.log(logObject, abp.log.levels.DEBUG);
    };

    abp.log.info = function (logObject) {
        abp.log.log("INFO: ", abp.log.levels.INFO);
        abp.log.log(logObject, abp.log.levels.INFO);
    };

    abp.log.warn = function (logObject) {
        abp.log.log("WARN: ", abp.log.levels.WARN);
        abp.log.log(logObject, abp.log.levels.WARN);
    };

    abp.log.error = function (logObject) {
        abp.log.log("ERROR: ", abp.log.levels.ERROR);
        abp.log.log(logObject, abp.log.levels.ERROR);
    };

    abp.log.fatal = function (logObject) {
        abp.log.log("FATAL: ", abp.log.levels.FATAL);
        abp.log.log(logObject, abp.log.levels.FATAL);
    };

    /* LOCALIZATION ***********************************************/

    abp.localization = abp.localization || {};

    abp.localization.values = {};

    abp.localization.localize = function (key, sourceName) {
        if (sourceName === '_') { //A convention to suppress the localization
            return key;
        }

        sourceName = sourceName || abp.localization.defaultResourceName;
        if (!sourceName) {
            abp.log.warn('Localization source name is not specified and the defaultResourceName was not defined!');
            return key;
        }

        var source = abp.localization.values[sourceName];
        if (!source) {
            abp.log.warn('Could not find localization source: ' + sourceName);
            return key;
        }

        var value = source[key];
        if (value == undefined) {
            return key;
        }

        var copiedArguments = Array.prototype.slice.call(arguments, 0);
        copiedArguments.splice(1, 1);
        copiedArguments[0] = value;

        return abp.utils.formatString.apply(this, copiedArguments);
    };

    abp.localization.isLocalized = function (key, sourceName) {
        if (sourceName === '_') { //A convention to suppress the localization
            return true;
        }

        sourceName = sourceName || abp.localization.defaultResourceName;
        if (!sourceName) {
            return false;
        }

        var source = abp.localization.values[sourceName];
        if (!source) {
            return false;
        }

        var value = source[key];
        if (value === undefined) {
            return false;
        }

        return true;
    };

    abp.localization.getResource = function (name) {
        return function () {
            var copiedArguments = Array.prototype.slice.call(arguments, 0);
            copiedArguments.splice(1, 0, name);
            return abp.localization.localize.apply(this, copiedArguments);
        };
    };

    abp.localization.defaultResourceName = undefined;

    /* AUTHORIZATION **********************************************/

    abp.auth = abp.auth || {};

    abp.auth.policies = abp.auth.policies || {};

    abp.auth.grantedPolicies = abp.auth.grantedPolicies || {};

    abp.auth.isGranted = function (policyName) {
        return abp.auth.policies[policyName] != undefined && abp.auth.grantedPolicies[policyName] != undefined;
    };

    abp.auth.isAnyGranted = function () {
        if (!arguments || arguments.length <= 0) {
            return true;
        }

        for (var i = 0; i < arguments.length; i++) {
            if (abp.auth.isGranted(arguments[i])) {
                return true;
            }
        }

        return false;
    };

    abp.auth.areAllGranted = function () {
        if (!arguments || arguments.length <= 0) {
            return true;
        }

        for (var i = 0; i < arguments.length; i++) {
            if (!abp.auth.isGranted(arguments[i])) {
                return false;
            }
        }

        return true;
    };

    abp.auth.tokenCookieName = 'Abp.AuthToken';

    abp.auth.setToken = function (authToken, expireDate) {
        abp.utils.setCookieValue(abp.auth.tokenCookieName, authToken, expireDate, abp.appPath, abp.domain);
    };

    abp.auth.getToken = function () {
        return abp.utils.getCookieValue(abp.auth.tokenCookieName);
    }

    abp.auth.clearToken = function () {
        abp.auth.setToken();
    }

    /* SETTINGS *************************************************/

    abp.setting = abp.setting || {};

    abp.setting.values = abp.setting.values || {};

    abp.setting.get = function (name) {
        return abp.setting.values[name];
    };

    abp.setting.getBoolean = function (name) {
        var value = abp.setting.get(name);
        return value == 'true' || value == 'True';
    };

    abp.setting.getInt = function (name) {
        return parseInt(abp.setting.values[name]);
    };

    /* NOTIFICATION *********************************************/
    //Defines Notification API, not implements it

    abp.notify = abp.notify || {};

    abp.notify.success = function (message, title, options) {
        abp.log.warn('abp.notify.success is not implemented!');
    };

    abp.notify.info = function (message, title, options) {
        abp.log.warn('abp.notify.info is not implemented!');
    };

    abp.notify.warn = function (message, title, options) {
        abp.log.warn('abp.notify.warn is not implemented!');
    };

    abp.notify.error = function (message, title, options) {
        abp.log.warn('abp.notify.error is not implemented!');
    };

    /* MESSAGE **************************************************/
    //Defines Message API, not implements it

    abp.message = abp.message || {};

    abp.message._showMessage = function (message, title) {
        alert((title || '') + ' ' + message);
    };

    abp.message.info = function (message, title) {
        abp.log.warn('abp.message.info is not implemented!');
        return abp.message._showMessage(message, title);
    };

    abp.message.success = function (message, title) {
        abp.log.warn('abp.message.success is not implemented!');
        return abp.message._showMessage(message, title);
    };

    abp.message.warn = function (message, title) {
        abp.log.warn('abp.message.warn is not implemented!');
        return abp.message._showMessage(message, title);
    };

    abp.message.error = function (message, title) {
        abp.log.warn('abp.message.error is not implemented!');
        return abp.message._showMessage(message, title);
    };

    abp.message.confirm = function (message, titleOrCallback, callback) {
        abp.log.warn('abp.message.confirm is not properly implemented!');

        if (titleOrCallback && !(typeof titleOrCallback == 'string')) {
            callback = titleOrCallback;
        }

        var result = confirm(message);
        callback && callback(result);
    };

    /* UI *******************************************************/

    abp.ui = abp.ui || {};

    /* UI BLOCK */
    //Defines UI Block API and implements basically

    var $abpBlockArea = document.createElement('div');
    $abpBlockArea.classList.add('abp-block-area');

    /* opts: { //Can be an object with options or a string for query a selector
     *  elm: a query selector (optional - default: document.body)
     *  busy: boolean (optional - default: false)
     *  promise: A promise with always or finally handler (optional - auto unblocks the ui if provided)
     * }
     */
    abp.ui.block = function (opts) {
        if (!opts) {
            opts = {};
        } else if (typeof opts == 'string') {
            opts = {
                elm: opts
            };
        }

        var $elm = document.querySelector(opts.elm) || document.body;

        if (opts.busy) {
            $abpBlockArea.classList.add('abp-block-area-busy');
        } else {
            $abpBlockArea.classList.remove('abp-block-area-busy');
        }

        if (document.querySelector(opts.elm)) {
            $abpBlockArea.style.position = 'absolute';
        } else {
            $abpBlockArea.style.position = 'fixed';
        }

        $elm.appendChild($abpBlockArea);

        if (opts.promise) {
            if (opts.promise.always) { //jQuery.Deferred style
                opts.promise.always(function () {
                    abp.ui.unblock({
                        $elm: opts.elm
                    });
                });
            } else if (opts.promise['finally']) { //Q style
                opts.promise['finally'](function () {
                    abp.ui.unblock({
                        $elm: opts.elm
                    });
                });
            }
        }
    };

    /* opts: {
     *    
     * }
     */
    abp.ui.unblock = function (opts) {
        var element = document.querySelector('.abp-block-area');
        if (element) {
            element.classList.add('abp-block-area-disappearing');
            setTimeout(function () {
                if (element) {
                    element.classList.remove('abp-block-area-disappearing');
                    element.parentElement.removeChild(element);
                }
            }, 250);
        }
    };

    /* UI BUSY */
    //Defines UI Busy API, not implements it

    abp.ui.setBusy = function (opts) {
        if (!opts) {
            opts = {
                busy: true
            };
        } else if (typeof opts == 'string') {
            opts = {
                elm: opts,
                busy: true
            };
        }

        abp.ui.block(opts);
    };

    abp.ui.clearBusy = function (opts) {
        abp.ui.unblock(opts);
    };

    /* SIMPLE EVENT BUS *****************************************/

    abp.event = (function () {

        var _callbacks = {};

        var on = function (eventName, callback) {
            if (!_callbacks[eventName]) {
                _callbacks[eventName] = [];
            }

            _callbacks[eventName].push(callback);
        };

        var off = function (eventName, callback) {
            var callbacks = _callbacks[eventName];
            if (!callbacks) {
                return;
            }

            var index = -1;
            for (var i = 0; i < callbacks.length; i++) {
                if (callbacks[i] === callback) {
                    index = i;
                    break;
                }
            }

            if (index < 0) {
                return;
            }

            _callbacks[eventName].splice(index, 1);
        };

        var trigger = function (eventName) {
            var callbacks = _callbacks[eventName];
            if (!callbacks || !callbacks.length) {
                return;
            }

            var args = Array.prototype.slice.call(arguments, 1);
            for (var i = 0; i < callbacks.length; i++) {
                callbacks[i].apply(this, args);
            }
        };

        // Public interface ///////////////////////////////////////////////////

        return {
            on: on,
            off: off,
            trigger: trigger
        };
    })();


    /* UTILS ***************************************************/

    abp.utils = abp.utils || {};

    /* Creates a name namespace.
    *  Example:
    *  var taskService = abp.utils.createNamespace(abp, 'services.task');
    *  taskService will be equal to abp.services.task
    *  first argument (root) must be defined first
    ************************************************************/
    abp.utils.createNamespace = function (root, ns) {
        var parts = ns.split('.');
        for (var i = 0; i < parts.length; i++) {
            if (typeof root[parts[i]] == 'undefined') {
                root[parts[i]] = {};
            }

            root = root[parts[i]];
        }

        return root;
    };

    /* Find and replaces a string (search) to another string (replacement) in
    *  given string (str).
    *  Example:
    *  abp.utils.replaceAll('This is a test string', 'is', 'X') = 'ThX X a test string'
    ************************************************************/
    abp.utils.replaceAll = function (str, search, replacement) {
        var fix = search.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
        return str.replace(new RegExp(fix, 'g'), replacement);
    };

    /* Formats a string just like string.format in C#.
    *  Example:
    *  abp.utils.formatString('Hello {0}','Tuana') = 'Hello Tuana'
    ************************************************************/
    abp.utils.formatString = function () {
        if (arguments.length < 1) {
            return null;
        }

        var str = arguments[0];

        for (var i = 1; i < arguments.length; i++) {
            var placeHolder = '{' + (i - 1) + '}';
            str = abp.utils.replaceAll(str, placeHolder, arguments[i]);
        }

        return str;
    };

    abp.utils.toPascalCase = function (str) {
        if (!str || !str.length) {
            return str;
        }

        if (str.length === 1) {
            return str.charAt(0).toUpperCase();
        }

        return str.charAt(0).toUpperCase() + str.substr(1);
    }

    abp.utils.toCamelCase = function (str) {
        if (!str || !str.length) {
            return str;
        }

        if (str.length === 1) {
            return str.charAt(0).toLowerCase();
        }

        return str.charAt(0).toLowerCase() + str.substr(1);
    }

    abp.utils.truncateString = function (str, maxLength) {
        if (!str || !str.length || str.length <= maxLength) {
            return str;
        }

        return str.substr(0, maxLength);
    };

    abp.utils.truncateStringWithPostfix = function (str, maxLength, postfix) {
        postfix = postfix || '...';

        if (!str || !str.length || str.length <= maxLength) {
            return str;
        }

        if (maxLength <= postfix.length) {
            return postfix.substr(0, maxLength);
        }

        return str.substr(0, maxLength - postfix.length) + postfix;
    };

    abp.utils.isFunction = function (obj) {
        return !!(obj && obj.constructor && obj.call && obj.apply);
    };

    /**
     * parameterInfos should be an array of { name, value } objects
     * where name is query string parameter name and value is it's value.
     * includeQuestionMark is true by default.
     */
    abp.utils.buildQueryString = function (parameterInfos, includeQuestionMark) {
        if (includeQuestionMark === undefined) {
            includeQuestionMark = true;
        }

        var qs = '';

        function addSeperator() {
            if (!qs.length) {
                if (includeQuestionMark) {
                    qs = qs + '?';
                }
            } else {
                qs = qs + '&';
            }
        }

        for (var i = 0; i < parameterInfos.length; ++i) {
            var parameterInfo = parameterInfos[i];
            if (parameterInfo.value === undefined) {
                continue;
            }

            if (parameterInfo.value === null) {
                parameterInfo.value = '';
            }

            addSeperator();

            if (parameterInfo.value.toJSON && typeof parameterInfo.value.toJSON === "function") {
                qs = qs + parameterInfo.name + '=' + encodeURIComponent(parameterInfo.value.toJSON());
            } else if (Array.isArray(parameterInfo.value) && parameterInfo.value.length) {
                for (var j = 0; j < parameterInfo.value.length; j++) {
                    if (j > 0) {
                        addSeperator();
                    }

                    qs = qs + parameterInfo.name + '[' + j + ']=' + encodeURIComponent(parameterInfo.value[j]);
                }
            } else {
                qs = qs + parameterInfo.name + '=' + encodeURIComponent(parameterInfo.value);
            }
        }

        return qs;
    }

    /**
     * Sets a cookie value for given key.
     * This is a simple implementation created to be used by ABP.
     * Please use a complete cookie library if you need.
     * @param {string} key
     * @param {string} value 
     * @param {Date} expireDate (optional). If not specified the cookie will expire at the end of session.
     * @param {string} path (optional)
     */
    abp.utils.setCookieValue = function (key, value, expireDate, path) {
        var cookieValue = encodeURIComponent(key) + '=';

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
    };

    /**
     * Gets a cookie with given key.
     * This is a simple implementation created to be used by ABP.
     * Please use a complete cookie library if you need.
     * @param {string} key
     * @returns {string} Cookie value or null
     */
    abp.utils.getCookieValue = function (key) {
        var equalities = document.cookie.split('; ');
        for (var i = 0; i < equalities.length; i++) {
            if (!equalities[i]) {
                continue;
            }

            var splitted = equalities[i].split('=');
            if (splitted.length != 2) {
                continue;
            }

            if (decodeURIComponent(splitted[0]) === key) {
                return decodeURIComponent(splitted[1] || '');
            }
        }

        return null;
    };

    /**
     * Deletes cookie for given key.
     * This is a simple implementation created to be used by ABP.
     * Please use a complete cookie library if you need.
     * @param {string} key
     * @param {string} path (optional)
     */
    abp.utils.deleteCookie = function (key, path) {
        var cookieValue = encodeURIComponent(key) + '=';

        cookieValue = cookieValue + "; expires=" + (new Date(new Date().getTime() - 86400000)).toUTCString();

        if (path) {
            cookieValue = cookieValue + "; path=" + path;
        }

        document.cookie = cookieValue;
    }

    /* SECURITY ***************************************/
    abp.security = abp.security || {};
    abp.security.antiForgery = abp.security.antiForgery || {};

    abp.security.antiForgery.tokenCookieName = 'XSRF-TOKEN';
    abp.security.antiForgery.tokenHeaderName = 'X-XSRF-TOKEN';

    abp.security.antiForgery.getToken = function () {
        return abp.utils.getCookieValue(abp.security.antiForgery.tokenCookieName);
    };

})();