/**
 * @fileoverview added by tsickle
 * Generated from: lib/services/config-state.service.ts
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLXN0YXRlLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvY29uZmlnLXN0YXRlLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSxXQUFXLEVBQUUsTUFBTSxXQUFXLENBQUM7OztBQUV4QztJQUlFLDRCQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7SUFFcEMsbUNBQU07OztJQUFOO1FBQ0UsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsTUFBTSxDQUFDLENBQUM7SUFDdkQsQ0FBQzs7OztJQUVELCtDQUFrQjs7O0lBQWxCO1FBQ0UsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsa0JBQWtCLENBQUMsQ0FBQztJQUNuRSxDQUFDOzs7OztJQUVELG1DQUFNOzs7O0lBQU47UUFBTyxjQUE4QzthQUE5QyxVQUE4QyxFQUE5QyxxQkFBOEMsRUFBOUMsSUFBOEM7WUFBOUMseUJBQThDOztRQUNuRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxNQUFNLE9BQWxCLFdBQVcsbUJBQVcsSUFBSSxHQUFFLENBQUM7SUFDaEUsQ0FBQzs7Ozs7SUFFRCxvQ0FBTzs7OztJQUFQO1FBQVEsY0FBK0M7YUFBL0MsVUFBK0MsRUFBL0MscUJBQStDLEVBQS9DLElBQStDO1lBQS9DLHlCQUErQzs7UUFDckQsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsT0FBTyxPQUFuQixXQUFXLG1CQUFZLElBQUksR0FBRSxDQUFDO0lBQ2pFLENBQUM7Ozs7O0lBRUQscUNBQVE7Ozs7SUFBUjtRQUFTLGNBQWdEO2FBQWhELFVBQWdELEVBQWhELHFCQUFnRCxFQUFoRCxJQUFnRDtZQUFoRCx5QkFBZ0Q7O1FBQ3ZELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFFBQVEsT0FBcEIsV0FBVyxtQkFBYSxJQUFJLEdBQUUsQ0FBQztJQUNsRSxDQUFDOzs7OztJQUVELHNDQUFTOzs7O0lBQVQ7UUFBVSxjQUFpRDthQUFqRCxVQUFpRCxFQUFqRCxxQkFBaUQsRUFBakQsSUFBaUQ7WUFBakQseUJBQWlEOztRQUN6RCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxTQUFTLE9BQXJCLFdBQVcsbUJBQWMsSUFBSSxHQUFFLENBQUM7SUFDbkUsQ0FBQzs7Ozs7SUFFRCx1Q0FBVTs7OztJQUFWO1FBQVcsY0FBa0Q7YUFBbEQsVUFBa0QsRUFBbEQscUJBQWtELEVBQWxELElBQWtEO1lBQWxELHlCQUFrRDs7UUFDM0QsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsVUFBVSxPQUF0QixXQUFXLG1CQUFlLElBQUksR0FBRSxDQUFDO0lBQ3BFLENBQUM7Ozs7O0lBRUQsd0NBQVc7Ozs7SUFBWDtRQUFZLGNBQW1EO2FBQW5ELFVBQW1ELEVBQW5ELHFCQUFtRCxFQUFuRCxJQUFtRDtZQUFuRCx5QkFBbUQ7O1FBQzdELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFdBQVcsT0FBdkIsV0FBVyxtQkFBZ0IsSUFBSSxHQUFFLENBQUM7SUFDckUsQ0FBQzs7Ozs7SUFFRCw2Q0FBZ0I7Ozs7SUFBaEI7UUFBaUIsY0FBd0Q7YUFBeEQsVUFBd0QsRUFBeEQscUJBQXdELEVBQXhELElBQXdEO1lBQXhELHlCQUF3RDs7UUFDdkUsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsZ0JBQWdCLE9BQTVCLFdBQVcsbUJBQXFCLElBQUksR0FBRSxDQUFDO0lBQzFFLENBQUM7Ozs7O0lBRUQsNENBQWU7Ozs7SUFBZjtRQUFnQixjQUF1RDthQUF2RCxVQUF1RCxFQUF2RCxxQkFBdUQsRUFBdkQsSUFBdUQ7WUFBdkQseUJBQXVEOztRQUNyRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxlQUFlLE9BQTNCLFdBQVcsbUJBQW9CLElBQUksR0FBRSxDQUFDO0lBQ3pFLENBQUM7O2dCQTVDRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLEtBQUs7Ozs2QkFEZDtDQWlEQyxBQTdDRCxJQTZDQztTQTFDWSxrQkFBa0I7Ozs7OztJQUNqQixtQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIENvbmZpZ1N0YXRlU2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGdldEFsbCgpIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBbGwpO1xuICB9XG5cbiAgZ2V0QXBwbGljYXRpb25JbmZvKCkge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFwcGxpY2F0aW9uSW5mbyk7XG4gIH1cblxuICBnZXRPbmUoLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0T25lPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldE9uZSguLi5hcmdzKSk7XG4gIH1cblxuICBnZXREZWVwKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldERlZXA+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0RGVlcCguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRSb3V0ZSguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRSb3V0ZT4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRSb3V0ZSguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRBcGlVcmwoLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0QXBpVXJsPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFwaVVybCguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRTZXR0aW5nKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldFNldHRpbmc+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0U2V0dGluZyguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRTZXR0aW5ncyguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRTZXR0aW5ncz4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRTZXR0aW5ncyguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRHcmFudGVkUG9saWN5KC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldEdyYW50ZWRQb2xpY3k+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeSguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRMb2NhbGl6YXRpb24oLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0TG9jYWxpemF0aW9uPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbiguLi5hcmdzKSk7XG4gIH1cbn1cbiJdfQ==