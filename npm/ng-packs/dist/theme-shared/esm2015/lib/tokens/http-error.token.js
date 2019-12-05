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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaHR0cC1lcnJvci50b2tlbi5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL3Rva2Vucy9odHRwLWVycm9yLnRva2VuLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFFLGNBQWMsRUFBRSxNQUFNLGVBQWUsQ0FBQzs7Ozs7QUFHL0MsTUFBTSxVQUFVLHNCQUFzQixDQUFDLE1BQU0sR0FBRyxtQkFBQSxFQUFFLEVBQW1CO0lBQ25FLElBQUksTUFBTSxDQUFDLFdBQVcsSUFBSSxNQUFNLENBQUMsV0FBVyxDQUFDLFNBQVMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxXQUFXLENBQUMsY0FBYyxFQUFFO1FBQzVGLE1BQU0sQ0FBQyxXQUFXLENBQUMsY0FBYyxHQUFHLENBQUMsR0FBRyxFQUFFLEdBQUcsRUFBRSxHQUFHLEVBQUUsR0FBRyxDQUFDLENBQUM7S0FDMUQ7SUFFRCxPQUFPLG1DQUNMLFdBQVcsRUFBRSxFQUFFLElBQ1osTUFBTSxHQUNTLENBQUM7QUFDdkIsQ0FBQzs7QUFFRCxNQUFNLE9BQU8saUJBQWlCLEdBQUcsSUFBSSxjQUFjLENBQUMsbUJBQW1CLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3Rpb25Ub2tlbiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgSHR0cEVycm9yQ29uZmlnIH0gZnJvbSAnLi4vbW9kZWxzL2NvbW1vbic7XG5cbmV4cG9ydCBmdW5jdGlvbiBodHRwRXJyb3JDb25maWdGYWN0b3J5KGNvbmZpZyA9IHt9IGFzIEh0dHBFcnJvckNvbmZpZykge1xuICBpZiAoY29uZmlnLmVycm9yU2NyZWVuICYmIGNvbmZpZy5lcnJvclNjcmVlbi5jb21wb25lbnQgJiYgIWNvbmZpZy5lcnJvclNjcmVlbi5mb3JXaGljaEVycm9ycykge1xuICAgIGNvbmZpZy5lcnJvclNjcmVlbi5mb3JXaGljaEVycm9ycyA9IFs0MDEsIDQwMywgNDA0LCA1MDBdO1xuICB9XG5cbiAgcmV0dXJuIHtcbiAgICBlcnJvclNjcmVlbjoge30sXG4gICAgLi4uY29uZmlnLFxuICB9IGFzIEh0dHBFcnJvckNvbmZpZztcbn1cblxuZXhwb3J0IGNvbnN0IEhUVFBfRVJST1JfQ09ORklHID0gbmV3IEluamVjdGlvblRva2VuKCdIVFRQX0VSUk9SX0NPTkZJRycpO1xuIl19