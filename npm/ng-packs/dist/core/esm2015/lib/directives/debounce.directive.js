/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Directive, Output, Renderer2, ElementRef, EventEmitter, Input } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { takeUntilDestroy } from '@ngx-validate/core';
export class InputEventDebounceDirective {
  /**
   * @param {?} renderer
   * @param {?} el
   */
  constructor(renderer, el) {
    this.renderer = renderer;
    this.el = el;
    this.debounce = 300;
    this.debounceEvent = new EventEmitter();
  }
  /**
   * @return {?}
   */
  ngOnInit() {
    fromEvent(this.el.nativeElement, 'input')
      .pipe(
        debounceTime(this.debounce),
        takeUntilDestroy(this),
      )
      .subscribe(
        /**
         * @param {?} event
         * @return {?}
         */
        event => {
          this.debounceEvent.emit(event);
        },
      );
  }
}
InputEventDebounceDirective.decorators = [
  {
    type: Directive,
    args: [
      {
        // tslint:disable-next-line: directive-selector
        selector: '[input.debounce]',
      },
    ],
  },
];
/** @nocollapse */
InputEventDebounceDirective.ctorParameters = () => [{ type: Renderer2 }, { type: ElementRef }];
InputEventDebounceDirective.propDecorators = {
  debounce: [{ type: Input }],
  debounceEvent: [{ type: Output, args: ['input.debounce'] }],
};
if (false) {
  /** @type {?} */
  InputEventDebounceDirective.prototype.debounce;
  /** @type {?} */
  InputEventDebounceDirective.prototype.debounceEvent;
  /**
   * @type {?}
   * @private
   */
  InputEventDebounceDirective.prototype.renderer;
  /**
   * @type {?}
   * @private
   */
  InputEventDebounceDirective.prototype.el;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZGVib3VuY2UuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZGVib3VuY2UuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sRUFBRSxTQUFTLEVBQUUsVUFBVSxFQUFVLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDdEcsT0FBTyxFQUFFLFNBQVMsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqQyxPQUFPLEVBQUUsWUFBWSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDOUMsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFNdEQsTUFBTSxPQUFPLDJCQUEyQjs7Ozs7SUFLdEMsWUFBb0IsUUFBbUIsRUFBVSxFQUFjO1FBQTNDLGFBQVEsR0FBUixRQUFRLENBQVc7UUFBVSxPQUFFLEdBQUYsRUFBRSxDQUFZO1FBSnRELGFBQVEsR0FBRyxHQUFHLENBQUM7UUFFVyxrQkFBYSxHQUFHLElBQUksWUFBWSxFQUFTLENBQUM7SUFFWCxDQUFDOzs7O0lBRW5FLFFBQVE7UUFDTixTQUFTLENBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxhQUFhLEVBQUUsT0FBTyxDQUFDO2FBQ3RDLElBQUksQ0FDSCxZQUFZLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUMzQixnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsQ0FDdkI7YUFDQSxTQUFTOzs7O1FBQUMsQ0FBQyxLQUFZLEVBQUUsRUFBRTtZQUMxQixJQUFJLENBQUMsYUFBYSxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztRQUNqQyxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7OztZQXBCRixTQUFTLFNBQUM7O2dCQUVULFFBQVEsRUFBRSxrQkFBa0I7YUFDN0I7Ozs7WUFSMkIsU0FBUztZQUFFLFVBQVU7Ozt1QkFVOUMsS0FBSzs0QkFFTCxNQUFNLFNBQUMsZ0JBQWdCOzs7O0lBRnhCLCtDQUF3Qjs7SUFFeEIsb0RBQTZFOzs7OztJQUVqRSwrQ0FBMkI7Ozs7O0lBQUUseUNBQXNCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgRGlyZWN0aXZlLCBPdXRwdXQsIFJlbmRlcmVyMiwgRWxlbWVudFJlZiwgT25Jbml0LCBFdmVudEVtaXR0ZXIsIElucHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBmcm9tRXZlbnQgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGRlYm91bmNlVGltZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuXG5ARGlyZWN0aXZlKHtcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBkaXJlY3RpdmUtc2VsZWN0b3JcbiAgc2VsZWN0b3I6ICdbaW5wdXQuZGVib3VuY2VdJ1xufSlcbmV4cG9ydCBjbGFzcyBJbnB1dEV2ZW50RGVib3VuY2VEaXJlY3RpdmUgaW1wbGVtZW50cyBPbkluaXQge1xuICBASW5wdXQoKSBkZWJvdW5jZSA9IDMwMDtcblxuICBAT3V0cHV0KCdpbnB1dC5kZWJvdW5jZScpIHJlYWRvbmx5IGRlYm91bmNlRXZlbnQgPSBuZXcgRXZlbnRFbWl0dGVyPEV2ZW50PigpO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMiwgcHJpdmF0ZSBlbDogRWxlbWVudFJlZikge31cblxuICBuZ09uSW5pdCgpOiB2b2lkIHtcbiAgICBmcm9tRXZlbnQodGhpcy5lbC5uYXRpdmVFbGVtZW50LCAnaW5wdXQnKVxuICAgICAgLnBpcGUoXG4gICAgICAgIGRlYm91bmNlVGltZSh0aGlzLmRlYm91bmNlKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKVxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoZXZlbnQ6IEV2ZW50KSA9PiB7XG4gICAgICAgIHRoaXMuZGVib3VuY2VFdmVudC5lbWl0KGV2ZW50KTtcbiAgICAgIH0pO1xuICB9XG59XG4iXX0=
