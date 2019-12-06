/**
 * @fileoverview added by tsickle
 * Generated from: lib/tokens/http-error.token.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { InjectionToken } from '@angular/core';
/**
 * @param {?=} config
 * @return {?}
 */
export function httpErrorConfigFactory(config = (/** @type {?} */ ({}))) {
    if (config.errorScreen && config.errorScreen.component && !config.errorScreen.forWhichErrors) {
        config.errorScreen.forWhichErrors = [401, 403, 404, 500];
    }
    return (/** @type {?} */ (Object.assign({ errorScreen: {} }, config)));
}
/** @type {?} */
export const HTTP_ERROR_CONFIG = new InjectionToken('HTTP_ERROR_CONFIG');
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaHR0cC1lcnJvci50b2tlbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3Rva2Vucy9odHRwLWVycm9yLnRva2VuLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGVBQWUsQ0FBQzs7Ozs7QUFHL0MsTUFBTSxVQUFVLHNCQUFzQixDQUFDLE1BQU0sR0FBRyxtQkFBQSxFQUFFLEVBQW1CO0lBQ25FLElBQUksTUFBTSxDQUFDLFdBQVcsSUFBSSxNQUFNLENBQUMsV0FBVyxDQUFDLFNBQVMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxXQUFXLENBQUMsY0FBYyxFQUFFO1FBQzVGLE1BQU0sQ0FBQyxXQUFXLENBQUMsY0FBYyxHQUFHLENBQUMsR0FBRyxFQUFFLEdBQUcsRUFBRSxHQUFHLEVBQUUsR0FBRyxDQUFDLENBQUM7S0FDMUQ7SUFFRCxPQUFPLG1DQUNMLFdBQVcsRUFBRSxFQUFFLElBQ1osTUFBTSxHQUNTLENBQUM7QUFDdkIsQ0FBQzs7QUFFRCxNQUFNLE9BQU8saUJBQWlCLEdBQUcsSUFBSSxjQUFjLENBQUMsbUJBQW1CLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3Rpb25Ub2tlbiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBIdHRwRXJyb3JDb25maWcgfSBmcm9tICcuLi9tb2RlbHMvY29tbW9uJztcclxuXHJcbmV4cG9ydCBmdW5jdGlvbiBodHRwRXJyb3JDb25maWdGYWN0b3J5KGNvbmZpZyA9IHt9IGFzIEh0dHBFcnJvckNvbmZpZykge1xyXG4gIGlmIChjb25maWcuZXJyb3JTY3JlZW4gJiYgY29uZmlnLmVycm9yU2NyZWVuLmNvbXBvbmVudCAmJiAhY29uZmlnLmVycm9yU2NyZWVuLmZvcldoaWNoRXJyb3JzKSB7XHJcbiAgICBjb25maWcuZXJyb3JTY3JlZW4uZm9yV2hpY2hFcnJvcnMgPSBbNDAxLCA0MDMsIDQwNCwgNTAwXTtcclxuICB9XHJcblxyXG4gIHJldHVybiB7XHJcbiAgICBlcnJvclNjcmVlbjoge30sXHJcbiAgICAuLi5jb25maWcsXHJcbiAgfSBhcyBIdHRwRXJyb3JDb25maWc7XHJcbn1cclxuXHJcbmV4cG9ydCBjb25zdCBIVFRQX0VSUk9SX0NPTkZJRyA9IG5ldyBJbmplY3Rpb25Ub2tlbignSFRUUF9FUlJPUl9DT05GSUcnKTtcclxuIl19