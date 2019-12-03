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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFxQmpILE1BQU0sT0FBTyxlQUFlOzs7O0lBOEQxQixZQUFvQixRQUFtQjtRQUFuQixhQUFRLEdBQVIsUUFBUSxDQUFXO1FBNUR2QyxhQUFRLEdBQUcsRUFBRSxDQUFDO1FBR2QsZ0JBQVcsR0FBRyxpQkFBaUIsQ0FBQztRQUdoQyxlQUFVLEdBQUcsUUFBUSxDQUFDO1FBTXRCLFlBQU8sR0FBRyxLQUFLLENBQUM7UUFHaEIsYUFBUSxHQUFHLEtBQUssQ0FBQzs7Ozs7OztRQVdFLFVBQUssR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOzs7Ozs7O1FBUXZDLFVBQUssR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOzs7Ozs7O1FBUXZDLFNBQUksR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOztRQUd0QyxhQUFRLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7UUFHMUMsYUFBUSxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7O1FBRzFDLFlBQU8sR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDO0lBU2xCLENBQUM7Ozs7SUFKM0MsSUFBSSxJQUFJO1FBQ04sT0FBTyxHQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLHVCQUF1QixDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUyxJQUFJLFFBQVEsRUFBRSxDQUFDO0lBQ2xGLENBQUM7Ozs7SUFJRCxRQUFRO1FBQ04sSUFBSSxJQUFJLENBQUMsVUFBVSxFQUFFO1lBQ25CLE1BQU0sQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxDQUFDLE9BQU87Ozs7WUFBQyxHQUFHLENBQUMsRUFBRTtnQkFDekMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxZQUFZLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxhQUFhLEVBQUUsR0FBRyxFQUFFLElBQUksQ0FBQyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztZQUN0RixDQUFDLEVBQUMsQ0FBQztTQUNKO0lBQ0gsQ0FBQzs7O1lBeEZGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsWUFBWTs7Z0JBRXRCLFFBQVEsRUFBRTs7Ozs7Ozs7Ozs7OztHQWFUO2FBQ0Y7Ozs7WUFwQnVFLFNBQVM7Ozt1QkFzQjlFLEtBQUs7MEJBR0wsS0FBSzt5QkFHTCxLQUFLO3dCQUdMLEtBQUs7c0JBR0wsS0FBSzt1QkFHTCxLQUFLO3lCQUdMLEtBQUs7b0JBU0wsTUFBTTtvQkFRTixNQUFNO21CQVFOLE1BQU07dUJBR04sTUFBTTt1QkFHTixNQUFNO3NCQUdOLE1BQU07d0JBRU4sU0FBUyxTQUFDLFFBQVEsRUFBRSxFQUFFLE1BQU0sRUFBRSxJQUFJLEVBQUU7Ozs7SUF0RHJDLG1DQUNjOztJQUVkLHNDQUNnQzs7SUFFaEMscUNBQ3NCOztJQUV0QixvQ0FDa0I7O0lBRWxCLGtDQUNnQjs7SUFFaEIsbUNBQ2lCOztJQUVqQixxQ0FDbUM7O0lBUW5DLGdDQUEwRDs7SUFRMUQsZ0NBQTBEOztJQVExRCwrQkFBeUQ7O0lBR3pELG1DQUE2RDs7SUFHN0QsbUNBQTZEOztJQUc3RCxrQ0FBNEQ7O0lBRTVELG9DQUN5Qzs7Ozs7SUFNN0IsbUNBQTJCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPdXRwdXQsIFZpZXdDaGlsZCwgRWxlbWVudFJlZiwgUmVuZGVyZXIyLCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLWJ1dHRvbicsXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtbWF4LWlubGluZS1kZWNsYXJhdGlvbnNcclxuICB0ZW1wbGF0ZTogYFxyXG4gICAgPGJ1dHRvblxyXG4gICAgICAjYnV0dG9uXHJcbiAgICAgIFtpZF09XCJidXR0b25JZFwiXHJcbiAgICAgIFthdHRyLnR5cGVdPVwiYnV0dG9uVHlwZVwiXHJcbiAgICAgIFtuZ0NsYXNzXT1cImJ1dHRvbkNsYXNzXCJcclxuICAgICAgW2Rpc2FibGVkXT1cImxvYWRpbmcgfHwgZGlzYWJsZWRcIlxyXG4gICAgICAoY2xpY2suc3RvcCk9XCJjbGljay5uZXh0KCRldmVudCk7IGFicENsaWNrLm5leHQoJGV2ZW50KVwiXHJcbiAgICAgIChmb2N1cyk9XCJmb2N1cy5uZXh0KCRldmVudCk7IGFicEZvY3VzLm5leHQoJGV2ZW50KVwiXHJcbiAgICAgIChibHVyKT1cImJsdXIubmV4dCgkZXZlbnQpOyBhYnBCbHVyLm5leHQoJGV2ZW50KVwiXHJcbiAgICA+XHJcbiAgICAgIDxpIFtuZ0NsYXNzXT1cImljb25cIiBjbGFzcz1cIm1yLTFcIj48L2k+PG5nLWNvbnRlbnQ+PC9uZy1jb250ZW50PlxyXG4gICAgPC9idXR0b24+XHJcbiAgYCxcclxufSlcclxuZXhwb3J0IGNsYXNzIEJ1dHRvbkNvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XHJcbiAgQElucHV0KClcclxuICBidXR0b25JZCA9ICcnO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGJ1dHRvbkNsYXNzID0gJ2J0biBidG4tcHJpbWFyeSc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgYnV0dG9uVHlwZSA9ICdidXR0b24nO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGljb25DbGFzczogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGxvYWRpbmcgPSBmYWxzZTtcclxuXHJcbiAgQElucHV0KClcclxuICBkaXNhYmxlZCA9IGZhbHNlO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGF0dHJpYnV0ZXM6IEFCUC5EaWN0aW9uYXJ5PHN0cmluZz47XHJcblxyXG4gIC8qXHJcbiAgICpcclxuICAgKlxyXG4gICAqIEBkZXByZWNhdGVkIHVzZSBhYnBDbGljayBpbnN0ZWFkXHJcbiAgICovXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGNsaWNrID0gbmV3IEV2ZW50RW1pdHRlcjxNb3VzZUV2ZW50PigpO1xyXG5cclxuICAvKlxyXG4gICAqXHJcbiAgICpcclxuICAgKiBAZGVwcmVjYXRlZCB1c2UgYWJwRm9jdXMgaW5zdGVhZFxyXG4gICAqL1xyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBmb2N1cyA9IG5ldyBFdmVudEVtaXR0ZXI8Rm9jdXNFdmVudD4oKTtcclxuXHJcbiAgLypcclxuICAgKlxyXG4gICAqXHJcbiAgICogQGRlcHJlY2F0ZWQgdXNlIGFicEJsdXIgaW5zdGVhZFxyXG4gICAqL1xyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBibHVyID0gbmV3IEV2ZW50RW1pdHRlcjxGb2N1c0V2ZW50PigpO1xyXG5cclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgYWJwQ2xpY2sgPSBuZXcgRXZlbnRFbWl0dGVyPE1vdXNlRXZlbnQ+KCk7XHJcblxyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBhYnBGb2N1cyA9IG5ldyBFdmVudEVtaXR0ZXI8Rm9jdXNFdmVudD4oKTtcclxuXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGFicEJsdXIgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XHJcblxyXG4gIEBWaWV3Q2hpbGQoJ2J1dHRvbicsIHsgc3RhdGljOiB0cnVlIH0pXHJcbiAgYnV0dG9uUmVmOiBFbGVtZW50UmVmPEhUTUxCdXR0b25FbGVtZW50PjtcclxuXHJcbiAgZ2V0IGljb24oKTogc3RyaW5nIHtcclxuICAgIHJldHVybiBgJHt0aGlzLmxvYWRpbmcgPyAnZmEgZmEtc3Bpbm5lciBmYS1zcGluJyA6IHRoaXMuaWNvbkNsYXNzIHx8ICdkLW5vbmUnfWA7XHJcbiAgfVxyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIHJlbmRlcmVyOiBSZW5kZXJlcjIpIHt9XHJcblxyXG4gIG5nT25Jbml0KCkge1xyXG4gICAgaWYgKHRoaXMuYXR0cmlidXRlcykge1xyXG4gICAgICBPYmplY3Qua2V5cyh0aGlzLmF0dHJpYnV0ZXMpLmZvckVhY2goa2V5ID0+IHtcclxuICAgICAgICB0aGlzLnJlbmRlcmVyLnNldEF0dHJpYnV0ZSh0aGlzLmJ1dHRvblJlZi5uYXRpdmVFbGVtZW50LCBrZXksIHRoaXMuYXR0cmlidXRlc1trZXldKTtcclxuICAgICAgfSk7XHJcbiAgICB9XHJcbiAgfVxyXG59XHJcbiJdfQ==