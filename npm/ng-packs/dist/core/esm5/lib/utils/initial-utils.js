/**
 * @fileoverview added by tsickle
 * Generated from: lib/utils/initial-utils.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { registerLocaleData } from '@angular/common';
import { Store } from '@ngxs/store';
import { GetAppConfiguration } from '../actions/config.actions';
import differentLocales from '../constants/different-locales';
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
        var lang = store.selectSnapshot((/**
         * @param {?} state
         * @return {?}
         */
        function (state) { return state.SessionState.language; })) || 'en';
        return new Promise((/**
         * @param {?} resolve
         * @param {?} reject
         * @return {?}
         */
        function (resolve, reject) {
            registerLocale(lang).then((/**
             * @return {?}
             */
            function () { return resolve('resolved'); }), reject);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9pbml0aWFsLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFFckQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUNoRSxPQUFPLGdCQUFnQixNQUFNLGdDQUFnQyxDQUFDOzs7OztBQUU5RCxNQUFNLFVBQVUsY0FBYyxDQUFDLFFBQWtCOztRQUN6QyxFQUFFOzs7SUFBRzs7WUFDSCxLQUFLLEdBQVUsUUFBUSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUM7UUFFeEMsT0FBTyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksbUJBQW1CLEVBQUUsQ0FBQyxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQy9ELENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsaUJBQWlCLENBQUMsUUFBa0I7O1FBQzVDLEVBQUU7OztJQUFHOztZQUNILEtBQUssR0FBVSxRQUFRLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQzs7WUFFbEMsSUFBSSxHQUFHLEtBQUssQ0FBQyxjQUFjOzs7O1FBQUMsVUFBQSxLQUFLLElBQUksT0FBQSxLQUFLLENBQUMsWUFBWSxDQUFDLFFBQVEsRUFBM0IsQ0FBMkIsRUFBQyxJQUFJLElBQUk7UUFFL0UsT0FBTyxJQUFJLE9BQU87Ozs7O1FBQUMsVUFBQyxPQUFPLEVBQUUsTUFBTTtZQUNqQyxjQUFjLENBQUMsSUFBSSxDQUFDLENBQUMsSUFBSTs7O1lBQUMsY0FBTSxPQUFBLE9BQU8sQ0FBQyxVQUFVLENBQUMsRUFBbkIsQ0FBbUIsR0FBRSxNQUFNLENBQUMsQ0FBQztRQUMvRCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsY0FBYyxDQUFDLE1BQWM7SUFDM0MsT0FBTyxNQUFNO0lBQ1gsNGNBQTRjO0lBQzVjLDhCQUEyQixnQkFBZ0IsQ0FBQyxNQUFNLENBQUMsSUFBSSxNQUFNLFNBQUssQ0FDbkUsQ0FBQyxJQUFJOzs7O0lBQUMsVUFBQSxNQUFNO1FBQ1gsa0JBQWtCLENBQUMsTUFBTSxDQUFDLE9BQU8sQ0FBQyxDQUFDO0lBQ3JDLENBQUMsRUFBQyxDQUFDO0FBQ0wsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IHJlZ2lzdGVyTG9jYWxlRGF0YSB9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbic7XHJcbmltcG9ydCB7IEluamVjdG9yIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5pbXBvcnQgeyBHZXRBcHBDb25maWd1cmF0aW9uIH0gZnJvbSAnLi4vYWN0aW9ucy9jb25maWcuYWN0aW9ucyc7XHJcbmltcG9ydCBkaWZmZXJlbnRMb2NhbGVzIGZyb20gJy4uL2NvbnN0YW50cy9kaWZmZXJlbnQtbG9jYWxlcyc7XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gZ2V0SW5pdGlhbERhdGEoaW5qZWN0b3I6IEluamVjdG9yKSB7XHJcbiAgY29uc3QgZm4gPSAoKSA9PiB7XHJcbiAgICBjb25zdCBzdG9yZTogU3RvcmUgPSBpbmplY3Rvci5nZXQoU3RvcmUpO1xyXG5cclxuICAgIHJldHVybiBzdG9yZS5kaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKS50b1Byb21pc2UoKTtcclxuICB9O1xyXG5cclxuICByZXR1cm4gZm47XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBsb2NhbGVJbml0aWFsaXplcihpbmplY3RvcjogSW5qZWN0b3IpIHtcclxuICBjb25zdCBmbiA9ICgpID0+IHtcclxuICAgIGNvbnN0IHN0b3JlOiBTdG9yZSA9IGluamVjdG9yLmdldChTdG9yZSk7XHJcblxyXG4gICAgY29uc3QgbGFuZyA9IHN0b3JlLnNlbGVjdFNuYXBzaG90KHN0YXRlID0+IHN0YXRlLlNlc3Npb25TdGF0ZS5sYW5ndWFnZSkgfHwgJ2VuJztcclxuXHJcbiAgICByZXR1cm4gbmV3IFByb21pc2UoKHJlc29sdmUsIHJlamVjdCkgPT4ge1xyXG4gICAgICByZWdpc3RlckxvY2FsZShsYW5nKS50aGVuKCgpID0+IHJlc29sdmUoJ3Jlc29sdmVkJyksIHJlamVjdCk7XHJcbiAgICB9KTtcclxuICB9O1xyXG5cclxuICByZXR1cm4gZm47XHJcbn1cclxuXHJcbmV4cG9ydCBmdW5jdGlvbiByZWdpc3RlckxvY2FsZShsb2NhbGU6IHN0cmluZykge1xyXG4gIHJldHVybiBpbXBvcnQoXHJcbiAgICAvKiB3ZWJwYWNrSW5jbHVkZTogLyhhZnxhbXxhci1TQXxhc3xhei1MYXRufGJlfGJnfGJuLUJEfGJuLUlOfGJzfGNhfGNhLUVTLVZBTEVOQ0lBfGNzfGN5fGRhfGRlfGRlfGVsfGVuLUdCfGVufGVzfGVufGVzLVVTfGVzLU1YfGV0fGV1fGZhfGZpfGVufGZyfGZyfGZyLUNBfGdhfGdkfGdsfGd1fGhhfGhlfGhpfGhyfGh1fGh5fGlkfGlnfGlzfGl0fGl0fGphfGthfGtrfGttfGtufGtvfGtva3xlbnxlbnxsYnxsdHxsdnxlbnxta3xtbHxtbnxtcnxtc3xtdHxuYnxuZXxubHxubC1CRXxubnxlbnxvcnxwYXxwYS1BcmFifHBsfGVufHB0fHB0LVBUfGVufGVufHJvfHJ1fHJ3fHBhLUFyYWJ8c2l8c2t8c2x8c3F8c3ItQ3lybC1CQXxzci1DeXJsfHNyLUxhdG58c3Z8c3d8dGF8dGV8dGd8dGh8dGl8dGt8dG58dHJ8dHR8dWd8dWt8dXJ8dXotTGF0bnx2aXx3b3x4aHx5b3x6aC1IYW5zfHpoLUhhbnR8enUpXFwuanMkLyAqL1xyXG4gICAgYEBhbmd1bGFyL2NvbW1vbi9sb2NhbGVzLyR7ZGlmZmVyZW50TG9jYWxlc1tsb2NhbGVdIHx8IGxvY2FsZX0uanNgXHJcbiAgKS50aGVuKG1vZHVsZSA9PiB7XHJcbiAgICByZWdpc3RlckxvY2FsZURhdGEobW9kdWxlLmRlZmF1bHQpO1xyXG4gIH0pO1xyXG59XHJcbiJdfQ==