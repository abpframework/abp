/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import * as i0 from '@angular/core';
import * as i1 from '@ngxs/store';
var ConfigStateService = /** @class */ (function() {
  function ConfigStateService(store) {
    this.store = store;
  }
  /**
   * @return {?}
   */
  ConfigStateService.prototype.getAll
  /**
   * @return {?}
   */ = function() {
    return this.store.selectSnapshot(ConfigState.getAll);
  };
  /**
   * @return {?}
   */
  ConfigStateService.prototype.getApplicationInfo
  /**
   * @return {?}
   */ = function() {
    return this.store.selectSnapshot(ConfigState.getApplicationInfo);
  };
  /**
   * @param {...?} args
   * @return {?}
   */
  ConfigStateService.prototype.getOne
  /**
   * @param {...?} args
   * @return {?}
   */ = function() {
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
  ConfigStateService.prototype.getDeep
  /**
   * @param {...?} args
   * @return {?}
   */ = function() {
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
  ConfigStateService.prototype.getRoute
  /**
   * @param {...?} args
   * @return {?}
   */ = function() {
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
  ConfigStateService.prototype.getApiUrl
  /**
   * @param {...?} args
   * @return {?}
   */ = function() {
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
  ConfigStateService.prototype.getSetting
  /**
   * @param {...?} args
   * @return {?}
   */ = function() {
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
  ConfigStateService.prototype.getSettings
  /**
   * @param {...?} args
   * @return {?}
   */ = function() {
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
  ConfigStateService.prototype.getGrantedPolicy
  /**
   * @param {...?} args
   * @return {?}
   */ = function() {
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
  ConfigStateService.prototype.getLocalization
  /**
   * @param {...?} args
   * @return {?}
   */ = function() {
    var args = [];
    for (var _i = 0; _i < arguments.length; _i++) {
      args[_i] = arguments[_i];
    }
    return this.store.selectSnapshot(ConfigState.getLocalization.apply(ConfigState, tslib_1.__spread(args)));
  };
  ConfigStateService.decorators = [
    {
      type: Injectable,
      args: [
        {
          providedIn: 'root',
        },
      ],
    },
  ];
  /** @nocollapse */
  ConfigStateService.ctorParameters = function() {
    return [{ type: Store }];
  };
  /** @nocollapse */ ConfigStateService.ngInjectableDef = i0.ɵɵdefineInjectable({
    factory: function ConfigStateService_Factory() {
      return new ConfigStateService(i0.ɵɵinject(i1.Store));
    },
    token: ConfigStateService,
    providedIn: 'root',
  });
  return ConfigStateService;
})();
export { ConfigStateService };
if (false) {
  /**
   * @type {?}
   * @private
   */
  ConfigStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLXN0YXRlLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvY29uZmlnLXN0YXRlLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzNDLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDcEMsT0FBTyxFQUFFLFdBQVcsRUFBRSxNQUFNLFdBQVcsQ0FBQzs7O0FBRXhDO0lBSUUsNEJBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO0lBQUcsQ0FBQzs7OztJQUVwQyxtQ0FBTTs7O0lBQU47UUFDRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUMsQ0FBQztJQUN2RCxDQUFDOzs7O0lBRUQsK0NBQWtCOzs7SUFBbEI7UUFDRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxrQkFBa0IsQ0FBQyxDQUFDO0lBQ25FLENBQUM7Ozs7O0lBRUQsbUNBQU07Ozs7SUFBTjtRQUFPLGNBQThDO2FBQTlDLFVBQThDLEVBQTlDLHFCQUE4QyxFQUE5QyxJQUE4QztZQUE5Qyx5QkFBOEM7O1FBQ25ELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLE1BQU0sT0FBbEIsV0FBVyxtQkFBVyxJQUFJLEdBQUUsQ0FBQztJQUNoRSxDQUFDOzs7OztJQUVELG9DQUFPOzs7O0lBQVA7UUFBUSxjQUErQzthQUEvQyxVQUErQyxFQUEvQyxxQkFBK0MsRUFBL0MsSUFBK0M7WUFBL0MseUJBQStDOztRQUNyRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxPQUFPLE9BQW5CLFdBQVcsbUJBQVksSUFBSSxHQUFFLENBQUM7SUFDakUsQ0FBQzs7Ozs7SUFFRCxxQ0FBUTs7OztJQUFSO1FBQVMsY0FBZ0Q7YUFBaEQsVUFBZ0QsRUFBaEQscUJBQWdELEVBQWhELElBQWdEO1lBQWhELHlCQUFnRDs7UUFDdkQsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsUUFBUSxPQUFwQixXQUFXLG1CQUFhLElBQUksR0FBRSxDQUFDO0lBQ2xFLENBQUM7Ozs7O0lBRUQsc0NBQVM7Ozs7SUFBVDtRQUFVLGNBQWlEO2FBQWpELFVBQWlELEVBQWpELHFCQUFpRCxFQUFqRCxJQUFpRDtZQUFqRCx5QkFBaUQ7O1FBQ3pELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFNBQVMsT0FBckIsV0FBVyxtQkFBYyxJQUFJLEdBQUUsQ0FBQztJQUNuRSxDQUFDOzs7OztJQUVELHVDQUFVOzs7O0lBQVY7UUFBVyxjQUFrRDthQUFsRCxVQUFrRCxFQUFsRCxxQkFBa0QsRUFBbEQsSUFBa0Q7WUFBbEQseUJBQWtEOztRQUMzRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxVQUFVLE9BQXRCLFdBQVcsbUJBQWUsSUFBSSxHQUFFLENBQUM7SUFDcEUsQ0FBQzs7Ozs7SUFFRCx3Q0FBVzs7OztJQUFYO1FBQVksY0FBbUQ7YUFBbkQsVUFBbUQsRUFBbkQscUJBQW1ELEVBQW5ELElBQW1EO1lBQW5ELHlCQUFtRDs7UUFDN0QsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsV0FBVyxPQUF2QixXQUFXLG1CQUFnQixJQUFJLEdBQUUsQ0FBQztJQUNyRSxDQUFDOzs7OztJQUVELDZDQUFnQjs7OztJQUFoQjtRQUFpQixjQUF3RDthQUF4RCxVQUF3RCxFQUF4RCxxQkFBd0QsRUFBeEQsSUFBd0Q7WUFBeEQseUJBQXdEOztRQUN2RSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxnQkFBZ0IsT0FBNUIsV0FBVyxtQkFBcUIsSUFBSSxHQUFFLENBQUM7SUFDMUUsQ0FBQzs7Ozs7SUFFRCw0Q0FBZTs7OztJQUFmO1FBQWdCLGNBQXVEO2FBQXZELFVBQXVELEVBQXZELHFCQUF1RCxFQUF2RCxJQUF1RDtZQUF2RCx5QkFBdUQ7O1FBQ3JFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGVBQWUsT0FBM0IsV0FBVyxtQkFBb0IsSUFBSSxHQUFFLENBQUM7SUFDekUsQ0FBQzs7Z0JBNUNGLFVBQVUsU0FBQztvQkFDVixVQUFVLEVBQUUsTUFBTTtpQkFDbkI7Ozs7Z0JBTFEsS0FBSzs7OzZCQURkO0NBaURDLEFBN0NELElBNkNDO1NBMUNZLGtCQUFrQjs7Ozs7O0lBQ2pCLG1DQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEluamVjdGFibGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgQ29uZmlnU3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMnO1xuXG5ASW5qZWN0YWJsZSh7XG4gIHByb3ZpZGVkSW46ICdyb290Jyxcbn0pXG5leHBvcnQgY2xhc3MgQ29uZmlnU3RhdGVTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgZ2V0QWxsKCkge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFsbCk7XG4gIH1cblxuICBnZXRBcHBsaWNhdGlvbkluZm8oKSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBwbGljYXRpb25JbmZvKTtcbiAgfVxuXG4gIGdldE9uZSguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRPbmU+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0T25lKC4uLmFyZ3MpKTtcbiAgfVxuXG4gIGdldERlZXAoLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0RGVlcD4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXREZWVwKC4uLmFyZ3MpKTtcbiAgfVxuXG4gIGdldFJvdXRlKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldFJvdXRlPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldFJvdXRlKC4uLmFyZ3MpKTtcbiAgfVxuXG4gIGdldEFwaVVybCguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRBcGlVcmw+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBpVXJsKC4uLmFyZ3MpKTtcbiAgfVxuXG4gIGdldFNldHRpbmcoLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0U2V0dGluZz4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRTZXR0aW5nKC4uLmFyZ3MpKTtcbiAgfVxuXG4gIGdldFNldHRpbmdzKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldFNldHRpbmdzPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldFNldHRpbmdzKC4uLmFyZ3MpKTtcbiAgfVxuXG4gIGdldEdyYW50ZWRQb2xpY3koLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeT4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRHcmFudGVkUG9saWN5KC4uLmFyZ3MpKTtcbiAgfVxuXG4gIGdldExvY2FsaXphdGlvbiguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRMb2NhbGl6YXRpb24+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0TG9jYWxpemF0aW9uKC4uLmFyZ3MpKTtcbiAgfVxufVxuIl19
