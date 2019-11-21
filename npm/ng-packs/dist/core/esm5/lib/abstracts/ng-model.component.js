/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Component, Injector, Input } from '@angular/core';
/**
 * @template T
 */
var AbstractNgModelComponent = /** @class */ (function() {
  function AbstractNgModelComponent(injector) {
    this.injector = injector;
    this.cdRef = injector.get(/** @type {?} */ (ChangeDetectorRef));
  }
  Object.defineProperty(AbstractNgModelComponent.prototype, 'value', {
    /**
     * @return {?}
     */
    get: function() {
      return this._value;
    },
    /**
     * @param {?} value
     * @return {?}
     */
    set: function(value) {
      this._value = value;
      this.notifyValueChange();
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @return {?}
   */
  AbstractNgModelComponent.prototype.notifyValueChange
  /**
   * @return {?}
   */ = function() {
    if (this.onChange) {
      this.onChange(this.value);
    }
  };
  /**
   * @param {?} value
   * @return {?}
   */
  AbstractNgModelComponent.prototype.writeValue
  /**
   * @param {?} value
   * @return {?}
   */ = function(value) {
    var _this = this;
    this._value = value;
    setTimeout(
      /**
       * @return {?}
       */
      function() {
        return _this.cdRef.detectChanges();
      },
      0,
    );
  };
  /**
   * @param {?} fn
   * @return {?}
   */
  AbstractNgModelComponent.prototype.registerOnChange
  /**
   * @param {?} fn
   * @return {?}
   */ = function(fn) {
    this.onChange = fn;
  };
  /**
   * @param {?} fn
   * @return {?}
   */
  AbstractNgModelComponent.prototype.registerOnTouched
  /**
   * @param {?} fn
   * @return {?}
   */ = function(fn) {
    this.onTouched = fn;
  };
  /**
   * @param {?} isDisabled
   * @return {?}
   */
  AbstractNgModelComponent.prototype.setDisabledState
  /**
   * @param {?} isDisabled
   * @return {?}
   */ = function(isDisabled) {
    this.disabled = isDisabled;
  };
  AbstractNgModelComponent.decorators = [
    { type: Component, args: [{ selector: 'abp-abstract-ng-model', template: '' }] },
  ];
  /** @nocollapse */
  AbstractNgModelComponent.ctorParameters = function() {
    return [{ type: Injector }];
  };
  AbstractNgModelComponent.propDecorators = {
    disabled: [{ type: Input }],
    value: [{ type: Input }],
  };
  return AbstractNgModelComponent;
})();
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibmctbW9kZWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2Fic3RyYWN0cy9uZy1tb2RlbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUNBLE9BQU8sRUFBRSxpQkFBaUIsRUFBRSxTQUFTLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBUSxNQUFNLGVBQWUsQ0FBQzs7OztBQUVwRjtJQW1CRSxrQ0FBbUIsUUFBa0I7UUFBbEIsYUFBUSxHQUFSLFFBQVEsQ0FBVTtRQUNuQyxJQUFJLENBQUMsS0FBSyxHQUFHLFFBQVEsQ0FBQyxHQUFHLENBQW9CLG1CQUFBLGlCQUFpQixFQUEyQixDQUFDLENBQUM7SUFDN0YsQ0FBQztJQWpCRCxzQkFBYSwyQ0FBSzs7OztRQUtsQjtZQUNFLE9BQU8sSUFBSSxDQUFDLE1BQU0sQ0FBQztRQUNyQixDQUFDOzs7OztRQVBELFVBQW1CLEtBQVE7WUFDekIsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7WUFDcEIsSUFBSSxDQUFDLGlCQUFpQixFQUFFLENBQUM7UUFDM0IsQ0FBQzs7O09BQUE7Ozs7SUFnQkQsb0RBQWlCOzs7SUFBakI7UUFDRSxJQUFJLElBQUksQ0FBQyxRQUFRLEVBQUU7WUFDakIsSUFBSSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7U0FDM0I7SUFDSCxDQUFDOzs7OztJQUVELDZDQUFVOzs7O0lBQVYsVUFBVyxLQUFRO1FBQW5CLGlCQUdDO1FBRkMsSUFBSSxDQUFDLE1BQU0sR0FBRyxLQUFLLENBQUM7UUFDcEIsVUFBVTs7O1FBQUMsY0FBTSxPQUFBLEtBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLEVBQTFCLENBQTBCLEdBQUUsQ0FBQyxDQUFDLENBQUM7SUFDbEQsQ0FBQzs7Ozs7SUFFRCxtREFBZ0I7Ozs7SUFBaEIsVUFBaUIsRUFBTztRQUN0QixJQUFJLENBQUMsUUFBUSxHQUFHLEVBQUUsQ0FBQztJQUNyQixDQUFDOzs7OztJQUVELG9EQUFpQjs7OztJQUFqQixVQUFrQixFQUFPO1FBQ3ZCLElBQUksQ0FBQyxTQUFTLEdBQUcsRUFBRSxDQUFDO0lBQ3RCLENBQUM7Ozs7O0lBRUQsbURBQWdCOzs7O0lBQWhCLFVBQWlCLFVBQW1CO1FBQ2xDLElBQUksQ0FBQyxRQUFRLEdBQUcsVUFBVSxDQUFDO0lBQzdCLENBQUM7O2dCQTVDRixTQUFTLFNBQUMsRUFBRSxRQUFRLEVBQUUsdUJBQXVCLEVBQUUsUUFBUSxFQUFFLEVBQUUsRUFBRTs7OztnQkFGdkIsUUFBUTs7OzJCQUk1QyxLQUFLO3dCQUVMLEtBQUs7O0lBeUNSLCtCQUFDO0NBQUEsQUE3Q0QsSUE2Q0M7U0E1Q1ksd0JBQXdCOzs7SUFDbkMsNENBQTJCOztJQVczQiw0Q0FBMkI7O0lBQzNCLDZDQUFvQjs7Ozs7SUFFcEIsMENBQW9COzs7OztJQUNwQix5Q0FBbUM7O0lBRXZCLDRDQUF5QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbnRyb2xWYWx1ZUFjY2Vzc29yIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgQ2hhbmdlRGV0ZWN0b3JSZWYsIENvbXBvbmVudCwgSW5qZWN0b3IsIElucHV0LCBUeXBlIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoeyBzZWxlY3RvcjogJ2FicC1hYnN0cmFjdC1uZy1tb2RlbCcsIHRlbXBsYXRlOiAnJyB9KVxuZXhwb3J0IGNsYXNzIEFic3RyYWN0TmdNb2RlbENvbXBvbmVudDxUID0gYW55PiBpbXBsZW1lbnRzIENvbnRyb2xWYWx1ZUFjY2Vzc29yIHtcbiAgQElucHV0KCkgZGlzYWJsZWQ6IGJvb2xlYW47XG5cbiAgQElucHV0KCkgc2V0IHZhbHVlKHZhbHVlOiBUKSB7XG4gICAgdGhpcy5fdmFsdWUgPSB2YWx1ZTtcbiAgICB0aGlzLm5vdGlmeVZhbHVlQ2hhbmdlKCk7XG4gIH1cblxuICBnZXQgdmFsdWUoKTogVCB7XG4gICAgcmV0dXJuIHRoaXMuX3ZhbHVlO1xuICB9XG5cbiAgb25DaGFuZ2U6ICh2YWx1ZTogVCkgPT4ge307XG4gIG9uVG91Y2hlZDogKCkgPT4ge307XG5cbiAgcHJvdGVjdGVkIF92YWx1ZTogVDtcbiAgcHJvdGVjdGVkIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZjtcblxuICBjb25zdHJ1Y3RvcihwdWJsaWMgaW5qZWN0b3I6IEluamVjdG9yKSB7XG4gICAgdGhpcy5jZFJlZiA9IGluamVjdG9yLmdldDxDaGFuZ2VEZXRlY3RvclJlZj4oQ2hhbmdlRGV0ZWN0b3JSZWYgYXMgVHlwZTxDaGFuZ2VEZXRlY3RvclJlZj4pO1xuICB9XG5cbiAgbm90aWZ5VmFsdWVDaGFuZ2UoKTogdm9pZCB7XG4gICAgaWYgKHRoaXMub25DaGFuZ2UpIHtcbiAgICAgIHRoaXMub25DaGFuZ2UodGhpcy52YWx1ZSk7XG4gICAgfVxuICB9XG5cbiAgd3JpdGVWYWx1ZSh2YWx1ZTogVCk6IHZvaWQge1xuICAgIHRoaXMuX3ZhbHVlID0gdmFsdWU7XG4gICAgc2V0VGltZW91dCgoKSA9PiB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKSwgMCk7XG4gIH1cblxuICByZWdpc3Rlck9uQ2hhbmdlKGZuOiBhbnkpOiB2b2lkIHtcbiAgICB0aGlzLm9uQ2hhbmdlID0gZm47XG4gIH1cblxuICByZWdpc3Rlck9uVG91Y2hlZChmbjogYW55KTogdm9pZCB7XG4gICAgdGhpcy5vblRvdWNoZWQgPSBmbjtcbiAgfVxuXG4gIHNldERpc2FibGVkU3RhdGUoaXNEaXNhYmxlZDogYm9vbGVhbik6IHZvaWQge1xuICAgIHRoaXMuZGlzYWJsZWQgPSBpc0Rpc2FibGVkO1xuICB9XG59XG4iXX0=
