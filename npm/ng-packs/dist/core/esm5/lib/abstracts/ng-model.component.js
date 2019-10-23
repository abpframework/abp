/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Component, Injector, Input } from '@angular/core';
/**
 * @template T
 */
var AbstractNgModelComponent = /** @class */ (function () {
    function AbstractNgModelComponent(injector) {
        this.injector = injector;
        this.cdRef = injector.get((/** @type {?} */ (ChangeDetectorRef)));
    }
    Object.defineProperty(AbstractNgModelComponent.prototype, "value", {
        get: /**
         * @return {?}
         */
        function () {
            return this._value;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            this._value = value;
            this.notifyValueChange();
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    AbstractNgModelComponent.prototype.notifyValueChange = /**
     * @return {?}
     */
    function () {
        if (this.onChange) {
            this.onChange(this.value);
        }
    };
    /**
     * @param {?} value
     * @return {?}
     */
    AbstractNgModelComponent.prototype.writeValue = /**
     * @param {?} value
     * @return {?}
     */
    function (value) {
        var _this = this;
        this._value = value;
        setTimeout((/**
         * @return {?}
         */
        function () { return _this.cdRef.detectChanges(); }), 0);
    };
    /**
     * @param {?} fn
     * @return {?}
     */
    AbstractNgModelComponent.prototype.registerOnChange = /**
     * @param {?} fn
     * @return {?}
     */
    function (fn) {
        this.onChange = fn;
    };
    /**
     * @param {?} fn
     * @return {?}
     */
    AbstractNgModelComponent.prototype.registerOnTouched = /**
     * @param {?} fn
     * @return {?}
     */
    function (fn) {
        this.onTouched = fn;
    };
    /**
     * @param {?} isDisabled
     * @return {?}
     */
    AbstractNgModelComponent.prototype.setDisabledState = /**
     * @param {?} isDisabled
     * @return {?}
     */
    function (isDisabled) {
        this.disabled = isDisabled;
    };
    AbstractNgModelComponent.decorators = [
        { type: Component, args: [{ selector: 'abp-abstract-ng-model', template: '' }] }
    ];
    /** @nocollapse */
    AbstractNgModelComponent.ctorParameters = function () { return [
        { type: Injector }
    ]; };
    AbstractNgModelComponent.propDecorators = {
        disabled: [{ type: Input }],
        value: [{ type: Input }]
    };
    return AbstractNgModelComponent;
}());
export { AbstractNgModelComponent };
if (false) {
    /** @type {?} */
    AbstractNgModelComponent.prototype.disabled;
    /** @type {?} */
    AbstractNgModelComponent.prototype.onChange;
    /** @type {?} */
    AbstractNgModelComponent.prototype.onTouched;
    /**
     * @type {?}
     * @protected
     */
    AbstractNgModelComponent.prototype._value;
    /**
     * @type {?}
     * @protected
     */
    AbstractNgModelComponent.prototype.cdRef;
    /** @type {?} */
    AbstractNgModelComponent.prototype.injector;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmctbW9kZWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy9uZy1tb2RlbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUNBLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBUSxNQUFNLGVBQWUsQ0FBQzs7OztBQUVwRjtJQW1CRSxrQ0FBbUIsUUFBa0I7UUFBbEIsYUFBUSxHQUFSLFFBQVEsQ0FBVTtRQUNuQyxJQUFJLENBQUMsS0FBSyxHQUFHLFFBQVEsQ0FBQyxHQUFHLENBQW9CLG1CQUFBLGlCQUFpQixFQUEyQixDQUFDLENBQUM7SUFDN0YsQ0FBQztJQWpCRCxzQkFBYSwyQ0FBSzs7OztRQUtsQjtZQUNFLE9BQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQztRQUNyQixDQUFDOzs7OztRQVBELFVBQW1CLEtBQVE7WUFDekIsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7WUFDcEIsSUFBSSxDQUFDLGlCQUFpQixFQUFFLENBQUM7UUFDM0IsQ0FBQzs7O09BQUE7Ozs7SUFnQkQsb0RBQWlCOzs7SUFBakI7UUFDRSxJQUFJLElBQUksQ0FBQyxRQUFRLEVBQUU7WUFDakIsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7U0FDM0I7SUFDSCxDQUFDOzs7OztJQUVELDZDQUFVOzs7O0lBQVYsVUFBVyxLQUFRO1FBQW5CLGlCQUdDO1FBRkMsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7UUFDcEIsVUFBVTs7O1FBQUMsY0FBTSxPQUFBLEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLEVBQTFCLENBQTBCLEdBQUUsQ0FBQyxDQUFDLENBQUM7SUFDbEQsQ0FBQzs7Ozs7SUFFRCxtREFBZ0I7Ozs7SUFBaEIsVUFBaUIsRUFBTztRQUN0QixJQUFJLENBQUMsUUFBUSxHQUFHLEVBQUUsQ0FBQztJQUNyQixDQUFDOzs7OztJQUVELG9EQUFpQjs7OztJQUFqQixVQUFrQixFQUFPO1FBQ3ZCLElBQUksQ0FBQyxTQUFTLEdBQUcsRUFBRSxDQUFDO0lBQ3RCLENBQUM7Ozs7O0lBRUQsbURBQWdCOzs7O0lBQWhCLFVBQWlCLFVBQW1CO1FBQ2xDLElBQUksQ0FBQyxRQUFRLEdBQUcsVUFBVSxDQUFDO0lBQzdCLENBQUM7O2dCQTVDRixTQUFTLFNBQUMsRUFBRSxRQUFRLEVBQUUsdUJBQXVCLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRTs7OztnQkFGdkIsUUFBUTs7OzJCQUk1QyxLQUFLO3dCQUVMLEtBQUs7O0lBeUNSLCtCQUFDO0NBQUEsQUE3Q0QsSUE2Q0M7U0E1Q1ksd0JBQXdCOzs7SUFDbkMsNENBQTJCOztJQVczQiw0Q0FBMkI7O0lBQzNCLDZDQUFvQjs7Ozs7SUFFcEIsMENBQW9COzs7OztJQUNwQix5Q0FBbUM7O0lBRXZCLDRDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbnRyb2xWYWx1ZUFjY2Vzc29yIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xyXG5pbXBvcnQgeyBDaGFuZ2VEZXRlY3RvclJlZiwgQ29tcG9uZW50LCBJbmplY3RvciwgSW5wdXQsIFR5cGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbkBDb21wb25lbnQoeyBzZWxlY3RvcjogJ2FicC1hYnN0cmFjdC1uZy1tb2RlbCcsIHRlbXBsYXRlOiAnJyB9KVxyXG5leHBvcnQgY2xhc3MgQWJzdHJhY3ROZ01vZGVsQ29tcG9uZW50PFQgPSBhbnk+IGltcGxlbWVudHMgQ29udHJvbFZhbHVlQWNjZXNzb3Ige1xyXG4gIEBJbnB1dCgpIGRpc2FibGVkOiBib29sZWFuO1xyXG5cclxuICBASW5wdXQoKSBzZXQgdmFsdWUodmFsdWU6IFQpIHtcclxuICAgIHRoaXMuX3ZhbHVlID0gdmFsdWU7XHJcbiAgICB0aGlzLm5vdGlmeVZhbHVlQ2hhbmdlKCk7XHJcbiAgfVxyXG5cclxuICBnZXQgdmFsdWUoKTogVCB7XHJcbiAgICByZXR1cm4gdGhpcy5fdmFsdWU7XHJcbiAgfVxyXG5cclxuICBvbkNoYW5nZTogKHZhbHVlOiBUKSA9PiB7fTtcclxuICBvblRvdWNoZWQ6ICgpID0+IHt9O1xyXG5cclxuICBwcm90ZWN0ZWQgX3ZhbHVlOiBUO1xyXG4gIHByb3RlY3RlZCBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWY7XHJcblxyXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBpbmplY3RvcjogSW5qZWN0b3IpIHtcclxuICAgIHRoaXMuY2RSZWYgPSBpbmplY3Rvci5nZXQ8Q2hhbmdlRGV0ZWN0b3JSZWY+KENoYW5nZURldGVjdG9yUmVmIGFzIFR5cGU8Q2hhbmdlRGV0ZWN0b3JSZWY+KTtcclxuICB9XHJcblxyXG4gIG5vdGlmeVZhbHVlQ2hhbmdlKCk6IHZvaWQge1xyXG4gICAgaWYgKHRoaXMub25DaGFuZ2UpIHtcclxuICAgICAgdGhpcy5vbkNoYW5nZSh0aGlzLnZhbHVlKTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIHdyaXRlVmFsdWUodmFsdWU6IFQpOiB2b2lkIHtcclxuICAgIHRoaXMuX3ZhbHVlID0gdmFsdWU7XHJcbiAgICBzZXRUaW1lb3V0KCgpID0+IHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpLCAwKTtcclxuICB9XHJcblxyXG4gIHJlZ2lzdGVyT25DaGFuZ2UoZm46IGFueSk6IHZvaWQge1xyXG4gICAgdGhpcy5vbkNoYW5nZSA9IGZuO1xyXG4gIH1cclxuXHJcbiAgcmVnaXN0ZXJPblRvdWNoZWQoZm46IGFueSk6IHZvaWQge1xyXG4gICAgdGhpcy5vblRvdWNoZWQgPSBmbjtcclxuICB9XHJcblxyXG4gIHNldERpc2FibGVkU3RhdGUoaXNEaXNhYmxlZDogYm9vbGVhbik6IHZvaWQge1xyXG4gICAgdGhpcy5kaXNhYmxlZCA9IGlzRGlzYWJsZWQ7XHJcbiAgfVxyXG59XHJcbiJdfQ==