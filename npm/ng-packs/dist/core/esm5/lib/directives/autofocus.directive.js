/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
                    selector: '[autofocus]',
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0b2ZvY3VzLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2F1dG9mb2N1cy5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFFLEtBQUssRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFFNUU7SUFPRSw0QkFBb0IsS0FBaUI7UUFBakIsVUFBSyxHQUFMLEtBQUssQ0FBWTtRQUZyQyxVQUFLLEdBQVcsQ0FBQyxDQUFDO0lBRXNCLENBQUM7Ozs7SUFFekMsNENBQWU7OztJQUFmO1FBQUEsaUJBRUM7UUFEQyxVQUFVOzs7UUFBQyxjQUFNLE9BQUEsS0FBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLENBQUMsS0FBSyxFQUFFLEVBQWhDLENBQWdDLEdBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO0lBQ2pFLENBQUM7O2dCQVhGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsYUFBYTtpQkFDeEI7Ozs7Z0JBSm1CLFVBQVU7Ozt3QkFNM0IsS0FBSyxTQUFDLFdBQVc7O0lBUXBCLHlCQUFDO0NBQUEsQUFaRCxJQVlDO1NBVFksa0JBQWtCOzs7SUFDN0IsbUNBQ2tCOzs7OztJQUVOLG1DQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgRWxlbWVudFJlZiwgSW5wdXQsIEFmdGVyVmlld0luaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2F1dG9mb2N1c10nLFxufSlcbmV4cG9ydCBjbGFzcyBBdXRvZm9jdXNEaXJlY3RpdmUgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0IHtcbiAgQElucHV0KCdhdXRvZm9jdXMnKVxuICBkZWxheTogbnVtYmVyID0gMDtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGVsUmVmOiBFbGVtZW50UmVmKSB7fVxuXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpOiB2b2lkIHtcbiAgICBzZXRUaW1lb3V0KCgpID0+IHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudC5mb2N1cygpLCB0aGlzLmRlbGF5KTtcbiAgfVxufVxuIl19