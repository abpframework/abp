declare namespace abp {
    const appPath: any;
    const pageLoadTime: Date;
    function toAbsAppPath(path: any): any;
    namespace log {
        enum levels {
            DEBUG = 1,
            INFO = 2,
            WARN = 3,
            ERROR = 4,
            FATAL = 5
        }
        const level = levels.DEBUG;
        function log(logObject: any, logLevel: levels): void;
        function debug(logObject: any): void;
        function info(logObject: any): void;
        function warn(logObject: any): void;
        function error(logObject: any): void;
        function fatal(logObject: any): void;
    }
    namespace localization {
        const values: {};
        const defaultResourceName: any;
        const currentCulture: {
            cultureName: any;
            name: any;
        };
        type LanguageModel = {
            name: string;
            value: string;
        };
        type LanguageMapModel = {
            [key: string]: LanguageModel[];
        };
        let languageFilesMap: LanguageMapModel;
        let languagesMap: LanguageMapModel;
        function localize(key: string, sourceName: string, ...args: any[]): string;
        function isLocalized(key: string, sourceName: string): boolean;
        function getResource(name: string): (key: string, ...args: any[]) => string;
        function getLanguagesMap(packageName: string, language: string): string;
        function getLanguageFilesMap(packageName: string, language: string): string;
    }
    namespace auth {
        const policies: any;
        const grantedPolicies: any;
        const tokenCookieName = "Abp.AuthToken";
        function isGranted(policyName: string): boolean;
        function isAnyGranted(...args: string[]): boolean;
        function areAllGranted(...args: string[]): boolean;
        function setToken(authToken?: string, expireDate?: Date): void;
        function getToken(): string;
        function clearToken(): void;
    }
    namespace setting {
        const values: any;
        function get(name: string): any;
        function getBoolean(name: string): boolean;
        function getInt(name: string): number;
    }
    namespace notify {
        function success(message: any, title: any, options: any): void;
        function info(message: any, title: any, options: any): void;
        function warn(message: any, title: any, options: any): void;
        function error(message: any, title: any, options: any): void;
    }
    namespace message {
        function _showMessage(msg: string, title?: string): void;
        function info(msg: string, title: string): void;
        function success(msg: string, title: string): void;
        function warn(msg: string, title: string): void;
        function error(msg: string, title: string): void;
        function confirm(msg: string, titleOrCallback?: any, callback?: any): void;
    }
    namespace ui {
        function block(opts: any): void;
        function unblock(opts: any): void;
        function setBusy(opts: any): void;
        function clearBusy(opts: any): void;
    }
    type EventBusCallback = (...args: any[]) => void;
    class EventBus {
        _callbacks: {
            [key: string]: EventBusCallback[];
        };
        on(eventName: string, callback: EventBusCallback): void;
        off(eventName: string, callback: EventBusCallback): void;
        trigger(eventName: string, ...args: any[]): void;
    }
    const event: EventBus;
    namespace utils {
        function createNamespace(root: any, ns: string): any;
        function replaceAll(str: string, search: string, replacement: string): string;
        function formatString(str: string, ...args: string[]): string;
        function toPascalCase(str: string): string;
        function toCamelCase(str: string): string;
        function truncateString(str: string, maxLength: number): string;
        function truncateStringWithPostfix(str: string, maxLength: number, postfix: string): string;
        function isFunction(obj: any): boolean;
        /**
         * parameterInfos should be an array of { name, value } objects
         * where name is query string parameter name and value is it's value.
         * includeQuestionMark is true by default.
         */
        function buildQueryString(parameterInfos: any, includeQuestionMark: any): string;
        /**
         * Sets a cookie value for given key.
         * This is a simple implementation created to be used by ABP.
         * Please use a complete cookie library if you need.
         * @param {string} key
         * @param {string} value
         * @param {Date} expireDate (optional). If not specified the cookie will expire at the end of session.
         * @param {string} path (optional)
         */
        function setCookieValue(key: string, value: string, expireDate: Date, path: string): void;
        /**
         * Gets a cookie with given key.
         * This is a simple implementation created to be used by ABP.
         * Please use a complete cookie library if you need.
         * @param {string} key
         * @returns {string} Cookie value or null
         */
        function getCookieValue(key: string): string;
        /**
         * Deletes cookie for given key.
         * This is a simple implementation created to be used by ABP.
         * Please use a complete cookie library if you need.
         * @param {string} key
         * @param {string} path (optional)
         */
        function deleteCookie(key: string, path?: string): void;
    }
    namespace security {
        const antiForgery: any;
    }
    namespace clock {
        let kind: string;
        function supportsMultipleTimezone(): boolean;
        function now(): number | Date;
        function normalize(date: Date): number | Date;
    }
    namespace features {
        const values: any;
        function isEnabled(name: string): boolean;
        function get(name: string): any;
    }
}
