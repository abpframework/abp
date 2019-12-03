/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/button/button.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output, ViewChild, ElementRef, Renderer2 } from '@angular/core';
export class ButtonComponent {
    /**
     * @param {?} renderer
     */
    constructor(renderer) {
        this.renderer = renderer;
        this.buttonId = '';
        this.buttonClass = 'btn btn-primary';
        this.buttonType = 'button';
        this.loading = false;
        this.disabled = false;
        /*
           *
           *
           * @deprecated use abpClick instead
           */
        // tslint:disable-next-line: no-output-native
        this.click = new EventEmitter();
        /*
           *
           *
           * @deprecated use abpFocus instead
           */
        // tslint:disable-next-line: no-output-native
        this.focus = new EventEmitter();
        /*
           *
           *
           * @deprecated use abpBlur instead
           */
        // tslint:disable-next-line: no-output-native
        this.blur = new EventEmitter();
        // tslint:disable-next-line: no-output-native
        this.abpClick = new EventEmitter();
        // tslint:disable-next-line: no-output-native
        this.abpFocus = new EventEmitter();
        // tslint:disable-next-line: no-output-native
        this.abpBlur = new EventEmitter();
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
      [id]="buttonId"
      [attr.type]="buttonType"
      [ngClass]="buttonClass"
      [disabled]="loading || disabled"
      (click.stop)="click.next($event); abpClick.next($event)"
      (focus)="focus.next($event); abpFocus.next($event)"
      (blur)="blur.next($event); abpBlur.next($event)"
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
    buttonId: [{ type: Input }],
    buttonClass: [{ type: Input }],
    buttonType: [{ type: Input }],
    iconClass: [{ type: Input }],
    loading: [{ type: Input }],
    disabled: [{ type: Input }],
    attributes: [{ type: Input }],
    click: [{ type: Output }],
    focus: [{ type: Output }],
    blur: [{ type: Output }],
    abpClick: [{ type: Output }],
    abpFocus: [{ type: Output }],
    abpBlur: [{ type: Output }],
    buttonRef: [{ type: ViewChild, args: ['button', { static: true },] }]
};
if (false) {
    /** @type {?} */
    ButtonComponent.prototype.buttonId;
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
    ButtonComponent.prototype.abpClick;
    /** @type {?} */
    ButtonComponent.prototype.abpFocus;
    /** @type {?} */
    ButtonComponent.prototype.abpBlur;
    /** @type {?} */
    ButtonComponent.prototype.buttonRef;
    /**
     * @type {?}
     * @private
     */
    ButtonComponent.prototype.renderer;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBcUJqSCxNQUFNLE9BQU8sZUFBZTs7OztJQThEMUIsWUFBb0IsUUFBbUI7UUFBbkIsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQTVEdkMsYUFBUSxHQUFHLEVBQUUsQ0FBQztRQUdkLGdCQUFXLEdBQUcsaUJBQWlCLENBQUM7UUFHaEMsZUFBVSxHQUFHLFFBQVEsQ0FBQztRQU10QixZQUFPLEdBQUcsS0FBSyxDQUFDO1FBR2hCLGFBQVEsR0FBRyxLQUFLLENBQUM7Ozs7Ozs7UUFXRSxVQUFLLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7Ozs7OztRQVF2QyxVQUFLLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7Ozs7OztRQVF2QyxTQUFJLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7UUFHdEMsYUFBUSxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7O1FBRzFDLGFBQVEsR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOztRQUcxQyxZQUFPLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQztJQVNsQixDQUFDOzs7O0lBSjNDLElBQUksSUFBSTtRQUNOLE9BQU8sR0FBRyxJQUFJLENBQUMsT0FBTyxDQUFDLENBQUMsQ0FBQyx1QkFBdUIsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVMsSUFBSSxRQUFRLEVBQUUsQ0FBQztJQUNsRixDQUFDOzs7O0lBSUQsUUFBUTtRQUNOLElBQUksSUFBSSxDQUFDLFVBQVUsRUFBRTtZQUNuQixNQUFNLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsQ0FBQyxPQUFPOzs7O1lBQUMsR0FBRyxDQUFDLEVBQUU7Z0JBQ3pDLElBQUksQ0FBQyxRQUFRLENBQUMsWUFBWSxDQUFDLElBQUksQ0FBQyxTQUFTLENBQUMsYUFBYSxFQUFFLEdBQUcsRUFBRSxJQUFJLENBQUMsVUFBVSxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7WUFDdEYsQ0FBQyxFQUFDLENBQUM7U0FDSjtJQUNILENBQUM7OztZQXhGRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLFlBQVk7O2dCQUV0QixRQUFRLEVBQUU7Ozs7Ozs7Ozs7Ozs7R0FhVDthQUNGOzs7O1lBcEJ1RSxTQUFTOzs7dUJBc0I5RSxLQUFLOzBCQUdMLEtBQUs7eUJBR0wsS0FBSzt3QkFHTCxLQUFLO3NCQUdMLEtBQUs7dUJBR0wsS0FBSzt5QkFHTCxLQUFLO29CQVNMLE1BQU07b0JBUU4sTUFBTTttQkFRTixNQUFNO3VCQUdOLE1BQU07dUJBR04sTUFBTTtzQkFHTixNQUFNO3dCQUVOLFNBQVMsU0FBQyxRQUFRLEVBQUUsRUFBRSxNQUFNLEVBQUUsSUFBSSxFQUFFOzs7O0lBdERyQyxtQ0FDYzs7SUFFZCxzQ0FDZ0M7O0lBRWhDLHFDQUNzQjs7SUFFdEIsb0NBQ2tCOztJQUVsQixrQ0FDZ0I7O0lBRWhCLG1DQUNpQjs7SUFFakIscUNBQ21DOztJQVFuQyxnQ0FBMEQ7O0lBUTFELGdDQUEwRDs7SUFRMUQsK0JBQXlEOztJQUd6RCxtQ0FBNkQ7O0lBRzdELG1DQUE2RDs7SUFHN0Qsa0NBQTREOztJQUU1RCxvQ0FDeUM7Ozs7O0lBTTdCLG1DQUEyQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0LCBWaWV3Q2hpbGQsIEVsZW1lbnRSZWYsIFJlbmRlcmVyMiwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtYnV0dG9uJyxcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtbWF4LWlubGluZS1kZWNsYXJhdGlvbnNcbiAgdGVtcGxhdGU6IGBcbiAgICA8YnV0dG9uXG4gICAgICAjYnV0dG9uXG4gICAgICBbaWRdPVwiYnV0dG9uSWRcIlxuICAgICAgW2F0dHIudHlwZV09XCJidXR0b25UeXBlXCJcbiAgICAgIFtuZ0NsYXNzXT1cImJ1dHRvbkNsYXNzXCJcbiAgICAgIFtkaXNhYmxlZF09XCJsb2FkaW5nIHx8IGRpc2FibGVkXCJcbiAgICAgIChjbGljay5zdG9wKT1cImNsaWNrLm5leHQoJGV2ZW50KTsgYWJwQ2xpY2submV4dCgkZXZlbnQpXCJcbiAgICAgIChmb2N1cyk9XCJmb2N1cy5uZXh0KCRldmVudCk7IGFicEZvY3VzLm5leHQoJGV2ZW50KVwiXG4gICAgICAoYmx1cik9XCJibHVyLm5leHQoJGV2ZW50KTsgYWJwQmx1ci5uZXh0KCRldmVudClcIlxuICAgID5cbiAgICAgIDxpIFtuZ0NsYXNzXT1cImljb25cIiBjbGFzcz1cIm1yLTFcIj48L2k+PG5nLWNvbnRlbnQ+PC9uZy1jb250ZW50PlxuICAgIDwvYnV0dG9uPlxuICBgLFxufSlcbmV4cG9ydCBjbGFzcyBCdXR0b25Db21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBASW5wdXQoKVxuICBidXR0b25JZCA9ICcnO1xuXG4gIEBJbnB1dCgpXG4gIGJ1dHRvbkNsYXNzID0gJ2J0biBidG4tcHJpbWFyeSc7XG5cbiAgQElucHV0KClcbiAgYnV0dG9uVHlwZSA9ICdidXR0b24nO1xuXG4gIEBJbnB1dCgpXG4gIGljb25DbGFzczogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIGxvYWRpbmcgPSBmYWxzZTtcblxuICBASW5wdXQoKVxuICBkaXNhYmxlZCA9IGZhbHNlO1xuXG4gIEBJbnB1dCgpXG4gIGF0dHJpYnV0ZXM6IEFCUC5EaWN0aW9uYXJ5PHN0cmluZz47XG5cbiAgLypcbiAgICpcbiAgICpcbiAgICogQGRlcHJlY2F0ZWQgdXNlIGFicENsaWNrIGluc3RlYWRcbiAgICovXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxuICBAT3V0cHV0KCkgcmVhZG9ubHkgY2xpY2sgPSBuZXcgRXZlbnRFbWl0dGVyPE1vdXNlRXZlbnQ+KCk7XG5cbiAgLypcbiAgICpcbiAgICpcbiAgICogQGRlcHJlY2F0ZWQgdXNlIGFicEZvY3VzIGluc3RlYWRcbiAgICovXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxuICBAT3V0cHV0KCkgcmVhZG9ubHkgZm9jdXMgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XG5cbiAgLypcbiAgICpcbiAgICpcbiAgICogQGRlcHJlY2F0ZWQgdXNlIGFicEJsdXIgaW5zdGVhZFxuICAgKi9cbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXG4gIEBPdXRwdXQoKSByZWFkb25seSBibHVyID0gbmV3IEV2ZW50RW1pdHRlcjxGb2N1c0V2ZW50PigpO1xuXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxuICBAT3V0cHV0KCkgcmVhZG9ubHkgYWJwQ2xpY2sgPSBuZXcgRXZlbnRFbWl0dGVyPE1vdXNlRXZlbnQ+KCk7XG5cbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXG4gIEBPdXRwdXQoKSByZWFkb25seSBhYnBGb2N1cyA9IG5ldyBFdmVudEVtaXR0ZXI8Rm9jdXNFdmVudD4oKTtcblxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGFicEJsdXIgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XG5cbiAgQFZpZXdDaGlsZCgnYnV0dG9uJywgeyBzdGF0aWM6IHRydWUgfSlcbiAgYnV0dG9uUmVmOiBFbGVtZW50UmVmPEhUTUxCdXR0b25FbGVtZW50PjtcblxuICBnZXQgaWNvbigpOiBzdHJpbmcge1xuICAgIHJldHVybiBgJHt0aGlzLmxvYWRpbmcgPyAnZmEgZmEtc3Bpbm5lciBmYS1zcGluJyA6IHRoaXMuaWNvbkNsYXNzIHx8ICdkLW5vbmUnfWA7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgaWYgKHRoaXMuYXR0cmlidXRlcykge1xuICAgICAgT2JqZWN0LmtleXModGhpcy5hdHRyaWJ1dGVzKS5mb3JFYWNoKGtleSA9PiB7XG4gICAgICAgIHRoaXMucmVuZGVyZXIuc2V0QXR0cmlidXRlKHRoaXMuYnV0dG9uUmVmLm5hdGl2ZUVsZW1lbnQsIGtleSwgdGhpcy5hdHRyaWJ1dGVzW2tleV0pO1xuICAgICAgfSk7XG4gICAgfVxuICB9XG59XG4iXX0=