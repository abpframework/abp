/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ConfigState } from '@abp/ng.core';
import { slideFromBottom } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
export class LayoutComponent {
  /**
   * @param {?} store
   */
  constructor(store) {
    this.store = store;
    this.isCollapsed = true;
  }
  /**
   * @return {?}
   */
  get appInfo() {
    return this.store.selectSnapshot(ConfigState.getApplicationInfo);
  }
}
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
LayoutComponent.ctorParameters = () => [{ type: Store }];
if (false) {
  /** @type {?} */
  LayoutComponent.prototype.isCollapsed;
  /**
   * @type {?}
   * @private
   */
  LayoutComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9sYXlvdXQvbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFVLFdBQVcsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNuRCxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdkQsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMxQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBT3BDLE1BQU0sT0FBTyxlQUFlOzs7O0lBTzFCLFlBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBTmhDLGdCQUFXLEdBQUcsSUFBSSxDQUFDO0lBTWdCLENBQUM7Ozs7SUFKcEMsSUFBSSxPQUFPO1FBQ1QsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsa0JBQWtCLENBQUMsQ0FBQztJQUNuRSxDQUFDOzs7WUFWRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGFBQWE7Z0JBQ3ZCLGc4QkFBc0M7Z0JBQ3RDLFVBQVUsRUFBRSxDQUFDLGVBQWUsQ0FBQzthQUM5Qjs7OztZQU5RLEtBQUs7Ozs7SUFRWixzQ0FBbUI7Ozs7O0lBTVAsZ0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29uZmlnLCBDb25maWdTdGF0ZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBzbGlkZUZyb21Cb3R0b20gfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICcgYWJwLWxheW91dCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9sYXlvdXQuY29tcG9uZW50Lmh0bWwnLFxuICBhbmltYXRpb25zOiBbc2xpZGVGcm9tQm90dG9tXVxufSlcbmV4cG9ydCBjbGFzcyBMYXlvdXRDb21wb25lbnQge1xuICBpc0NvbGxhcHNlZCA9IHRydWU7XG5cbiAgZ2V0IGFwcEluZm8oKTogQ29uZmlnLkFwcGxpY2F0aW9uIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcHBsaWNhdGlvbkluZm8pO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG59XG4iXX0=
