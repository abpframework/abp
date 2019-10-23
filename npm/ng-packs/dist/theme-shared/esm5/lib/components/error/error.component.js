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
                    template: "<div class=\"error\">\r\n  <button id=\"abp-close-button mr-3\" type=\"button\" class=\"close\" (click)=\"destroy()\">\r\n    <span aria-hidden=\"true\">&times;</span>\r\n  </button>\r\n  <div class=\"row centered\">\r\n    <div class=\"col-md-12\">\r\n      <div class=\"error-template\">\r\n        <h1>\r\n          {{ title | abpLocalization }}\r\n        </h1>\r\n        <div class=\"error-details\">\r\n          {{ details | abpLocalization }}\r\n        </div>\r\n        <div class=\"error-actions\">\r\n          <a (click)=\"destroy()\" routerLink=\"/\" class=\"btn btn-primary btn-md mt-2\"\r\n            ><span class=\"glyphicon glyphicon-home\"></span>\r\n            {{ { key: '::Menu:Home', defaultValue: 'Home' } | abpLocalization }}\r\n          </a>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n",
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9lcnJvci9lcnJvci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQXlCLE1BQU0sZUFBZSxDQUFDO0FBR2pFO0lBQUE7UUFNRSxVQUFLLEdBQTRDLE9BQU8sQ0FBQztRQUV6RCxZQUFPLEdBQTRDLDhCQUE4QixDQUFDO0lBV3BGLENBQUM7Ozs7SUFIQyxnQ0FBTzs7O0lBQVA7UUFDRSxJQUFJLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFFLElBQUksQ0FBQyxVQUFVLENBQUMsYUFBYSxDQUFDLENBQUM7SUFDdEUsQ0FBQzs7Z0JBbEJGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsV0FBVztvQkFDckIsdzFCQUFxQzs7aUJBRXRDOztJQWVELHFCQUFDO0NBQUEsQUFuQkQsSUFtQkM7U0FkWSxjQUFjOzs7SUFDekIsK0JBQXlEOztJQUV6RCxpQ0FBa0Y7O0lBRWxGLGtDQUFvQjs7SUFFcEIsb0NBQXVCOztJQUV2Qiw4QkFBVSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgUmVuZGVyZXIyLCBFbGVtZW50UmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IENvbmZpZyB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1lcnJvcicsXHJcbiAgdGVtcGxhdGVVcmw6ICcuL2Vycm9yLmNvbXBvbmVudC5odG1sJyxcclxuICBzdHlsZVVybHM6IFsnZXJyb3IuY29tcG9uZW50LnNjc3MnXSxcclxufSlcclxuZXhwb3J0IGNsYXNzIEVycm9yQ29tcG9uZW50IHtcclxuICB0aXRsZTogc3RyaW5nIHwgQ29uZmlnLkxvY2FsaXphdGlvbldpdGhEZWZhdWx0ID0gJ09vcHMhJztcclxuXHJcbiAgZGV0YWlsczogc3RyaW5nIHwgQ29uZmlnLkxvY2FsaXphdGlvbldpdGhEZWZhdWx0ID0gJ1NvcnJ5LCBhbiBlcnJvciBoYXMgb2NjdXJlZC4nO1xyXG5cclxuICByZW5kZXJlcjogUmVuZGVyZXIyO1xyXG5cclxuICBlbGVtZW50UmVmOiBFbGVtZW50UmVmO1xyXG5cclxuICBob3N0OiBhbnk7XHJcblxyXG4gIGRlc3Ryb3koKSB7XHJcbiAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNoaWxkKHRoaXMuaG9zdCwgdGhpcy5lbGVtZW50UmVmLm5hdGl2ZUVsZW1lbnQpO1xyXG4gIH1cclxufVxyXG4iXX0=