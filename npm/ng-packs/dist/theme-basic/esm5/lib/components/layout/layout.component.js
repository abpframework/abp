/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ConfigState } from '@abp/ng.core';
import { slideFromBottom } from '@abp/ng.theme.shared';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
var LayoutComponent = /** @class */ (function () {
    function LayoutComponent(store) {
        this.store = store;
        this.isCollapsed = true;
    }
    Object.defineProperty(LayoutComponent.prototype, "appInfo", {
        get: /**
         * @return {?}
         */
        function () {
            return this.store.selectSnapshot(ConfigState.getApplicationInfo);
        },
        enumerable: true,
        configurable: true
    });
    LayoutComponent.decorators = [
        { type: Component, args: [{
                    selector: ' abp-layout',
                    template: "<nav class=\"navbar navbar-expand-md navbar-dark bg-dark fixed-top\" id=\"main-navbar\">\n  <a class=\"navbar-brand\" routerLink=\"/\">\n    <img *ngIf=\"appInfo.logoUrl; else appName\" [src]=\"appInfo.logoUrl\" [alt]=\"appInfo.name\" />\n  </a>\n  <button class=\"navbar-toggler\" type=\"button\" [attr.aria-expanded]=\"!isCollapsed\" (click)=\"isCollapsed = !isCollapsed\">\n    <span class=\"navbar-toggler-icon\"></span>\n  </button>\n  <div class=\"collapse navbar-collapse\" id=\"main-navbar-collapse\" [ngbCollapse]=\"isCollapsed\">\n    <ng-content></ng-content>\n  </div>\n</nav>\n\n<div\n  [@routeAnimations]=\"outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path\"\n  style=\"padding-top: 5rem;\"\n  class=\"container\"\n>\n  <router-outlet #outlet=\"outlet\"></router-outlet>\n</div>\n\n<abp-confirmation></abp-confirmation>\n<abp-toast></abp-toast>\n\n<ng-template #appName>\n  {{ appInfo.name }}\n</ng-template>\n",
                    animations: [slideFromBottom]
                }] }
    ];
    /** @nocollapse */
    LayoutComponent.ctorParameters = function () { return [
        { type: Store }
    ]; };
    return LayoutComponent;
}());
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9sYXlvdXQvbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFVLFdBQVcsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNuRCxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdkQsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMxQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBRXBDO0lBWUUseUJBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBTmhDLGdCQUFXLEdBQVksSUFBSSxDQUFDO0lBTU8sQ0FBQztJQUpwQyxzQkFBSSxvQ0FBTzs7OztRQUFYO1lBQ0UsT0FBTyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxXQUFXLENBQUMsa0JBQWtCLENBQUMsQ0FBQztRQUNuRSxDQUFDOzs7T0FBQTs7Z0JBVkYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxhQUFhO29CQUN2QixnOEJBQXNDO29CQUN0QyxVQUFVLEVBQUUsQ0FBQyxlQUFlLENBQUM7aUJBQzlCOzs7O2dCQU5RLEtBQUs7O0lBZWQsc0JBQUM7Q0FBQSxBQWJELElBYUM7U0FSWSxlQUFlOzs7SUFDMUIsc0NBQTRCOzs7OztJQU1oQixnQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb25maWcsIENvbmZpZ1N0YXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IHNsaWRlRnJvbUJvdHRvbSB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJyBhYnAtbGF5b3V0JyxcbiAgdGVtcGxhdGVVcmw6ICcuL2xheW91dC5jb21wb25lbnQuaHRtbCcsXG4gIGFuaW1hdGlvbnM6IFtzbGlkZUZyb21Cb3R0b21dLFxufSlcbmV4cG9ydCBjbGFzcyBMYXlvdXRDb21wb25lbnQge1xuICBpc0NvbGxhcHNlZDogYm9vbGVhbiA9IHRydWU7XG5cbiAgZ2V0IGFwcEluZm8oKTogQ29uZmlnLkFwcGxpY2F0aW9uIHtcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcHBsaWNhdGlvbkluZm8pO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG59XG4iXX0=