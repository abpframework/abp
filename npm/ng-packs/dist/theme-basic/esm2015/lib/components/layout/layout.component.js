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
    { type: Component, args: [{
                selector: ' abp-layout',
                template: "<nav class=\"navbar navbar-expand-md navbar-dark bg-dark fixed-top\" id=\"main-navbar\">\r\n  <a class=\"navbar-brand\" routerLink=\"/\">\r\n    <img *ngIf=\"appInfo.logoUrl; else appName\" [src]=\"appInfo.logoUrl\" [alt]=\"appInfo.name\" />\r\n  </a>\r\n  <button class=\"navbar-toggler\" type=\"button\" [attr.aria-expanded]=\"!isCollapsed\" (click)=\"isCollapsed = !isCollapsed\">\r\n    <span class=\"navbar-toggler-icon\"></span>\r\n  </button>\r\n  <div class=\"collapse navbar-collapse\" id=\"main-navbar-collapse\" [ngbCollapse]=\"isCollapsed\">\r\n    <ng-content></ng-content>\r\n  </div>\r\n</nav>\r\n\r\n<div\r\n  [@slideFromBottom]=\"outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path\"\r\n  style=\"padding-top: 5rem;\"\r\n  class=\"container\"\r\n>\r\n  <router-outlet #outlet=\"outlet\"></router-outlet>\r\n</div>\r\n\r\n<abp-confirmation></abp-confirmation>\r\n<abp-toast></abp-toast>\r\n\r\n<ng-template #appName>\r\n  {{ appInfo.name }}\r\n</ng-template>\r\n",
                animations: [slideFromBottom]
            }] }
];
/** @nocollapse */
LayoutComponent.ctorParameters = () => [
    { type: Store }
];
if (false) {
    /** @type {?} */
    LayoutComponent.prototype.isCollapsed;
    /**
     * @type {?}
     * @private
     */
    LayoutComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9sYXlvdXQvbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFVLFdBQVcsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNuRCxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdkQsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMxQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBT3BDLE1BQU0sT0FBTyxlQUFlOzs7O0lBTzFCLFlBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBTmhDLGdCQUFXLEdBQUcsSUFBSSxDQUFDO0lBTWdCLENBQUM7Ozs7SUFKcEMsSUFBSSxPQUFPO1FBQ1QsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsa0JBQWtCLENBQUMsQ0FBQztJQUNuRSxDQUFDOzs7WUFWRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGFBQWE7Z0JBQ3ZCLG8vQkFBc0M7Z0JBQ3RDLFVBQVUsRUFBRSxDQUFDLGVBQWUsQ0FBQzthQUM5Qjs7OztZQU5RLEtBQUs7Ozs7SUFRWixzQ0FBbUI7Ozs7O0lBTVAsZ0NBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29uZmlnLCBDb25maWdTdGF0ZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcbmltcG9ydCB7IHNsaWRlRnJvbUJvdHRvbSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcclxuaW1wb3J0IHsgQ29tcG9uZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICcgYWJwLWxheW91dCcsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2xheW91dC5jb21wb25lbnQuaHRtbCcsXHJcbiAgYW5pbWF0aW9uczogW3NsaWRlRnJvbUJvdHRvbV1cclxufSlcclxuZXhwb3J0IGNsYXNzIExheW91dENvbXBvbmVudCB7XHJcbiAgaXNDb2xsYXBzZWQgPSB0cnVlO1xyXG5cclxuICBnZXQgYXBwSW5mbygpOiBDb25maWcuQXBwbGljYXRpb24ge1xyXG4gICAgcmV0dXJuIHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoQ29uZmlnU3RhdGUuZ2V0QXBwbGljYXRpb25JbmZvKTtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxyXG59XHJcbiJdfQ==