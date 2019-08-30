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
    Object.defineProperty(EllipsisDirective.prototype, "inlineClass", {
        get: /**
         * @return {?}
         */
        function () {
            return this.enabled && this.width;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipsisDirective.prototype, "class", {
        get: /**
         * @return {?}
         */
        function () {
            return this.enabled && !this.width;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipsisDirective.prototype, "maxWidth", {
        get: /**
         * @return {?}
         */
        function () {
            return this.enabled && this.width ? this.width || '170px' : undefined;
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
        width: [{ type: Input, args: ['abpEllipsis',] }],
        title: [{ type: HostBinding, args: ['title',] }, { type: Input }],
        enabled: [{ type: Input, args: ['abpEllipsisEnabled',] }],
        inlineClass: [{ type: HostBinding, args: ['class.abp-ellipsis-inline',] }],
        class: [{ type: HostBinding, args: ['class.abp-ellipsis',] }],
        maxWidth: [{ type: HostBinding, args: ['style.max-width',] }]
    };
    return EllipsisDirective;
}());
export { EllipsisDirective };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZWxsaXBzaXMuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZWxsaXBzaXMuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQW9CLGlCQUFpQixFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsV0FBVyxFQUFFLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUUvRztJQTZCRSwyQkFBb0IsS0FBd0IsRUFBVSxLQUFpQjtRQUFuRCxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQUFVLFVBQUssR0FBTCxLQUFLLENBQVk7UUFqQnZFLFlBQU8sR0FBRyxJQUFJLENBQUM7SUFpQjJELENBQUM7SUFmM0Usc0JBQ0ksMENBQVc7Ozs7UUFEZjtZQUVFLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBSSxJQUFJLENBQUMsS0FBSyxDQUFDO1FBQ3BDLENBQUM7OztPQUFBO0lBRUQsc0JBQ0ksb0NBQUs7Ozs7UUFEVDtZQUVFLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUM7UUFDckMsQ0FBQzs7O09BQUE7SUFFRCxzQkFDSSx1Q0FBUTs7OztRQURaO1lBRUUsT0FBTyxJQUFJLENBQUMsT0FBTyxJQUFJLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxLQUFLLElBQUksT0FBTyxDQUFDLENBQUMsQ0FBQyxTQUFTLENBQUM7UUFDeEUsQ0FBQzs7O09BQUE7Ozs7SUFJRCw4Q0FBa0I7OztJQUFsQjtRQUFBLGlCQVNDO1FBUkMsVUFBVTs7O1FBQUM7O2dCQUNILEtBQUssR0FBRyxLQUFJLENBQUMsS0FBSztZQUN4QixLQUFJLENBQUMsS0FBSyxHQUFHLEtBQUssSUFBSSxDQUFDLG1CQUFBLEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFlLENBQUMsQ0FBQyxTQUFTLENBQUM7WUFFMUUsSUFBSSxLQUFJLENBQUMsS0FBSyxLQUFLLEtBQUssRUFBRTtnQkFDeEIsS0FBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLEVBQUUsQ0FBQzthQUM1QjtRQUNILENBQUMsR0FBRSxDQUFDLENBQUMsQ0FBQztJQUNSLENBQUM7O2dCQXhDRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGVBQWU7aUJBQzFCOzs7O2dCQUowQixpQkFBaUI7Z0JBQWEsVUFBVTs7O3dCQU1oRSxLQUFLLFNBQUMsYUFBYTt3QkFHbkIsV0FBVyxTQUFDLE9BQU8sY0FDbkIsS0FBSzswQkFHTCxLQUFLLFNBQUMsb0JBQW9COzhCQUcxQixXQUFXLFNBQUMsMkJBQTJCO3dCQUt2QyxXQUFXLFNBQUMsb0JBQW9COzJCQUtoQyxXQUFXLFNBQUMsaUJBQWlCOztJQWlCaEMsd0JBQUM7Q0FBQSxBQXpDRCxJQXlDQztTQXRDWSxpQkFBaUI7OztJQUM1QixrQ0FDYzs7SUFFZCxrQ0FFYzs7SUFFZCxvQ0FDZTs7Ozs7SUFpQkgsa0NBQWdDOzs7OztJQUFFLGtDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IEFmdGVyQ29udGVudEluaXQsIENoYW5nZURldGVjdG9yUmVmLCBEaXJlY3RpdmUsIEVsZW1lbnRSZWYsIEhvc3RCaW5kaW5nLCBJbnB1dCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5ARGlyZWN0aXZlKHtcbiAgc2VsZWN0b3I6ICdbYWJwRWxsaXBzaXNdJyxcbn0pXG5leHBvcnQgY2xhc3MgRWxsaXBzaXNEaXJlY3RpdmUgaW1wbGVtZW50cyBBZnRlckNvbnRlbnRJbml0IHtcbiAgQElucHV0KCdhYnBFbGxpcHNpcycpXG4gIHdpZHRoOiBzdHJpbmc7XG5cbiAgQEhvc3RCaW5kaW5nKCd0aXRsZScpXG4gIEBJbnB1dCgpXG4gIHRpdGxlOiBzdHJpbmc7XG5cbiAgQElucHV0KCdhYnBFbGxpcHNpc0VuYWJsZWQnKVxuICBlbmFibGVkID0gdHJ1ZTtcblxuICBASG9zdEJpbmRpbmcoJ2NsYXNzLmFicC1lbGxpcHNpcy1pbmxpbmUnKVxuICBnZXQgaW5saW5lQ2xhc3MoKSB7XG4gICAgcmV0dXJuIHRoaXMuZW5hYmxlZCAmJiB0aGlzLndpZHRoO1xuICB9XG5cbiAgQEhvc3RCaW5kaW5nKCdjbGFzcy5hYnAtZWxsaXBzaXMnKVxuICBnZXQgY2xhc3MoKSB7XG4gICAgcmV0dXJuIHRoaXMuZW5hYmxlZCAmJiAhdGhpcy53aWR0aDtcbiAgfVxuXG4gIEBIb3N0QmluZGluZygnc3R5bGUubWF4LXdpZHRoJylcbiAgZ2V0IG1heFdpZHRoKCkge1xuICAgIHJldHVybiB0aGlzLmVuYWJsZWQgJiYgdGhpcy53aWR0aCA/IHRoaXMud2lkdGggfHwgJzE3MHB4JyA6IHVuZGVmaW5lZDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgY2RSZWY6IENoYW5nZURldGVjdG9yUmVmLCBwcml2YXRlIGVsUmVmOiBFbGVtZW50UmVmKSB7fVxuXG4gIG5nQWZ0ZXJDb250ZW50SW5pdCgpIHtcbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIGNvbnN0IHRpdGxlID0gdGhpcy50aXRsZTtcbiAgICAgIHRoaXMudGl0bGUgPSB0aXRsZSB8fCAodGhpcy5lbFJlZi5uYXRpdmVFbGVtZW50IGFzIEhUTUxFbGVtZW50KS5pbm5lclRleHQ7XG5cbiAgICAgIGlmICh0aGlzLnRpdGxlICE9PSB0aXRsZSkge1xuICAgICAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcbiAgICAgIH1cbiAgICB9LCAwKTtcbiAgfVxufVxuIl19