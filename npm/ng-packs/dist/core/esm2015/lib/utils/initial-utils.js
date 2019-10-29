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
  const fn
  /**
   * @return {?}
   */ = (() => {
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
  const fn
  /**
   * @return {?}
   */ = (() => {
    /** @type {?} */
    const store = injector.get(Store);
    /** @type {?} */
    const lang =
      store.selectSnapshot(
        /**
         * @param {?} state
         * @return {?}
         */
        (state => state.SessionState.language),
      ) || 'en';
    return new Promise
    /**
     * @param {?} resolve
     * @param {?} reject
     * @return {?}
     */((resolve, reject) => {
      registerLocale(lang).then(
        /**
         * @return {?}
         */
        () => resolve(),
        reject,
      );
    });
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
    `@angular/common/locales/${differentLocales[locale] || locale}.js`
  ).then(
    /**
     * @param {?} module
     * @return {?}
     */
    module => {
      registerLocaleData(module.default);
    },
  );
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaW5pdGlhbC11dGlscy5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi91dGlscy9pbml0aWFsLXV0aWxzLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsa0JBQWtCLEVBQUUsTUFBTSxpQkFBaUIsQ0FBQztBQUVyRCxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLDJCQUEyQixDQUFDO0FBQ2hFLE9BQU8sZ0JBQWdCLE1BQU0sZ0NBQWdDLENBQUM7Ozs7O0FBRTlELE1BQU0sVUFBVSxjQUFjLENBQUMsUUFBa0I7O1VBQ3pDLEVBQUU7OztJQUFHLEdBQUcsRUFBRTs7Y0FDUixLQUFLLEdBQVUsUUFBUSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUM7UUFFeEMsT0FBTyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksbUJBQW1CLEVBQUUsQ0FBQyxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQy9ELENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsaUJBQWlCLENBQUMsUUFBa0I7O1VBQzVDLEVBQUU7OztJQUFHLEdBQUcsRUFBRTs7Y0FDUixLQUFLLEdBQVUsUUFBUSxDQUFDLEdBQUcsQ0FBQyxLQUFLLENBQUM7O2NBRWxDLElBQUksR0FBRyxLQUFLLENBQUMsY0FBYzs7OztRQUFDLEtBQUssQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDLFlBQVksQ0FBQyxRQUFRLEVBQUMsSUFBSSxJQUFJO1FBRS9FLE9BQU8sSUFBSSxPQUFPOzs7OztRQUFDLENBQUMsT0FBTyxFQUFFLE1BQU0sRUFBRSxFQUFFO1lBQ3JDLGNBQWMsQ0FBQyxJQUFJLENBQUMsQ0FBQyxJQUFJOzs7WUFBQyxHQUFHLEVBQUUsQ0FBQyxPQUFPLEVBQUUsR0FBRSxNQUFNLENBQUMsQ0FBQztRQUNyRCxDQUFDLEVBQUMsQ0FBQztJQUNMLENBQUMsQ0FBQTtJQUVELE9BQU8sRUFBRSxDQUFDO0FBQ1osQ0FBQzs7Ozs7QUFFRCxNQUFNLFVBQVUsY0FBYyxDQUFDLE1BQWM7SUFDM0MsT0FBTyxNQUFNO0lBQ1gsNGNBQTRjO0lBQzVjLDJCQUEyQixnQkFBZ0IsQ0FBQyxNQUFNLENBQUMsSUFBSSxNQUFNLEtBQUssQ0FDbkUsQ0FBQyxJQUFJOzs7O0lBQUMsTUFBTSxDQUFDLEVBQUU7UUFDZCxrQkFBa0IsQ0FBQyxNQUFNLENBQUMsT0FBTyxDQUFDLENBQUM7SUFDckMsQ0FBQyxFQUFDLENBQUM7QUFDTCxDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgcmVnaXN0ZXJMb2NhbGVEYXRhIH0gZnJvbSAnQGFuZ3VsYXIvY29tbW9uJztcbmltcG9ydCB7IEluamVjdG9yIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IEdldEFwcENvbmZpZ3VyYXRpb24gfSBmcm9tICcuLi9hY3Rpb25zL2NvbmZpZy5hY3Rpb25zJztcbmltcG9ydCBkaWZmZXJlbnRMb2NhbGVzIGZyb20gJy4uL2NvbnN0YW50cy9kaWZmZXJlbnQtbG9jYWxlcyc7XG5cbmV4cG9ydCBmdW5jdGlvbiBnZXRJbml0aWFsRGF0YShpbmplY3RvcjogSW5qZWN0b3IpIHtcbiAgY29uc3QgZm4gPSAoKSA9PiB7XG4gICAgY29uc3Qgc3RvcmU6IFN0b3JlID0gaW5qZWN0b3IuZ2V0KFN0b3JlKTtcblxuICAgIHJldHVybiBzdG9yZS5kaXNwYXRjaChuZXcgR2V0QXBwQ29uZmlndXJhdGlvbigpKS50b1Byb21pc2UoKTtcbiAgfTtcblxuICByZXR1cm4gZm47XG59XG5cbmV4cG9ydCBmdW5jdGlvbiBsb2NhbGVJbml0aWFsaXplcihpbmplY3RvcjogSW5qZWN0b3IpIHtcbiAgY29uc3QgZm4gPSAoKSA9PiB7XG4gICAgY29uc3Qgc3RvcmU6IFN0b3JlID0gaW5qZWN0b3IuZ2V0KFN0b3JlKTtcblxuICAgIGNvbnN0IGxhbmcgPSBzdG9yZS5zZWxlY3RTbmFwc2hvdChzdGF0ZSA9PiBzdGF0ZS5TZXNzaW9uU3RhdGUubGFuZ3VhZ2UpIHx8ICdlbic7XG5cbiAgICByZXR1cm4gbmV3IFByb21pc2UoKHJlc29sdmUsIHJlamVjdCkgPT4ge1xuICAgICAgcmVnaXN0ZXJMb2NhbGUobGFuZykudGhlbigoKSA9PiByZXNvbHZlKCksIHJlamVjdCk7XG4gICAgfSk7XG4gIH07XG5cbiAgcmV0dXJuIGZuO1xufVxuXG5leHBvcnQgZnVuY3Rpb24gcmVnaXN0ZXJMb2NhbGUobG9jYWxlOiBzdHJpbmcpIHtcbiAgcmV0dXJuIGltcG9ydChcbiAgICAvKiB3ZWJwYWNrSW5jbHVkZTogLyhhZnxhbXxhci1TQXxhc3xhei1MYXRufGJlfGJnfGJuLUJEfGJuLUlOfGJzfGNhfGNhLUVTLVZBTEVOQ0lBfGNzfGN5fGRhfGRlfGRlfGVsfGVuLUdCfGVufGVzfGVufGVzLVVTfGVzLU1YfGV0fGV1fGZhfGZpfGVufGZyfGZyfGZyLUNBfGdhfGdkfGdsfGd1fGhhfGhlfGhpfGhyfGh1fGh5fGlkfGlnfGlzfGl0fGl0fGphfGthfGtrfGttfGtufGtvfGtva3xlbnxlbnxsYnxsdHxsdnxlbnxta3xtbHxtbnxtcnxtc3xtdHxuYnxuZXxubHxubC1CRXxubnxlbnxvcnxwYXxwYS1BcmFifHBsfGVufHB0fHB0LVBUfGVufGVufHJvfHJ1fHJ3fHBhLUFyYWJ8c2l8c2t8c2x8c3F8c3ItQ3lybC1CQXxzci1DeXJsfHNyLUxhdG58c3Z8c3d8dGF8dGV8dGd8dGh8dGl8dGt8dG58dHJ8dHR8dWd8dWt8dXJ8dXotTGF0bnx2aXx3b3x4aHx5b3x6aC1IYW5zfHpoLUhhbnR8enUpXFwuanMkLyAqL1xuICAgIGBAYW5ndWxhci9jb21tb24vbG9jYWxlcy8ke2RpZmZlcmVudExvY2FsZXNbbG9jYWxlXSB8fCBsb2NhbGV9LmpzYFxuICApLnRoZW4obW9kdWxlID0+IHtcbiAgICByZWdpc3RlckxvY2FsZURhdGEobW9kdWxlLmRlZmF1bHQpO1xuICB9KTtcbn1cbiJdfQ==
