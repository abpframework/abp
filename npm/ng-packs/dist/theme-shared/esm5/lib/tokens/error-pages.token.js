/**
 * @fileoverview added by tsickle
 * Generated from: lib/tokens/error-pages.token.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3ItcGFnZXMudG9rZW4uanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi90b2tlbnMvZXJyb3ItcGFnZXMudG9rZW4udHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGVBQWUsQ0FBQzs7Ozs7QUFHL0MsTUFBTSxVQUFVLHNCQUFzQixDQUFDLE1BQThCO0lBQTlCLHVCQUFBLEVBQUEsNEJBQVMsRUFBRSxFQUFtQjtJQUNuRSxJQUFJLE1BQU0sQ0FBQyxXQUFXLElBQUksTUFBTSxDQUFDLFdBQVcsQ0FBQyxTQUFTLElBQUksQ0FBQyxNQUFNLENBQUMsV0FBVyxDQUFDLGNBQWMsRUFBRTtRQUM1RixNQUFNLENBQUMsV0FBVyxDQUFDLGNBQWMsR0FBRyxDQUFDLEdBQUcsRUFBRSxHQUFHLEVBQUUsR0FBRyxFQUFFLEdBQUcsQ0FBQyxDQUFDO0tBQzFEO0lBRUQsT0FBTyxzQ0FDTCxXQUFXLEVBQUUsRUFBRSxJQUNaLE1BQU0sR0FDUyxDQUFDO0FBQ3ZCLENBQUM7O0FBRUQsTUFBTSxLQUFPLGlCQUFpQixHQUFHLElBQUksY0FBYyxDQUFDLG1CQUFtQixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0aW9uVG9rZW4gfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgSHR0cEVycm9yQ29uZmlnIH0gZnJvbSAnLi4vbW9kZWxzL2NvbW1vbic7XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gaHR0cEVycm9yQ29uZmlnRmFjdG9yeShjb25maWcgPSB7fSBhcyBIdHRwRXJyb3JDb25maWcpIHtcclxuICBpZiAoY29uZmlnLmVycm9yU2NyZWVuICYmIGNvbmZpZy5lcnJvclNjcmVlbi5jb21wb25lbnQgJiYgIWNvbmZpZy5lcnJvclNjcmVlbi5mb3JXaGljaEVycm9ycykge1xyXG4gICAgY29uZmlnLmVycm9yU2NyZWVuLmZvcldoaWNoRXJyb3JzID0gWzQwMSwgNDAzLCA0MDQsIDUwMF07XHJcbiAgfVxyXG5cclxuICByZXR1cm4ge1xyXG4gICAgZXJyb3JTY3JlZW46IHt9LFxyXG4gICAgLi4uY29uZmlnLFxyXG4gIH0gYXMgSHR0cEVycm9yQ29uZmlnO1xyXG59XHJcblxyXG5leHBvcnQgY29uc3QgSFRUUF9FUlJPUl9DT05GSUcgPSBuZXcgSW5qZWN0aW9uVG9rZW4oJ0hUVFBfRVJST1JfQ09ORklHJyk7XHJcbiJdfQ==