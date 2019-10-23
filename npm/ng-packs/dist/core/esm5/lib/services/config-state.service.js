/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import * as i0 from "@angular/core";
import * as i1 from "@ngxs/store";
var ConfigStateService = /** @class */ (function () {
    function ConfigStateService(store) {
        this.store = store;
    }
    /**
     * @return {?}
     */
    ConfigStateService.prototype.getAll = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(ConfigState.getAll);
    };
    /**
     * @return {?}
     */
    ConfigStateService.prototype.getApplicationInfo = /**
     * @return {?}
     */
    function () {
        return this.store.selectSnapshot(ConfigState.getApplicationInfo);
    };
    /**
     * @param {...?} args
     * @return {?}
     */
    ConfigStateService.prototype.getOne = /**
     * @param {...?} args
     * @return {?}
     */
    function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getOne.apply(ConfigState, tslib_1.__spread(args)));
    };
    /**
     * @param {...?} args
     * @return {?}
     */
    ConfigStateService.prototype.getDeep = /**
     * @param {...?} args
     * @return {?}
     */
    function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getDeep.apply(ConfigState, tslib_1.__spread(args)));
    };
    /**
     * @param {...?} args
     * @return {?}
     */
    ConfigStateService.prototype.getRoute = /**
     * @param {...?} args
     * @return {?}
     */
    function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getRoute.apply(ConfigState, tslib_1.__spread(args)));
    };
    /**
     * @param {...?} args
     * @return {?}
     */
    ConfigStateService.prototype.getApiUrl = /**
     * @param {...?} args
     * @return {?}
     */
    function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getApiUrl.apply(ConfigState, tslib_1.__spread(args)));
    };
    /**
     * @param {...?} args
     * @return {?}
     */
    ConfigStateService.prototype.getSetting = /**
     * @param {...?} args
     * @return {?}
     */
    function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getSetting.apply(ConfigState, tslib_1.__spread(args)));
    };
    /**
     * @param {...?} args
     * @return {?}
     */
    ConfigStateService.prototype.getSettings = /**
     * @param {...?} args
     * @return {?}
     */
    function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getSettings.apply(ConfigState, tslib_1.__spread(args)));
    };
    /**
     * @param {...?} args
     * @return {?}
     */
    ConfigStateService.prototype.getGrantedPolicy = /**
     * @param {...?} args
     * @return {?}
     */
    function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getGrantedPolicy.apply(ConfigState, tslib_1.__spread(args)));
    };
    /**
     * @param {...?} args
     * @return {?}
     */
    ConfigStateService.prototype.getLocalization = /**
     * @param {...?} args
     * @return {?}
     */
    function () {
        var args = [];
        for (var _i = 0; _i < arguments.length; _i++) {
            args[_i] = arguments[_i];
        }
        return this.store.selectSnapshot(ConfigState.getLocalization.apply(ConfigState, tslib_1.__spread(args)));
    };
    ConfigStateService.decorators = [
        { type: Injectable, args: [{
                    providedIn: 'root',
                },] }
    ];
    /** @nocollapse */
    ConfigStateService.ctorParameters = function () { return [
        { type: Store }
    ]; };
    /** @nocollapse */ ConfigStateService.ngInjectableDef = i0.ɵɵdefineInjectable({ factory: function ConfigStateService_Factory() { return new ConfigStateService(i0.ɵɵinject(i1.Store)); }, token: ConfigStateService, providedIn: "root" });
    return ConfigStateService;
}());
export { ConfigStateService };
if (false) {
    /**
     * @type {?}
     * @private
     */
    ConfigStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLXN0YXRlLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvY29uZmlnLXN0YXRlLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQzs7O0FBRXhDO0lBSUUsNEJBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7OztJQUVwQyxtQ0FBTTs7O0lBQU47UUFDRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUMsQ0FBQztJQUN2RCxDQUFDOzs7O0lBRUQsK0NBQWtCOzs7SUFBbEI7UUFDRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxrQkFBa0IsQ0FBQyxDQUFDO0lBQ25FLENBQUM7Ozs7O0lBRUQsbUNBQU07Ozs7SUFBTjtRQUFPLGNBQThDO2FBQTlDLFVBQThDLEVBQTlDLHFCQUE4QyxFQUE5QyxJQUE4QztZQUE5Qyx5QkFBOEM7O1FBQ25ELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLE1BQU0sT0FBbEIsV0FBVyxtQkFBVyxJQUFJLEdBQUUsQ0FBQztJQUNoRSxDQUFDOzs7OztJQUVELG9DQUFPOzs7O0lBQVA7UUFBUSxjQUErQzthQUEvQyxVQUErQyxFQUEvQyxxQkFBK0MsRUFBL0MsSUFBK0M7WUFBL0MseUJBQStDOztRQUNyRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxPQUFPLE9BQW5CLFdBQVcsbUJBQVksSUFBSSxHQUFFLENBQUM7SUFDakUsQ0FBQzs7Ozs7SUFFRCxxQ0FBUTs7OztJQUFSO1FBQVMsY0FBZ0Q7YUFBaEQsVUFBZ0QsRUFBaEQscUJBQWdELEVBQWhELElBQWdEO1lBQWhELHlCQUFnRDs7UUFDdkQsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsUUFBUSxPQUFwQixXQUFXLG1CQUFhLElBQUksR0FBRSxDQUFDO0lBQ2xFLENBQUM7Ozs7O0lBRUQsc0NBQVM7Ozs7SUFBVDtRQUFVLGNBQWlEO2FBQWpELFVBQWlELEVBQWpELHFCQUFpRCxFQUFqRCxJQUFpRDtZQUFqRCx5QkFBaUQ7O1FBQ3pELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFNBQVMsT0FBckIsV0FBVyxtQkFBYyxJQUFJLEdBQUUsQ0FBQztJQUNuRSxDQUFDOzs7OztJQUVELHVDQUFVOzs7O0lBQVY7UUFBVyxjQUFrRDthQUFsRCxVQUFrRCxFQUFsRCxxQkFBa0QsRUFBbEQsSUFBa0Q7WUFBbEQseUJBQWtEOztRQUMzRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxVQUFVLE9BQXRCLFdBQVcsbUJBQWUsSUFBSSxHQUFFLENBQUM7SUFDcEUsQ0FBQzs7Ozs7SUFFRCx3Q0FBVzs7OztJQUFYO1FBQVksY0FBbUQ7YUFBbkQsVUFBbUQsRUFBbkQscUJBQW1ELEVBQW5ELElBQW1EO1lBQW5ELHlCQUFtRDs7UUFDN0QsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsV0FBVyxPQUF2QixXQUFXLG1CQUFnQixJQUFJLEdBQUUsQ0FBQztJQUNyRSxDQUFDOzs7OztJQUVELDZDQUFnQjs7OztJQUFoQjtRQUFpQixjQUF3RDthQUF4RCxVQUF3RCxFQUF4RCxxQkFBd0QsRUFBeEQsSUFBd0Q7WUFBeEQseUJBQXdEOztRQUN2RSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsT0FBNUIsV0FBVyxtQkFBcUIsSUFBSSxHQUFFLENBQUM7SUFDMUUsQ0FBQzs7Ozs7SUFFRCw0Q0FBZTs7OztJQUFmO1FBQWdCLGNBQXVEO2FBQXZELFVBQXVELEVBQXZELHFCQUF1RCxFQUF2RCxJQUF1RDtZQUF2RCx5QkFBdUQ7O1FBQ3JFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGVBQWUsT0FBM0IsV0FBVyxtQkFBb0IsSUFBSSxHQUFFLENBQUM7SUFDekUsQ0FBQzs7Z0JBNUNGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBTFEsS0FBSzs7OzZCQURkO0NBaURDLEFBN0NELElBNkNDO1NBMUNZLGtCQUFrQjs7Ozs7O0lBQ2pCLG1DQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcclxuXHJcbkBJbmplY3RhYmxlKHtcclxuICBwcm92aWRlZEluOiAncm9vdCcsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBDb25maWdTdGF0ZVNlcnZpY2Uge1xyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG5cclxuICBnZXRBbGwoKSB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBbGwpO1xyXG4gIH1cclxuXHJcbiAgZ2V0QXBwbGljYXRpb25JbmZvKCkge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBwbGljYXRpb25JbmZvKTtcclxuICB9XHJcblxyXG4gIGdldE9uZSguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRPbmU+KSB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRPbmUoLi4uYXJncykpO1xyXG4gIH1cclxuXHJcbiAgZ2V0RGVlcCguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXREZWVwPikge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0RGVlcCguLi5hcmdzKSk7XHJcbiAgfVxyXG5cclxuICBnZXRSb3V0ZSguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRSb3V0ZT4pIHtcclxuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldFJvdXRlKC4uLmFyZ3MpKTtcclxuICB9XHJcblxyXG4gIGdldEFwaVVybCguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRBcGlVcmw+KSB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcGlVcmwoLi4uYXJncykpO1xyXG4gIH1cclxuXHJcbiAgZ2V0U2V0dGluZyguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRTZXR0aW5nPikge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0U2V0dGluZyguLi5hcmdzKSk7XHJcbiAgfVxyXG5cclxuICBnZXRTZXR0aW5ncyguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRTZXR0aW5ncz4pIHtcclxuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldFNldHRpbmdzKC4uLmFyZ3MpKTtcclxuICB9XHJcblxyXG4gIGdldEdyYW50ZWRQb2xpY3koLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeT4pIHtcclxuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEdyYW50ZWRQb2xpY3koLi4uYXJncykpO1xyXG4gIH1cclxuXHJcbiAgZ2V0TG9jYWxpemF0aW9uKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbj4pIHtcclxuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbiguLi5hcmdzKSk7XHJcbiAgfVxyXG59XHJcbiJdfQ==