/**
 * @fileoverview added by tsickle
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
            () => resolve()), reject);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9pbml0aWFsLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUVyRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ2hFLE9BQU8sZ0JBQWdCLE1BQU0sZ0NBQWdDLENBQUM7Ozs7O0FBRTlELE1BQU0sVUFBVSxjQUFjLENBQUMsUUFBa0I7O1VBQ3pDLEVBQUU7OztJQUFHLEdBQUcsRUFBRTs7Y0FDUixLQUFLLEdBQVUsUUFBUSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUM7UUFFeEMsT0FBTyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksbUJBQW1CLEVBQUUsQ0FBQyxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQy9ELENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsaUJBQWlCLENBQUMsUUFBa0I7O1VBQzVDLEVBQUU7OztJQUFHLEdBQUcsRUFBRTs7Y0FDUixLQUFLLEdBQVUsUUFBUSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUM7O2NBRWxDLElBQUksR0FBRyxLQUFLLENBQUMsY0FBYzs7OztRQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDLFlBQVksQ0FBQyxRQUFRLEVBQUMsSUFBSSxJQUFJO1FBRS9FLE9BQU8sSUFBSSxPQUFPOzs7OztRQUFDLENBQUMsT0FBTyxFQUFFLE1BQU0sRUFBRSxFQUFFO1lBQ3JDLGNBQWMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxJQUFJOzs7WUFBQyxHQUFHLEVBQUUsQ0FBQyxPQUFPLEVBQUUsR0FBRSxNQUFNLENBQUMsQ0FBQztRQUNyRCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsY0FBYyxDQUFDLE1BQWM7SUFDM0MsT0FBTyxNQUFNO0lBQ1gsNGNBQTRjO0lBQzVjLDJCQUEyQixnQkFBZ0IsQ0FBQyxNQUFNLENBQUMsSUFBSSxNQUFNLEtBQUssQ0FDbkUsQ0FBQyxJQUFJOzs7O0lBQUMsTUFBTSxDQUFDLEVBQUU7UUFDZCxrQkFBa0IsQ0FBQyxNQUFNLENBQUMsT0FBTyxDQUFDLENBQUM7SUFDckMsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgcmVnaXN0ZXJMb2NhbGVEYXRhIH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uJztcclxuaW1wb3J0IHsgSW5qZWN0b3IgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IEdldEFwcENvbmZpZ3VyYXRpb24gfSBmcm9tICcuLi9hY3Rpb25zL2NvbmZpZy5hY3Rpb25zJztcclxuaW1wb3J0IGRpZmZlcmVudExvY2FsZXMgZnJvbSAnLi4vY29uc3RhbnRzL2RpZmZlcmVudC1sb2NhbGVzJztcclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBnZXRJbml0aWFsRGF0YShpbmplY3RvcjogSW5qZWN0b3IpIHtcclxuICBjb25zdCBmbiA9ICgpID0+IHtcclxuICAgIGNvbnN0IHN0b3JlOiBTdG9yZSA9IGluamVjdG9yLmdldChTdG9yZSk7XHJcblxyXG4gICAgcmV0dXJuIHN0b3JlLmRpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpLnRvUHJvbWlzZSgpO1xyXG4gIH07XHJcblxyXG4gIHJldHVybiBmbjtcclxufVxyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIGxvY2FsZUluaXRpYWxpemVyKGluamVjdG9yOiBJbmplY3Rvcikge1xyXG4gIGNvbnN0IGZuID0gKCkgPT4ge1xyXG4gICAgY29uc3Qgc3RvcmU6IFN0b3JlID0gaW5qZWN0b3IuZ2V0KFN0b3JlKTtcclxuXHJcbiAgICBjb25zdCBsYW5nID0gc3RvcmUuc2VsZWN0U25hcHNob3Qoc3RhdGUgPT4gc3RhdGUuU2Vzc2lvblN0YXRlLmxhbmd1YWdlKSB8fCAnZW4nO1xyXG5cclxuICAgIHJldHVybiBuZXcgUHJvbWlzZSgocmVzb2x2ZSwgcmVqZWN0KSA9PiB7XHJcbiAgICAgIHJlZ2lzdGVyTG9jYWxlKGxhbmcpLnRoZW4oKCkgPT4gcmVzb2x2ZSgpLCByZWplY3QpO1xyXG4gICAgfSk7XHJcbiAgfTtcclxuXHJcbiAgcmV0dXJuIGZuO1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gcmVnaXN0ZXJMb2NhbGUobG9jYWxlOiBzdHJpbmcpIHtcclxuICByZXR1cm4gaW1wb3J0KFxyXG4gICAgLyogd2VicGFja0luY2x1ZGU6IC8oYWZ8YW18YXItU0F8YXN8YXotTGF0bnxiZXxiZ3xibi1CRHxibi1JTnxic3xjYXxjYS1FUy1WQUxFTkNJQXxjc3xjeXxkYXxkZXxkZXxlbHxlbi1HQnxlbnxlc3xlbnxlcy1VU3xlcy1NWHxldHxldXxmYXxmaXxlbnxmcnxmcnxmci1DQXxnYXxnZHxnbHxndXxoYXxoZXxoaXxocnxodXxoeXxpZHxpZ3xpc3xpdHxpdHxqYXxrYXxra3xrbXxrbnxrb3xrb2t8ZW58ZW58bGJ8bHR8bHZ8ZW58bWt8bWx8bW58bXJ8bXN8bXR8bmJ8bmV8bmx8bmwtQkV8bm58ZW58b3J8cGF8cGEtQXJhYnxwbHxlbnxwdHxwdC1QVHxlbnxlbnxyb3xydXxyd3xwYS1BcmFifHNpfHNrfHNsfHNxfHNyLUN5cmwtQkF8c3ItQ3lybHxzci1MYXRufHN2fHN3fHRhfHRlfHRnfHRofHRpfHRrfHRufHRyfHR0fHVnfHVrfHVyfHV6LUxhdG58dml8d298eGh8eW98emgtSGFuc3x6aC1IYW50fHp1KVxcLmpzJC8gKi9cclxuICAgIGBAYW5ndWxhci9jb21tb24vbG9jYWxlcy8ke2RpZmZlcmVudExvY2FsZXNbbG9jYWxlXSB8fCBsb2NhbGV9LmpzYFxyXG4gICkudGhlbihtb2R1bGUgPT4ge1xyXG4gICAgcmVnaXN0ZXJMb2NhbGVEYXRhKG1vZHVsZS5kZWZhdWx0KTtcclxuICB9KTtcclxufVxyXG4iXX0=