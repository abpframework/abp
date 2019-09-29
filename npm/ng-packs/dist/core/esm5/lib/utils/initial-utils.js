/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { registerLocaleData } from '@angular/common';
import { Store } from '@ngxs/store';
import { GetAppConfiguration } from '../actions/config.actions';
import differentLocales from '../constants/different-locales';
import { SessionState } from '../states/session.state';
/**
 * @param {?} injector
 * @return {?}
 */
export function getInitialData(injector) {
    /** @type {?} */
    var fn = (/**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var store = injector.get(Store);
        return store.dispatch(new GetAppConfiguration()).toPromise();
    });
    return fn;
}
/**
 * @param {?} injector
 * @return {?}
 */
export function localeInitializer(injector) {
    /** @type {?} */
    var fn = (/**
     * @return {?}
     */
    function () {
        /** @type {?} */
        var store = injector.get(Store);
        /** @type {?} */
        var lang = store.selectSnapshot(SessionState.getLanguage) || 'en';
        return new Promise((/**
         * @param {?} resolve
         * @param {?} reject
         * @return {?}
         */
        function (resolve, reject) {
            registerLocale(lang).then((/**
             * @return {?}
             */
            function () { return resolve(); }), reject);
        }));
    });
    return fn;
}
/**
 * @param {?} locale
 * @return {?}
 */
export function registerLocale(locale) {
    return import(
    /* webpackInclude: /(af|am|ar-SA|as|az-Latn|be|bg|bn-BD|bn-IN|bs|ca|ca-ES-VALENCIA|cs|cy|da|de|de|el|en-GB|en|es|en|es-US|es-MX|et|eu|fa|fi|en|fr|fr|fr-CA|ga|gd|gl|gu|ha|he|hi|hr|hu|hy|id|ig|is|it|it|ja|ka|kk|km|kn|ko|kok|en|en|lb|lt|lv|en|mk|ml|mn|mr|ms|mt|nb|ne|nl|nl-BE|nn|en|or|pa|pa-Arab|pl|en|pt|pt-PT|en|en|ro|ru|rw|pa-Arab|si|sk|sl|sq|sr-Cyrl-BA|sr-Cyrl|sr-Latn|sv|sw|ta|te|tg|th|ti|tk|tn|tr|tt|ug|uk|ur|uz-Latn|vi|wo|xh|yo|zh-Hans|zh-Hant|zu)\.js$/ */
    "@angular/common/locales/" + (differentLocales[locale] || locale) + ".js").then((/**
     * @param {?} module
     * @return {?}
     */
    function (module) {
        registerLocaleData(module.default);
    }));
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9pbml0aWFsLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUVyRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ2hFLE9BQU8sZ0JBQWdCLE1BQU0sZ0NBQWdDLENBQUM7QUFDOUQsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHlCQUF5QixDQUFDOzs7OztBQUV2RCxNQUFNLFVBQVUsY0FBYyxDQUFDLFFBQWtCOztRQUN6QyxFQUFFOzs7SUFBRzs7WUFDSCxLQUFLLEdBQVUsUUFBUSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUM7UUFFeEMsT0FBTyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksbUJBQW1CLEVBQUUsQ0FBQyxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQy9ELENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsaUJBQWlCLENBQUMsUUFBa0I7O1FBQzVDLEVBQUU7OztJQUFHOztZQUNILEtBQUssR0FBVSxRQUFRLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQzs7WUFFbEMsSUFBSSxHQUFHLEtBQUssQ0FBQyxjQUFjLENBQUMsWUFBWSxDQUFDLFdBQVcsQ0FBQyxJQUFJLElBQUk7UUFFbkUsT0FBTyxJQUFJLE9BQU87Ozs7O1FBQUMsVUFBQyxPQUFPLEVBQUUsTUFBTTtZQUNqQyxjQUFjLENBQUMsSUFBSSxDQUFDLENBQUMsSUFBSTs7O1lBQUMsY0FBTSxPQUFBLE9BQU8sRUFBRSxFQUFULENBQVMsR0FBRSxNQUFNLENBQUMsQ0FBQztRQUNyRCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsY0FBYyxDQUFDLE1BQWM7SUFDM0MsT0FBTyxNQUFNO0lBQ1gsNGNBQTRjO0lBQzVjLDhCQUEyQixnQkFBZ0IsQ0FBQyxNQUFNLENBQUMsSUFBSSxNQUFNLFNBQUssQ0FDbkUsQ0FBQyxJQUFJOzs7O0lBQUMsVUFBQSxNQUFNO1FBQ1gsa0JBQWtCLENBQUMsTUFBTSxDQUFDLE9BQU8sQ0FBQyxDQUFDO0lBQ3JDLENBQUMsRUFBQyxDQUFDO0FBQ0wsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IHJlZ2lzdGVyTG9jYWxlRGF0YSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbic7XG5pbXBvcnQgeyBJbmplY3RvciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uIH0gZnJvbSAnLi4vYWN0aW9ucy9jb25maWcuYWN0aW9ucyc7XG5pbXBvcnQgZGlmZmVyZW50TG9jYWxlcyBmcm9tICcuLi9jb25zdGFudHMvZGlmZmVyZW50LWxvY2FsZXMnO1xuaW1wb3J0IHsgU2Vzc2lvblN0YXRlIH0gZnJvbSAnLi4vc3RhdGVzL3Nlc3Npb24uc3RhdGUnO1xuXG5leHBvcnQgZnVuY3Rpb24gZ2V0SW5pdGlhbERhdGEoaW5qZWN0b3I6IEluamVjdG9yKSB7XG4gIGNvbnN0IGZuID0gZnVuY3Rpb24oKSB7XG4gICAgY29uc3Qgc3RvcmU6IFN0b3JlID0gaW5qZWN0b3IuZ2V0KFN0b3JlKTtcblxuICAgIHJldHVybiBzdG9yZS5kaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKS50b1Byb21pc2UoKTtcbiAgfTtcblxuICByZXR1cm4gZm47XG59XG5cbmV4cG9ydCBmdW5jdGlvbiBsb2NhbGVJbml0aWFsaXplcihpbmplY3RvcjogSW5qZWN0b3IpIHtcbiAgY29uc3QgZm4gPSBmdW5jdGlvbigpIHtcbiAgICBjb25zdCBzdG9yZTogU3RvcmUgPSBpbmplY3Rvci5nZXQoU3RvcmUpO1xuXG4gICAgY29uc3QgbGFuZyA9IHN0b3JlLnNlbGVjdFNuYXBzaG90KFNlc3Npb25TdGF0ZS5nZXRMYW5ndWFnZSkgfHwgJ2VuJztcblxuICAgIHJldHVybiBuZXcgUHJvbWlzZSgocmVzb2x2ZSwgcmVqZWN0KSA9PiB7XG4gICAgICByZWdpc3RlckxvY2FsZShsYW5nKS50aGVuKCgpID0+IHJlc29sdmUoKSwgcmVqZWN0KTtcbiAgICB9KTtcbiAgfTtcblxuICByZXR1cm4gZm47XG59XG5cbmV4cG9ydCBmdW5jdGlvbiByZWdpc3RlckxvY2FsZShsb2NhbGU6IHN0cmluZykge1xuICByZXR1cm4gaW1wb3J0KFxuICAgIC8qIHdlYnBhY2tJbmNsdWRlOiAvKGFmfGFtfGFyLVNBfGFzfGF6LUxhdG58YmV8Ymd8Ym4tQkR8Ym4tSU58YnN8Y2F8Y2EtRVMtVkFMRU5DSUF8Y3N8Y3l8ZGF8ZGV8ZGV8ZWx8ZW4tR0J8ZW58ZXN8ZW58ZXMtVVN8ZXMtTVh8ZXR8ZXV8ZmF8Zml8ZW58ZnJ8ZnJ8ZnItQ0F8Z2F8Z2R8Z2x8Z3V8aGF8aGV8aGl8aHJ8aHV8aHl8aWR8aWd8aXN8aXR8aXR8amF8a2F8a2t8a218a258a298a29rfGVufGVufGxifGx0fGx2fGVufG1rfG1sfG1ufG1yfG1zfG10fG5ifG5lfG5sfG5sLUJFfG5ufGVufG9yfHBhfHBhLUFyYWJ8cGx8ZW58cHR8cHQtUFR8ZW58ZW58cm98cnV8cnd8cGEtQXJhYnxzaXxza3xzbHxzcXxzci1DeXJsLUJBfHNyLUN5cmx8c3ItTGF0bnxzdnxzd3x0YXx0ZXx0Z3x0aHx0aXx0a3x0bnx0cnx0dHx1Z3x1a3x1cnx1ei1MYXRufHZpfHdvfHhofHlvfHpoLUhhbnN8emgtSGFudHx6dSlcXC5qcyQvICovXG4gICAgYEBhbmd1bGFyL2NvbW1vbi9sb2NhbGVzLyR7ZGlmZmVyZW50TG9jYWxlc1tsb2NhbGVdIHx8IGxvY2FsZX0uanNgXG4gICkudGhlbihtb2R1bGUgPT4ge1xuICAgIHJlZ2lzdGVyTG9jYWxlRGF0YShtb2R1bGUuZGVmYXVsdCk7XG4gIH0pO1xufVxuIl19