/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import * as i0 from '@angular/core';
import * as i1 from '@ngxs/store';
export class ConfigService {
  /**
   * @param {?} store
   */
  constructor(store) {
    this.store = store;
  }
  /**
   * @return {?}
   */
  getAll() {
    return this.store.selectSnapshot(ConfigState.getAll);
  }
  /**
   * @param {?} key
   * @return {?}
   */
  getOne(key) {
    return this.store.selectSnapshot(ConfigState.getOne(key));
  }
  /**
   * @param {?} keys
   * @return {?}
   */
  getDeep(keys) {
    return this.store.selectSnapshot(ConfigState.getDeep(keys));
  }
  /**
   * @param {?} key
   * @return {?}
   */
  getSetting(key) {
    return this.store.selectSnapshot(ConfigState.getSetting(key));
  }
}
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
ConfigService.ctorParameters = () => [{ type: Store }];
/** @nocollapse */ ConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({
  factory: function ConfigService_Factory() {
    return new ConfigService(i0.ɵɵinject(i1.Store));
  },
  token: ConfigService,
  providedIn: 'root',
});
if (false) {
  /**
   * @type {?}
   * @private
   */
  ConfigService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvY29uZmlnLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDOzs7QUFLeEMsTUFBTSxPQUFPLGFBQWE7Ozs7SUFDeEIsWUFBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLE1BQU07UUFDSixPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxNQUFNLENBQUMsQ0FBQztJQUN2RCxDQUFDOzs7OztJQUVELE1BQU0sQ0FBQyxHQUFXO1FBQ2hCLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLE1BQU0sQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO0lBQzVELENBQUM7Ozs7O0lBRUQsT0FBTyxDQUFDLElBQXVCO1FBQzdCLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLE9BQU8sQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDO0lBQzlELENBQUM7Ozs7O0lBRUQsVUFBVSxDQUFDLEdBQVc7UUFDcEIsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsVUFBVSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7SUFDaEUsQ0FBQzs7O1lBcEJGLFVBQVUsU0FBQztnQkFDVixVQUFVLEVBQUUsTUFBTTthQUNuQjs7OztZQUxRLEtBQUs7Ozs7Ozs7O0lBT0EsOEJBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5pbXBvcnQgeyBDb25maWdTdGF0ZSB9IGZyb20gJy4uL3N0YXRlcyc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBDb25maWdTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgZ2V0QWxsKCkge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFsbCk7XG4gIH1cblxuICBnZXRPbmUoa2V5OiBzdHJpbmcpIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRPbmUoa2V5KSk7XG4gIH1cblxuICBnZXREZWVwKGtleXM6IHN0cmluZ1tdIHwgc3RyaW5nKSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0RGVlcChrZXlzKSk7XG4gIH1cblxuICBnZXRTZXR0aW5nKGtleTogc3RyaW5nKSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0U2V0dGluZyhrZXkpKTtcbiAgfVxufVxuIl19
