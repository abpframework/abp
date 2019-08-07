/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Directive, ElementRef, HostBinding, Input } from '@angular/core';
export class EllipsisDirective {
    /**
     * @param {?} cdRef
     * @param {?} elRef
     */
    constructor(cdRef, elRef) {
        this.cdRef = cdRef;
        this.elRef = elRef;
        this.enabled = true;
    }
    /**
     * @return {?}
     */
    get class() {
        return this.enabled;
    }
    /**
     * @return {?}
     */
    get maxWidth() {
        return this.enabled ? this.witdh || '180px' : undefined;
    }
    /**
     * @return {?}
     */
    ngAfterContentInit() {
        setTimeout((/**
         * @return {?}
         */
        () => {
            /** @type {?} */
            const title = this.title;
            this.title = title || ((/** @type {?} */ (this.elRef.nativeElement))).innerText;
            if (this.title !== title) {
                this.cdRef.detectChanges();
            }
        }), 0);
    }
}
EllipsisDirective.decorators = [
    { type: Directive, args: [{
                selector: '[abpEllipsis]',
            },] }
];
/** @nocollapse */
EllipsisDirective.ctorParameters = () => [
    { type: ChangeDetectorRef },
    { type: ElementRef }
];
EllipsisDirective.propDecorators = {
    witdh: [{ type: Input, args: ['abpEllipsis',] }],
    title: [{ type: HostBinding, args: ['title',] }, { type: Input }],
    enabled: [{ type: Input, args: ['abpEllipsisEnabled',] }],
    class: [{ type: HostBinding, args: ['class.abp-ellipsis',] }],
    maxWidth: [{ type: HostBinding, args: ['style.max-width',] }]
};
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZWxsaXBzaXMuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZWxsaXBzaXMuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQW9CLGlCQUFpQixFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsV0FBVyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUsvRyxNQUFNLE9BQU8saUJBQWlCOzs7OztJQXFCNUIsWUFBb0IsS0FBd0IsRUFBVSxLQUFpQjtRQUFuRCxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQUFVLFVBQUssR0FBTCxLQUFLLENBQVk7UUFadkUsWUFBTyxHQUFHLElBQUksQ0FBQztJQVkyRCxDQUFDOzs7O0lBVjNFLElBQ0ksS0FBSztRQUNQLE9BQU8sSUFBSSxDQUFDLE9BQU8sQ0FBQztJQUN0QixDQUFDOzs7O0lBRUQsSUFDSSxRQUFRO1FBQ1YsT0FBTyxJQUFJLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxJQUFJLE9BQU8sQ0FBQyxDQUFDLENBQUMsU0FBUyxDQUFDO0lBQzFELENBQUM7Ozs7SUFJRCxrQkFBa0I7UUFDaEIsVUFBVTs7O1FBQUMsR0FBRyxFQUFFOztrQkFDUixLQUFLLEdBQUcsSUFBSSxDQUFDLEtBQUs7WUFDeEIsSUFBSSxDQUFDLEtBQUssR0FBRyxLQUFLLElBQUksQ0FBQyxtQkFBQSxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBZSxDQUFDLENBQUMsU0FBUyxDQUFDO1lBRTFFLElBQUksSUFBSSxDQUFDLEtBQUssS0FBSyxLQUFLLEVBQUU7Z0JBQ3hCLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLENBQUM7YUFDNUI7UUFDSCxDQUFDLEdBQUUsQ0FBQyxDQUFDLENBQUM7SUFDUixDQUFDOzs7WUFuQ0YsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxlQUFlO2FBQzFCOzs7O1lBSjBCLGlCQUFpQjtZQUFhLFVBQVU7OztvQkFNaEUsS0FBSyxTQUFDLGFBQWE7b0JBR25CLFdBQVcsU0FBQyxPQUFPLGNBQ25CLEtBQUs7c0JBR0wsS0FBSyxTQUFDLG9CQUFvQjtvQkFHMUIsV0FBVyxTQUFDLG9CQUFvQjt1QkFLaEMsV0FBVyxTQUFDLGlCQUFpQjs7OztJQWY5QixrQ0FDYzs7SUFFZCxrQ0FFYzs7SUFFZCxvQ0FDZTs7Ozs7SUFZSCxrQ0FBZ0M7Ozs7O0lBQUUsa0NBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWZ0ZXJDb250ZW50SW5pdCwgQ2hhbmdlRGV0ZWN0b3JSZWYsIERpcmVjdGl2ZSwgRWxlbWVudFJlZiwgSG9zdEJpbmRpbmcsIElucHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ1thYnBFbGxpcHNpc10nLFxufSlcbmV4cG9ydCBjbGFzcyBFbGxpcHNpc0RpcmVjdGl2ZSBpbXBsZW1lbnRzIEFmdGVyQ29udGVudEluaXQge1xuICBASW5wdXQoJ2FicEVsbGlwc2lzJylcbiAgd2l0ZGg6IHN0cmluZztcblxuICBASG9zdEJpbmRpbmcoJ3RpdGxlJylcbiAgQElucHV0KClcbiAgdGl0bGU6IHN0cmluZztcblxuICBASW5wdXQoJ2FicEVsbGlwc2lzRW5hYmxlZCcpXG4gIGVuYWJsZWQgPSB0cnVlO1xuXG4gIEBIb3N0QmluZGluZygnY2xhc3MuYWJwLWVsbGlwc2lzJylcbiAgZ2V0IGNsYXNzKCkge1xuICAgIHJldHVybiB0aGlzLmVuYWJsZWQ7XG4gIH1cblxuICBASG9zdEJpbmRpbmcoJ3N0eWxlLm1heC13aWR0aCcpXG4gIGdldCBtYXhXaWR0aCgpIHtcbiAgICByZXR1cm4gdGhpcy5lbmFibGVkID8gdGhpcy53aXRkaCB8fCAnMTgwcHgnIDogdW5kZWZpbmVkO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWYsIHByaXZhdGUgZWxSZWY6IEVsZW1lbnRSZWYpIHt9XG5cbiAgbmdBZnRlckNvbnRlbnRJbml0KCkge1xuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgY29uc3QgdGl0bGUgPSB0aGlzLnRpdGxlO1xuICAgICAgdGhpcy50aXRsZSA9IHRpdGxlIHx8ICh0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQgYXMgSFRNTEVsZW1lbnQpLmlubmVyVGV4dDtcblxuICAgICAgaWYgKHRoaXMudGl0bGUgIT09IHRpdGxlKSB7XG4gICAgICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICAgICAgfVxuICAgIH0sIDApO1xuICB9XG59XG4iXX0=