/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Component, Injector, Input } from '@angular/core';
/**
 * @template T
 */
export class AbstractNgModelComponent {
    /**
     * @param {?} injector
     */
    constructor(injector) {
        this.injector = injector;
        this.cdRef = injector.get((/** @type {?} */ (ChangeDetectorRef)));
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set value(value) {
        this._value = value;
        this.notifyValueChange();
    }
    /**
     * @return {?}
     */
    get value() {
        return this._value;
    }
    /**
     * @return {?}
     */
    notifyValueChange() {
        if (this.onChange) {
            this.onChange(this.value);
        }
    }
    /**
     * @param {?} value
     * @return {?}
     */
    writeValue(value) {
        this._value = value;
        setTimeout((/**
         * @return {?}
         */
        () => this.cdRef.detectChanges()), 0);
    }
    /**
     * @param {?} fn
     * @return {?}
     */
    registerOnChange(fn) {
        this.onChange = fn;
    }
    /**
     * @param {?} fn
     * @return {?}
     */
    registerOnTouched(fn) {
        this.onTouched = fn;
    }
    /**
     * @param {?} isDisabled
     * @return {?}
     */
    setDisabledState(isDisabled) {
        this.disabled = isDisabled;
    }
}
AbstractNgModelComponent.decorators = [
    { type: Component, args: [{ selector: 'abp-abstract-ng-model', template: '' }] }
];
/** @nocollapse */
AbstractNgModelComponent.ctorParameters = () => [
    { type: Injector }
];
AbstractNgModelComponent.propDecorators = {
    disabled: [{ type: Input }],
    value: [{ type: Input }]
};
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmctbW9kZWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy9uZy1tb2RlbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUNBLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBUSxNQUFNLGVBQWUsQ0FBQzs7OztBQUdwRixNQUFNLE9BQU8sd0JBQXdCOzs7O0lBa0JuQyxZQUFtQixRQUFrQjtRQUFsQixhQUFRLEdBQVIsUUFBUSxDQUFVO1FBQ25DLElBQUksQ0FBQyxLQUFLLEdBQUcsUUFBUSxDQUFDLEdBQUcsQ0FBb0IsbUJBQUEsaUJBQWlCLEVBQTJCLENBQUMsQ0FBQztJQUM3RixDQUFDOzs7OztJQWpCRCxJQUFhLEtBQUssQ0FBQyxLQUFRO1FBQ3pCLElBQUksQ0FBQyxNQUFNLEdBQUcsS0FBSyxDQUFDO1FBQ3BCLElBQUksQ0FBQyxpQkFBaUIsRUFBRSxDQUFDO0lBQzNCLENBQUM7Ozs7SUFFRCxJQUFJLEtBQUs7UUFDUCxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUM7SUFDckIsQ0FBQzs7OztJQVlELGlCQUFpQjtRQUNmLElBQUksSUFBSSxDQUFDLFFBQVEsRUFBRTtZQUNqQixJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztTQUMzQjtJQUNILENBQUM7Ozs7O0lBRUQsVUFBVSxDQUFDLEtBQVE7UUFDakIsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7UUFDcEIsVUFBVTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLEVBQUUsR0FBRSxDQUFDLENBQUMsQ0FBQztJQUNsRCxDQUFDOzs7OztJQUVELGdCQUFnQixDQUFDLEVBQU87UUFDdEIsSUFBSSxDQUFDLFFBQVEsR0FBRyxFQUFFLENBQUM7SUFDckIsQ0FBQzs7Ozs7SUFFRCxpQkFBaUIsQ0FBQyxFQUFPO1FBQ3ZCLElBQUksQ0FBQyxTQUFTLEdBQUcsRUFBRSxDQUFDO0lBQ3RCLENBQUM7Ozs7O0lBRUQsZ0JBQWdCLENBQUMsVUFBbUI7UUFDbEMsSUFBSSxDQUFDLFFBQVEsR0FBRyxVQUFVLENBQUM7SUFDN0IsQ0FBQzs7O1lBNUNGLFNBQVMsU0FBQyxFQUFFLFFBQVEsRUFBRSx1QkFBdUIsRUFBRSxRQUFRLEVBQUUsRUFBRSxFQUFFOzs7O1lBRnZCLFFBQVE7Ozt1QkFJNUMsS0FBSztvQkFFTCxLQUFLOzs7O0lBRk4sNENBQTJCOztJQVczQiw0Q0FBMkI7O0lBQzNCLDZDQUFvQjs7Ozs7SUFFcEIsMENBQW9COzs7OztJQUNwQix5Q0FBbUM7O0lBRXZCLDRDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbnRyb2xWYWx1ZUFjY2Vzc29yIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xyXG5pbXBvcnQgeyBDaGFuZ2VEZXRlY3RvclJlZiwgQ29tcG9uZW50LCBJbmplY3RvciwgSW5wdXQsIFR5cGUgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuXHJcbkBDb21wb25lbnQoeyBzZWxlY3RvcjogJ2FicC1hYnN0cmFjdC1uZy1tb2RlbCcsIHRlbXBsYXRlOiAnJyB9KVxyXG5leHBvcnQgY2xhc3MgQWJzdHJhY3ROZ01vZGVsQ29tcG9uZW50PFQgPSBhbnk+IGltcGxlbWVudHMgQ29udHJvbFZhbHVlQWNjZXNzb3Ige1xyXG4gIEBJbnB1dCgpIGRpc2FibGVkOiBib29sZWFuO1xyXG5cclxuICBASW5wdXQoKSBzZXQgdmFsdWUodmFsdWU6IFQpIHtcclxuICAgIHRoaXMuX3ZhbHVlID0gdmFsdWU7XHJcbiAgICB0aGlzLm5vdGlmeVZhbHVlQ2hhbmdlKCk7XHJcbiAgfVxyXG5cclxuICBnZXQgdmFsdWUoKTogVCB7XHJcbiAgICByZXR1cm4gdGhpcy5fdmFsdWU7XHJcbiAgfVxyXG5cclxuICBvbkNoYW5nZTogKHZhbHVlOiBUKSA9PiB7fTtcclxuICBvblRvdWNoZWQ6ICgpID0+IHt9O1xyXG5cclxuICBwcm90ZWN0ZWQgX3ZhbHVlOiBUO1xyXG4gIHByb3RlY3RlZCBjZFJlZjogQ2hhbmdlRGV0ZWN0b3JSZWY7XHJcblxyXG4gIGNvbnN0cnVjdG9yKHB1YmxpYyBpbmplY3RvcjogSW5qZWN0b3IpIHtcclxuICAgIHRoaXMuY2RSZWYgPSBpbmplY3Rvci5nZXQ8Q2hhbmdlRGV0ZWN0b3JSZWY+KENoYW5nZURldGVjdG9yUmVmIGFzIFR5cGU8Q2hhbmdlRGV0ZWN0b3JSZWY+KTtcclxuICB9XHJcblxyXG4gIG5vdGlmeVZhbHVlQ2hhbmdlKCk6IHZvaWQge1xyXG4gICAgaWYgKHRoaXMub25DaGFuZ2UpIHtcclxuICAgICAgdGhpcy5vbkNoYW5nZSh0aGlzLnZhbHVlKTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIHdyaXRlVmFsdWUodmFsdWU6IFQpOiB2b2lkIHtcclxuICAgIHRoaXMuX3ZhbHVlID0gdmFsdWU7XHJcbiAgICBzZXRUaW1lb3V0KCgpID0+IHRoaXMuY2RSZWYuZGV0ZWN0Q2hhbmdlcygpLCAwKTtcclxuICB9XHJcblxyXG4gIHJlZ2lzdGVyT25DaGFuZ2UoZm46IGFueSk6IHZvaWQge1xyXG4gICAgdGhpcy5vbkNoYW5nZSA9IGZuO1xyXG4gIH1cclxuXHJcbiAgcmVnaXN0ZXJPblRvdWNoZWQoZm46IGFueSk6IHZvaWQge1xyXG4gICAgdGhpcy5vblRvdWNoZWQgPSBmbjtcclxuICB9XHJcblxyXG4gIHNldERpc2FibGVkU3RhdGUoaXNEaXNhYmxlZDogYm9vbGVhbik6IHZvaWQge1xyXG4gICAgdGhpcy5kaXNhYmxlZCA9IGlzRGlzYWJsZWQ7XHJcbiAgfVxyXG59XHJcbiJdfQ==