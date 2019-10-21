/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
var ErrorComponent = /** @class */ (function () {
    function ErrorComponent() {
        this.title = 'Oops!';
        this.details = 'Sorry, an error has occured.';
    }
    /**
     * @return {?}
     */
    ErrorComponent.prototype.destroy = /**
     * @return {?}
     */
    function () {
        this.renderer.removeChild(this.host, this.elementRef.nativeElement);
    };
    ErrorComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-error',
                    template: "<div class=\"error\">\n  <button id=\"abp-close-button mr-3\" type=\"button\" class=\"close\" (click)=\"destroy()\">\n    <span aria-hidden=\"true\">&times;</span>\n  </button>\n  <div class=\"row centered\">\n    <div class=\"col-md-12\">\n      <div class=\"error-template\">\n        <h1>\n          {{ title | abpLocalization }}\n        </h1>\n        <div class=\"error-details\">\n          {{ details | abpLocalization }}\n        </div>\n        <div class=\"error-actions\">\n          <a (click)=\"destroy()\" routerLink=\"/\" class=\"btn btn-primary btn-md mt-2\"\n            ><span class=\"glyphicon glyphicon-home\"></span>\n            {{ { key: '::Menu:Home', defaultValue: 'Home' } | abpLocalization }}\n          </a>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n",
                    styles: [".error{position:fixed;top:0;background-color:#fff;width:100vw;height:100vh;z-index:999999}.centered{position:fixed;top:50%;left:50%;transform:translate(-50%,-50%)}"]
                }] }
    ];
    return ErrorComponent;
}());
export { ErrorComponent };
if (false) {
    /** @type {?} */
    ErrorComponent.prototype.title;
    /** @type {?} */
    ErrorComponent.prototype.details;
    /** @type {?} */
    ErrorComponent.prototype.renderer;
    /** @type {?} */
    ErrorComponent.prototype.elementRef;
    /** @type {?} */
    ErrorComponent.prototype.host;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9lcnJvci9lcnJvci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQXlCLE1BQU0sZUFBZSxDQUFDO0FBR2pFO0lBQUE7UUFNRSxVQUFLLEdBQTRDLE9BQU8sQ0FBQztRQUV6RCxZQUFPLEdBQTRDLDhCQUE4QixDQUFDO0lBV3BGLENBQUM7Ozs7SUFIQyxnQ0FBTzs7O0lBQVA7UUFDRSxJQUFJLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFFLElBQUksQ0FBQyxVQUFVLENBQUMsYUFBYSxDQUFDLENBQUM7SUFDdEUsQ0FBQzs7Z0JBbEJGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsV0FBVztvQkFDckIsMHlCQUFxQzs7aUJBRXRDOztJQWVELHFCQUFDO0NBQUEsQUFuQkQsSUFtQkM7U0FkWSxjQUFjOzs7SUFDekIsK0JBQXlEOztJQUV6RCxpQ0FBa0Y7O0lBRWxGLGtDQUFvQjs7SUFFcEIsb0NBQXVCOztJQUV2Qiw4QkFBVSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgUmVuZGVyZXIyLCBFbGVtZW50UmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBDb25maWcgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtZXJyb3InLFxuICB0ZW1wbGF0ZVVybDogJy4vZXJyb3IuY29tcG9uZW50Lmh0bWwnLFxuICBzdHlsZVVybHM6IFsnZXJyb3IuY29tcG9uZW50LnNjc3MnXSxcbn0pXG5leHBvcnQgY2xhc3MgRXJyb3JDb21wb25lbnQge1xuICB0aXRsZTogc3RyaW5nIHwgQ29uZmlnLkxvY2FsaXphdGlvbldpdGhEZWZhdWx0ID0gJ09vcHMhJztcblxuICBkZXRhaWxzOiBzdHJpbmcgfCBDb25maWcuTG9jYWxpemF0aW9uV2l0aERlZmF1bHQgPSAnU29ycnksIGFuIGVycm9yIGhhcyBvY2N1cmVkLic7XG5cbiAgcmVuZGVyZXI6IFJlbmRlcmVyMjtcblxuICBlbGVtZW50UmVmOiBFbGVtZW50UmVmO1xuXG4gIGhvc3Q6IGFueTtcblxuICBkZXN0cm95KCkge1xuICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlQ2hpbGQodGhpcy5ob3N0LCB0aGlzLmVsZW1lbnRSZWYubmF0aXZlRWxlbWVudCk7XG4gIH1cbn1cbiJdfQ==