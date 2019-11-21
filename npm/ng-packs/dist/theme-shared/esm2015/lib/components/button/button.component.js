/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output, ViewChild, ElementRef, Renderer2 } from '@angular/core';
export class ButtonComponent {
  /**
   * @param {?} renderer
   */
  constructor(renderer) {
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
  /**
   * @return {?}
   */
  get icon() {
    return `${this.loading ? 'fa fa-spinner fa-spin' : this.iconClass || 'd-none'}`;
  }
  /**
   * @return {?}
   */
  ngOnInit() {
    if (this.attributes) {
      Object.keys(this.attributes).forEach(
        /**
         * @param {?} key
         * @return {?}
         */
        key => {
          this.renderer.setAttribute(this.buttonRef.nativeElement, key, this.attributes[key]);
        },
      );
    }
  }
}
ButtonComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-button',
        // tslint:disable-next-line: component-max-inline-declarations
        template: `
    <button
      #button
      [attr.type]="buttonType || type"
      [ngClass]="buttonClass"
      [disabled]="loading || disabled"
      (click)="click.emit($event)"
      (focus)="focus.emit($event)"
      (blur)="blur.emit($event)"
    >
      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>
    </button>
  `,
      },
    ],
  },
];
/** @nocollapse */
ButtonComponent.ctorParameters = () => [{ type: Renderer2 }];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFvQmpILE1BQU0sT0FBTyxlQUFlOzs7O0lBd0MxQixZQUFvQixRQUFtQjtRQUFuQixhQUFRLEdBQVIsUUFBUSxDQUFXO1FBdEN2QyxnQkFBVyxHQUFHLGlCQUFpQixDQUFDO1FBU2hDLFlBQU8sR0FBRyxLQUFLLENBQUM7UUFHaEIsYUFBUSxHQUFHLEtBQUssQ0FBQzs7UUFNRSxVQUFLLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7UUFHdkMsVUFBSyxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7O1FBR3ZDLFNBQUksR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOzs7O1FBUWhELFNBQUksR0FBRyxRQUFRLENBQUM7SUFNaUIsQ0FBQzs7OztJQUozQyxJQUFJLElBQUk7UUFDTixPQUFPLEdBQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsdUJBQXVCLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksUUFBUSxFQUFFLENBQUM7SUFDbEYsQ0FBQzs7OztJQUlELFFBQVE7UUFDTixJQUFJLElBQUksQ0FBQyxVQUFVLEVBQUU7WUFDbkIsTUFBTSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsVUFBVSxDQUFDLENBQUMsT0FBTzs7OztZQUFDLEdBQUcsQ0FBQyxFQUFFO2dCQUN6QyxJQUFJLENBQUMsUUFBUSxDQUFDLFlBQVksQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLGFBQWEsRUFBRSxHQUFHLEVBQUUsSUFBSSxDQUFDLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO1lBQ3RGLENBQUMsRUFBQyxDQUFDO1NBQ0o7SUFDSCxDQUFDOzs7WUFqRUYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxZQUFZOztnQkFFdEIsUUFBUSxFQUFFOzs7Ozs7Ozs7Ozs7R0FZVDthQUNGOzs7O1lBbkJ1RSxTQUFTOzs7MEJBcUI5RSxLQUFLO3lCQUdMLEtBQUs7d0JBR0wsS0FBSztzQkFHTCxLQUFLO3VCQUdMLEtBQUs7eUJBR0wsS0FBSztvQkFJTCxNQUFNO29CQUdOLE1BQU07bUJBR04sTUFBTTt3QkFFTixTQUFTLFNBQUMsUUFBUSxFQUFFLEVBQUUsTUFBTSxFQUFFLElBQUksRUFBRTttQkFNcEMsS0FBSzs7OztJQWpDTixzQ0FDZ0M7O0lBRWhDLHFDQUNXOztJQUVYLG9DQUNrQjs7SUFFbEIsa0NBQ2dCOztJQUVoQixtQ0FDaUI7O0lBRWpCLHFDQUNtQzs7SUFHbkMsZ0NBQTBEOztJQUcxRCxnQ0FBMEQ7O0lBRzFELCtCQUF5RDs7SUFFekQsb0NBQ3lDOzs7OztJQUt6QywrQkFBeUI7Ozs7O0lBTWIsbUNBQTJCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPdXRwdXQsIFZpZXdDaGlsZCwgRWxlbWVudFJlZiwgUmVuZGVyZXIyLCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1idXR0b24nLFxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGNvbXBvbmVudC1tYXgtaW5saW5lLWRlY2xhcmF0aW9uc1xuICB0ZW1wbGF0ZTogYFxuICAgIDxidXR0b25cbiAgICAgICNidXR0b25cbiAgICAgIFthdHRyLnR5cGVdPVwiYnV0dG9uVHlwZSB8fCB0eXBlXCJcbiAgICAgIFtuZ0NsYXNzXT1cImJ1dHRvbkNsYXNzXCJcbiAgICAgIFtkaXNhYmxlZF09XCJsb2FkaW5nIHx8IGRpc2FibGVkXCJcbiAgICAgIChjbGljayk9XCJjbGljay5lbWl0KCRldmVudClcIlxuICAgICAgKGZvY3VzKT1cImZvY3VzLmVtaXQoJGV2ZW50KVwiXG4gICAgICAoYmx1cik9XCJibHVyLmVtaXQoJGV2ZW50KVwiXG4gICAgPlxuICAgICAgPGkgW25nQ2xhc3NdPVwiaWNvblwiIGNsYXNzPVwibXItMVwiPjwvaT48bmctY29udGVudD48L25nLWNvbnRlbnQ+XG4gICAgPC9idXR0b24+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIEJ1dHRvbkNvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIEBJbnB1dCgpXG4gIGJ1dHRvbkNsYXNzID0gJ2J0biBidG4tcHJpbWFyeSc7XG5cbiAgQElucHV0KClcbiAgYnV0dG9uVHlwZTsgLy8gVE9ETzogQWRkIGluaXRpYWwgdmFsdWUuXG5cbiAgQElucHV0KClcbiAgaWNvbkNsYXNzOiBzdHJpbmc7XG5cbiAgQElucHV0KClcbiAgbG9hZGluZyA9IGZhbHNlO1xuXG4gIEBJbnB1dCgpXG4gIGRpc2FibGVkID0gZmFsc2U7XG5cbiAgQElucHV0KClcbiAgYXR0cmlidXRlczogQUJQLkRpY3Rpb25hcnk8c3RyaW5nPjtcblxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGNsaWNrID0gbmV3IEV2ZW50RW1pdHRlcjxNb3VzZUV2ZW50PigpO1xuXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxuICBAT3V0cHV0KCkgcmVhZG9ubHkgZm9jdXMgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XG5cbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXG4gIEBPdXRwdXQoKSByZWFkb25seSBibHVyID0gbmV3IEV2ZW50RW1pdHRlcjxGb2N1c0V2ZW50PigpO1xuXG4gIEBWaWV3Q2hpbGQoJ2J1dHRvbicsIHsgc3RhdGljOiB0cnVlIH0pXG4gIGJ1dHRvblJlZjogRWxlbWVudFJlZjxIVE1MQnV0dG9uRWxlbWVudD47XG5cbiAgLyoqXG4gICAqIEBkZXByZWNhdGVkIFVzZSBidXR0b25UeXBlIGluc3RlYWQuIFRvIGJlIGRlbGV0ZWQgaW4gdjFcbiAgICovXG4gIEBJbnB1dCgpIHR5cGUgPSAnYnV0dG9uJztcblxuICBnZXQgaWNvbigpOiBzdHJpbmcge1xuICAgIHJldHVybiBgJHt0aGlzLmxvYWRpbmcgPyAnZmEgZmEtc3Bpbm5lciBmYS1zcGluJyA6IHRoaXMuaWNvbkNsYXNzIHx8ICdkLW5vbmUnfWA7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgaWYgKHRoaXMuYXR0cmlidXRlcykge1xuICAgICAgT2JqZWN0LmtleXModGhpcy5hdHRyaWJ1dGVzKS5mb3JFYWNoKGtleSA9PiB7XG4gICAgICAgIHRoaXMucmVuZGVyZXIuc2V0QXR0cmlidXRlKHRoaXMuYnV0dG9uUmVmLm5hdGl2ZUVsZW1lbnQsIGtleSwgdGhpcy5hdHRyaWJ1dGVzW2tleV0pO1xuICAgICAgfSk7XG4gICAgfVxuICB9XG59XG4iXX0=
