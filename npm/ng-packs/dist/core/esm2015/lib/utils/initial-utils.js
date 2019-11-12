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
    const fn = (/**
     * @return {?}
     */
    () => {
        /** @type {?} */
        const store = injector.get(Store);
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
    const fn = (/**
     * @return {?}
     */
    () => {
        /** @type {?} */
        const store = injector.get(Store);
        /** @type {?} */
        const lang = store.selectSnapshot((/**
         * @param {?} state
         * @return {?}
         */
        state => state.SessionState.language)) || 'en';
        return new Promise((/**
         * @param {?} resolve
         * @param {?} reject
         * @return {?}
         */
        (resolve, reject) => {
            registerLocale(lang).then((/**
             * @return {?}
             */
            () => resolve('resolved')), reject);
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
    `@angular/common/locales/${differentLocales[locale] || locale}.js`).then((/**
     * @param {?} module
     * @return {?}
     */
    module => {
        registerLocaleData(module.default);
    }));
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9pbml0aWFsLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFFckQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUNoRSxPQUFPLGdCQUFnQixNQUFNLGdDQUFnQyxDQUFDOzs7OztBQUU5RCxNQUFNLFVBQVUsY0FBYyxDQUFDLFFBQWtCOztVQUN6QyxFQUFFOzs7SUFBRyxHQUFHLEVBQUU7O2NBQ1IsS0FBSyxHQUFVLFFBQVEsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDO1FBRXhDLE9BQU8sS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixFQUFFLENBQUMsQ0FBQyxTQUFTLEVBQUUsQ0FBQztJQUMvRCxDQUFDLENBQUE7SUFFRCxPQUFPLEVBQUUsQ0FBQztBQUNaLENBQUM7Ozs7O0FBRUQsTUFBTSxVQUFVLGlCQUFpQixDQUFDLFFBQWtCOztVQUM1QyxFQUFFOzs7SUFBRyxHQUFHLEVBQUU7O2NBQ1IsS0FBSyxHQUFVLFFBQVEsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDOztjQUVsQyxJQUFJLEdBQUcsS0FBSyxDQUFDLGNBQWM7Ozs7UUFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQyxZQUFZLENBQUMsUUFBUSxFQUFDLElBQUksSUFBSTtRQUUvRSxPQUFPLElBQUksT0FBTzs7Ozs7UUFBQyxDQUFDLE9BQU8sRUFBRSxNQUFNLEVBQUUsRUFBRTtZQUNyQyxjQUFjLENBQUMsSUFBSSxDQUFDLENBQUMsSUFBSTs7O1lBQUMsR0FBRyxFQUFFLENBQUMsT0FBTyxDQUFDLFVBQVUsQ0FBQyxHQUFFLE1BQU0sQ0FBQyxDQUFDO1FBQy9ELENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQyxDQUFBO0lBRUQsT0FBTyxFQUFFLENBQUM7QUFDWixDQUFDOzs7OztBQUVELE1BQU0sVUFBVSxjQUFjLENBQUMsTUFBYztJQUMzQyxPQUFPLE1BQU07SUFDWCw0Y0FBNGM7SUFDNWMsMkJBQTJCLGdCQUFnQixDQUFDLE1BQU0sQ0FBQyxJQUFJLE1BQU0sS0FBSyxDQUNuRSxDQUFDLElBQUk7Ozs7SUFBQyxNQUFNLENBQUMsRUFBRTtRQUNkLGtCQUFrQixDQUFDLE1BQU0sQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUNyQyxDQUFDLEVBQUMsQ0FBQztBQUNMLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyByZWdpc3RlckxvY2FsZURhdGEgfSBmcm9tICdAYW5ndWxhci9jb21tb24nO1xyXG5pbXBvcnQgeyBJbmplY3RvciB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcclxuaW1wb3J0IHsgR2V0QXBwQ29uZmlndXJhdGlvbiB9IGZyb20gJy4uL2FjdGlvbnMvY29uZmlnLmFjdGlvbnMnO1xyXG5pbXBvcnQgZGlmZmVyZW50TG9jYWxlcyBmcm9tICcuLi9jb25zdGFudHMvZGlmZmVyZW50LWxvY2FsZXMnO1xyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIGdldEluaXRpYWxEYXRhKGluamVjdG9yOiBJbmplY3Rvcikge1xyXG4gIGNvbnN0IGZuID0gKCkgPT4ge1xyXG4gICAgY29uc3Qgc3RvcmU6IFN0b3JlID0gaW5qZWN0b3IuZ2V0KFN0b3JlKTtcclxuXHJcbiAgICByZXR1cm4gc3RvcmUuZGlzcGF0Y2gobmV3IEdldEFwcENvbmZpZ3VyYXRpb24oKSkudG9Qcm9taXNlKCk7XHJcbiAgfTtcclxuXHJcbiAgcmV0dXJuIGZuO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gbG9jYWxlSW5pdGlhbGl6ZXIoaW5qZWN0b3I6IEluamVjdG9yKSB7XHJcbiAgY29uc3QgZm4gPSAoKSA9PiB7XHJcbiAgICBjb25zdCBzdG9yZTogU3RvcmUgPSBpbmplY3Rvci5nZXQoU3RvcmUpO1xyXG5cclxuICAgIGNvbnN0IGxhbmcgPSBzdG9yZS5zZWxlY3RTbmFwc2hvdChzdGF0ZSA9PiBzdGF0ZS5TZXNzaW9uU3RhdGUubGFuZ3VhZ2UpIHx8ICdlbic7XHJcblxyXG4gICAgcmV0dXJuIG5ldyBQcm9taXNlKChyZXNvbHZlLCByZWplY3QpID0+IHtcclxuICAgICAgcmVnaXN0ZXJMb2NhbGUobGFuZykudGhlbigoKSA9PiByZXNvbHZlKCdyZXNvbHZlZCcpLCByZWplY3QpO1xyXG4gICAgfSk7XHJcbiAgfTtcclxuXHJcbiAgcmV0dXJuIGZuO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gcmVnaXN0ZXJMb2NhbGUobG9jYWxlOiBzdHJpbmcpIHtcclxuICByZXR1cm4gaW1wb3J0KFxyXG4gICAgLyogd2VicGFja0luY2x1ZGU6IC8oYWZ8YW18YXItU0F8YXN8YXotTGF0bnxiZXxiZ3xibi1CRHxibi1JTnxic3xjYXxjYS1FUy1WQUxFTkNJQXxjc3xjeXxkYXxkZXxkZXxlbHxlbi1HQnxlbnxlc3xlbnxlcy1VU3xlcy1NWHxldHxldXxmYXxmaXxlbnxmcnxmcnxmci1DQXxnYXxnZHxnbHxndXxoYXxoZXxoaXxocnxodXxoeXxpZHxpZ3xpc3xpdHxpdHxqYXxrYXxra3xrbXxrbnxrb3xrb2t8ZW58ZW58bGJ8bHR8bHZ8ZW58bWt8bWx8bW58bXJ8bXN8bXR8bmJ8bmV8bmx8bmwtQkV8bm58ZW58b3J8cGF8cGEtQXJhYnxwbHxlbnxwdHxwdC1QVHxlbnxlbnxyb3xydXxyd3xwYS1BcmFifHNpfHNrfHNsfHNxfHNyLUN5cmwtQkF8c3ItQ3lybHxzci1MYXRufHN2fHN3fHRhfHRlfHRnfHRofHRpfHRrfHRufHRyfHR0fHVnfHVrfHVyfHV6LUxhdG58dml8d298eGh8eW98emgtSGFuc3x6aC1IYW50fHp1KVxcLmpzJC8gKi9cclxuICAgIGBAYW5ndWxhci9jb21tb24vbG9jYWxlcy8ke2RpZmZlcmVudExvY2FsZXNbbG9jYWxlXSB8fCBsb2NhbGV9LmpzYFxyXG4gICkudGhlbihtb2R1bGUgPT4ge1xyXG4gICAgcmVnaXN0ZXJMb2NhbGVEYXRhKG1vZHVsZS5kZWZhdWx0KTtcclxuICB9KTtcclxufVxyXG4iXX0=