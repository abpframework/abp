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
            Object.keys(this.attributes).forEach((/**
             * @param {?} key
             * @return {?}
             */
            key => {
                this.renderer.setAttribute(this.buttonRef.nativeElement, key, this.attributes[key]);
            }));
        }
    }
}
ButtonComponent.decorators = [
    { type: Component, args: [{
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
  `
            }] }
];
/** @nocollapse */
ButtonComponent.ctorParameters = () => [
    { type: Renderer2 }
];
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
    buttonRef: [{ type: ViewChild, args: ['button', { static: true },] }],
    type: [{ type: Input }]
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFvQmpILE1BQU0sT0FBTyxlQUFlOzs7O0lBd0MxQixZQUFvQixRQUFtQjtRQUFuQixhQUFRLEdBQVIsUUFBUSxDQUFXO1FBdEN2QyxnQkFBVyxHQUFHLGlCQUFpQixDQUFDO1FBU2hDLFlBQU8sR0FBRyxLQUFLLENBQUM7UUFHaEIsYUFBUSxHQUFHLEtBQUssQ0FBQzs7UUFNRSxVQUFLLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7UUFHdkMsVUFBSyxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7O1FBR3ZDLFNBQUksR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOzs7O1FBUWhELFNBQUksR0FBRyxRQUFRLENBQUM7SUFNaUIsQ0FBQzs7OztJQUozQyxJQUFJLElBQUk7UUFDTixPQUFPLEdBQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsdUJBQXVCLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksUUFBUSxFQUFFLENBQUM7SUFDbEYsQ0FBQzs7OztJQUlELFFBQVE7UUFDTixJQUFJLElBQUksQ0FBQyxVQUFVLEVBQUU7WUFDbkIsTUFBTSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsVUFBVSxDQUFDLENBQUMsT0FBTzs7OztZQUFDLEdBQUcsQ0FBQyxFQUFFO2dCQUN6QyxJQUFJLENBQUMsUUFBUSxDQUFDLFlBQVksQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLGFBQWEsRUFBRSxHQUFHLEVBQUUsSUFBSSxDQUFDLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO1lBQ3RGLENBQUMsRUFBQyxDQUFDO1NBQ0o7SUFDSCxDQUFDOzs7WUFqRUYsU0FBUyxTQUFDO2dCQUNULFFBQVEsRUFBRSxZQUFZOztnQkFFdEIsUUFBUSxFQUFFOzs7Ozs7Ozs7Ozs7R0FZVDthQUNGOzs7O1lBbkJ1RSxTQUFTOzs7MEJBcUI5RSxLQUFLO3lCQUdMLEtBQUs7d0JBR0wsS0FBSztzQkFHTCxLQUFLO3VCQUdMLEtBQUs7eUJBR0wsS0FBSztvQkFJTCxNQUFNO29CQUdOLE1BQU07bUJBR04sTUFBTTt3QkFFTixTQUFTLFNBQUMsUUFBUSxFQUFFLEVBQUUsTUFBTSxFQUFFLElBQUksRUFBRTttQkFNcEMsS0FBSzs7OztJQWpDTixzQ0FDZ0M7O0lBRWhDLHFDQUNXOztJQUVYLG9DQUNrQjs7SUFFbEIsa0NBQ2dCOztJQUVoQixtQ0FDaUI7O0lBRWpCLHFDQUNtQzs7SUFHbkMsZ0NBQTBEOztJQUcxRCxnQ0FBMEQ7O0lBRzFELCtCQUF5RDs7SUFFekQsb0NBQ3lDOzs7OztJQUt6QywrQkFBeUI7Ozs7O0lBTWIsbUNBQTJCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPdXRwdXQsIFZpZXdDaGlsZCwgRWxlbWVudFJlZiwgUmVuZGVyZXIyLCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLWJ1dHRvbicsXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtbWF4LWlubGluZS1kZWNsYXJhdGlvbnNcclxuICB0ZW1wbGF0ZTogYFxyXG4gICAgPGJ1dHRvblxyXG4gICAgICAjYnV0dG9uXHJcbiAgICAgIFthdHRyLnR5cGVdPVwiYnV0dG9uVHlwZSB8fCB0eXBlXCJcclxuICAgICAgW25nQ2xhc3NdPVwiYnV0dG9uQ2xhc3NcIlxyXG4gICAgICBbZGlzYWJsZWRdPVwibG9hZGluZyB8fCBkaXNhYmxlZFwiXHJcbiAgICAgIChjbGljayk9XCJjbGljay5lbWl0KCRldmVudClcIlxyXG4gICAgICAoZm9jdXMpPVwiZm9jdXMuZW1pdCgkZXZlbnQpXCJcclxuICAgICAgKGJsdXIpPVwiYmx1ci5lbWl0KCRldmVudClcIlxyXG4gICAgPlxyXG4gICAgICA8aSBbbmdDbGFzc109XCJpY29uXCIgY2xhc3M9XCJtci0xXCI+PC9pPjxuZy1jb250ZW50PjwvbmctY29udGVudD5cclxuICAgIDwvYnV0dG9uPlxyXG4gIGAsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBCdXR0b25Db21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xyXG4gIEBJbnB1dCgpXHJcbiAgYnV0dG9uQ2xhc3MgPSAnYnRuIGJ0bi1wcmltYXJ5JztcclxuXHJcbiAgQElucHV0KClcclxuICBidXR0b25UeXBlOyAvLyBUT0RPOiBBZGQgaW5pdGlhbCB2YWx1ZS5cclxuXHJcbiAgQElucHV0KClcclxuICBpY29uQ2xhc3M6IHN0cmluZztcclxuXHJcbiAgQElucHV0KClcclxuICBsb2FkaW5nID0gZmFsc2U7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgZGlzYWJsZWQgPSBmYWxzZTtcclxuXHJcbiAgQElucHV0KClcclxuICBhdHRyaWJ1dGVzOiBBQlAuRGljdGlvbmFyeTxzdHJpbmc+O1xyXG5cclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgY2xpY2sgPSBuZXcgRXZlbnRFbWl0dGVyPE1vdXNlRXZlbnQ+KCk7XHJcblxyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBmb2N1cyA9IG5ldyBFdmVudEVtaXR0ZXI8Rm9jdXNFdmVudD4oKTtcclxuXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGJsdXIgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XHJcblxyXG4gIEBWaWV3Q2hpbGQoJ2J1dHRvbicsIHsgc3RhdGljOiB0cnVlIH0pXHJcbiAgYnV0dG9uUmVmOiBFbGVtZW50UmVmPEhUTUxCdXR0b25FbGVtZW50PjtcclxuXHJcbiAgLyoqXHJcbiAgICogQGRlcHJlY2F0ZWQgVXNlIGJ1dHRvblR5cGUgaW5zdGVhZC4gVG8gYmUgZGVsZXRlZCBpbiB2MVxyXG4gICAqL1xyXG4gIEBJbnB1dCgpIHR5cGUgPSAnYnV0dG9uJztcclxuXHJcbiAgZ2V0IGljb24oKTogc3RyaW5nIHtcclxuICAgIHJldHVybiBgJHt0aGlzLmxvYWRpbmcgPyAnZmEgZmEtc3Bpbm5lciBmYS1zcGluJyA6IHRoaXMuaWNvbkNsYXNzIHx8ICdkLW5vbmUnfWA7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XHJcblxyXG4gIG5nT25Jbml0KCkge1xyXG4gICAgaWYgKHRoaXMuYXR0cmlidXRlcykge1xyXG4gICAgICBPYmplY3Qua2V5cyh0aGlzLmF0dHJpYnV0ZXMpLmZvckVhY2goa2V5ID0+IHtcclxuICAgICAgICB0aGlzLnJlbmRlcmVyLnNldEF0dHJpYnV0ZSh0aGlzLmJ1dHRvblJlZi5uYXRpdmVFbGVtZW50LCBrZXksIHRoaXMuYXR0cmlidXRlc1trZXldKTtcclxuICAgICAgfSk7XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiJdfQ==