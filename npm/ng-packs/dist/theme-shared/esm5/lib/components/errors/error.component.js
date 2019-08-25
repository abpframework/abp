/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                    template: "\n    <div class=\"error\">\n      <button id=\"abp-close-button mr-2\" type=\"button\" class=\"close\" (click)=\"destroy()\">\n        <span aria-hidden=\"true\">&times;</span>\n      </button>\n      <div class=\"row centered\">\n        <div class=\"col-md-12\">\n          <div class=\"error-template\">\n            <h1>\n              {{ title | abpLocalization }}\n            </h1>\n            <div class=\"error-details\">\n              {{ details | abpLocalization }}\n            </div>\n            <div class=\"error-actions\">\n              <a (click)=\"destroy()\" routerLink=\"/\" class=\"btn btn-primary btn-md mt-2\"\n                ><span class=\"glyphicon glyphicon-home\"></span> {{ '::Menu:Home' | abpLocalization }}\n              </a>\n            </div>\n          </div>\n        </div>\n      </div>\n    </div>\n  ",
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZXJyb3IuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9lcnJvcnMvZXJyb3IuY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUF5QixNQUFNLGVBQWUsQ0FBQztBQUVqRTtJQUFBO1FBNkJFLFVBQUssR0FBRyxPQUFPLENBQUM7UUFFaEIsWUFBTyxHQUFHLDhCQUE4QixDQUFDO0lBVzNDLENBQUM7Ozs7SUFIQyxnQ0FBTzs7O0lBQVA7UUFDRSxJQUFJLENBQUMsUUFBUSxDQUFDLFdBQVcsQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFFLElBQUksQ0FBQyxVQUFVLENBQUMsYUFBYSxDQUFDLENBQUM7SUFDdEUsQ0FBQzs7Z0JBekNGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsV0FBVztvQkFDckIsUUFBUSxFQUFFLGcxQkF1QlQ7O2lCQUVGOztJQWVELHFCQUFDO0NBQUEsQUExQ0QsSUEwQ0M7U0FkWSxjQUFjOzs7SUFDekIsK0JBQWdCOztJQUVoQixpQ0FBeUM7O0lBRXpDLGtDQUFvQjs7SUFFcEIsb0NBQXVCOztJQUV2Qiw4QkFBVSIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgUmVuZGVyZXIyLCBFbGVtZW50UmVmIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1lcnJvcicsXG4gIHRlbXBsYXRlOiBgXG4gICAgPGRpdiBjbGFzcz1cImVycm9yXCI+XG4gICAgICA8YnV0dG9uIGlkPVwiYWJwLWNsb3NlLWJ1dHRvbiBtci0yXCIgdHlwZT1cImJ1dHRvblwiIGNsYXNzPVwiY2xvc2VcIiAoY2xpY2spPVwiZGVzdHJveSgpXCI+XG4gICAgICAgIDxzcGFuIGFyaWEtaGlkZGVuPVwidHJ1ZVwiPiZ0aW1lczs8L3NwYW4+XG4gICAgICA8L2J1dHRvbj5cbiAgICAgIDxkaXYgY2xhc3M9XCJyb3cgY2VudGVyZWRcIj5cbiAgICAgICAgPGRpdiBjbGFzcz1cImNvbC1tZC0xMlwiPlxuICAgICAgICAgIDxkaXYgY2xhc3M9XCJlcnJvci10ZW1wbGF0ZVwiPlxuICAgICAgICAgICAgPGgxPlxuICAgICAgICAgICAgICB7eyB0aXRsZSB8IGFicExvY2FsaXphdGlvbiB9fVxuICAgICAgICAgICAgPC9oMT5cbiAgICAgICAgICAgIDxkaXYgY2xhc3M9XCJlcnJvci1kZXRhaWxzXCI+XG4gICAgICAgICAgICAgIHt7IGRldGFpbHMgfCBhYnBMb2NhbGl6YXRpb24gfX1cbiAgICAgICAgICAgIDwvZGl2PlxuICAgICAgICAgICAgPGRpdiBjbGFzcz1cImVycm9yLWFjdGlvbnNcIj5cbiAgICAgICAgICAgICAgPGEgKGNsaWNrKT1cImRlc3Ryb3koKVwiIHJvdXRlckxpbms9XCIvXCIgY2xhc3M9XCJidG4gYnRuLXByaW1hcnkgYnRuLW1kIG10LTJcIlxuICAgICAgICAgICAgICAgID48c3BhbiBjbGFzcz1cImdseXBoaWNvbiBnbHlwaGljb24taG9tZVwiPjwvc3Bhbj4ge3sgJzo6TWVudTpIb21lJyB8IGFicExvY2FsaXphdGlvbiB9fVxuICAgICAgICAgICAgICA8L2E+XG4gICAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgICA8L2Rpdj5cbiAgICAgICAgPC9kaXY+XG4gICAgICA8L2Rpdj5cbiAgICA8L2Rpdj5cbiAgYCxcbiAgc3R5bGVVcmxzOiBbJ2Vycm9yLmNvbXBvbmVudC5zY3NzJ10sXG59KVxuZXhwb3J0IGNsYXNzIEVycm9yQ29tcG9uZW50IHtcbiAgdGl0bGUgPSAnT29wcyEnO1xuXG4gIGRldGFpbHMgPSAnU29ycnksIGFuIGVycm9yIGhhcyBvY2N1cmVkLic7XG5cbiAgcmVuZGVyZXI6IFJlbmRlcmVyMjtcblxuICBlbGVtZW50UmVmOiBFbGVtZW50UmVmO1xuXG4gIGhvc3Q6IGFueTtcblxuICBkZXN0cm95KCkge1xuICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlQ2hpbGQodGhpcy5ob3N0LCB0aGlzLmVsZW1lbnRSZWYubmF0aXZlRWxlbWVudCk7XG4gIH1cbn1cbiJdfQ==