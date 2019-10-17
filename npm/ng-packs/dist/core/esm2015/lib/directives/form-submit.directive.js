/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { ChangeDetectorRef, Directive, ElementRef, EventEmitter, Input, Output, Self } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';
import { fromEvent } from 'rxjs';
import { debounceTime, filter } from 'rxjs/operators';
import { takeUntilDestroy } from '../utils';
export class FormSubmitDirective {
  /**
   * @param {?} formGroupDirective
   * @param {?} host
   * @param {?} cdRef
   */
  constructor(formGroupDirective, host, cdRef) {
    this.formGroupDirective = formGroupDirective;
    this.host = host;
    this.cdRef = cdRef;
    this.ngSubmit = new EventEmitter();
    this.executedNgSubmit = false;
  }
  /**
   * @return {?}
   */
  ngOnInit() {
    this.formGroupDirective.ngSubmit.pipe(takeUntilDestroy(this)).subscribe(
      /**
       * @return {?}
       */
      () => {
        this.markAsDirty();
        this.executedNgSubmit = true;
      },
    );
    fromEvent(/** @type {?} */ (this.host.nativeElement), 'keyup')
      .pipe(
        debounceTime(200),
        filter(
          /**
           * @param {?} key
           * @return {?}
           */
          key => key && key.key === 'Enter',
        ),
        takeUntilDestroy(this),
      )
      .subscribe(
        /**
         * @return {?}
         */
        () => {
          if (!this.executedNgSubmit) {
            this.host.nativeElement.dispatchEvent(new Event('submit', { bubbles: true, cancelable: true }));
          }
          this.executedNgSubmit = false;
        },
      );
    fromEvent(this.host.nativeElement, 'submit')
      .pipe(
        takeUntilDestroy(this),
        filter(
          /**
           * @return {?}
           */
          () => !this.notValidateOnSubmit && typeof this.notValidateOnSubmit !== 'string',
        ),
      )
      .subscribe(
        /**
         * @return {?}
         */
        () => {
          if (!this.executedNgSubmit) {
            this.markAsDirty();
          }
        },
      );
  }
  /**
   * @return {?}
   */
  ngOnDestroy() {}
  /**
   * @return {?}
   */
  markAsDirty() {
    const { form } = this.formGroupDirective;
    setDirty(/** @type {?} */ (form.controls));
    form.markAsDirty();
    this.cdRef.detectChanges();
  }
}
FormSubmitDirective.decorators = [
  {
    type: Directive,
    args: [
      {
        // tslint:disable-next-line: directive-selector
        selector: 'form[ngSubmit][formGroup]',
      },
    ],
  },
];
/** @nocollapse */
FormSubmitDirective.ctorParameters = () => [
  { type: FormGroupDirective, decorators: [{ type: Self }] },
  { type: ElementRef },
  { type: ChangeDetectorRef },
];
FormSubmitDirective.propDecorators = {
  notValidateOnSubmit: [{ type: Input }],
  ngSubmit: [{ type: Output }],
};
if (false) {
  /** @type {?} */
  FormSubmitDirective.prototype.notValidateOnSubmit;
  /** @type {?} */
  FormSubmitDirective.prototype.ngSubmit;
  /** @type {?} */
  FormSubmitDirective.prototype.executedNgSubmit;
  /**
   * @type {?}
   * @private
   */
  FormSubmitDirective.prototype.formGroupDirective;
  /**
   * @type {?}
   * @private
   */
  FormSubmitDirective.prototype.host;
  /**
   * @type {?}
   * @private
   */
  FormSubmitDirective.prototype.cdRef;
}
/**
 * @param {?} controls
 * @return {?}
 */
function setDirty(controls) {
  if (Array.isArray(controls)) {
    controls.forEach(
      /**
       * @param {?} group
       * @return {?}
       */
      group => {
        setDirty(/** @type {?} */ (group.controls));
      },
    );
    return;
  }
  Object.keys(controls).forEach(
    /**
     * @param {?} key
     * @return {?}
     */
    key => {
      controls[key].markAsDirty();
      controls[key].updateValueAndValidity();
    },
  );
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiZm9ybS1zdWJtaXQuZGlyZWN0aXZlLmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5jb3JlLyIsInNvdXJjZXMiOlsibGliL2RpcmVjdGl2ZXMvZm9ybS1zdWJtaXQuZGlyZWN0aXZlLnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQ0wsaUJBQWlCLEVBQ2pCLFNBQVMsRUFDVCxVQUFVLEVBQ1YsWUFBWSxFQUNaLEtBQUssRUFHTCxNQUFNLEVBQ04sSUFBSSxFQUNMLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBMEIsa0JBQWtCLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUM1RSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2pDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdEQsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sVUFBVSxDQUFDO0FBUTVDLE1BQU0sT0FBTyxtQkFBbUI7Ozs7OztJQVE5QixZQUNrQixrQkFBc0MsRUFDOUMsSUFBaUMsRUFDakMsS0FBd0I7UUFGaEIsdUJBQWtCLEdBQWxCLGtCQUFrQixDQUFvQjtRQUM5QyxTQUFJLEdBQUosSUFBSSxDQUE2QjtRQUNqQyxVQUFLLEdBQUwsS0FBSyxDQUFtQjtRQVBmLGFBQVEsR0FBRyxJQUFJLFlBQVksRUFBRSxDQUFDO1FBRWpELHFCQUFnQixHQUFHLEtBQUssQ0FBQztJQU10QixDQUFDOzs7O0lBRUosUUFBUTtRQUNOLElBQUksQ0FBQyxrQkFBa0IsQ0FBQyxRQUFRLENBQUMsSUFBSSxDQUFDLGdCQUFnQixDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsU0FBUzs7O1FBQUMsR0FBRyxFQUFFO1lBQzNFLElBQUksQ0FBQyxXQUFXLEVBQUUsQ0FBQztZQUNuQixJQUFJLENBQUMsZ0JBQWdCLEdBQUcsSUFBSSxDQUFDO1FBQy9CLENBQUMsRUFBQyxDQUFDO1FBRUgsU0FBUyxDQUFDLG1CQUFBLElBQUksQ0FBQyxJQUFJLENBQUMsYUFBYSxFQUFlLEVBQUUsT0FBTyxDQUFDO2FBQ3ZELElBQUksQ0FDSCxZQUFZLENBQUMsR0FBRyxDQUFDLEVBQ2pCLE1BQU07Ozs7UUFBQyxDQUFDLEdBQWtCLEVBQUUsRUFBRSxDQUFDLEdBQUcsSUFBSSxHQUFHLENBQUMsR0FBRyxLQUFLLE9BQU8sRUFBQyxFQUMxRCxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsQ0FDdkI7YUFDQSxTQUFTOzs7UUFBQyxHQUFHLEVBQUU7WUFDZCxJQUFJLENBQUMsSUFBSSxDQUFDLGdCQUFnQixFQUFFO2dCQUMxQixJQUFJLENBQUMsSUFBSSxDQUFDLGFBQWEsQ0FBQyxhQUFhLENBQUMsSUFBSSxLQUFLLENBQUMsUUFBUSxFQUFFLEVBQUUsT0FBTyxFQUFFLElBQUksRUFBRSxVQUFVLEVBQUUsSUFBSSxFQUFFLENBQUMsQ0FBQyxDQUFDO2FBQ2pHO1lBRUQsSUFBSSxDQUFDLGdCQUFnQixHQUFHLEtBQUssQ0FBQztRQUNoQyxDQUFDLEVBQUMsQ0FBQztRQUVMLFNBQVMsQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLGFBQWEsRUFBRSxRQUFRLENBQUM7YUFDekMsSUFBSSxDQUNILGdCQUFnQixDQUFDLElBQUksQ0FBQyxFQUN0QixNQUFNOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxDQUFDLElBQUksQ0FBQyxtQkFBbUIsSUFBSSxPQUFPLElBQUksQ0FBQyxtQkFBbUIsS0FBSyxRQUFRLEVBQUMsQ0FDeEY7YUFDQSxTQUFTOzs7UUFBQyxHQUFHLEVBQUU7WUFDZCxJQUFJLENBQUMsSUFBSSxDQUFDLGdCQUFnQixFQUFFO2dCQUMxQixJQUFJLENBQUMsV0FBVyxFQUFFLENBQUM7YUFDcEI7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7SUFFRCxXQUFXLEtBQVUsQ0FBQzs7OztJQUV0QixXQUFXO2NBQ0gsRUFBRSxJQUFJLEVBQUUsR0FBRyxJQUFJLENBQUMsa0JBQWtCO1FBRXhDLFFBQVEsQ0FBQyxtQkFBQSxJQUFJLENBQUMsUUFBUSxFQUFrQyxDQUFDLENBQUM7UUFDMUQsSUFBSSxDQUFDLFdBQVcsRUFBRSxDQUFDO1FBRW5CLElBQUksQ0FBQyxLQUFLLENBQUMsYUFBYSxFQUFFLENBQUM7SUFDN0IsQ0FBQzs7O1lBM0RGLFNBQVMsU0FBQzs7Z0JBRVQsUUFBUSxFQUFFLDJCQUEyQjthQUN0Qzs7OztZQVZnQyxrQkFBa0IsdUJBb0I5QyxJQUFJO1lBNUJQLFVBQVU7WUFGVixpQkFBaUI7OztrQ0FzQmhCLEtBQUs7dUJBR0wsTUFBTTs7OztJQUhQLGtEQUNzQzs7SUFFdEMsdUNBQWlEOztJQUVqRCwrQ0FBeUI7Ozs7O0lBR3ZCLGlEQUFzRDs7Ozs7SUFDdEQsbUNBQXlDOzs7OztJQUN6QyxvQ0FBZ0M7Ozs7OztBQStDcEMsU0FBUyxRQUFRLENBQUMsUUFBa0I7SUFDbEMsSUFBSSxLQUFLLENBQUMsT0FBTyxDQUFDLFFBQVEsQ0FBQyxFQUFFO1FBQzNCLFFBQVEsQ0FBQyxPQUFPOzs7O1FBQUMsS0FBSyxDQUFDLEVBQUU7WUFDdkIsUUFBUSxDQUFDLG1CQUFBLEtBQUssQ0FBQyxRQUFRLEVBQWtDLENBQUMsQ0FBQztRQUM3RCxDQUFDLEVBQUMsQ0FBQztRQUNILE9BQU87S0FDUjtJQUVELE1BQU0sQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLENBQUMsT0FBTzs7OztJQUFDLEdBQUcsQ0FBQyxFQUFFO1FBQ2xDLFFBQVEsQ0FBQyxHQUFHLENBQUMsQ0FBQyxXQUFXLEVBQUUsQ0FBQztRQUM1QixRQUFRLENBQUMsR0FBRyxDQUFDLENBQUMsc0JBQXNCLEVBQUUsQ0FBQztJQUN6QyxDQUFDLEVBQUMsQ0FBQztBQUNMLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBDaGFuZ2VEZXRlY3RvclJlZixcbiAgRGlyZWN0aXZlLFxuICBFbGVtZW50UmVmLFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkRlc3Ryb3ksXG4gIE9uSW5pdCxcbiAgT3V0cHV0LFxuICBTZWxmXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUNvbnRyb2wsIEZvcm1Hcm91cCwgRm9ybUdyb3VwRGlyZWN0aXZlIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgZnJvbUV2ZW50IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUsIGZpbHRlciB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IHRha2VVbnRpbERlc3Ryb3kgfSBmcm9tICcuLi91dGlscyc7XG5cbnR5cGUgQ29udHJvbHMgPSB7IFtrZXk6IHN0cmluZ106IEZvcm1Db250cm9sIH0gfCBGb3JtR3JvdXBbXTtcblxuQERpcmVjdGl2ZSh7XG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogZGlyZWN0aXZlLXNlbGVjdG9yXG4gIHNlbGVjdG9yOiAnZm9ybVtuZ1N1Ym1pdF1bZm9ybUdyb3VwXSdcbn0pXG5leHBvcnQgY2xhc3MgRm9ybVN1Ym1pdERpcmVjdGl2ZSBpbXBsZW1lbnRzIE9uSW5pdCwgT25EZXN0cm95IHtcbiAgQElucHV0KClcbiAgbm90VmFsaWRhdGVPblN1Ym1pdDogc3RyaW5nIHwgYm9vbGVhbjtcblxuICBAT3V0cHV0KCkgcmVhZG9ubHkgbmdTdWJtaXQgPSBuZXcgRXZlbnRFbWl0dGVyKCk7XG5cbiAgZXhlY3V0ZWROZ1N1Ym1pdCA9IGZhbHNlO1xuXG4gIGNvbnN0cnVjdG9yKFxuICAgIEBTZWxmKCkgcHJpdmF0ZSBmb3JtR3JvdXBEaXJlY3RpdmU6IEZvcm1Hcm91cERpcmVjdGl2ZSxcbiAgICBwcml2YXRlIGhvc3Q6IEVsZW1lbnRSZWY8SFRNTEZvcm1FbGVtZW50PixcbiAgICBwcml2YXRlIGNkUmVmOiBDaGFuZ2VEZXRlY3RvclJlZlxuICApIHt9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgdGhpcy5mb3JtR3JvdXBEaXJlY3RpdmUubmdTdWJtaXQucGlwZSh0YWtlVW50aWxEZXN0cm95KHRoaXMpKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgdGhpcy5tYXJrQXNEaXJ0eSgpO1xuICAgICAgdGhpcy5leGVjdXRlZE5nU3VibWl0ID0gdHJ1ZTtcbiAgICB9KTtcblxuICAgIGZyb21FdmVudCh0aGlzLmhvc3QubmF0aXZlRWxlbWVudCBhcyBIVE1MRWxlbWVudCwgJ2tleXVwJylcbiAgICAgIC5waXBlKFxuICAgICAgICBkZWJvdW5jZVRpbWUoMjAwKSxcbiAgICAgICAgZmlsdGVyKChrZXk6IEtleWJvYXJkRXZlbnQpID0+IGtleSAmJiBrZXkua2V5ID09PSAnRW50ZXInKSxcbiAgICAgICAgdGFrZVVudGlsRGVzdHJveSh0aGlzKVxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIGlmICghdGhpcy5leGVjdXRlZE5nU3VibWl0KSB7XG4gICAgICAgICAgdGhpcy5ob3N0Lm5hdGl2ZUVsZW1lbnQuZGlzcGF0Y2hFdmVudChuZXcgRXZlbnQoJ3N1Ym1pdCcsIHsgYnViYmxlczogdHJ1ZSwgY2FuY2VsYWJsZTogdHJ1ZSB9KSk7XG4gICAgICAgIH1cblxuICAgICAgICB0aGlzLmV4ZWN1dGVkTmdTdWJtaXQgPSBmYWxzZTtcbiAgICAgIH0pO1xuXG4gICAgZnJvbUV2ZW50KHRoaXMuaG9zdC5uYXRpdmVFbGVtZW50LCAnc3VibWl0JylcbiAgICAgIC5waXBlKFxuICAgICAgICB0YWtlVW50aWxEZXN0cm95KHRoaXMpLFxuICAgICAgICBmaWx0ZXIoKCkgPT4gIXRoaXMubm90VmFsaWRhdGVPblN1Ym1pdCAmJiB0eXBlb2YgdGhpcy5ub3RWYWxpZGF0ZU9uU3VibWl0ICE9PSAnc3RyaW5nJylcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICBpZiAoIXRoaXMuZXhlY3V0ZWROZ1N1Ym1pdCkge1xuICAgICAgICAgIHRoaXMubWFya0FzRGlydHkoKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cblxuICBuZ09uRGVzdHJveSgpOiB2b2lkIHt9XG5cbiAgbWFya0FzRGlydHkoKSB7XG4gICAgY29uc3QgeyBmb3JtIH0gPSB0aGlzLmZvcm1Hcm91cERpcmVjdGl2ZTtcblxuICAgIHNldERpcnR5KGZvcm0uY29udHJvbHMgYXMgeyBba2V5OiBzdHJpbmddOiBGb3JtQ29udHJvbCB9KTtcbiAgICBmb3JtLm1hcmtBc0RpcnR5KCk7XG5cbiAgICB0aGlzLmNkUmVmLmRldGVjdENoYW5nZXMoKTtcbiAgfVxufVxuXG5mdW5jdGlvbiBzZXREaXJ0eShjb250cm9sczogQ29udHJvbHMpIHtcbiAgaWYgKEFycmF5LmlzQXJyYXkoY29udHJvbHMpKSB7XG4gICAgY29udHJvbHMuZm9yRWFjaChncm91cCA9PiB7XG4gICAgICBzZXREaXJ0eShncm91cC5jb250cm9scyBhcyB7IFtrZXk6IHN0cmluZ106IEZvcm1Db250cm9sIH0pO1xuICAgIH0pO1xuICAgIHJldHVybjtcbiAgfVxuXG4gIE9iamVjdC5rZXlzKGNvbnRyb2xzKS5mb3JFYWNoKGtleSA9PiB7XG4gICAgY29udHJvbHNba2V5XS5tYXJrQXNEaXJ0eSgpO1xuICAgIGNvbnRyb2xzW2tleV0udXBkYXRlVmFsdWVBbmRWYWxpZGl0eSgpO1xuICB9KTtcbn1cbiJdfQ==
