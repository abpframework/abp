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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9pbml0aWFsLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLGtCQUFrQixFQUFFLE1BQU0saUJBQWlCLENBQUM7QUFFckQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSwyQkFBMkIsQ0FBQztBQUNoRSxPQUFPLGdCQUFnQixNQUFNLGdDQUFnQyxDQUFDOzs7OztBQUU5RCxNQUFNLFVBQVUsY0FBYyxDQUFDLFFBQWtCOztVQUN6QyxFQUFFOzs7SUFBRyxHQUFHLEVBQUU7O2NBQ1IsS0FBSyxHQUFVLFFBQVEsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDO1FBRXhDLE9BQU8sS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLG1CQUFtQixFQUFFLENBQUMsQ0FBQyxTQUFTLEVBQUUsQ0FBQztJQUMvRCxDQUFDLENBQUE7SUFFRCxPQUFPLEVBQUUsQ0FBQztBQUNaLENBQUM7Ozs7O0FBRUQsTUFBTSxVQUFVLGlCQUFpQixDQUFDLFFBQWtCOztVQUM1QyxFQUFFOzs7SUFBRyxHQUFHLEVBQUU7O2NBQ1IsS0FBSyxHQUFVLFFBQVEsQ0FBQyxHQUFHLENBQUMsS0FBSyxDQUFDOztjQUVsQyxJQUFJLEdBQUcsS0FBSyxDQUFDLGNBQWM7Ozs7UUFBQyxLQUFLLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQyxZQUFZLENBQUMsUUFBUSxFQUFDLElBQUksSUFBSTtRQUUvRSxPQUFPLElBQUksT0FBTzs7Ozs7UUFBQyxDQUFDLE9BQU8sRUFBRSxNQUFNLEVBQUUsRUFBRTtZQUNyQyxjQUFjLENBQUMsSUFBSSxDQUFDLENBQUMsSUFBSTs7O1lBQUMsR0FBRyxFQUFFLENBQUMsT0FBTyxDQUFDLFVBQVUsQ0FBQyxHQUFFLE1BQU0sQ0FBQyxDQUFDO1FBQy9ELENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQyxDQUFBO0lBRUQsT0FBTyxFQUFFLENBQUM7QUFDWixDQUFDOzs7OztBQUVELE1BQU0sVUFBVSxjQUFjLENBQUMsTUFBYztJQUMzQyxPQUFPLE1BQU07SUFDWCw0Y0FBNGM7SUFDNWMsMkJBQTJCLGdCQUFnQixDQUFDLE1BQU0sQ0FBQyxJQUFJLE1BQU0sS0FBSyxDQUNuRSxDQUFDLElBQUk7Ozs7SUFBQyxNQUFNLENBQUMsRUFBRTtRQUNkLGtCQUFrQixDQUFDLE1BQU0sQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUNyQyxDQUFDLEVBQUMsQ0FBQztBQUNMLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyByZWdpc3RlckxvY2FsZURhdGEgfSBmcm9tICdAYW5ndWxhci9jb21tb24nO1xuaW1wb3J0IHsgSW5qZWN0b3IgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgR2V0QXBwQ29uZmlndXJhdGlvbiB9IGZyb20gJy4uL2FjdGlvbnMvY29uZmlnLmFjdGlvbnMnO1xuaW1wb3J0IGRpZmZlcmVudExvY2FsZXMgZnJvbSAnLi4vY29uc3RhbnRzL2RpZmZlcmVudC1sb2NhbGVzJztcblxuZXhwb3J0IGZ1bmN0aW9uIGdldEluaXRpYWxEYXRhKGluamVjdG9yOiBJbmplY3Rvcikge1xuICBjb25zdCBmbiA9ICgpID0+IHtcbiAgICBjb25zdCBzdG9yZTogU3RvcmUgPSBpbmplY3Rvci5nZXQoU3RvcmUpO1xuXG4gICAgcmV0dXJuIHN0b3JlLmRpc3BhdGNoKG5ldyBHZXRBcHBDb25maWd1cmF0aW9uKCkpLnRvUHJvbWlzZSgpO1xuICB9O1xuXG4gIHJldHVybiBmbjtcbn1cblxuZXhwb3J0IGZ1bmN0aW9uIGxvY2FsZUluaXRpYWxpemVyKGluamVjdG9yOiBJbmplY3Rvcikge1xuICBjb25zdCBmbiA9ICgpID0+IHtcbiAgICBjb25zdCBzdG9yZTogU3RvcmUgPSBpbmplY3Rvci5nZXQoU3RvcmUpO1xuXG4gICAgY29uc3QgbGFuZyA9IHN0b3JlLnNlbGVjdFNuYXBzaG90KHN0YXRlID0+IHN0YXRlLlNlc3Npb25TdGF0ZS5sYW5ndWFnZSkgfHwgJ2VuJztcblxuICAgIHJldHVybiBuZXcgUHJvbWlzZSgocmVzb2x2ZSwgcmVqZWN0KSA9PiB7XG4gICAgICByZWdpc3RlckxvY2FsZShsYW5nKS50aGVuKCgpID0+IHJlc29sdmUoJ3Jlc29sdmVkJyksIHJlamVjdCk7XG4gICAgfSk7XG4gIH07XG5cbiAgcmV0dXJuIGZuO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gcmVnaXN0ZXJMb2NhbGUobG9jYWxlOiBzdHJpbmcpIHtcbiAgcmV0dXJuIGltcG9ydChcbiAgICAvKiB3ZWJwYWNrSW5jbHVkZTogLyhhZnxhbXxhci1TQXxhc3xhei1MYXRufGJlfGJnfGJuLUJEfGJuLUlOfGJzfGNhfGNhLUVTLVZBTEVOQ0lBfGNzfGN5fGRhfGRlfGRlfGVsfGVuLUdCfGVufGVzfGVufGVzLVVTfGVzLU1YfGV0fGV1fGZhfGZpfGVufGZyfGZyfGZyLUNBfGdhfGdkfGdsfGd1fGhhfGhlfGhpfGhyfGh1fGh5fGlkfGlnfGlzfGl0fGl0fGphfGthfGtrfGttfGtufGtvfGtva3xlbnxlbnxsYnxsdHxsdnxlbnxta3xtbHxtbnxtcnxtc3xtdHxuYnxuZXxubHxubC1CRXxubnxlbnxvcnxwYXxwYS1BcmFifHBsfGVufHB0fHB0LVBUfGVufGVufHJvfHJ1fHJ3fHBhLUFyYWJ8c2l8c2t8c2x8c3F8c3ItQ3lybC1CQXxzci1DeXJsfHNyLUxhdG58c3Z8c3d8dGF8dGV8dGd8dGh8dGl8dGt8dG58dHJ8dHR8dWd8dWt8dXJ8dXotTGF0bnx2aXx3b3x4aHx5b3x6aC1IYW5zfHpoLUhhbnR8enUpXFwuanMkLyAqL1xuICAgIGBAYW5ndWxhci9jb21tb24vbG9jYWxlcy8ke2RpZmZlcmVudExvY2FsZXNbbG9jYWxlXSB8fCBsb2NhbGV9LmpzYFxuICApLnRoZW4obW9kdWxlID0+IHtcbiAgICByZWdpc3RlckxvY2FsZURhdGEobW9kdWxlLmRlZmF1bHQpO1xuICB9KTtcbn1cbiJdfQ==