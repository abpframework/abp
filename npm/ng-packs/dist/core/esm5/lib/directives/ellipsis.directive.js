/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Directive, ElementRef, HostBinding, Input } from '@angular/core';
var EllipsisDirective = /** @class */ (function () {
    function EllipsisDirective(cdRef, elRef) {
        this.cdRef = cdRef;
        this.elRef = elRef;
        this.enabled = true;
    }
    Object.defineProperty(EllipsisDirective.prototype, "maxWidth", {
        get: /**
         * @return {?}
         */
        function () {
            return this.witdh || '180px';
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    EllipsisDirective.prototype.ngAfterContentInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        setTimeout((/**
         * @return {?}
         */
        function () {
            /** @type {?} */
            var title = _this.title;
            _this.title = title || ((/** @type {?} */ (_this.elRef.nativeElement))).innerText;
            if (_this.title !== title) {
                _this.cdRef.detectChanges();
            }
        }), 0);
    };
    EllipsisDirective.decorators = [
        { type: Directive, args: [{
                    selector: '[abpEllipsis]',
                },] }
    ];
    /** @nocollapse */
    EllipsisDirective.ctorParameters = function () { return [
        { type: ChangeDetectorRef },
        { type: ElementRef }
    ]; };
    EllipsisDirective.propDecorators = {
        witdh: [{ type: Input, args: ['abpEllipsis',] }],
        title: [{ type: HostBinding, args: ['title',] }, { type: Input }],
        enabled: [{ type: HostBinding, args: ['class.abp-ellipsis',] }],
        maxWidth: [{ type: HostBinding, args: ['style.max-width',] }]
    };
    return EllipsisDirective;
}());
export { EllipsisDirective };
if (false) {
    /** @type {?} */
    EllipsisDirective.prototype.witdh;
    /** @type {?} */
    EllipsisDirective.prototype.title;
    /** @type {?} */
    EllipsisDirective.prototype.enabled;
    /**
     * @type {?}
     * @private
     */
    EllipsisDirective.prototype.cdRef;
    /**
     * @type {?}
     * @private
     */
    EllipsisDirective.prototype.elRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZWxsaXBzaXMuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZWxsaXBzaXMuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQW9CLGlCQUFpQixFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsV0FBVyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUvRztJQW1CRSwyQkFBb0IsS0FBd0IsRUFBVSxLQUFpQjtRQUFuRCxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQUFVLFVBQUssR0FBTCxLQUFLLENBQVk7UUFQdkUsWUFBTyxHQUFHLElBQUksQ0FBQztJQU8yRCxDQUFDO0lBTDNFLHNCQUNJLHVDQUFROzs7O1FBRFo7WUFFRSxPQUFPLElBQUksQ0FBQyxLQUFLLElBQUksT0FBTyxDQUFDO1FBQy9CLENBQUM7OztPQUFBOzs7O0lBSUQsOENBQWtCOzs7SUFBbEI7UUFBQSxpQkFTQztRQVJDLFVBQVU7OztRQUFDOztnQkFDSCxLQUFLLEdBQUcsS0FBSSxDQUFDLEtBQUs7WUFDeEIsS0FBSSxDQUFDLEtBQUssR0FBRyxLQUFLLElBQUksQ0FBQyxtQkFBQSxLQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBZSxDQUFDLENBQUMsU0FBUyxDQUFDO1lBRTFFLElBQUksS0FBSSxDQUFDLEtBQUssS0FBSyxLQUFLLEVBQUU7Z0JBQ3hCLEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLENBQUM7YUFDNUI7UUFDSCxDQUFDLEdBQUUsQ0FBQyxDQUFDLENBQUM7SUFDUixDQUFDOztnQkE5QkYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxlQUFlO2lCQUMxQjs7OztnQkFKMEIsaUJBQWlCO2dCQUFhLFVBQVU7Ozt3QkFNaEUsS0FBSyxTQUFDLGFBQWE7d0JBR25CLFdBQVcsU0FBQyxPQUFPLGNBQ25CLEtBQUs7MEJBR0wsV0FBVyxTQUFDLG9CQUFvQjsyQkFHaEMsV0FBVyxTQUFDLGlCQUFpQjs7SUFpQmhDLHdCQUFDO0NBQUEsQUEvQkQsSUErQkM7U0E1QlksaUJBQWlCOzs7SUFDNUIsa0NBQ2M7O0lBRWQsa0NBRWM7O0lBRWQsb0NBQ2U7Ozs7O0lBT0gsa0NBQWdDOzs7OztJQUFFLGtDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFmdGVyQ29udGVudEluaXQsIENoYW5nZURldGVjdG9yUmVmLCBEaXJlY3RpdmUsIEVsZW1lbnRSZWYsIEhvc3RCaW5kaW5nLCBJbnB1dCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5ARGlyZWN0aXZlKHtcbiAgc2VsZWN0b3I6ICdbYWJwRWxsaXBzaXNdJyxcbn0pXG5leHBvcnQgY2xhc3MgRWxsaXBzaXNEaXJlY3RpdmUgaW1wbGVtZW50cyBBZnRlckNvbnRlbnRJbml0IHtcbiAgQElucHV0KCdhYnBFbGxpcHNpcycpXG4gIHdpdGRoOiBzdHJpbmc7XG5cbiAgQEhvc3RCaW5kaW5nKCd0aXRsZScpXG4gIEBJbnB1dCgpXG4gIHRpdGxlOiBzdHJpbmc7XG5cbiAgQEhvc3RCaW5kaW5nKCdjbGFzcy5hYnAtZWxsaXBzaXMnKVxuICBlbmFibGVkID0gdHJ1ZTtcblxuICBASG9zdEJpbmRpbmcoJ3N0eWxlLm1heC13aWR0aCcpXG4gIGdldCBtYXhXaWR0aCgpIHtcbiAgICByZXR1cm4gdGhpcy53aXRkaCB8fCAnMTgwcHgnO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWYsIHByaXZhdGUgZWxSZWY6IEVsZW1lbnRSZWYpIHt9XG5cbiAgbmdBZnRlckNvbnRlbnRJbml0KCkge1xuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgY29uc3QgdGl0bGUgPSB0aGlzLnRpdGxlO1xuICAgICAgdGhpcy50aXRsZSA9IHRpdGxlIHx8ICh0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQgYXMgSFRNTEVsZW1lbnQpLmlubmVyVGV4dDtcblxuICAgICAgaWYgKHRoaXMudGl0bGUgIT09IHRpdGxlKSB7XG4gICAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgICAgfVxuICAgIH0sIDApO1xuICB9XG59XG4iXX0=