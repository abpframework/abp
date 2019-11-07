/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import * as i0 from '@angular/core';
import * as i1 from '@ngxs/store';
var ConfigService = /** @class */ (function() {
  function ConfigService(store) {
    this.store = store;
  }
  /**
   * @return {?}
   */
  ConfigService.prototype.getAll
  /**
   * @return {?}
   */ = function() {
    return this.store.selectSnapshot(ConfigState.getAll);
  };
  /**
   * @param {?} key
   * @return {?}
   */
  ConfigService.prototype.getOne
  /**
   * @param {?} key
   * @return {?}
   */ = function(key) {
    return this.store.selectSnapshot(ConfigState.getOne(key));
  };
  /**
   * @param {?} keys
   * @return {?}
   */
  ConfigService.prototype.getDeep
  /**
   * @param {?} keys
   * @return {?}
   */ = function(keys) {
    return this.store.selectSnapshot(ConfigState.getDeep(keys));
  };
  /**
   * @param {?} key
   * @return {?}
   */
  ConfigService.prototype.getSetting
  /**
   * @param {?} key
   * @return {?}
   */ = function(key) {
    return this.store.selectSnapshot(ConfigState.getSetting(key));
  };
  ConfigService.decorators = [
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
  ConfigService.ctorParameters = function() {
    return [{ type: Store }];
  };
  /** @nocollapse */ ConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({
    factory: function ConfigService_Factory() {
      return new ConfigService(i0.ɵɵinject(i1.Store));
    },
    token: ConfigService,
    providedIn: 'root',
  });
  return ConfigService;
})();
export { ConfigService };
if (false) {
  /**
   * @type {?}
   * @private
   */
  ConfigService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvY29uZmlnLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDOzs7QUFFeEM7SUFJRSx1QkFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLDhCQUFNOzs7SUFBTjtRQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0lBQ3ZELENBQUM7Ozs7O0lBRUQsOEJBQU07Ozs7SUFBTixVQUFPLEdBQVc7UUFDaEIsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsTUFBTSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7SUFDNUQsQ0FBQzs7Ozs7SUFFRCwrQkFBTzs7OztJQUFQLFVBQVEsSUFBdUI7UUFDN0IsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsT0FBTyxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUM7SUFDOUQsQ0FBQzs7Ozs7SUFFRCxrQ0FBVTs7OztJQUFWLFVBQVcsR0FBVztRQUNwQixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztJQUNoRSxDQUFDOztnQkFwQkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFMUSxLQUFLOzs7d0JBRGQ7Q0F5QkMsQUFyQkQsSUFxQkM7U0FsQlksYUFBYTs7Ozs7O0lBQ1osOEJBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBDb25maWdTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgZ2V0QWxsKCkge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFsbCk7XG4gIH1cblxuICBnZXRPbmUoa2V5OiBzdHJpbmcpIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRPbmUoa2V5KSk7XG4gIH1cblxuICBnZXREZWVwKGtleXM6IHN0cmluZ1tdIHwgc3RyaW5nKSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0RGVlcChrZXlzKSk7XG4gIH1cblxuICBnZXRTZXR0aW5nKGtleTogc3RyaW5nKSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0U2V0dGluZyhrZXkpKTtcbiAgfVxufVxuIl19
