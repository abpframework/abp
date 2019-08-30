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
    const fn = (/**
     * @return {?}
     */
    function () {
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
    function () {
        /** @type {?} */
        const store = injector.get(Store);
        /** @type {?} */
        const lang = store.selectSnapshot(SessionState.getLanguage) || 'en';
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9pbml0aWFsLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUVyRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ2hFLE9BQU8sZ0JBQWdCLE1BQU0sZ0NBQWdDLENBQUM7QUFDOUQsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLHlCQUF5QixDQUFDOzs7OztBQUV2RCxNQUFNLFVBQVUsY0FBYyxDQUFDLFFBQWtCOztVQUN6QyxFQUFFOzs7SUFBRzs7Y0FDSCxLQUFLLEdBQVUsUUFBUSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUM7UUFFeEMsT0FBTyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksbUJBQW1CLEVBQUUsQ0FBQyxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQy9ELENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsaUJBQWlCLENBQUMsUUFBa0I7O1VBQzVDLEVBQUU7OztJQUFHOztjQUNILEtBQUssR0FBVSxRQUFRLENBQUMsR0FBRyxDQUFDLEtBQUssQ0FBQzs7Y0FFbEMsSUFBSSxHQUFHLEtBQUssQ0FBQyxjQUFjLENBQUMsWUFBWSxDQUFDLFdBQVcsQ0FBQyxJQUFJLElBQUk7UUFFbkUsT0FBTyxJQUFJLE9BQU87Ozs7O1FBQUMsQ0FBQyxPQUFPLEVBQUUsTUFBTSxFQUFFLEVBQUU7WUFDckMsY0FBYyxDQUFDLElBQUksQ0FBQyxDQUFDLElBQUk7OztZQUFDLEdBQUcsRUFBRSxDQUFDLE9BQU8sRUFBRSxHQUFFLE1BQU0sQ0FBQyxDQUFDO1FBQ3JELENBQUMsRUFBQyxDQUFDO0lBQ0wsQ0FBQyxDQUFBO0lBRUQsT0FBTyxFQUFFLENBQUM7QUFDWixDQUFDOzs7OztBQUVELE1BQU0sVUFBVSxjQUFjLENBQUMsTUFBYztJQUMzQyxPQUFPLE1BQU07SUFDWCw0Y0FBNGM7SUFDNWMsMkJBQTJCLGdCQUFnQixDQUFDLE1BQU0sQ0FBQyxJQUFJLE1BQU0sS0FBSyxDQUNuRSxDQUFDLElBQUk7Ozs7SUFBQyxNQUFNLENBQUMsRUFBRTtRQUNkLGtCQUFrQixDQUFDLE1BQU0sQ0FBQyxPQUFPLENBQUMsQ0FBQztJQUNyQyxDQUFDLEVBQUMsQ0FBQztBQUNMLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyByZWdpc3RlckxvY2FsZURhdGEgfSBmcm9tICdAYW5ndWxhci9jb21tb24nO1xuaW1wb3J0IHsgSW5qZWN0b3IgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgR2V0QXBwQ29uZmlndXJhdGlvbiB9IGZyb20gJy4uL2FjdGlvbnMvY29uZmlnLmFjdGlvbnMnO1xuaW1wb3J0IGRpZmZlcmVudExvY2FsZXMgZnJvbSAnLi4vY29uc3RhbnRzL2RpZmZlcmVudC1sb2NhbGVzJztcbmltcG9ydCB7IFNlc3Npb25TdGF0ZSB9IGZyb20gJy4uL3N0YXRlcy9zZXNzaW9uLnN0YXRlJztcblxuZXhwb3J0IGZ1bmN0aW9uIGdldEluaXRpYWxEYXRhKGluamVjdG9yOiBJbmplY3Rvcikge1xuICBjb25zdCBmbiA9IGZ1bmN0aW9uKCkge1xuICAgIGNvbnN0IHN0b3JlOiBTdG9yZSA9IGluamVjdG9yLmdldChTdG9yZSk7XG5cbiAgICByZXR1cm4gc3RvcmUuZGlzcGF0Y2gobmV3IEdldEFwcENvbmZpZ3VyYXRpb24oKSkudG9Qcm9taXNlKCk7XG4gIH07XG5cbiAgcmV0dXJuIGZuO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gbG9jYWxlSW5pdGlhbGl6ZXIoaW5qZWN0b3I6IEluamVjdG9yKSB7XG4gIGNvbnN0IGZuID0gZnVuY3Rpb24oKSB7XG4gICAgY29uc3Qgc3RvcmU6IFN0b3JlID0gaW5qZWN0b3IuZ2V0KFN0b3JlKTtcblxuICAgIGNvbnN0IGxhbmcgPSBzdG9yZS5zZWxlY3RTbmFwc2hvdChTZXNzaW9uU3RhdGUuZ2V0TGFuZ3VhZ2UpIHx8ICdlbic7XG5cbiAgICByZXR1cm4gbmV3IFByb21pc2UoKHJlc29sdmUsIHJlamVjdCkgPT4ge1xuICAgICAgcmVnaXN0ZXJMb2NhbGUobGFuZykudGhlbigoKSA9PiByZXNvbHZlKCksIHJlamVjdCk7XG4gICAgfSk7XG4gIH07XG5cbiAgcmV0dXJuIGZuO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gcmVnaXN0ZXJMb2NhbGUobG9jYWxlOiBzdHJpbmcpIHtcbiAgcmV0dXJuIGltcG9ydChcbiAgICAvKiB3ZWJwYWNrSW5jbHVkZTogLyhhZnxhbXxhci1TQXxhc3xhei1MYXRufGJlfGJnfGJuLUJEfGJuLUlOfGJzfGNhfGNhLUVTLVZBTEVOQ0lBfGNzfGN5fGRhfGRlfGRlfGVsfGVuLUdCfGVufGVzfGVufGVzLVVTfGVzLU1YfGV0fGV1fGZhfGZpfGVufGZyfGZyfGZyLUNBfGdhfGdkfGdsfGd1fGhhfGhlfGhpfGhyfGh1fGh5fGlkfGlnfGlzfGl0fGl0fGphfGthfGtrfGttfGtufGtvfGtva3xlbnxlbnxsYnxsdHxsdnxlbnxta3xtbHxtbnxtcnxtc3xtdHxuYnxuZXxubHxubC1CRXxubnxlbnxvcnxwYXxwYS1BcmFifHBsfGVufHB0fHB0LVBUfGVufGVufHJvfHJ1fHJ3fHBhLUFyYWJ8c2l8c2t8c2x8c3F8c3ItQ3lybC1CQXxzci1DeXJsfHNyLUxhdG58c3Z8c3d8dGF8dGV8dGd8dGh8dGl8dGt8dG58dHJ8dHR8dWd8dWt8dXJ8dXotTGF0bnx2aXx3b3x4aHx5b3x6aC1IYW5zfHpoLUhhbnR8enUpXFwuanMkLyAqL1xuICAgIGBAYW5ndWxhci9jb21tb24vbG9jYWxlcy8ke2RpZmZlcmVudExvY2FsZXNbbG9jYWxlXSB8fCBsb2NhbGV9LmpzYFxuICApLnRoZW4obW9kdWxlID0+IHtcbiAgICByZWdpc3RlckxvY2FsZURhdGEobW9kdWxlLmRlZmF1bHQpO1xuICB9KTtcbn1cbiJdfQ==