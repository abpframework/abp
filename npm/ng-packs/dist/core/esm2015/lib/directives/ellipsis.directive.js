/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
    get inlineClass() {
        return this.enabled && this.width;
    }
    /**
     * @return {?}
     */
    get class() {
        return this.enabled && !this.width;
    }
    /**
     * @return {?}
     */
    get maxWidth() {
        return this.enabled && this.width ? this.width || '170px' : undefined;
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
    width: [{ type: Input, args: ['abpEllipsis',] }],
    title: [{ type: HostBinding, args: ['title',] }, { type: Input }],
    enabled: [{ type: Input, args: ['abpEllipsisEnabled',] }],
    inlineClass: [{ type: HostBinding, args: ['class.abp-ellipsis-inline',] }],
    class: [{ type: HostBinding, args: ['class.abp-ellipsis',] }],
    maxWidth: [{ type: HostBinding, args: ['style.max-width',] }]
};
if (false) {
    /** @type {?} */
    EllipsisDirective.prototype.width;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZWxsaXBzaXMuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZWxsaXBzaXMuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQW9CLGlCQUFpQixFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsV0FBVyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUsvRyxNQUFNLE9BQU8saUJBQWlCOzs7OztJQTBCNUIsWUFBb0IsS0FBd0IsRUFBVSxLQUFpQjtRQUFuRCxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQUFVLFVBQUssR0FBTCxLQUFLLENBQVk7UUFqQnZFLFlBQU8sR0FBRyxJQUFJLENBQUM7SUFpQjJELENBQUM7Ozs7SUFmM0UsSUFDSSxXQUFXO1FBQ2IsT0FBTyxJQUFJLENBQUMsT0FBTyxJQUFJLElBQUksQ0FBQyxLQUFLLENBQUM7SUFDcEMsQ0FBQzs7OztJQUVELElBQ0ksS0FBSztRQUNQLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUM7SUFDckMsQ0FBQzs7OztJQUVELElBQ0ksUUFBUTtRQUNWLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBSSxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxJQUFJLE9BQU8sQ0FBQyxDQUFDLENBQUMsU0FBUyxDQUFDO0lBQ3hFLENBQUM7Ozs7SUFJRCxrQkFBa0I7UUFDaEIsVUFBVTs7O1FBQUMsR0FBRyxFQUFFOztrQkFDUixLQUFLLEdBQUcsSUFBSSxDQUFDLEtBQUs7WUFDeEIsSUFBSSxDQUFDLEtBQUssR0FBRyxLQUFLLElBQUksQ0FBQyxtQkFBQSxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBZSxDQUFDLENBQUMsU0FBUyxDQUFDO1lBRTFFLElBQUksSUFBSSxDQUFDLEtBQUssS0FBSyxLQUFLLEVBQUU7Z0JBQ3hCLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLENBQUM7YUFDNUI7UUFDSCxDQUFDLEdBQUUsQ0FBQyxDQUFDLENBQUM7SUFDUixDQUFDOzs7WUF4Q0YsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxlQUFlO2FBQzFCOzs7O1lBSjBCLGlCQUFpQjtZQUFhLFVBQVU7OztvQkFNaEUsS0FBSyxTQUFDLGFBQWE7b0JBR25CLFdBQVcsU0FBQyxPQUFPLGNBQ25CLEtBQUs7c0JBR0wsS0FBSyxTQUFDLG9CQUFvQjswQkFHMUIsV0FBVyxTQUFDLDJCQUEyQjtvQkFLdkMsV0FBVyxTQUFDLG9CQUFvQjt1QkFLaEMsV0FBVyxTQUFDLGlCQUFpQjs7OztJQXBCOUIsa0NBQ2M7O0lBRWQsa0NBRWM7O0lBRWQsb0NBQ2U7Ozs7O0lBaUJILGtDQUFnQzs7Ozs7SUFBRSxrQ0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBZnRlckNvbnRlbnRJbml0LCBDaGFuZ2VEZXRlY3RvclJlZiwgRGlyZWN0aXZlLCBFbGVtZW50UmVmLCBIb3N0QmluZGluZywgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbkBEaXJlY3RpdmUoe1xyXG4gIHNlbGVjdG9yOiAnW2FicEVsbGlwc2lzXScsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBFbGxpcHNpc0RpcmVjdGl2ZSBpbXBsZW1lbnRzIEFmdGVyQ29udGVudEluaXQge1xyXG4gIEBJbnB1dCgnYWJwRWxsaXBzaXMnKVxyXG4gIHdpZHRoOiBzdHJpbmc7XHJcblxyXG4gIEBIb3N0QmluZGluZygndGl0bGUnKVxyXG4gIEBJbnB1dCgpXHJcbiAgdGl0bGU6IHN0cmluZztcclxuXHJcbiAgQElucHV0KCdhYnBFbGxpcHNpc0VuYWJsZWQnKVxyXG4gIGVuYWJsZWQgPSB0cnVlO1xyXG5cclxuICBASG9zdEJpbmRpbmcoJ2NsYXNzLmFicC1lbGxpcHNpcy1pbmxpbmUnKVxyXG4gIGdldCBpbmxpbmVDbGFzcygpIHtcclxuICAgIHJldHVybiB0aGlzLmVuYWJsZWQgJiYgdGhpcy53aWR0aDtcclxuICB9XHJcblxyXG4gIEBIb3N0QmluZGluZygnY2xhc3MuYWJwLWVsbGlwc2lzJylcclxuICBnZXQgY2xhc3MoKSB7XHJcbiAgICByZXR1cm4gdGhpcy5lbmFibGVkICYmICF0aGlzLndpZHRoO1xyXG4gIH1cclxuXHJcbiAgQEhvc3RCaW5kaW5nKCdzdHlsZS5tYXgtd2lkdGgnKVxyXG4gIGdldCBtYXhXaWR0aCgpIHtcclxuICAgIHJldHVybiB0aGlzLmVuYWJsZWQgJiYgdGhpcy53aWR0aCA/IHRoaXMud2lkdGggfHwgJzE3MHB4JyA6IHVuZGVmaW5lZDtcclxuICB9XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgY2RSZWY6IENoYW5nZURldGVjdG9yUmVmLCBwcml2YXRlIGVsUmVmOiBFbGVtZW50UmVmKSB7fVxyXG5cclxuICBuZ0FmdGVyQ29udGVudEluaXQoKSB7XHJcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcclxuICAgICAgY29uc3QgdGl0bGUgPSB0aGlzLnRpdGxlO1xyXG4gICAgICB0aGlzLnRpdGxlID0gdGl0bGUgfHwgKHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudCBhcyBIVE1MRWxlbWVudCkuaW5uZXJUZXh0O1xyXG5cclxuICAgICAgaWYgKHRoaXMudGl0bGUgIT09IHRpdGxlKSB7XHJcbiAgICAgICAgdGhpcy5jZFJlZi5kZXRlY3RDaGFuZ2VzKCk7XHJcbiAgICAgIH1cclxuICAgIH0sIDApO1xyXG4gIH1cclxufVxyXG4iXX0=