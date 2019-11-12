/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { Injectable } from '@angular/core';
import { addAbpRoutes, PatchRouteByName } from '@abp/ng.core';
import { getSettingTabs } from '@abp/ng.theme.shared';
import { Store } from '@ngxs/store';
import * as i0 from '@angular/core';
import * as i1 from '@ngxs/store';
var SettingManagementConfigService = /** @class */ (function() {
  function SettingManagementConfigService(store) {
    var _this = this;
    this.store = store;
    /** @type {?} */
    var route = /** @type {?} */ ({
      name: 'AbpSettingManagement::Settings',
      path: 'setting-management',
      parentName: 'AbpUiNavigation::Menu:Administration',
      layout: 'application' /* application */,
      order: 6,
      iconClass: 'fa fa-cog',
    });
    addAbpRoutes(route);
    setTimeout(
      /**
       * @return {?}
       */
      function() {
        /** @type {?} */
        var tabs = getSettingTabs();
        if (!tabs || !tabs.length) {
          _this.store.dispatch(
            new PatchRouteByName('AbpSettingManagement::Settings', tslib_1.__assign({}, route, { invisible: true })),
          );
        }
      },
    );
  }
  SettingManagementConfigService.decorators = [
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
  SettingManagementConfigService.ctorParameters = function() {
    return [{ type: Store }];
  };
  /** @nocollapse */ SettingManagementConfigService.ngInjectableDef = i0.ɵɵdefineInjectable({
    factory: function SettingManagementConfigService_Factory() {
      return new SettingManagementConfigService(i0.ɵɵinject(i1.Store));
    },
    token: SettingManagementConfigService,
    providedIn: 'root',
  });
  return SettingManagementConfigService;
})();
export { SettingManagementConfigService };
if (false) {
  /**
   * @type {?}
   * @private
   */
  SettingManagementConfigService.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoic2V0dGluZy1tYW5hZ2VtZW50LWNvbmZpZy5zZXJ2aWNlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5zZXR0aW5nLW1hbmFnZW1lbnQuY29uZmlnLyIsInNvdXJjZXMiOlsibGliL3NlcnZpY2VzL3NldHRpbmctbWFuYWdlbWVudC1jb25maWcuc2VydmljZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0MsT0FBTyxFQUFFLFlBQVksRUFBZSxnQkFBZ0IsRUFBTyxNQUFNLGNBQWMsQ0FBQztBQUNoRixPQUFPLEVBQUUsY0FBYyxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdEQsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQzs7O0FBRXBDO0lBSUUsd0NBQW9CLEtBQVk7UUFBaEMsaUJBa0JDO1FBbEJtQixVQUFLLEdBQUwsS0FBSyxDQUFPOztZQUN4QixLQUFLLEdBQUcsbUJBQUE7WUFDWixJQUFJLEVBQUUsZ0NBQWdDO1lBQ3RDLElBQUksRUFBRSxvQkFBb0I7WUFDMUIsVUFBVSxFQUFFLHNDQUFzQztZQUNsRCxNQUFNLGlDQUF5QjtZQUMvQixLQUFLLEVBQUUsQ0FBQztZQUNSLFNBQVMsRUFBRSxXQUFXO1NBQ3ZCLEVBQWlCO1FBRWxCLFlBQVksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUVwQixVQUFVOzs7UUFBQzs7Z0JBQ0gsSUFBSSxHQUFHLGNBQWMsRUFBRTtZQUM3QixJQUFJLENBQUMsSUFBSSxJQUFJLENBQUMsSUFBSSxDQUFDLE1BQU0sRUFBRTtnQkFDekIsS0FBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxnQkFBZ0IsQ0FBQyxnQ0FBZ0MsdUJBQU8sS0FBSyxJQUFFLFNBQVMsRUFBRSxJQUFJLElBQUcsQ0FBQyxDQUFDO2FBQzVHO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOztnQkF0QkYsVUFBVSxTQUFDO29CQUNWLFVBQVUsRUFBRSxNQUFNO2lCQUNuQjs7OztnQkFKUSxLQUFLOzs7eUNBSGQ7Q0E0QkMsQUF2QkQsSUF1QkM7U0FwQlksOEJBQThCOzs7Ozs7SUFDN0IsK0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgSW5qZWN0YWJsZSB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgYWRkQWJwUm91dGVzLCBlTGF5b3V0VHlwZSwgUGF0Y2hSb3V0ZUJ5TmFtZSwgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IGdldFNldHRpbmdUYWJzIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5cbkBJbmplY3RhYmxlKHtcbiAgcHJvdmlkZWRJbjogJ3Jvb3QnLFxufSlcbmV4cG9ydCBjbGFzcyBTZXR0aW5nTWFuYWdlbWVudENvbmZpZ1NlcnZpY2Uge1xuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge1xuICAgIGNvbnN0IHJvdXRlID0ge1xuICAgICAgbmFtZTogJ0FicFNldHRpbmdNYW5hZ2VtZW50OjpTZXR0aW5ncycsXG4gICAgICBwYXRoOiAnc2V0dGluZy1tYW5hZ2VtZW50JyxcbiAgICAgIHBhcmVudE5hbWU6ICdBYnBVaU5hdmlnYXRpb246Ok1lbnU6QWRtaW5pc3RyYXRpb24nLFxuICAgICAgbGF5b3V0OiBlTGF5b3V0VHlwZS5hcHBsaWNhdGlvbixcbiAgICAgIG9yZGVyOiA2LFxuICAgICAgaWNvbkNsYXNzOiAnZmEgZmEtY29nJyxcbiAgICB9IGFzIEFCUC5GdWxsUm91dGU7XG5cbiAgICBhZGRBYnBSb3V0ZXMocm91dGUpO1xuXG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICBjb25zdCB0YWJzID0gZ2V0U2V0dGluZ1RhYnMoKTtcbiAgICAgIGlmICghdGFicyB8fCAhdGFicy5sZW5ndGgpIHtcbiAgICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgUGF0Y2hSb3V0ZUJ5TmFtZSgnQWJwU2V0dGluZ01hbmFnZW1lbnQ6OlNldHRpbmdzJywgeyAuLi5yb3V0ZSwgaW52aXNpYmxlOiB0cnVlIH0pKTtcbiAgICAgIH1cbiAgICB9KTtcbiAgfVxufVxuIl19
