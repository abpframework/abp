/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, Input } from '@angular/core';
export class AutofocusDirective {
    /**
     * @param {?} elRef
     */
    constructor(elRef) {
        this.elRef = elRef;
        this.delay = 0;
    }
    /**
     * @return {?}
     */
    ngAfterViewInit() {
        setTimeout((/**
         * @return {?}
         */
        () => this.elRef.nativeElement.focus()), this.delay);
    }
}
AutofocusDirective.decorators = [
    { type: Directive, args: [{
                selector: '[autofocus]',
            },] }
];
/** @nocollapse */
AutofocusDirective.ctorParameters = () => [
    { type: ElementRef }
];
AutofocusDirective.propDecorators = {
    delay: [{ type: Input, args: ['autofocus',] }]
};
if (false) {
    /** @type {?} */
    AutofocusDirective.prototype.delay;
    /**
     * @type {?}
     * @private
     */
    AutofocusDirective.prototype.elRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0b2ZvY3VzLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2F1dG9mb2N1cy5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFFLEtBQUssRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFLNUUsTUFBTSxPQUFPLGtCQUFrQjs7OztJQUk3QixZQUFvQixLQUFpQjtRQUFqQixVQUFLLEdBQUwsS0FBSyxDQUFZO1FBRnJDLFVBQUssR0FBVyxDQUFDLENBQUM7SUFFc0IsQ0FBQzs7OztJQUV6QyxlQUFlO1FBQ2IsVUFBVTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLENBQUMsS0FBSyxFQUFFLEdBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO0lBQ2pFLENBQUM7OztZQVhGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsYUFBYTthQUN4Qjs7OztZQUptQixVQUFVOzs7b0JBTTNCLEtBQUssU0FBQyxXQUFXOzs7O0lBQWxCLG1DQUNrQjs7Ozs7SUFFTixtQ0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIEVsZW1lbnRSZWYsIElucHV0LCBBZnRlclZpZXdJbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ1thdXRvZm9jdXNdJyxcbn0pXG5leHBvcnQgY2xhc3MgQXV0b2ZvY3VzRGlyZWN0aXZlIGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCB7XG4gIEBJbnB1dCgnYXV0b2ZvY3VzJylcbiAgZGVsYXk6IG51bWJlciA9IDA7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBlbFJlZjogRWxlbWVudFJlZikge31cblxuICBuZ0FmdGVyVmlld0luaXQoKTogdm9pZCB7XG4gICAgc2V0VGltZW91dCgoKSA9PiB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQuZm9jdXMoKSwgdGhpcy5kZWxheSk7XG4gIH1cbn1cbiJdfQ==