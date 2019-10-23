/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
export class ErrorComponent {
    constructor() {
        this.title = 'Oops!';
        this.details = 'Sorry, an error has occured.';
    }
    /**
     * @return {?}
     */
    destroy() {
        this.renderer.removeChild(this.host, this.elementRef.nativeElement);
    }
}
ErrorComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-error',
                template: "<div class=\"error\">\r\n  <button id=\"abp-close-button mr-3\" type=\"button\" class=\"close\" (click)=\"destroy()\">\r\n    <span aria-hidden=\"true\">&times;</span>\r\n  </button>\r\n  <div class=\"row centered\">\r\n    <div class=\"col-md-12\">\r\n      <div class=\"error-template\">\r\n        <h1>\r\n          {{ title | abpLocalization }}\r\n        </h1>\r\n        <div class=\"error-details\">\r\n          {{ details | abpLocalization }}\r\n        </div>\r\n        <div class=\"error-actions\">\r\n          <a (click)=\"destroy()\" routerLink=\"/\" class=\"btn btn-primary btn-md mt-2\"\r\n            ><span class=\"glyphicon glyphicon-home\"></span>\r\n            {{ { key: '::Menu:Home', defaultValue: 'Home' } | abpLocalization }}\r\n          </a>\r\n        </div>\r\n      </div>\r\n    </div>\r\n  </div>\r\n</div>\r\n",
                styles: [".error{position:fixed;top:0;background-color:#fff;width:100vw;height:100vh;z-index:999999}.centered{position:fixed;top:50%;left:50%;transform:translate(-50%,-50%)}"]
            }] }
];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9lcnJvci9lcnJvci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQXlCLE1BQU0sZUFBZSxDQUFDO0FBUWpFLE1BQU0sT0FBTyxjQUFjO0lBTDNCO1FBTUUsVUFBSyxHQUE0QyxPQUFPLENBQUM7UUFFekQsWUFBTyxHQUE0Qyw4QkFBOEIsQ0FBQztJQVdwRixDQUFDOzs7O0lBSEMsT0FBTztRQUNMLElBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUUsSUFBSSxDQUFDLFVBQVUsQ0FBQyxhQUFhLENBQUMsQ0FBQztJQUN0RSxDQUFDOzs7WUFsQkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxXQUFXO2dCQUNyQix3MUJBQXFDOzthQUV0Qzs7OztJQUVDLCtCQUF5RDs7SUFFekQsaUNBQWtGOztJQUVsRixrQ0FBb0I7O0lBRXBCLG9DQUF1Qjs7SUFFdkIsOEJBQVUiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIFJlbmRlcmVyMiwgRWxlbWVudFJlZiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBDb25maWcgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtZXJyb3InLFxyXG4gIHRlbXBsYXRlVXJsOiAnLi9lcnJvci5jb21wb25lbnQuaHRtbCcsXHJcbiAgc3R5bGVVcmxzOiBbJ2Vycm9yLmNvbXBvbmVudC5zY3NzJ10sXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBFcnJvckNvbXBvbmVudCB7XHJcbiAgdGl0bGU6IHN0cmluZyB8IENvbmZpZy5Mb2NhbGl6YXRpb25XaXRoRGVmYXVsdCA9ICdPb3BzISc7XHJcblxyXG4gIGRldGFpbHM6IHN0cmluZyB8IENvbmZpZy5Mb2NhbGl6YXRpb25XaXRoRGVmYXVsdCA9ICdTb3JyeSwgYW4gZXJyb3IgaGFzIG9jY3VyZWQuJztcclxuXHJcbiAgcmVuZGVyZXI6IFJlbmRlcmVyMjtcclxuXHJcbiAgZWxlbWVudFJlZjogRWxlbWVudFJlZjtcclxuXHJcbiAgaG9zdDogYW55O1xyXG5cclxuICBkZXN0cm95KCkge1xyXG4gICAgdGhpcy5yZW5kZXJlci5yZW1vdmVDaGlsZCh0aGlzLmhvc3QsIHRoaXMuZWxlbWVudFJlZi5uYXRpdmVFbGVtZW50KTtcclxuICB9XHJcbn1cclxuIl19