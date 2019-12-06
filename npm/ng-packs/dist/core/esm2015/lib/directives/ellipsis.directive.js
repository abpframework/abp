/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/ellipsis.directive.ts
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
    ngAfterViewInit() {
        this.title = this.title || ((/** @type {?} */ (this.elRef.nativeElement))).innerText;
        this.cdRef.detectChanges();
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZWxsaXBzaXMuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZWxsaXBzaXMuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFpQixpQkFBaUIsRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFFLFdBQVcsRUFBRSxLQUFLLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFLNUcsTUFBTSxPQUFPLGlCQUFpQjs7Ozs7SUEwQjVCLFlBQW9CLEtBQXdCLEVBQVUsS0FBaUI7UUFBbkQsVUFBSyxHQUFMLEtBQUssQ0FBbUI7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFZO1FBakJ2RSxZQUFPLEdBQUcsSUFBSSxDQUFDO0lBaUIyRCxDQUFDOzs7O0lBZjNFLElBQ0ksV0FBVztRQUNiLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBSSxJQUFJLENBQUMsS0FBSyxDQUFDO0lBQ3BDLENBQUM7Ozs7SUFFRCxJQUNJLEtBQUs7UUFDUCxPQUFPLElBQUksQ0FBQyxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDO0lBQ3JDLENBQUM7Ozs7SUFFRCxJQUNJLFFBQVE7UUFDVixPQUFPLElBQUksQ0FBQyxPQUFPLElBQUksSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLEtBQUssSUFBSSxPQUFPLENBQUMsQ0FBQyxDQUFDLFNBQVMsQ0FBQztJQUN4RSxDQUFDOzs7O0lBSUQsZUFBZTtRQUNiLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDLEtBQUssSUFBSSxDQUFDLG1CQUFBLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFlLENBQUMsQ0FBQyxTQUFTLENBQUM7UUFDL0UsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLEVBQUUsQ0FBQztJQUM3QixDQUFDOzs7WUFsQ0YsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxlQUFlO2FBQzFCOzs7O1lBSnVCLGlCQUFpQjtZQUFhLFVBQVU7OztvQkFNN0QsS0FBSyxTQUFDLGFBQWE7b0JBR25CLFdBQVcsU0FBQyxPQUFPLGNBQ25CLEtBQUs7c0JBR0wsS0FBSyxTQUFDLG9CQUFvQjswQkFHMUIsV0FBVyxTQUFDLDJCQUEyQjtvQkFLdkMsV0FBVyxTQUFDLG9CQUFvQjt1QkFLaEMsV0FBVyxTQUFDLGlCQUFpQjs7OztJQXBCOUIsa0NBQ2M7O0lBRWQsa0NBRWM7O0lBRWQsb0NBQ2U7Ozs7O0lBaUJILGtDQUFnQzs7Ozs7SUFBRSxrQ0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBZnRlclZpZXdJbml0LCBDaGFuZ2VEZXRlY3RvclJlZiwgRGlyZWN0aXZlLCBFbGVtZW50UmVmLCBIb3N0QmluZGluZywgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2FicEVsbGlwc2lzXScsXG59KVxuZXhwb3J0IGNsYXNzIEVsbGlwc2lzRGlyZWN0aXZlIGltcGxlbWVudHMgQWZ0ZXJWaWV3SW5pdCB7XG4gIEBJbnB1dCgnYWJwRWxsaXBzaXMnKVxuICB3aWR0aDogc3RyaW5nO1xuXG4gIEBIb3N0QmluZGluZygndGl0bGUnKVxuICBASW5wdXQoKVxuICB0aXRsZTogc3RyaW5nO1xuXG4gIEBJbnB1dCgnYWJwRWxsaXBzaXNFbmFibGVkJylcbiAgZW5hYmxlZCA9IHRydWU7XG5cbiAgQEhvc3RCaW5kaW5nKCdjbGFzcy5hYnAtZWxsaXBzaXMtaW5saW5lJylcbiAgZ2V0IGlubGluZUNsYXNzKCkge1xuICAgIHJldHVybiB0aGlzLmVuYWJsZWQgJiYgdGhpcy53aWR0aDtcbiAgfVxuXG4gIEBIb3N0QmluZGluZygnY2xhc3MuYWJwLWVsbGlwc2lzJylcbiAgZ2V0IGNsYXNzKCkge1xuICAgIHJldHVybiB0aGlzLmVuYWJsZWQgJiYgIXRoaXMud2lkdGg7XG4gIH1cblxuICBASG9zdEJpbmRpbmcoJ3N0eWxlLm1heC13aWR0aCcpXG4gIGdldCBtYXhXaWR0aCgpIHtcbiAgICByZXR1cm4gdGhpcy5lbmFibGVkICYmIHRoaXMud2lkdGggPyB0aGlzLndpZHRoIHx8ICcxNzBweCcgOiB1bmRlZmluZWQ7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZiwgcHJpdmF0ZSBlbFJlZjogRWxlbWVudFJlZikge31cblxuICBuZ0FmdGVyVmlld0luaXQoKSB7XG4gICAgdGhpcy50aXRsZSA9IHRoaXMudGl0bGUgfHwgKHRoaXMuZWxSZWYubmF0aXZlRWxlbWVudCBhcyBIVE1MRWxlbWVudCkuaW5uZXJUZXh0O1xuICAgIHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpO1xuICB9XG59XG4iXX0=