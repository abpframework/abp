/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input } from '@angular/core';
var ButtonComponent = /** @class */ (function() {
  function ButtonComponent() {
    this.buttonClass = 'btn btn-primary';
    this.loading = false;
    this.disabled = false;
    /**
     * @deprecated Use buttonType instead. To be deleted in v1
     */
    this.type = 'button';
  }
  Object.defineProperty(ButtonComponent.prototype, 'icon', {
    /**
     * @return {?}
     */
    get: function() {
      return '' + (this.loading ? 'fa fa-pulse fa-spinner' : this.iconClass || 'd-none');
    },
    enumerable: true,
    configurable: true,
  });
  ButtonComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-button',
          template:
            '\n    <button [attr.type]="type" [ngClass]="buttonClass" [disabled]="loading || disabled">\n      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>\n    </button>\n  ',
        },
      ],
    },
  ];
  ButtonComponent.propDecorators = {
    buttonClass: [{ type: Input }],
    buttonType: [{ type: Input }],
    iconClass: [{ type: Input }],
    loading: [{ type: Input }],
    disabled: [{ type: Input }],
    type: [{ type: Input }],
  };
  return ButtonComponent;
})();
export { ButtonComponent };
if (false) {
  /** @type {?} */
  ButtonComponent.prototype.buttonClass;
  /** @type {?} */
  ButtonComponent.prototype.buttonType;
  /** @type {?} */
  ButtonComponent.prototype.iconClass;
  /** @type {?} */
  ButtonComponent.prototype.loading;
  /** @type {?} */
  ButtonComponent.prototype.disabled;
  /**
   * @deprecated Use buttonType instead. To be deleted in v1
   * @type {?}
   */
  ButtonComponent.prototype.type;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRWpEO0lBQUE7UUFVRSxnQkFBVyxHQUFHLGlCQUFpQixDQUFDO1FBU2hDLFlBQU8sR0FBRyxLQUFLLENBQUM7UUFHaEIsYUFBUSxHQUFHLEtBQUssQ0FBQzs7OztRQUtSLFNBQUksR0FBRyxRQUFRLENBQUM7SUFLM0IsQ0FBQztJQUhDLHNCQUFJLGlDQUFJOzs7O1FBQVI7WUFDRSxPQUFPLE1BQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsd0JBQXdCLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksUUFBUSxDQUFFLENBQUM7UUFDbkYsQ0FBQzs7O09BQUE7O2dCQS9CRixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLFlBQVk7b0JBQ3RCLFFBQVEsRUFBRSwrTEFJVDtpQkFDRjs7OzhCQUVFLEtBQUs7NkJBR0wsS0FBSzs0QkFHTCxLQUFLOzBCQUdMLEtBQUs7MkJBR0wsS0FBSzt1QkFNTCxLQUFLOztJQUtSLHNCQUFDO0NBQUEsQUFoQ0QsSUFnQ0M7U0F4QlksZUFBZTs7O0lBQzFCLHNDQUNnQzs7SUFFaEMscUNBQ1c7O0lBRVgsb0NBQ2tCOztJQUVsQixrQ0FDZ0I7O0lBRWhCLG1DQUNpQjs7Ozs7SUFLakIsK0JBQXlCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBJbnB1dCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtYnV0dG9uJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8YnV0dG9uIFthdHRyLnR5cGVdPVwidHlwZVwiIFtuZ0NsYXNzXT1cImJ1dHRvbkNsYXNzXCIgW2Rpc2FibGVkXT1cImxvYWRpbmcgfHwgZGlzYWJsZWRcIj5cbiAgICAgIDxpIFtuZ0NsYXNzXT1cImljb25cIiBjbGFzcz1cIm1yLTFcIj48L2k+PG5nLWNvbnRlbnQ+PC9uZy1jb250ZW50PlxuICAgIDwvYnV0dG9uPlxuICBgLFxufSlcbmV4cG9ydCBjbGFzcyBCdXR0b25Db21wb25lbnQge1xuICBASW5wdXQoKVxuICBidXR0b25DbGFzcyA9ICdidG4gYnRuLXByaW1hcnknO1xuXG4gIEBJbnB1dCgpXG4gIGJ1dHRvblR5cGU7IC8vIFRPRE86IEFkZCBpbml0aWFsIHZhbHVlLlxuXG4gIEBJbnB1dCgpXG4gIGljb25DbGFzczogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIGxvYWRpbmcgPSBmYWxzZTtcblxuICBASW5wdXQoKVxuICBkaXNhYmxlZCA9IGZhbHNlO1xuXG4gIC8qKlxuICAgKiBAZGVwcmVjYXRlZCBVc2UgYnV0dG9uVHlwZSBpbnN0ZWFkLiBUbyBiZSBkZWxldGVkIGluIHYxXG4gICAqL1xuICBASW5wdXQoKSB0eXBlID0gJ2J1dHRvbic7XG5cbiAgZ2V0IGljb24oKTogc3RyaW5nIHtcbiAgICByZXR1cm4gYCR7dGhpcy5sb2FkaW5nID8gJ2ZhIGZhLXB1bHNlIGZhLXNwaW5uZXInIDogdGhpcy5pY29uQ2xhc3MgfHwgJ2Qtbm9uZSd9YDtcbiAgfVxufVxuIl19
