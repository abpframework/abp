/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output, ViewChild, ElementRef, Renderer2 } from '@angular/core';
var ButtonComponent = /** @class */ (function() {
  function ButtonComponent(renderer) {
    this.renderer = renderer;
    this.buttonClass = 'btn btn-primary';
    this.loading = false;
    this.disabled = false;
    // tslint:disable-next-line: no-output-native
    this.click = new EventEmitter();
    // tslint:disable-next-line: no-output-native
    this.focus = new EventEmitter();
    // tslint:disable-next-line: no-output-native
    this.blur = new EventEmitter();
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
      return '' + (this.loading ? 'fa fa-spinner fa-spin' : this.iconClass || 'd-none');
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @return {?}
   */
  ButtonComponent.prototype.ngOnInit
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    if (this.attributes) {
      Object.keys(this.attributes).forEach(
        /**
         * @param {?} key
         * @return {?}
         */
        function(key) {
          _this.renderer.setAttribute(_this.buttonRef.nativeElement, key, _this.attributes[key]);
        },
      );
    }
  };
  ButtonComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-button',
          // tslint:disable-next-line: component-max-inline-declarations
          template:
            '\n    <button\n      #button\n      [attr.type]="buttonType || type"\n      [ngClass]="buttonClass"\n      [disabled]="loading || disabled"\n      (click)="click.emit($event)"\n      (focus)="focus.emit($event)"\n      (blur)="blur.emit($event)"\n    >\n      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>\n    </button>\n  ',
        },
      ],
    },
  ];
  /** @nocollapse */
  ButtonComponent.ctorParameters = function() {
    return [{ type: Renderer2 }];
  };
  ButtonComponent.propDecorators = {
    buttonClass: [{ type: Input }],
    buttonType: [{ type: Input }],
    iconClass: [{ type: Input }],
    loading: [{ type: Input }],
    disabled: [{ type: Input }],
    attributes: [{ type: Input }],
    click: [{ type: Output }],
    focus: [{ type: Output }],
    blur: [{ type: Output }],
    buttonRef: [{ type: ViewChild, args: ['button', { static: true }] }],
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
  /** @type {?} */
  ButtonComponent.prototype.attributes;
  /** @type {?} */
  ButtonComponent.prototype.click;
  /** @type {?} */
  ButtonComponent.prototype.focus;
  /** @type {?} */
  ButtonComponent.prototype.blur;
  /** @type {?} */
  ButtonComponent.prototype.buttonRef;
  /**
   * @deprecated Use buttonType instead. To be deleted in v1
   * @type {?}
   */
  ButtonComponent.prototype.type;
  /**
   * @type {?}
   * @private
   */
  ButtonComponent.prototype.renderer;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFHakg7SUF5REUseUJBQW9CLFFBQW1CO1FBQW5CLGFBQVEsR0FBUixRQUFRLENBQVc7UUF0Q3ZDLGdCQUFXLEdBQUcsaUJBQWlCLENBQUM7UUFTaEMsWUFBTyxHQUFHLEtBQUssQ0FBQztRQUdoQixhQUFRLEdBQUcsS0FBSyxDQUFDOztRQU1FLFVBQUssR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOztRQUd2QyxVQUFLLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7UUFHdkMsU0FBSSxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7Ozs7UUFRaEQsU0FBSSxHQUFHLFFBQVEsQ0FBQztJQU1pQixDQUFDO0lBSjNDLHNCQUFJLGlDQUFJOzs7O1FBQVI7WUFDRSxPQUFPLE1BQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsdUJBQXVCLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksUUFBUSxDQUFFLENBQUM7UUFDbEYsQ0FBQzs7O09BQUE7Ozs7SUFJRCxrQ0FBUTs7O0lBQVI7UUFBQSxpQkFNQztRQUxDLElBQUksSUFBSSxDQUFDLFVBQVUsRUFBRTtZQUNuQixNQUFNLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsQ0FBQyxPQUFPOzs7O1lBQUMsVUFBQSxHQUFHO2dCQUN0QyxLQUFJLENBQUMsUUFBUSxDQUFDLFlBQVksQ0FBQyxLQUFJLENBQUMsU0FBUyxDQUFDLGFBQWEsRUFBRSxHQUFHLEVBQUUsS0FBSSxDQUFDLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO1lBQ3RGLENBQUMsRUFBQyxDQUFDO1NBQ0o7SUFDSCxDQUFDOztnQkFqRUYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxZQUFZOztvQkFFdEIsUUFBUSxFQUFFLHVXQVlUO2lCQUNGOzs7O2dCQW5CdUUsU0FBUzs7OzhCQXFCOUUsS0FBSzs2QkFHTCxLQUFLOzRCQUdMLEtBQUs7MEJBR0wsS0FBSzsyQkFHTCxLQUFLOzZCQUdMLEtBQUs7d0JBSUwsTUFBTTt3QkFHTixNQUFNO3VCQUdOLE1BQU07NEJBRU4sU0FBUyxTQUFDLFFBQVEsRUFBRSxFQUFFLE1BQU0sRUFBRSxJQUFJLEVBQUU7dUJBTXBDLEtBQUs7O0lBZVIsc0JBQUM7Q0FBQSxBQWxFRCxJQWtFQztTQWpEWSxlQUFlOzs7SUFDMUIsc0NBQ2dDOztJQUVoQyxxQ0FDVzs7SUFFWCxvQ0FDa0I7O0lBRWxCLGtDQUNnQjs7SUFFaEIsbUNBQ2lCOztJQUVqQixxQ0FDbUM7O0lBR25DLGdDQUEwRDs7SUFHMUQsZ0NBQTBEOztJQUcxRCwrQkFBeUQ7O0lBRXpELG9DQUN5Qzs7Ozs7SUFLekMsK0JBQXlCOzs7OztJQU1iLG1DQUEyQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0LCBWaWV3Q2hpbGQsIEVsZW1lbnRSZWYsIFJlbmRlcmVyMiwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtYnV0dG9uJyxcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtbWF4LWlubGluZS1kZWNsYXJhdGlvbnNcbiAgdGVtcGxhdGU6IGBcbiAgICA8YnV0dG9uXG4gICAgICAjYnV0dG9uXG4gICAgICBbYXR0ci50eXBlXT1cImJ1dHRvblR5cGUgfHwgdHlwZVwiXG4gICAgICBbbmdDbGFzc109XCJidXR0b25DbGFzc1wiXG4gICAgICBbZGlzYWJsZWRdPVwibG9hZGluZyB8fCBkaXNhYmxlZFwiXG4gICAgICAoY2xpY2spPVwiY2xpY2suZW1pdCgkZXZlbnQpXCJcbiAgICAgIChmb2N1cyk9XCJmb2N1cy5lbWl0KCRldmVudClcIlxuICAgICAgKGJsdXIpPVwiYmx1ci5lbWl0KCRldmVudClcIlxuICAgID5cbiAgICAgIDxpIFtuZ0NsYXNzXT1cImljb25cIiBjbGFzcz1cIm1yLTFcIj48L2k+PG5nLWNvbnRlbnQ+PC9uZy1jb250ZW50PlxuICAgIDwvYnV0dG9uPlxuICBgLFxufSlcbmV4cG9ydCBjbGFzcyBCdXR0b25Db21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBASW5wdXQoKVxuICBidXR0b25DbGFzcyA9ICdidG4gYnRuLXByaW1hcnknO1xuXG4gIEBJbnB1dCgpXG4gIGJ1dHRvblR5cGU7IC8vIFRPRE86IEFkZCBpbml0aWFsIHZhbHVlLlxuXG4gIEBJbnB1dCgpXG4gIGljb25DbGFzczogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIGxvYWRpbmcgPSBmYWxzZTtcblxuICBASW5wdXQoKVxuICBkaXNhYmxlZCA9IGZhbHNlO1xuXG4gIEBJbnB1dCgpXG4gIGF0dHJpYnV0ZXM6IEFCUC5EaWN0aW9uYXJ5PHN0cmluZz47XG5cbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXG4gIEBPdXRwdXQoKSByZWFkb25seSBjbGljayA9IG5ldyBFdmVudEVtaXR0ZXI8TW91c2VFdmVudD4oKTtcblxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGZvY3VzID0gbmV3IEV2ZW50RW1pdHRlcjxGb2N1c0V2ZW50PigpO1xuXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxuICBAT3V0cHV0KCkgcmVhZG9ubHkgYmx1ciA9IG5ldyBFdmVudEVtaXR0ZXI8Rm9jdXNFdmVudD4oKTtcblxuICBAVmlld0NoaWxkKCdidXR0b24nLCB7IHN0YXRpYzogdHJ1ZSB9KVxuICBidXR0b25SZWY6IEVsZW1lbnRSZWY8SFRNTEJ1dHRvbkVsZW1lbnQ+O1xuXG4gIC8qKlxuICAgKiBAZGVwcmVjYXRlZCBVc2UgYnV0dG9uVHlwZSBpbnN0ZWFkLiBUbyBiZSBkZWxldGVkIGluIHYxXG4gICAqL1xuICBASW5wdXQoKSB0eXBlID0gJ2J1dHRvbic7XG5cbiAgZ2V0IGljb24oKTogc3RyaW5nIHtcbiAgICByZXR1cm4gYCR7dGhpcy5sb2FkaW5nID8gJ2ZhIGZhLXNwaW5uZXIgZmEtc3BpbicgOiB0aGlzLmljb25DbGFzcyB8fCAnZC1ub25lJ31gO1xuICB9XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxuXG4gIG5nT25Jbml0KCkge1xuICAgIGlmICh0aGlzLmF0dHJpYnV0ZXMpIHtcbiAgICAgIE9iamVjdC5rZXlzKHRoaXMuYXR0cmlidXRlcykuZm9yRWFjaChrZXkgPT4ge1xuICAgICAgICB0aGlzLnJlbmRlcmVyLnNldEF0dHJpYnV0ZSh0aGlzLmJ1dHRvblJlZi5uYXRpdmVFbGVtZW50LCBrZXksIHRoaXMuYXR0cmlidXRlc1trZXldKTtcbiAgICAgIH0pO1xuICAgIH1cbiAgfVxufVxuIl19
