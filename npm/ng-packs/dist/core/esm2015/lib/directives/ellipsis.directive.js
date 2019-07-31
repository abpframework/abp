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
    get maxWidth() {
        return this.witdh || '180px';
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
    enabled: [{ type: HostBinding, args: ['class.abp-ellipsis',] }],
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZWxsaXBzaXMuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZWxsaXBzaXMuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQW9CLGlCQUFpQixFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsV0FBVyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUsvRyxNQUFNLE9BQU8saUJBQWlCOzs7OztJQWdCNUIsWUFBb0IsS0FBd0IsRUFBVSxLQUFpQjtRQUFuRCxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQUFVLFVBQUssR0FBTCxLQUFLLENBQVk7UUFQdkUsWUFBTyxHQUFHLElBQUksQ0FBQztJQU8yRCxDQUFDOzs7O0lBTDNFLElBQ0ksUUFBUTtRQUNWLE9BQU8sSUFBSSxDQUFDLEtBQUssSUFBSSxPQUFPLENBQUM7SUFDL0IsQ0FBQzs7OztJQUlELGtCQUFrQjtRQUNoQixVQUFVOzs7UUFBQyxHQUFHLEVBQUU7O2tCQUNSLEtBQUssR0FBRyxJQUFJLENBQUMsS0FBSztZQUN4QixJQUFJLENBQUMsS0FBSyxHQUFHLEtBQUssSUFBSSxDQUFDLG1CQUFBLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFlLENBQUMsQ0FBQyxTQUFTLENBQUM7WUFFMUUsSUFBSSxJQUFJLENBQUMsS0FBSyxLQUFLLEtBQUssRUFBRTtnQkFDeEIsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLEVBQUUsQ0FBQzthQUM1QjtRQUNILENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztJQUNSLENBQUM7OztZQTlCRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGVBQWU7YUFDMUI7Ozs7WUFKMEIsaUJBQWlCO1lBQWEsVUFBVTs7O29CQU1oRSxLQUFLLFNBQUMsYUFBYTtvQkFHbkIsV0FBVyxTQUFDLE9BQU8sY0FDbkIsS0FBSztzQkFHTCxXQUFXLFNBQUMsb0JBQW9CO3VCQUdoQyxXQUFXLFNBQUMsaUJBQWlCOzs7O0lBVjlCLGtDQUNjOztJQUVkLGtDQUVjOztJQUVkLG9DQUNlOzs7OztJQU9ILGtDQUFnQzs7Ozs7SUFBRSxrQ0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBZnRlckNvbnRlbnRJbml0LCBDaGFuZ2VEZXRlY3RvclJlZiwgRGlyZWN0aXZlLCBFbGVtZW50UmVmLCBIb3N0QmluZGluZywgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQERpcmVjdGl2ZSh7XG4gIHNlbGVjdG9yOiAnW2FicEVsbGlwc2lzXScsXG59KVxuZXhwb3J0IGNsYXNzIEVsbGlwc2lzRGlyZWN0aXZlIGltcGxlbWVudHMgQWZ0ZXJDb250ZW50SW5pdCB7XG4gIEBJbnB1dCgnYWJwRWxsaXBzaXMnKVxuICB3aXRkaDogc3RyaW5nO1xuXG4gIEBIb3N0QmluZGluZygndGl0bGUnKVxuICBASW5wdXQoKVxuICB0aXRsZTogc3RyaW5nO1xuXG4gIEBIb3N0QmluZGluZygnY2xhc3MuYWJwLWVsbGlwc2lzJylcbiAgZW5hYmxlZCA9IHRydWU7XG5cbiAgQEhvc3RCaW5kaW5nKCdzdHlsZS5tYXgtd2lkdGgnKVxuICBnZXQgbWF4V2lkdGgoKSB7XG4gICAgcmV0dXJuIHRoaXMud2l0ZGggfHwgJzE4MHB4JztcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgY2RSZWY6IENoYW5nZURldGVjdG9yUmVmLCBwcml2YXRlIGVsUmVmOiBFbGVtZW50UmVmKSB7fVxuXG4gIG5nQWZ0ZXJDb250ZW50SW5pdCgpIHtcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIGNvbnN0IHRpdGxlID0gdGhpcy50aXRsZTtcbiAgICAgIHRoaXMudGl0bGUgPSB0aXRsZSB8fCAodGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50IGFzIEhUTUxFbGVtZW50KS5pbm5lclRleHQ7XG5cbiAgICAgIGlmICh0aGlzLnRpdGxlICE9PSB0aXRsZSkge1xuICAgICAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcbiAgICAgIH1cbiAgICB9LCAwKTtcbiAgfVxufVxuIl19