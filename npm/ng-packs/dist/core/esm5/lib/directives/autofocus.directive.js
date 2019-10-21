/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0b2ZvY3VzLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2F1dG9mb2N1cy5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFFLEtBQUssRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFFNUU7SUFRRSw0QkFBb0IsS0FBaUI7UUFBakIsVUFBSyxHQUFMLEtBQUssQ0FBWTtRQUZyQyxVQUFLLEdBQUcsQ0FBQyxDQUFDO0lBRThCLENBQUM7Ozs7SUFFekMsNENBQWU7OztJQUFmO1FBQUEsaUJBRUM7UUFEQyxVQUFVOzs7UUFBQyxjQUFNLE9BQUEsS0FBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLENBQUMsS0FBSyxFQUFFLEVBQWhDLENBQWdDLEdBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO0lBQ2pFLENBQUM7O2dCQVpGLFNBQVMsU0FBQzs7b0JBRVQsUUFBUSxFQUFFLGFBQWE7aUJBQ3hCOzs7O2dCQUxtQixVQUFVOzs7d0JBTzNCLEtBQUssU0FBQyxXQUFXOztJQVFwQix5QkFBQztDQUFBLEFBYkQsSUFhQztTQVRZLGtCQUFrQjs7O0lBQzdCLG1DQUNVOzs7OztJQUVFLG1DQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IERpcmVjdGl2ZSwgRWxlbWVudFJlZiwgSW5wdXQsIEFmdGVyVmlld0luaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQERpcmVjdGl2ZSh7XG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogZGlyZWN0aXZlLXNlbGVjdG9yXG4gIHNlbGVjdG9yOiAnW2F1dG9mb2N1c10nXG59KVxuZXhwb3J0IGNsYXNzIEF1dG9mb2N1c0RpcmVjdGl2ZSBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQge1xuICBASW5wdXQoJ2F1dG9mb2N1cycpXG4gIGRlbGF5ID0gMDtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGVsUmVmOiBFbGVtZW50UmVmKSB7fVxuXG4gIG5nQWZ0ZXJWaWV3SW5pdCgpOiB2b2lkIHtcbiAgICBzZXRUaW1lb3V0KCgpID0+IHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudC5mb2N1cygpLCB0aGlzLmRlbGF5KTtcbiAgfVxufVxuIl19