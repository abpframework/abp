/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/autofocus.directive.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, Input } from '@angular/core';
var AutofocusDirective = /** @class */ (function () {
    function AutofocusDirective(elRef) {
        this.elRef = elRef;
        this.delay = 0;
    }
    /**
     * @return {?}
     */
    AutofocusDirective.prototype.ngAfterViewInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        setTimeout((/**
         * @return {?}
         */
        function () { return _this.elRef.nativeElement.focus(); }), this.delay);
    };
    AutofocusDirective.decorators = [
        { type: Directive, args: [{
                    // tslint:disable-next-line: directive-selector
                    selector: '[autofocus]'
                },] }
    ];
    /** @nocollapse */
    AutofocusDirective.ctorParameters = function () { return [
        { type: ElementRef }
    ]; };
    AutofocusDirective.propDecorators = {
        delay: [{ type: Input, args: ['autofocus',] }]
    };
    return AutofocusDirective;
}());
export { AutofocusDirective };
if (false) {
    /** @type {?} */
    AutofocusDirective.prototype.delay;
    /**
     * @type {?}
     * @private
     */
    AutofocusDirective.prototype.elRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0b2ZvY3VzLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2F1dG9mb2N1cy5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxLQUFLLEVBQWlCLE1BQU0sZUFBZSxDQUFDO0FBRTVFO0lBUUUsNEJBQW9CLEtBQWlCO1FBQWpCLFVBQUssR0FBTCxLQUFLLENBQVk7UUFGckMsVUFBSyxHQUFHLENBQUMsQ0FBQztJQUU4QixDQUFDOzs7O0lBRXpDLDRDQUFlOzs7SUFBZjtRQUFBLGlCQUVDO1FBREMsVUFBVTs7O1FBQUMsY0FBTSxPQUFBLEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxDQUFDLEtBQUssRUFBRSxFQUFoQyxDQUFnQyxHQUFFLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUNqRSxDQUFDOztnQkFaRixTQUFTLFNBQUM7O29CQUVULFFBQVEsRUFBRSxhQUFhO2lCQUN4Qjs7OztnQkFMbUIsVUFBVTs7O3dCQU8zQixLQUFLLFNBQUMsV0FBVzs7SUFRcEIseUJBQUM7Q0FBQSxBQWJELElBYUM7U0FUWSxrQkFBa0I7OztJQUM3QixtQ0FDVTs7Ozs7SUFFRSxtQ0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIEVsZW1lbnRSZWYsIElucHV0LCBBZnRlclZpZXdJbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBEaXJlY3RpdmUoe1xuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGRpcmVjdGl2ZS1zZWxlY3RvclxuICBzZWxlY3RvcjogJ1thdXRvZm9jdXNdJ1xufSlcbmV4cG9ydCBjbGFzcyBBdXRvZm9jdXNEaXJlY3RpdmUgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0IHtcbiAgQElucHV0KCdhdXRvZm9jdXMnKVxuICBkZWxheSA9IDA7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBlbFJlZjogRWxlbWVudFJlZikge31cblxuICBuZ0FmdGVyVmlld0luaXQoKTogdm9pZCB7XG4gICAgc2V0VGltZW91dCgoKSA9PiB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQuZm9jdXMoKSwgdGhpcy5kZWxheSk7XG4gIH1cbn1cbiJdfQ==