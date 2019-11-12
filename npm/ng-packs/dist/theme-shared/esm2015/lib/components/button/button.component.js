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
        // tslint:disable-next-line: no-output-native
        this.click = new EventEmitter();
        // tslint:disable-next-line: no-output-native
        this.focus = new EventEmitter();
        // tslint:disable-next-line: no-output-native
        this.blur = new EventEmitter();
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
    /**
     * @param {?} event
     * @return {?}
     */
    onClick(event) {
        event.stopPropagation();
        this.click.next(event);
    }
    /**
     * @param {?} event
     * @return {?}
     */
    onFocus(event) {
        event.stopPropagation();
        this.focus.next(event);
    }
    /**
     * @param {?} event
     * @return {?}
     */
    onBlur(event) {
        event.stopPropagation();
        this.blur.next(event);
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
      (click)="onClick($event)"
      (focus)="onFocus($event)"
      (blur)="onBlur($event)"
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
    ButtonComponent.prototype.buttonRef;
    /**
     * @type {?}
     * @private
     */
    ButtonComponent.prototype.renderer;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFlBQVksRUFBRSxLQUFLLEVBQUUsTUFBTSxFQUFFLFNBQVMsRUFBRSxVQUFVLEVBQUUsU0FBUyxFQUFVLE1BQU0sZUFBZSxDQUFDO0FBcUJqSCxNQUFNLE9BQU8sZUFBZTs7OztJQXNDMUIsWUFBb0IsUUFBbUI7UUFBbkIsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQXBDdkMsYUFBUSxHQUFHLEVBQUUsQ0FBQztRQUdkLGdCQUFXLEdBQUcsaUJBQWlCLENBQUM7UUFHaEMsZUFBVSxHQUFHLFFBQVEsQ0FBQztRQU10QixZQUFPLEdBQUcsS0FBSyxDQUFDO1FBR2hCLGFBQVEsR0FBRyxLQUFLLENBQUM7O1FBTUUsVUFBSyxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7O1FBR3ZDLFVBQUssR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOztRQUd2QyxTQUFJLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQztJQVNmLENBQUM7Ozs7SUFKM0MsSUFBSSxJQUFJO1FBQ04sT0FBTyxHQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLHVCQUF1QixDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUyxJQUFJLFFBQVEsRUFBRSxDQUFDO0lBQ2xGLENBQUM7Ozs7SUFJRCxRQUFRO1FBQ04sSUFBSSxJQUFJLENBQUMsVUFBVSxFQUFFO1lBQ25CLE1BQU0sQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLFVBQVUsQ0FBQyxDQUFDLE9BQU87Ozs7WUFBQyxHQUFHLENBQUMsRUFBRTtnQkFDekMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxZQUFZLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxhQUFhLEVBQUUsR0FBRyxFQUFFLElBQUksQ0FBQyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztZQUN0RixDQUFDLEVBQUMsQ0FBQztTQUNKO0lBQ0gsQ0FBQzs7Ozs7SUFFRCxPQUFPLENBQUMsS0FBaUI7UUFDdkIsS0FBSyxDQUFDLGVBQWUsRUFBRSxDQUFDO1FBQ3hCLElBQUksQ0FBQyxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO0lBQ3pCLENBQUM7Ozs7O0lBRUQsT0FBTyxDQUFDLEtBQWlCO1FBQ3ZCLEtBQUssQ0FBQyxlQUFlLEVBQUUsQ0FBQztRQUN4QixJQUFJLENBQUMsS0FBSyxDQUFDLElBQUksQ0FBQyxLQUFLLENBQUMsQ0FBQztJQUN6QixDQUFDOzs7OztJQUVELE1BQU0sQ0FBQyxLQUFpQjtRQUN0QixLQUFLLENBQUMsZUFBZSxFQUFFLENBQUM7UUFDeEIsSUFBSSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDLENBQUM7SUFDeEIsQ0FBQzs7O1lBL0VGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsWUFBWTs7Z0JBRXRCLFFBQVEsRUFBRTs7Ozs7Ozs7Ozs7OztHQWFUO2FBQ0Y7Ozs7WUFwQnVFLFNBQVM7Ozt1QkFzQjlFLEtBQUs7MEJBR0wsS0FBSzt5QkFHTCxLQUFLO3dCQUdMLEtBQUs7c0JBR0wsS0FBSzt1QkFHTCxLQUFLO3lCQUdMLEtBQUs7b0JBSUwsTUFBTTtvQkFHTixNQUFNO21CQUdOLE1BQU07d0JBRU4sU0FBUyxTQUFDLFFBQVEsRUFBRSxFQUFFLE1BQU0sRUFBRSxJQUFJLEVBQUU7Ozs7SUE5QnJDLG1DQUNjOztJQUVkLHNDQUNnQzs7SUFFaEMscUNBQ3NCOztJQUV0QixvQ0FDa0I7O0lBRWxCLGtDQUNnQjs7SUFFaEIsbUNBQ2lCOztJQUVqQixxQ0FDbUM7O0lBR25DLGdDQUEwRDs7SUFHMUQsZ0NBQTBEOztJQUcxRCwrQkFBeUQ7O0lBRXpELG9DQUN5Qzs7Ozs7SUFNN0IsbUNBQTJCIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBFdmVudEVtaXR0ZXIsIElucHV0LCBPdXRwdXQsIFZpZXdDaGlsZCwgRWxlbWVudFJlZiwgUmVuZGVyZXIyLCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcclxuaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLWJ1dHRvbicsXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtbWF4LWlubGluZS1kZWNsYXJhdGlvbnNcclxuICB0ZW1wbGF0ZTogYFxyXG4gICAgPGJ1dHRvblxyXG4gICAgICAjYnV0dG9uXHJcbiAgICAgIFtpZF09XCJidXR0b25JZFwiXHJcbiAgICAgIFthdHRyLnR5cGVdPVwiYnV0dG9uVHlwZVwiXHJcbiAgICAgIFtuZ0NsYXNzXT1cImJ1dHRvbkNsYXNzXCJcclxuICAgICAgW2Rpc2FibGVkXT1cImxvYWRpbmcgfHwgZGlzYWJsZWRcIlxyXG4gICAgICAoY2xpY2spPVwib25DbGljaygkZXZlbnQpXCJcclxuICAgICAgKGZvY3VzKT1cIm9uRm9jdXMoJGV2ZW50KVwiXHJcbiAgICAgIChibHVyKT1cIm9uQmx1cigkZXZlbnQpXCJcclxuICAgID5cclxuICAgICAgPGkgW25nQ2xhc3NdPVwiaWNvblwiIGNsYXNzPVwibXItMVwiPjwvaT48bmctY29udGVudD48L25nLWNvbnRlbnQ+XHJcbiAgICA8L2J1dHRvbj5cclxuICBgLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQnV0dG9uQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcclxuICBASW5wdXQoKVxyXG4gIGJ1dHRvbklkID0gJyc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgYnV0dG9uQ2xhc3MgPSAnYnRuIGJ0bi1wcmltYXJ5JztcclxuXHJcbiAgQElucHV0KClcclxuICBidXR0b25UeXBlID0gJ2J1dHRvbic7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgaWNvbkNsYXNzOiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgbG9hZGluZyA9IGZhbHNlO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGRpc2FibGVkID0gZmFsc2U7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgYXR0cmlidXRlczogQUJQLkRpY3Rpb25hcnk8c3RyaW5nPjtcclxuXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGNsaWNrID0gbmV3IEV2ZW50RW1pdHRlcjxNb3VzZUV2ZW50PigpO1xyXG5cclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgZm9jdXMgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XHJcblxyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBibHVyID0gbmV3IEV2ZW50RW1pdHRlcjxGb2N1c0V2ZW50PigpO1xyXG5cclxuICBAVmlld0NoaWxkKCdidXR0b24nLCB7IHN0YXRpYzogdHJ1ZSB9KVxyXG4gIGJ1dHRvblJlZjogRWxlbWVudFJlZjxIVE1MQnV0dG9uRWxlbWVudD47XHJcblxyXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gYCR7dGhpcy5sb2FkaW5nID8gJ2ZhIGZhLXNwaW5uZXIgZmEtc3BpbicgOiB0aGlzLmljb25DbGFzcyB8fCAnZC1ub25lJ31gO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpIHtcclxuICAgIGlmICh0aGlzLmF0dHJpYnV0ZXMpIHtcclxuICAgICAgT2JqZWN0LmtleXModGhpcy5hdHRyaWJ1dGVzKS5mb3JFYWNoKGtleSA9PiB7XHJcbiAgICAgICAgdGhpcy5yZW5kZXJlci5zZXRBdHRyaWJ1dGUodGhpcy5idXR0b25SZWYubmF0aXZlRWxlbWVudCwga2V5LCB0aGlzLmF0dHJpYnV0ZXNba2V5XSk7XHJcbiAgICAgIH0pO1xyXG4gICAgfVxyXG4gIH1cclxuXHJcbiAgb25DbGljayhldmVudDogTW91c2VFdmVudCkge1xyXG4gICAgZXZlbnQuc3RvcFByb3BhZ2F0aW9uKCk7XHJcbiAgICB0aGlzLmNsaWNrLm5leHQoZXZlbnQpO1xyXG4gIH1cclxuXHJcbiAgb25Gb2N1cyhldmVudDogRm9jdXNFdmVudCkge1xyXG4gICAgZXZlbnQuc3RvcFByb3BhZ2F0aW9uKCk7XHJcbiAgICB0aGlzLmZvY3VzLm5leHQoZXZlbnQpO1xyXG4gIH1cclxuXHJcbiAgb25CbHVyKGV2ZW50OiBGb2N1c0V2ZW50KSB7XHJcbiAgICBldmVudC5zdG9wUHJvcGFnYXRpb24oKTtcclxuICAgIHRoaXMuYmx1ci5uZXh0KGV2ZW50KTtcclxuICB9XHJcbn1cclxuIl19