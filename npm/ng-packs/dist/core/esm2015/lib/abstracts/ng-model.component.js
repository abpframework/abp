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
    this.cdRef = injector.get(/** @type {?} */ (ChangeDetectorRef));
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
    setTimeout(
      /**
       * @return {?}
       */
      () => this.cdRef.detectChanges(),
      0,
    );
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
  { type: Component, args: [{ selector: 'abp-abstract-ng-model', template: '' }] },
];
/** @nocollapse */
AbstractNgModelComponent.ctorParameters = () => [{ type: Injector }];
AbstractNgModelComponent.propDecorators = {
  disabled: [{ type: Input }],
  value: [{ type: Input }],
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmctbW9kZWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy9uZy1tb2RlbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUNBLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBUSxNQUFNLGVBQWUsQ0FBQzs7OztBQUdwRixNQUFNLE9BQU8sd0JBQXdCOzs7O0lBa0JuQyxZQUFtQixRQUFrQjtRQUFsQixhQUFRLEdBQVIsUUFBUSxDQUFVO1FBQ25DLElBQUksQ0FBQyxLQUFLLEdBQUcsUUFBUSxDQUFDLEdBQUcsQ0FBb0IsbUJBQUEsaUJBQWlCLEVBQTJCLENBQUMsQ0FBQztJQUM3RixDQUFDOzs7OztJQWpCRCxJQUFhLEtBQUssQ0FBQyxLQUFRO1FBQ3pCLElBQUksQ0FBQyxNQUFNLEdBQUcsS0FBSyxDQUFDO1FBQ3BCLElBQUksQ0FBQyxpQkFBaUIsRUFBRSxDQUFDO0lBQzNCLENBQUM7Ozs7SUFFRCxJQUFJLEtBQUs7UUFDUCxPQUFPLElBQUksQ0FBQyxNQUFNLENBQUM7SUFDckIsQ0FBQzs7OztJQVlELGlCQUFpQjtRQUNmLElBQUksSUFBSSxDQUFDLFFBQVEsRUFBRTtZQUNqQixJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztTQUMzQjtJQUNILENBQUM7Ozs7O0lBRUQsVUFBVSxDQUFDLEtBQVE7UUFDakIsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7UUFDcEIsVUFBVTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLEVBQUUsR0FBRSxDQUFDLENBQUMsQ0FBQztJQUNsRCxDQUFDOzs7OztJQUVELGdCQUFnQixDQUFDLEVBQU87UUFDdEIsSUFBSSxDQUFDLFFBQVEsR0FBRyxFQUFFLENBQUM7SUFDckIsQ0FBQzs7Ozs7SUFFRCxpQkFBaUIsQ0FBQyxFQUFPO1FBQ3ZCLElBQUksQ0FBQyxTQUFTLEdBQUcsRUFBRSxDQUFDO0lBQ3RCLENBQUM7Ozs7O0lBRUQsZ0JBQWdCLENBQUMsVUFBbUI7UUFDbEMsSUFBSSxDQUFDLFFBQVEsR0FBRyxVQUFVLENBQUM7SUFDN0IsQ0FBQzs7O1lBNUNGLFNBQVMsU0FBQyxFQUFFLFFBQVEsRUFBRSx1QkFBdUIsRUFBRSxRQUFRLEVBQUUsRUFBRSxFQUFFOzs7O1lBRnZCLFFBQVE7Ozt1QkFJNUMsS0FBSztvQkFFTCxLQUFLOzs7O0lBRk4sNENBQTJCOztJQVczQiw0Q0FBMkI7O0lBQzNCLDZDQUFvQjs7Ozs7SUFFcEIsMENBQW9COzs7OztJQUNwQix5Q0FBbUM7O0lBRXZCLDRDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbnRyb2xWYWx1ZUFjY2Vzc29yIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0b3JSZWYsIENvbXBvbmVudCwgSW5qZWN0b3IsIElucHV0LCBUeXBlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoeyBzZWxlY3RvcjogJ2FicC1hYnN0cmFjdC1uZy1tb2RlbCcsIHRlbXBsYXRlOiAnJyB9KVxuZXhwb3J0IGNsYXNzIEFic3RyYWN0TmdNb2RlbENvbXBvbmVudDxUID0gYW55PiBpbXBsZW1lbnRzIENvbnRyb2xWYWx1ZUFjY2Vzc29yIHtcbiAgQElucHV0KCkgZGlzYWJsZWQ6IGJvb2xlYW47XG5cbiAgQElucHV0KCkgc2V0IHZhbHVlKHZhbHVlOiBUKSB7XG4gICAgdGhpcy5fdmFsdWUgPSB2YWx1ZTtcbiAgICB0aGlzLm5vdGlmeVZhbHVlQ2hhbmdlKCk7XG4gIH1cblxuICBnZXQgdmFsdWUoKTogVCB7XG4gICAgcmV0dXJuIHRoaXMuX3ZhbHVlO1xuICB9XG5cbiAgb25DaGFuZ2U6ICh2YWx1ZTogVCkgPT4ge307XG4gIG9uVG91Y2hlZDogKCkgPT4ge307XG5cbiAgcHJvdGVjdGVkIF92YWx1ZTogVDtcbiAgcHJvdGVjdGVkIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZjtcblxuICBjb25zdHJ1Y3RvcihwdWJsaWMgaW5qZWN0b3I6IEluamVjdG9yKSB7XG4gICAgdGhpcy5jZFJlZiA9IGluamVjdG9yLmdldDxDaGFuZ2VEZXRlY3RvclJlZj4oQ2hhbmdlRGV0ZWN0b3JSZWYgYXMgVHlwZTxDaGFuZ2VEZXRlY3RvclJlZj4pO1xuICB9XG5cbiAgbm90aWZ5VmFsdWVDaGFuZ2UoKTogdm9pZCB7XG4gICAgaWYgKHRoaXMub25DaGFuZ2UpIHtcbiAgICAgIHRoaXMub25DaGFuZ2UodGhpcy52YWx1ZSk7XG4gICAgfVxuICB9XG5cbiAgd3JpdGVWYWx1ZSh2YWx1ZTogVCk6IHZvaWQge1xuICAgIHRoaXMuX3ZhbHVlID0gdmFsdWU7XG4gICAgc2V0VGltZW91dCgoKSA9PiB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKSwgMCk7XG4gIH1cblxuICByZWdpc3Rlck9uQ2hhbmdlKGZuOiBhbnkpOiB2b2lkIHtcbiAgICB0aGlzLm9uQ2hhbmdlID0gZm47XG4gIH1cblxuICByZWdpc3Rlck9uVG91Y2hlZChmbjogYW55KTogdm9pZCB7XG4gICAgdGhpcy5vblRvdWNoZWQgPSBmbjtcbiAgfVxuXG4gIHNldERpc2FibGVkU3RhdGUoaXNEaXNhYmxlZDogYm9vbGVhbik6IHZvaWQge1xuICAgIHRoaXMuZGlzYWJsZWQgPSBpc0Rpc2FibGVkO1xuICB9XG59XG4iXX0=
