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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaHR0cC1lcnJvci50b2tlbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3Rva2Vucy9odHRwLWVycm9yLnRva2VuLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7OztBQUFBLE9BQU8sRUFBRSxjQUFjLEVBQUUsTUFBTSxlQUFlLENBQUM7Ozs7O0FBRy9DLE1BQU0sVUFBVSxzQkFBc0IsQ0FBQyxNQUE4QjtJQUE5Qix1QkFBQSxFQUFBLDRCQUFTLEVBQUUsRUFBbUI7SUFDbkUsSUFBSSxNQUFNLENBQUMsV0FBVyxJQUFJLE1BQU0sQ0FBQyxXQUFXLENBQUMsU0FBUyxJQUFJLENBQUMsTUFBTSxDQUFDLFdBQVcsQ0FBQyxjQUFjLEVBQUU7UUFDNUYsTUFBTSxDQUFDLFdBQVcsQ0FBQyxjQUFjLEdBQUcsQ0FBQyxHQUFHLEVBQUUsR0FBRyxFQUFFLEdBQUcsRUFBRSxHQUFHLENBQUMsQ0FBQztLQUMxRDtJQUVELE9BQU8sc0NBQ0wsV0FBVyxFQUFFLEVBQUUsSUFDWixNQUFNLEdBQ1MsQ0FBQztBQUN2QixDQUFDOztBQUVELE1BQU0sS0FBTyxpQkFBaUIsR0FBRyxJQUFJLGNBQWMsQ0FBQyxtQkFBbUIsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGlvblRva2VuIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEh0dHBFcnJvckNvbmZpZyB9IGZyb20gJy4uL21vZGVscy9jb21tb24nO1xyXG5cclxuZXhwb3J0IGZ1bmN0aW9uIGh0dHBFcnJvckNvbmZpZ0ZhY3RvcnkoY29uZmlnID0ge30gYXMgSHR0cEVycm9yQ29uZmlnKSB7XHJcbiAgaWYgKGNvbmZpZy5lcnJvclNjcmVlbiAmJiBjb25maWcuZXJyb3JTY3JlZW4uY29tcG9uZW50ICYmICFjb25maWcuZXJyb3JTY3JlZW4uZm9yV2hpY2hFcnJvcnMpIHtcclxuICAgIGNvbmZpZy5lcnJvclNjcmVlbi5mb3JXaGljaEVycm9ycyA9IFs0MDEsIDQwMywgNDA0LCA1MDBdO1xyXG4gIH1cclxuXHJcbiAgcmV0dXJuIHtcclxuICAgIGVycm9yU2NyZWVuOiB7fSxcclxuICAgIC4uLmNvbmZpZyxcclxuICB9IGFzIEh0dHBFcnJvckNvbmZpZztcclxufVxyXG5cclxuZXhwb3J0IGNvbnN0IEhUVFBfRVJST1JfQ09ORklHID0gbmV3IEluamVjdGlvblRva2VuKCdIVFRQX0VSUk9SX0NPTkZJRycpO1xyXG4iXX0=