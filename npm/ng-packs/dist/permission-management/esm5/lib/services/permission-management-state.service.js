/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Injectable } from '@angular/core';
import { Store } from '@ngxs/store';
import { PermissionManagementState } from '../states/permission-management.state';
import * as i0 from '@angular/core';
import * as i1 from '@ngxs/store';
var PermissionManagementStateService = /** @class */ (function() {
  function PermissionManagementStateService(store) {
    this.store = store;
  }
  /**
   * @return {?}
   */
  PermissionManagementStateService.prototype.getPermissionGroups
  /**
   * @return {?}
   */ = function() {
    return this.store.selectSnapshot(PermissionManagementState.getPermissionGroups);
  };
  /**
   * @return {?}
   */
  PermissionManagementStateService.prototype.getEntityDisplayName
  /**
   * @return {?}
   */ = function() {
    return this.store.selectSnapshot(PermissionManagementState.getPermissionGroups);
  };
  PermissionManagementStateService.decorators = [
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
  PermissionManagementStateService.ctorParameters = function() {
    return [{ type: Store }];
  };
  /** @nocollapse */ PermissionManagementStateService.ngInjectableDef = i0.ɵɵdefineInjectable({
    factory: function PermissionManagementStateService_Factory() {
      return new PermissionManagementStateService(i0.ɵɵinject(i1.Store));
    },
    token: PermissionManagementStateService,
    providedIn: 'root',
  });
  return PermissionManagementStateService;
})();
export { PermissionManagementStateService };
if (false) {
  /**
   * @type {?}
   * @private
   */
  PermissionManagementStateService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicGVybWlzc2lvbi1tYW5hZ2VtZW50LXN0YXRlLnNlcnZpY2UuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnBlcm1pc3Npb24tbWFuYWdlbWVudC8iLCJzb3VyY2VzIjpbImxpYi9zZXJ2aWNlcy9wZXJtaXNzaW9uLW1hbmFnZW1lbnQtc3RhdGUuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMzQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQ3BDLE9BQU8sRUFBRSx5QkFBeUIsRUFBRSxNQUFNLHVDQUF1QyxDQUFDOzs7QUFFbEY7SUFJRSwwQ0FBb0IsS0FBWTtRQUFaLFVBQUssR0FBTCxLQUFLLENBQU87SUFBRyxDQUFDOzs7O0lBRXBDLDhEQUFtQjs7O0lBQW5CO1FBQ0UsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyx5QkFBeUIsQ0FBQyxtQkFBbUIsQ0FBQyxDQUFDO0lBQ2xGLENBQUM7Ozs7SUFDRCwrREFBb0I7OztJQUFwQjtRQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMseUJBQXlCLENBQUMsbUJBQW1CLENBQUMsQ0FBQztJQUNsRixDQUFDOztnQkFYRixVQUFVLFNBQUM7b0JBQ1YsVUFBVSxFQUFFLE1BQU07aUJBQ25COzs7O2dCQUxRLEtBQUs7OzsyQ0FEZDtDQWdCQyxBQVpELElBWUM7U0FUWSxnQ0FBZ0M7Ozs7OztJQUMvQixpREFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBJbmplY3RhYmxlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUgfSBmcm9tICcuLi9zdGF0ZXMvcGVybWlzc2lvbi1tYW5hZ2VtZW50LnN0YXRlJztcblxuQEluamVjdGFibGUoe1xuICBwcm92aWRlZEluOiAncm9vdCcsXG59KVxuZXhwb3J0IGNsYXNzIFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGVTZXJ2aWNlIHtcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgZ2V0UGVybWlzc2lvbkdyb3VwcygpIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChQZXJtaXNzaW9uTWFuYWdlbWVudFN0YXRlLmdldFBlcm1pc3Npb25Hcm91cHMpO1xuICB9XG4gIGdldEVudGl0eURpc3BsYXlOYW1lKCkge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFBlcm1pc3Npb25NYW5hZ2VtZW50U3RhdGUuZ2V0UGVybWlzc2lvbkdyb3Vwcyk7XG4gIH1cbn1cbiJdfQ==
