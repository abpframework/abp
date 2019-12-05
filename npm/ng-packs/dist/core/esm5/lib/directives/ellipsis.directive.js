/**
 * @fileoverview added by tsickle
 * Generated from: lib/directives/ellipsis.directive.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
    EllipsisDirective.prototype.ngAfterViewInit = /**
     * @return {?}
     */
    function () {
        this.title = this.title || ((/** @type {?} */ (this.elRef.nativeElement))).innerText;
        this.cdRef.detectChanges();
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZWxsaXBzaXMuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZWxsaXBzaXMuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7O0FBQUEsT0FBTyxFQUFpQixpQkFBaUIsRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFFLFdBQVcsRUFBRSxLQUFLLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFFNUc7SUE2QkUsMkJBQW9CLEtBQXdCLEVBQVUsS0FBaUI7UUFBbkQsVUFBSyxHQUFMLEtBQUssQ0FBbUI7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFZO1FBakJ2RSxZQUFPLEdBQUcsSUFBSSxDQUFDO0lBaUIyRCxDQUFDO0lBZjNFLHNCQUNJLDBDQUFXOzs7O1FBRGY7WUFFRSxPQUFPLElBQUksQ0FBQyxPQUFPLElBQUksSUFBSSxDQUFDLEtBQUssQ0FBQztRQUNwQyxDQUFDOzs7T0FBQTtJQUVELHNCQUNJLG9DQUFLOzs7O1FBRFQ7WUFFRSxPQUFPLElBQUksQ0FBQyxPQUFPLElBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDO1FBQ3JDLENBQUM7OztPQUFBO0lBRUQsc0JBQ0ksdUNBQVE7Ozs7UUFEWjtZQUVFLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBSSxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsS0FBSyxJQUFJLE9BQU8sQ0FBQyxDQUFDLENBQUMsU0FBUyxDQUFDO1FBQ3hFLENBQUM7OztPQUFBOzs7O0lBSUQsMkNBQWU7OztJQUFmO1FBQ0UsSUFBSSxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUMsS0FBSyxJQUFJLENBQUMsbUJBQUEsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLEVBQWUsQ0FBQyxDQUFDLFNBQVMsQ0FBQztRQUMvRSxJQUFJLENBQUMsS0FBSyxDQUFDLGFBQWEsRUFBRSxDQUFDO0lBQzdCLENBQUM7O2dCQWxDRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGVBQWU7aUJBQzFCOzs7O2dCQUp1QixpQkFBaUI7Z0JBQWEsVUFBVTs7O3dCQU03RCxLQUFLLFNBQUMsYUFBYTt3QkFHbkIsV0FBVyxTQUFDLE9BQU8sY0FDbkIsS0FBSzswQkFHTCxLQUFLLFNBQUMsb0JBQW9COzhCQUcxQixXQUFXLFNBQUMsMkJBQTJCO3dCQUt2QyxXQUFXLFNBQUMsb0JBQW9COzJCQUtoQyxXQUFXLFNBQUMsaUJBQWlCOztJQVdoQyx3QkFBQztDQUFBLEFBbkNELElBbUNDO1NBaENZLGlCQUFpQjs7O0lBQzVCLGtDQUNjOztJQUVkLGtDQUVjOztJQUVkLG9DQUNlOzs7OztJQWlCSCxrQ0FBZ0M7Ozs7O0lBQUUsa0NBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQWZ0ZXJWaWV3SW5pdCwgQ2hhbmdlRGV0ZWN0b3JSZWYsIERpcmVjdGl2ZSwgRWxlbWVudFJlZiwgSG9zdEJpbmRpbmcsIElucHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBEaXJlY3RpdmUoe1xuICBzZWxlY3RvcjogJ1thYnBFbGxpcHNpc10nLFxufSlcbmV4cG9ydCBjbGFzcyBFbGxpcHNpc0RpcmVjdGl2ZSBpbXBsZW1lbnRzIEFmdGVyVmlld0luaXQge1xuICBASW5wdXQoJ2FicEVsbGlwc2lzJylcbiAgd2lkdGg6IHN0cmluZztcblxuICBASG9zdEJpbmRpbmcoJ3RpdGxlJylcbiAgQElucHV0KClcbiAgdGl0bGU6IHN0cmluZztcblxuICBASW5wdXQoJ2FicEVsbGlwc2lzRW5hYmxlZCcpXG4gIGVuYWJsZWQgPSB0cnVlO1xuXG4gIEBIb3N0QmluZGluZygnY2xhc3MuYWJwLWVsbGlwc2lzLWlubGluZScpXG4gIGdldCBpbmxpbmVDbGFzcygpIHtcbiAgICByZXR1cm4gdGhpcy5lbmFibGVkICYmIHRoaXMud2lkdGg7XG4gIH1cblxuICBASG9zdEJpbmRpbmcoJ2NsYXNzLmFicC1lbGxpcHNpcycpXG4gIGdldCBjbGFzcygpIHtcbiAgICByZXR1cm4gdGhpcy5lbmFibGVkICYmICF0aGlzLndpZHRoO1xuICB9XG5cbiAgQEhvc3RCaW5kaW5nKCdzdHlsZS5tYXgtd2lkdGgnKVxuICBnZXQgbWF4V2lkdGgoKSB7XG4gICAgcmV0dXJuIHRoaXMuZW5hYmxlZCAmJiB0aGlzLndpZHRoID8gdGhpcy53aWR0aCB8fCAnMTcwcHgnIDogdW5kZWZpbmVkO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWYsIHByaXZhdGUgZWxSZWY6IEVsZW1lbnRSZWYpIHt9XG5cbiAgbmdBZnRlclZpZXdJbml0KCkge1xuICAgIHRoaXMudGl0bGUgPSB0aGlzLnRpdGxlIHx8ICh0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQgYXMgSFRNTEVsZW1lbnQpLmlubmVyVGV4dDtcbiAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcbiAgfVxufVxuIl19