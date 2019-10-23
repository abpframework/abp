/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, ElementRef, Input } from '@angular/core';
export class AutofocusDirective {
  /**
   * @param {?} elRef
   */
  constructor(elRef) {
    this.elRef = elRef;
    this.delay = 0;
  }
  /**
   * @return {?}
   */
  ngAfterViewInit() {
    setTimeout(
      /**
       * @return {?}
       */
      () => this.elRef.nativeElement.focus(),
      this.delay,
    );
  }
}
AutofocusDirective.decorators = [
  {
    type: Directive,
    args: [
      {
        // tslint:disable-next-line: directive-selector
        selector: '[autofocus]',
      },
    ],
  },
];
/** @nocollapse */
AutofocusDirective.ctorParameters = () => [{ type: ElementRef }];
AutofocusDirective.propDecorators = {
  delay: [{ type: Input, args: ['autofocus'] }],
};
if (false) {
  /** @type {?} */
  AutofocusDirective.prototype.delay;
  /**
   * @type {?}
   * @private
   */
  AutofocusDirective.prototype.elRef;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYXV0b2ZvY3VzLmRpcmVjdGl2ZS5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcuY29yZS8iLCJzb3VyY2VzIjpbImxpYi9kaXJlY3RpdmVzL2F1dG9mb2N1cy5kaXJlY3RpdmUudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFFLEtBQUssRUFBaUIsTUFBTSxlQUFlLENBQUM7QUFNNUUsTUFBTSxPQUFPLGtCQUFrQjs7OztJQUk3QixZQUFvQixLQUFpQjtRQUFqQixVQUFLLEdBQUwsS0FBSyxDQUFZO1FBRnJDLFVBQUssR0FBRyxDQUFDLENBQUM7SUFFOEIsQ0FBQzs7OztJQUV6QyxlQUFlO1FBQ2IsVUFBVTs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxhQUFhLENBQUMsS0FBSyxFQUFFLEdBQUUsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO0lBQ2pFLENBQUM7OztZQVpGLFNBQVMsU0FBQzs7Z0JBRVQsUUFBUSxFQUFFLGFBQWE7YUFDeEI7Ozs7WUFMbUIsVUFBVTs7O29CQU8zQixLQUFLLFNBQUMsV0FBVzs7OztJQUFsQixtQ0FDVTs7Ozs7SUFFRSxtQ0FBeUIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBEaXJlY3RpdmUsIEVsZW1lbnRSZWYsIElucHV0LCBBZnRlclZpZXdJbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBEaXJlY3RpdmUoe1xuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGRpcmVjdGl2ZS1zZWxlY3RvclxuICBzZWxlY3RvcjogJ1thdXRvZm9jdXNdJ1xufSlcbmV4cG9ydCBjbGFzcyBBdXRvZm9jdXNEaXJlY3RpdmUgaW1wbGVtZW50cyBBZnRlclZpZXdJbml0IHtcbiAgQElucHV0KCdhdXRvZm9jdXMnKVxuICBkZWxheSA9IDA7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBlbFJlZjogRWxlbWVudFJlZikge31cblxuICBuZ0FmdGVyVmlld0luaXQoKTogdm9pZCB7XG4gICAgc2V0VGltZW91dCgoKSA9PiB0aGlzLmVsUmVmLm5hdGl2ZUVsZW1lbnQuZm9jdXMoKSwgdGhpcy5kZWxheSk7XG4gIH1cbn1cbiJdfQ==
