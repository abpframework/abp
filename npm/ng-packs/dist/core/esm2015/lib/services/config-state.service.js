/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import * as i0 from '@angular/core';
import * as i1 from '@ngxs/store';
export class ConfigStateService {
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
   * @return {?}
   */
  getApplicationInfo() {
    return this.store.selectSnapshot(ConfigState.getApplicationInfo);
  }
  /**
   * @param {...?} args
   * @return {?}
   */
  getOne(...args) {
    return this.store.selectSnapshot(ConfigState.getOne(...args));
  }
  /**
   * @param {...?} args
   * @return {?}
   */
  getDeep(...args) {
    return this.store.selectSnapshot(ConfigState.getDeep(...args));
  }
  /**
   * @param {...?} args
   * @return {?}
   */
  getRoute(...args) {
    return this.store.selectSnapshot(ConfigState.getRoute(...args));
  }
  /**
   * @param {...?} args
   * @return {?}
   */
  getApiUrl(...args) {
    return this.store.selectSnapshot(ConfigState.getApiUrl(...args));
  }
  /**
   * @param {...?} args
   * @return {?}
   */
  getSetting(...args) {
    return this.store.selectSnapshot(ConfigState.getSetting(...args));
  }
  /**
   * @param {...?} args
   * @return {?}
   */
  getSettings(...args) {
    return this.store.selectSnapshot(ConfigState.getSettings(...args));
  }
  /**
   * @param {...?} args
   * @return {?}
   */
  getGrantedPolicy(...args) {
    return this.store.selectSnapshot(ConfigState.getGrantedPolicy(...args));
  }
  /**
   * @param {...?} args
   * @return {?}
   */
  getLocalization(...args) {
    return this.store.selectSnapshot(ConfigState.getLocalization(...args));
  }
}
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
ConfigStateService.ctorParameters = () => [{ type: Store }];
/** @nocollapse */ ConfigStateService.ngInjectableDef = i0.ɵɵdefineInjectable({
  factory: function ConfigStateService_Factory() {
    return new ConfigStateService(i0.ɵɵinject(i1.Store));
  },
  token: ConfigStateService,
  providedIn: 'root',
});
if (false) {
  /**
   * @type {?}
   * @private
   */
  ConfigStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlnLXN0YXRlLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmNvcmUvIiwic291cmNlcyI6WyJsaWIvc2VydmljZXMvY29uZmlnLXN0YXRlLnNlcnZpY2UudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUNwQyxPQUFPLEVBQUUsV0FBVyxFQUFFLE1BQU0sV0FBVyxDQUFDOzs7QUFLeEMsTUFBTSxPQUFPLGtCQUFrQjs7OztJQUM3QixZQUFvQixLQUFZO1FBQVosVUFBSyxHQUFMLEtBQUssQ0FBTztJQUFHLENBQUM7Ozs7SUFFcEMsTUFBTTtRQUNKLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0lBQ3ZELENBQUM7Ozs7SUFFRCxrQkFBa0I7UUFDaEIsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsa0JBQWtCLENBQUMsQ0FBQztJQUNuRSxDQUFDOzs7OztJQUVELE1BQU0sQ0FBQyxHQUFHLElBQTJDO1FBQ25ELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLE1BQU0sQ0FBQyxHQUFHLElBQUksQ0FBQyxDQUFDLENBQUM7SUFDaEUsQ0FBQzs7Ozs7SUFFRCxPQUFPLENBQUMsR0FBRyxJQUE0QztRQUNyRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxPQUFPLENBQUMsR0FBRyxJQUFJLENBQUMsQ0FBQyxDQUFDO0lBQ2pFLENBQUM7Ozs7O0lBRUQsUUFBUSxDQUFDLEdBQUcsSUFBNkM7UUFDdkQsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsUUFBUSxDQUFDLEdBQUcsSUFBSSxDQUFDLENBQUMsQ0FBQztJQUNsRSxDQUFDOzs7OztJQUVELFNBQVMsQ0FBQyxHQUFHLElBQThDO1FBQ3pELE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLFNBQVMsQ0FBQyxHQUFHLElBQUksQ0FBQyxDQUFDLENBQUM7SUFDbkUsQ0FBQzs7Ozs7SUFFRCxVQUFVLENBQUMsR0FBRyxJQUErQztRQUMzRCxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUFDLFdBQVcsQ0FBQyxVQUFVLENBQUMsR0FBRyxJQUFJLENBQUMsQ0FBQyxDQUFDO0lBQ3BFLENBQUM7Ozs7O0lBRUQsV0FBVyxDQUFDLEdBQUcsSUFBZ0Q7UUFDN0QsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsV0FBVyxDQUFDLEdBQUcsSUFBSSxDQUFDLENBQUMsQ0FBQztJQUNyRSxDQUFDOzs7OztJQUVELGdCQUFnQixDQUFDLEdBQUcsSUFBcUQ7UUFDdkUsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsZ0JBQWdCLENBQUMsR0FBRyxJQUFJLENBQUMsQ0FBQyxDQUFDO0lBQzFFLENBQUM7Ozs7O0lBRUQsZUFBZSxDQUFDLEdBQUcsSUFBb0Q7UUFDckUsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsZUFBZSxDQUFDLEdBQUcsSUFBSSxDQUFDLENBQUMsQ0FBQztJQUN6RSxDQUFDOzs7WUE1Q0YsVUFBVSxTQUFDO2dCQUNWLFVBQVUsRUFBRSxNQUFNO2FBQ25COzs7O1lBTFEsS0FBSzs7Ozs7Ozs7SUFPQSxtQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IENvbmZpZ1N0YXRlIH0gZnJvbSAnLi4vc3RhdGVzJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIENvbmZpZ1N0YXRlU2VydmljZSB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIGdldEFsbCgpIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBbGwpO1xuICB9XG5cbiAgZ2V0QXBwbGljYXRpb25JbmZvKCkge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFwcGxpY2F0aW9uSW5mbyk7XG4gIH1cblxuICBnZXRPbmUoLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0T25lPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldE9uZSguLi5hcmdzKSk7XG4gIH1cblxuICBnZXREZWVwKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldERlZXA+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0RGVlcCguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRSb3V0ZSguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRSb3V0ZT4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRSb3V0ZSguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRBcGlVcmwoLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0QXBpVXJsPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFwaVVybCguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRTZXR0aW5nKC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldFNldHRpbmc+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0U2V0dGluZyguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRTZXR0aW5ncyguLi5hcmdzOiBQYXJhbWV0ZXJzPHR5cGVvZiBDb25maWdTdGF0ZS5nZXRTZXR0aW5ncz4pIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRTZXR0aW5ncyguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRHcmFudGVkUG9saWN5KC4uLmFyZ3M6IFBhcmFtZXRlcnM8dHlwZW9mIENvbmZpZ1N0YXRlLmdldEdyYW50ZWRQb2xpY3k+KSB7XG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0R3JhbnRlZFBvbGljeSguLi5hcmdzKSk7XG4gIH1cblxuICBnZXRMb2NhbGl6YXRpb24oLi4uYXJnczogUGFyYW1ldGVyczx0eXBlb2YgQ29uZmlnU3RhdGUuZ2V0TG9jYWxpemF0aW9uPikge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldExvY2FsaXphdGlvbiguLi5hcmdzKSk7XG4gIH1cbn1cbiJdfQ==
