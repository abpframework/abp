/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ConfigState } from '@abp/ng.core';
import { slideFromBottom } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
var LayoutComponent = /** @class */ (function() {
  function LayoutComponent(store) {
    this.store = store;
    this.isCollapsed = true;
  }
  Object.defineProperty(LayoutComponent.prototype, 'appInfo', {
    /**
     * @return {?}
     */
    get: function() {
      return this.store.selectSnapshot(ConfigState.getApplicationInfo);
    },
    enumerable: true,
    configurable: true,
  });
  LayoutComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: ' abp-layout',
          template:
            '<nav class="navbar navbar-expand-md navbar-dark bg-dark fixed-top" id="main-navbar">\n  <a class="navbar-brand" routerLink="/">\n    <img *ngIf="appInfo.logoUrl; else appName" [src]="appInfo.logoUrl" [alt]="appInfo.name" />\n  </a>\n  <button class="navbar-toggler" type="button" [attr.aria-expanded]="!isCollapsed" (click)="isCollapsed = !isCollapsed">\n    <span class="navbar-toggler-icon"></span>\n  </button>\n  <div class="collapse navbar-collapse" id="main-navbar-collapse" [ngbCollapse]="isCollapsed">\n    <ng-content></ng-content>\n  </div>\n</nav>\n\n<div\n  [@slideFromBottom]="outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path"\n  style="padding-top: 5rem;"\n  class="container"\n>\n  <router-outlet #outlet="outlet"></router-outlet>\n</div>\n\n<abp-confirmation></abp-confirmation>\n<abp-toast></abp-toast>\n\n<ng-template #appName>\n  {{ appInfo.name }}\n</ng-template>\n',
          animations: [slideFromBottom],
        },
      ],
    },
  ];
  /** @nocollapse */
  LayoutComponent.ctorParameters = function() {
    return [{ type: Store }];
  };
  return LayoutComponent;
})();
export { LayoutComponent };
if (false) {
  /** @type {?} */
  LayoutComponent.prototype.isCollapsed;
  /**
   * @type {?}
   * @private
   */
  LayoutComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9sYXlvdXQvbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFVLFdBQVcsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNuRCxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdkQsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMxQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBRXBDO0lBWUUseUJBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBTmhDLGdCQUFXLEdBQUcsSUFBSSxDQUFDO0lBTWdCLENBQUM7SUFKcEMsc0JBQUksb0NBQU87Ozs7UUFBWDtZQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGtCQUFrQixDQUFDLENBQUM7UUFDbkUsQ0FBQzs7O09BQUE7O2dCQVZGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsYUFBYTtvQkFDdkIsZzhCQUFzQztvQkFDdEMsVUFBVSxFQUFFLENBQUMsZUFBZSxDQUFDO2lCQUM5Qjs7OztnQkFOUSxLQUFLOztJQWVkLHNCQUFDO0NBQUEsQUFiRCxJQWFDO1NBUlksZUFBZTs7O0lBQzFCLHNDQUFtQjs7Ozs7SUFNUCxnQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb25maWcsIENvbmZpZ1N0YXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IHNsaWRlRnJvbUJvdHRvbSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJyBhYnAtbGF5b3V0JyxcbiAgdGVtcGxhdGVVcmw6ICcuL2xheW91dC5jb21wb25lbnQuaHRtbCcsXG4gIGFuaW1hdGlvbnM6IFtzbGlkZUZyb21Cb3R0b21dXG59KVxuZXhwb3J0IGNsYXNzIExheW91dENvbXBvbmVudCB7XG4gIGlzQ29sbGFwc2VkID0gdHJ1ZTtcblxuICBnZXQgYXBwSW5mbygpOiBDb25maWcuQXBwbGljYXRpb24ge1xuICAgIHJldHVybiB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KENvbmZpZ1N0YXRlLmdldEFwcGxpY2F0aW9uSW5mbyk7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cbn1cbiJdfQ==
