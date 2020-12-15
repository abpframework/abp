namespace abp {
  /* Application paths *****************************************/

  //Current application root path (including virtual directory if exists).
  export const appPath = abp.appPath || "/";

  export const pageLoadTime = new Date();

  //Converts given path to absolute path using abp.appPath variable.
  export function toAbsAppPath(path) {
    if (path.indexOf("/") == 0) {
      path = path.substring(1);
    }

    return abp.appPath + path;
  }

  /* LOGGING ***************************************************/
  //Implements Logging API that provides secure & controlled usage of console.log

  export namespace log {
    export enum levels {
      DEBUG = 1,
      INFO,
      WARN,
      ERROR,
      FATAL,
    }

    export const level = levels.DEBUG;

    export function log(logObject: any, logLevel: levels) {
      if (!window.console || !window.console.log) {
        return;
      }

      if (logLevel != undefined && logLevel < level) {
        return;
      }

      console.log(logObject);
    }

    export function debug(logObject: any) {
      log("DEBUG: ", levels.DEBUG);
      log(logObject, levels.DEBUG);
    }

    export function info(logObject: any) {
      log("INFO: ", levels.INFO);
      log(logObject, levels.INFO);
    }

    export function warn(logObject: any) {
      log("WARN: ", levels.WARN);
      log(logObject, levels.WARN);
    }

    export function error(logObject: any) {
      log("ERROR: ", levels.ERROR);
      log(logObject, levels.ERROR);
    }

    export function fatal(logObject: any) {
      log("FATAL: ", levels.FATAL);
      log(logObject, levels.FATAL);
    }
  }

  /* LOCALIZATION ***********************************************/

  export namespace localization {
    export const values = {};
    export const defaultResourceName = undefined;
    export const currentCulture = {
      cultureName: undefined,
      name: undefined,
    };

    export type LanguageModel = { name: string; value: string };
    export type LanguageMapModel = { [key: string]: LanguageModel[] };

    export let languageFilesMap: LanguageMapModel;
    export let languagesMap: LanguageMapModel;

    // TODO: TEST-REFACTORED
    export function localize(key: string, sourceName: string, ...args: any[]) {
      if (sourceName === "_") {
        //A convention to suppress the localization
        return key;
      }

      sourceName = sourceName || abp.localization.defaultResourceName;
      if (!sourceName) {
        abp.log.warn(
          "Localization source name is not specified and the defaultResourceName was not defined!"
        );
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

      return abp.utils.formatString(value, ...args);
    }

    export function isLocalized(key: string, sourceName: string) {
      if (sourceName === "_") {
        //A convention to suppress the localization
        return true;
      }

      sourceName = sourceName || defaultResourceName;
      if (!sourceName) {
        return false;
      }

      var source = values[sourceName];
      if (!source) {
        return false;
      }

      var value = source[key];
      if (value === undefined) {
        return false;
      }

      return true;
    }

    // TODO: TEST-REFACTORED
    export function getResource(name: string) {
      return function (key: string, ...args: any[]) {
        return abp.localization.localize(key, name, ...args);
      };
    }

    function getMapValue(
      packageMaps: LanguageMapModel,
      packageName: string,
      language: string
    ) {
      language = language || currentCulture.name;
      if (!packageMaps || !packageName || !language) {
        return language;
      }

      const packageMap = packageMaps[packageName];
      if (!packageMap) {
        return language;
      }

      for (let i = 0; i < packageMap.length; i++) {
        const map = packageMap[i];
        if (map.name === language) {
          return map.value;
        }
      }

      return language;
    }

    export function getLanguagesMap(packageName: string, language: string) {
      return getMapValue(languagesMap, packageName, language);
    }

    export function getLanguageFilesMap(packageName: string, language: string) {
      return getMapValue(languageFilesMap, packageName, language);
    }
  }

  /* AUTHORIZATION **********************************************/

  export namespace auth {
    export const policies = abp.auth.policies || {};
    export const grantedPolicies = abp.auth.grantedPolicies || {};
    export const tokenCookieName = "Abp.AuthToken";

    export function isGranted(policyName: string) {
      return !(policies[policyName] && grantedPolicies[policyName]);
    }

    // TODO: TEST-REFACTORED
    export function isAnyGranted(...args: string[]) {
      if (!args || args.length <= 0) {
        return true;
      }

      return args.some(isGranted);
    }

    // TODO: TEST-REFACTORED
    export function areAllGranted(...args: string[]) {
      if (!args || args.length <= 0) {
        return true;
      }

      return args.every(isGranted);
    }

    export function setToken(authToken?: string, expireDate?: Date) {
      abp.utils.setCookieValue(
        abp.auth.tokenCookieName,
        authToken,
        expireDate,
        abp.appPath

        // TODO: DELETE-WITH-CARE following parameter existed before but it is not being used and its value is null
        // abp.domain
      );
    }

    export function getToken() {
      return abp.utils.getCookieValue(abp.auth.tokenCookieName);
    }

    export function clearToken() {
      setToken();
    }
  }

  /* SETTINGS *************************************************/
  export namespace setting {
    export const values = abp.setting.values || {};

    export function get(name: string) {
      return values[name];
    }

    export function getBoolean(name: string) {
      var value = get(name);
      return value == "true" || value == "True";
    }

    export function getInt(name: string) {
      return parseInt(values[name]);
    }
  }

  /* NOTIFICATION *********************************************/
  //Defines Notification API, not implements it

  export namespace notify {
    export function success(message, title, options) {
      abp.log.warn("abp.notify.success is not implemented!");
    }

    export function info(message, title, options) {
      abp.log.warn("abp.notify.info is not implemented!");
    }

    export function warn(message, title, options) {
      abp.log.warn("abp.notify.warn is not implemented!");
    }

    export function error(message, title, options) {
      abp.log.warn("abp.notify.error is not implemented!");
    }
  }

  /* MESSAGE **************************************************/
  //Defines Message API, not implements it

  export namespace message {
    export function _showMessage(msg: string, title = "") {
      alert(title + " " + msg);
    }

    export function info(msg: string, title: string) {
      abp.log.warn("abp.message.info is not implemented!");
      return _showMessage(msg, title);
    }

    export function success(msg: string, title: string) {
      abp.log.warn("abp.message.success is not implemented!");
      return _showMessage(msg, title);
    }

    export function warn(msg: string, title: string) {
      abp.log.warn("abp.message.warn is not implemented!");
      return _showMessage(msg, title);
    }

    export function error(msg: string, title: string) {
      abp.log.warn("abp.message.error is not implemented!");
      return _showMessage(msg, title);
    }

    export function confirm(msg: string, titleOrCallback?, callback?) {
      abp.log.warn("abp.message.confirm is not properly implemented!");

      if (titleOrCallback && !(typeof titleOrCallback == "string")) {
        callback = titleOrCallback;
      }

      const result = confirm(msg);
      callback && callback(result);
    }
  }

  /* UI *******************************************************/

  export namespace ui {
    /* opts: { //Can be an object with options or a string for query a selector
     *  elm: a query selector (optional - default: document.body)
     *  busy: boolean (optional - default: false)
     *  promise: A promise with always or finally handler (optional - auto unblocks the ui if provided)
     * }
     */
    export function block(opts) {
      if (!opts) {
        opts = {};
      } else if (typeof opts == "string") {
        opts = {
          elm: opts,
        };
      }

      var $elm = document.querySelector(opts.elm) || document.body;

      if (opts.busy) {
        $abpBlockArea.classList.add("abp-block-area-busy");
      } else {
        $abpBlockArea.classList.remove("abp-block-area-busy");
      }

      if (document.querySelector(opts.elm)) {
        $abpBlockArea.style.position = "absolute";
      } else {
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
        } else if (opts.promise["finally"]) {
          //Q style
          opts.promise["finally"](function () {
            unblock({
              $elm: opts.elm,
            });
          });
        }
      }
    }

    /* opts: {
     *
     * }
     */
    export function unblock(opts) {
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

    /* UI BUSY */
    //Defines UI Busy API, not implements it

    export function setBusy(opts) {
      if (!opts) {
        opts = {
          busy: true,
        };
      } else if (typeof opts == "string") {
        opts = {
          elm: opts,
          busy: true,
        };
      }

      block(opts);
    }

    export function clearBusy(opts) {
      unblock(opts);
    }
  }

  /* UI BLOCK */
  //Defines UI Block API and implements basically

  var $abpBlockArea = document.createElement("div");
  $abpBlockArea.classList.add("abp-block-area");

  /* SIMPLE EVENT BUS *****************************************/
  export type EventBusCallback = (...args: any[]) => void;

  // TODO: TEST-REFACTORED
  export class EventBus {
    _callbacks: { [key: string]: EventBusCallback[] } = {};

    on(eventName: string, callback: EventBusCallback) {
      if (!this._callbacks[eventName]) {
        this._callbacks[eventName] = [];
      }

      this._callbacks[eventName].push(callback);
    }

    // TODO: TEST-REFACTORED
    off(eventName: string, callback: EventBusCallback) {
      const callbacks = this._callbacks[eventName];
      if (!callbacks) {
        return;
      }

      this._callbacks[eventName] = callbacks.filter((c) => c === callback);
    }

    // TODO: TEST-REFACTORED
    trigger(eventName: string, ...args: any[]) {
      const callbacks = this._callbacks[eventName];
      if (!callbacks || !callbacks.length) {
        return;
      }

      callbacks.forEach((callback) => callback.apply(this, args));
    }
  }

  export const event = new EventBus();

  /* UTILS ***************************************************/

  export namespace utils {
    /* Creates a name namespace.
     *  Example:
     *  var taskService = abp.utils.createNamespace(abp, 'services.task');
     *  taskService will be equal to abp.services.task
     *  first argument (root) must be defined first
     ************************************************************/
    export function createNamespace(root: any, ns: string) {
      var parts = ns.split(".");
      for (var i = 0; i < parts.length; i++) {
        if (typeof root[parts[i]] == "undefined") {
          root[parts[i]] = {};
        }

        root = root[parts[i]];
      }

      return root;
    }

    /* Find and replaces a string (search) to another string (replacement) in
     *  given string (str).
     *  Example:
     *  abp.utils.replaceAll('This is a test string', 'is', 'X') = 'ThX X a test string'
     ************************************************************/
    export function replaceAll(
      str: string,
      search: string,
      replacement: string
    ) {
      var fix = search.replace(/[.*+?^${}()|[\]\\]/g, "\\$&");
      return str.replace(new RegExp(fix, "g"), replacement);
    }

    // TODO: TEST-REFACTORED
    /* Formats a string just like string.format in C#.
     *  Example:
     *  abp.utils.formatString('Hello {0}','Tuana') = 'Hello Tuana'
     ************************************************************/
    export function formatString(str: string, ...args: string[]) {
      if (!args.length) {
        return str;
      }

      for (var i = 0; i < args.length; i++) {
        var placeHolder = "{" + i + "}";
        str = replaceAll(str, placeHolder, args[i]);
      }

      return str;
    }

    export function toPascalCase(str: string) {
      if (!str || !str.length) {
        return str;
      }

      if (str.length === 1) {
        return str.charAt(0).toUpperCase();
      }

      return str.charAt(0).toUpperCase() + str.substr(1);
    }

    export function toCamelCase(str: string) {
      if (!str || !str.length) {
        return str;
      }

      if (str.length === 1) {
        return str.charAt(0).toLowerCase();
      }

      return str.charAt(0).toLowerCase() + str.substr(1);
    }

    export function truncateString(str: string, maxLength: number) {
      if (!str || !str.length || str.length <= maxLength) {
        return str;
      }

      return str.substr(0, maxLength);
    }

    export function truncateStringWithPostfix(
      str: string,
      maxLength: number,
      postfix: string
    ) {
      postfix = postfix || "...";

      if (!str || !str.length || str.length <= maxLength) {
        return str;
      }

      if (maxLength <= postfix.length) {
        return postfix.substr(0, maxLength);
      }

      return str.substr(0, maxLength - postfix.length) + postfix;
    }

    export function isFunction(obj: any) {
      return !!(obj && obj.constructor && obj.call && obj.apply);
    }

    /**
     * parameterInfos should be an array of { name, value } objects
     * where name is query string parameter name and value is it's value.
     * includeQuestionMark is true by default.
     */
    export function buildQueryString(parameterInfos, includeQuestionMark) {
      if (includeQuestionMark === undefined) {
        includeQuestionMark = true;
      }

      var qs = "";

      function addSeperator() {
        if (!qs.length) {
          if (includeQuestionMark) {
            qs = qs + "?";
          }
        } else {
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

        if (
          parameterInfo.value.toJSON &&
          typeof parameterInfo.value.toJSON === "function"
        ) {
          qs =
            qs +
            parameterInfo.name +
            "=" +
            encodeURIComponent(parameterInfo.value.toJSON());
        } else if (
          Array.isArray(parameterInfo.value) &&
          parameterInfo.value.length
        ) {
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
        } else {
          qs =
            qs +
            parameterInfo.name +
            "=" +
            encodeURIComponent(parameterInfo.value);
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
    export function setCookieValue(
      key: string,
      value: string,
      expireDate: Date,
      path: string
    ) {
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

    /**
     * Gets a cookie with given key.
     * This is a simple implementation created to be used by ABP.
     * Please use a complete cookie library if you need.
     * @param {string} key
     * @returns {string} Cookie value or null
     */
    export function getCookieValue(key: string) {
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

    /**
     * Deletes cookie for given key.
     * This is a simple implementation created to be used by ABP.
     * Please use a complete cookie library if you need.
     * @param {string} key
     * @param {string} path (optional)
     */
    export function deleteCookie(key: string, path?: string) {
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
  }

  /* SECURITY ***************************************/
  export namespace security {
    export const antiForgery = abp.security.antiForgery || {};

    antiForgery.tokenCookieName = "XSRF-TOKEN";
    antiForgery.tokenHeaderName = "RequestVerificationToken";

    antiForgery.getToken = function () {
      return abp.utils.getCookieValue(abp.security.antiForgery.tokenCookieName);
    };
  }

  /* CLOCK *****************************************/
  export namespace clock {
    export let kind = "Unspecified";

    export function supportsMultipleTimezone() {
      return kind === "Utc";
    }

    function toLocal(date: Date) {
      return new Date(
        date.getFullYear(),
        date.getMonth(),
        date.getDate(),
        date.getHours(),
        date.getMinutes(),
        date.getSeconds(),
        date.getMilliseconds()
      );
    }

    function toUtc(date: Date) {
      return Date.UTC(
        date.getUTCFullYear(),
        date.getUTCMonth(),
        date.getUTCDate(),
        date.getUTCHours(),
        date.getUTCMinutes(),
        date.getUTCSeconds(),
        date.getUTCMilliseconds()
      );
    }

    export function now() {
      if (kind === "Utc") {
        return toUtc(new Date());
      }
      return new Date();
    }

    export function normalize(date: Date) {
      if (kind === "Unspecified") {
        return date;
      }

      if (kind === "Local") {
        return toLocal(date);
      }

      if (kind === "Utc") {
        return toUtc(date);
      }
    }
  }

  /* FEATURES *************************************************/

  export namespace features {
    export const values = abp.features.values || {};

    export function isEnabled(name: string) {
      var value = get(name);
      return value == "true" || value == "True";
    }

    export function get(name: string) {
      return values[name];
    }
  }
}
