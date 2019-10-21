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
                template: "<div class=\"error\">\n  <button id=\"abp-close-button mr-3\" type=\"button\" class=\"close\" (click)=\"destroy()\">\n    <span aria-hidden=\"true\">&times;</span>\n  </button>\n  <div class=\"row centered\">\n    <div class=\"col-md-12\">\n      <div class=\"error-template\">\n        <h1>\n          {{ title | abpLocalization }}\n        </h1>\n        <div class=\"error-details\">\n          {{ details | abpLocalization }}\n        </div>\n        <div class=\"error-actions\">\n          <a (click)=\"destroy()\" routerLink=\"/\" class=\"btn btn-primary btn-md mt-2\"\n            ><span class=\"glyphicon glyphicon-home\"></span>\n            {{ { key: '::Menu:Home', defaultValue: 'Home' } | abpLocalization }}\n          </a>\n        </div>\n      </div>\n    </div>\n  </div>\n</div>\n",
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9lcnJvci9lcnJvci5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQXlCLE1BQU0sZUFBZSxDQUFDO0FBUWpFLE1BQU0sT0FBTyxjQUFjO0lBTDNCO1FBTUUsVUFBSyxHQUE0QyxPQUFPLENBQUM7UUFFekQsWUFBTyxHQUE0Qyw4QkFBOEIsQ0FBQztJQVdwRixDQUFDOzs7O0lBSEMsT0FBTztRQUNMLElBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUUsSUFBSSxDQUFDLFVBQVUsQ0FBQyxhQUFhLENBQUMsQ0FBQztJQUN0RSxDQUFDOzs7WUFsQkYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxXQUFXO2dCQUNyQiwweUJBQXFDOzthQUV0Qzs7OztJQUVDLCtCQUF5RDs7SUFFekQsaUNBQWtGOztJQUVsRixrQ0FBb0I7O0lBRXBCLG9DQUF1Qjs7SUFFdkIsOEJBQVUiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIFJlbmRlcmVyMiwgRWxlbWVudFJlZiB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQ29uZmlnIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWVycm9yJyxcbiAgdGVtcGxhdGVVcmw6ICcuL2Vycm9yLmNvbXBvbmVudC5odG1sJyxcbiAgc3R5bGVVcmxzOiBbJ2Vycm9yLmNvbXBvbmVudC5zY3NzJ10sXG59KVxuZXhwb3J0IGNsYXNzIEVycm9yQ29tcG9uZW50IHtcbiAgdGl0bGU6IHN0cmluZyB8IENvbmZpZy5Mb2NhbGl6YXRpb25XaXRoRGVmYXVsdCA9ICdPb3BzISc7XG5cbiAgZGV0YWlsczogc3RyaW5nIHwgQ29uZmlnLkxvY2FsaXphdGlvbldpdGhEZWZhdWx0ID0gJ1NvcnJ5LCBhbiBlcnJvciBoYXMgb2NjdXJlZC4nO1xuXG4gIHJlbmRlcmVyOiBSZW5kZXJlcjI7XG5cbiAgZWxlbWVudFJlZjogRWxlbWVudFJlZjtcblxuICBob3N0OiBhbnk7XG5cbiAgZGVzdHJveSgpIHtcbiAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNoaWxkKHRoaXMuaG9zdCwgdGhpcy5lbGVtZW50UmVmLm5hdGl2ZUVsZW1lbnQpO1xuICB9XG59XG4iXX0=