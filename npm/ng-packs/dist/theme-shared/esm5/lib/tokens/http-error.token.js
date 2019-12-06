/**
 * @fileoverview added by tsickle
 * Generated from: lib/tokens/http-error.token.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { InjectionToken } from '@angular/core';
/**
 * @param {?=} config
 * @return {?}
 */
export function httpErrorConfigFactory(config) {
    if (config === void 0) { config = (/** @type {?} */ ({})); }
    if (config.errorScreen && config.errorScreen.component && !config.errorScreen.forWhichErrors) {
        config.errorScreen.forWhichErrors = [401, 403, 404, 500];
    }
    return (/** @type {?} */ (tslib_1.__assign({ errorScreen: {} }, config)));
}
/** @type {?} */
export var HTTP_ERROR_CONFIG = new InjectionToken('HTTP_ERROR_CONFIG');
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaHR0cC1lcnJvci50b2tlbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3Rva2Vucy9odHRwLWVycm9yLnRva2VuLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxlQUFlLENBQUM7Ozs7O0FBRy9DLE1BQU0sVUFBVSxzQkFBc0IsQ0FBQyxNQUE4QjtJQUE5Qix1QkFBQSxFQUFBLDRCQUFTLEVBQUUsRUFBbUI7SUFDbkUsSUFBSSxNQUFNLENBQUMsV0FBVyxJQUFJLE1BQU0sQ0FBQyxXQUFXLENBQUMsU0FBUyxJQUFJLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQyxjQUFjLEVBQUU7UUFDNUYsTUFBTSxDQUFDLFdBQVcsQ0FBQyxjQUFjLEdBQUcsQ0FBQyxHQUFHLEVBQUUsR0FBRyxFQUFFLEdBQUcsRUFBRSxHQUFHLENBQUMsQ0FBQztLQUMxRDtJQUVELE9BQU8sc0NBQ0wsV0FBVyxFQUFFLEVBQUUsSUFDWixNQUFNLEdBQ1MsQ0FBQztBQUN2QixDQUFDOztBQUVELE1BQU0sS0FBTyxpQkFBaUIsR0FBRyxJQUFJLGNBQWMsQ0FBQyxtQkFBbUIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGlvblRva2VuIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBIdHRwRXJyb3JDb25maWcgfSBmcm9tICcuLi9tb2RlbHMvY29tbW9uJztcblxuZXhwb3J0IGZ1bmN0aW9uIGh0dHBFcnJvckNvbmZpZ0ZhY3RvcnkoY29uZmlnID0ge30gYXMgSHR0cEVycm9yQ29uZmlnKSB7XG4gIGlmIChjb25maWcuZXJyb3JTY3JlZW4gJiYgY29uZmlnLmVycm9yU2NyZWVuLmNvbXBvbmVudCAmJiAhY29uZmlnLmVycm9yU2NyZWVuLmZvcldoaWNoRXJyb3JzKSB7XG4gICAgY29uZmlnLmVycm9yU2NyZWVuLmZvcldoaWNoRXJyb3JzID0gWzQwMSwgNDAzLCA0MDQsIDUwMF07XG4gIH1cblxuICByZXR1cm4ge1xuICAgIGVycm9yU2NyZWVuOiB7fSxcbiAgICAuLi5jb25maWcsXG4gIH0gYXMgSHR0cEVycm9yQ29uZmlnO1xufVxuXG5leHBvcnQgY29uc3QgSFRUUF9FUlJPUl9DT05GSUcgPSBuZXcgSW5qZWN0aW9uVG9rZW4oJ0hUVFBfRVJST1JfQ09ORklHJyk7XG4iXX0=