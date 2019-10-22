/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                    template: "<nav class=\"navbar navbar-expand-md navbar-dark bg-dark fixed-top\" id=\"main-navbar\">\r\n  <a class=\"navbar-brand\" routerLink=\"/\">\r\n    <img *ngIf=\"appInfo.logoUrl; else appName\" [src]=\"appInfo.logoUrl\" [alt]=\"appInfo.name\" />\r\n  </a>\r\n  <button class=\"navbar-toggler\" type=\"button\" [attr.aria-expanded]=\"!isCollapsed\" (click)=\"isCollapsed = !isCollapsed\">\r\n    <span class=\"navbar-toggler-icon\"></span>\r\n  </button>\r\n  <div class=\"collapse navbar-collapse\" id=\"main-navbar-collapse\" [ngbCollapse]=\"isCollapsed\">\r\n    <ng-content></ng-content>\r\n  </div>\r\n</nav>\r\n\r\n<div\r\n  [@slideFromBottom]=\"outlet && outlet.activatedRoute && outlet.activatedRoute.routeConfig.path\"\r\n  style=\"padding-top: 5rem;\"\r\n  class=\"container\"\r\n>\r\n  <router-outlet #outlet=\"outlet\"></router-outlet>\r\n</div>\r\n\r\n<abp-confirmation></abp-confirmation>\r\n<abp-toast></abp-toast>\r\n\r\n<ng-template #appName>\r\n  {{ appInfo.name }}\r\n</ng-template>\r\n",
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibGF5b3V0LmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuYmFzaWMvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9sYXlvdXQvbGF5b3V0LmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7O0FBQUEsT0FBTyxFQUFVLFdBQVcsRUFBRSxNQUFNLGNBQWMsQ0FBQztBQUNuRCxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sc0JBQXNCLENBQUM7QUFDdkQsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUMxQyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBRXBDO0lBWUUseUJBQW9CLEtBQVk7UUFBWixVQUFLLEdBQUwsS0FBSyxDQUFPO1FBTmhDLGdCQUFXLEdBQUcsSUFBSSxDQUFDO0lBTWdCLENBQUM7SUFKcEMsc0JBQUksb0NBQU87Ozs7UUFBWDtZQUNFLE9BQU8sSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsV0FBVyxDQUFDLGtCQUFrQixDQUFDLENBQUM7UUFDbkUsQ0FBQzs7O09BQUE7O2dCQVZGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsYUFBYTtvQkFDdkIsby9CQUFzQztvQkFDdEMsVUFBVSxFQUFFLENBQUMsZUFBZSxDQUFDO2lCQUM5Qjs7OztnQkFOUSxLQUFLOztJQWVkLHNCQUFDO0NBQUEsQUFiRCxJQWFDO1NBUlksZUFBZTs7O0lBQzFCLHNDQUFtQjs7Ozs7SUFNUCxnQ0FBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb25maWcsIENvbmZpZ1N0YXRlIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuaW1wb3J0IHsgc2xpZGVGcm9tQm90dG9tIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xyXG5pbXBvcnQgeyBDb21wb25lbnQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgU3RvcmUgfSBmcm9tICdAbmd4cy9zdG9yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJyBhYnAtbGF5b3V0JyxcclxuICB0ZW1wbGF0ZVVybDogJy4vbGF5b3V0LmNvbXBvbmVudC5odG1sJyxcclxuICBhbmltYXRpb25zOiBbc2xpZGVGcm9tQm90dG9tXVxyXG59KVxyXG5leHBvcnQgY2xhc3MgTGF5b3V0Q29tcG9uZW50IHtcclxuICBpc0NvbGxhcHNlZCA9IHRydWU7XHJcblxyXG4gIGdldCBhcHBJbmZvKCk6IENvbmZpZy5BcHBsaWNhdGlvbiB7XHJcbiAgICByZXR1cm4gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChDb25maWdTdGF0ZS5nZXRBcHBsaWNhdGlvbkluZm8pO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XHJcbn1cclxuIl19