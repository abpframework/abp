/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output, ViewChild, ElementRef, Renderer2 } from '@angular/core';
var ButtonComponent = /** @class */ (function () {
    function ButtonComponent(renderer) {
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
    Object.defineProperty(ButtonComponent.prototype, "icon", {
        get: /**
         * @return {?}
         */
        function () {
            return "" + (this.loading ? 'fa fa-spinner fa-spin' : this.iconClass || 'd-none');
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    ButtonComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.attributes) {
            Object.keys(this.attributes).forEach((/**
             * @param {?} key
             * @return {?}
             */
            function (key) {
                _this.renderer.setAttribute(_this.buttonRef.nativeElement, key, _this.attributes[key]);
            }));
        }
    };
    ButtonComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-button',
                    // tslint:disable-next-line: component-max-inline-declarations
                    template: "\n    <button\n      #button\n      [id]=\"buttonId\"\n      [attr.type]=\"buttonType\"\n      [ngClass]=\"buttonClass\"\n      [disabled]=\"loading || disabled\"\n      (click.stop)=\"click.next($event); abpClick.next($event)\"\n      (focus)=\"focus.next($event); abpFocus.next($event)\"\n      (blur)=\"blur.next($event); abpBlur.next($event)\"\n    >\n      <i [ngClass]=\"icon\" class=\"mr-1\"></i><ng-content></ng-content>\n    </button>\n  "
                }] }
    ];
    /** @nocollapse */
    ButtonComponent.ctorParameters = function () { return [
        { type: Renderer2 }
    ]; };
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
    return ButtonComponent;
}());
export { ButtonComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFHakg7SUFnRkUseUJBQW9CLFFBQW1CO1FBQW5CLGFBQVEsR0FBUixRQUFRLENBQVc7UUE1RHZDLGFBQVEsR0FBRyxFQUFFLENBQUM7UUFHZCxnQkFBVyxHQUFHLGlCQUFpQixDQUFDO1FBR2hDLGVBQVUsR0FBRyxRQUFRLENBQUM7UUFNdEIsWUFBTyxHQUFHLEtBQUssQ0FBQztRQUdoQixhQUFRLEdBQUcsS0FBSyxDQUFDOzs7Ozs7O1FBV0UsVUFBSyxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7Ozs7Ozs7UUFRdkMsVUFBSyxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7Ozs7Ozs7UUFRdkMsU0FBSSxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7O1FBR3RDLGFBQVEsR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOztRQUcxQyxhQUFRLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7UUFHMUMsWUFBTyxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7SUFTbEIsQ0FBQztJQUozQyxzQkFBSSxpQ0FBSTs7OztRQUFSO1lBQ0UsT0FBTyxNQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLHVCQUF1QixDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUyxJQUFJLFFBQVEsQ0FBRSxDQUFDO1FBQ2xGLENBQUM7OztPQUFBOzs7O0lBSUQsa0NBQVE7OztJQUFSO1FBQUEsaUJBTUM7UUFMQyxJQUFJLElBQUksQ0FBQyxVQUFVLEVBQUU7WUFDbkIsTUFBTSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsVUFBVSxDQUFDLENBQUMsT0FBTzs7OztZQUFDLFVBQUEsR0FBRztnQkFDdEMsS0FBSSxDQUFDLFFBQVEsQ0FBQyxZQUFZLENBQUMsS0FBSSxDQUFDLFNBQVMsQ0FBQyxhQUFhLEVBQUUsR0FBRyxFQUFFLEtBQUksQ0FBQyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztZQUN0RixDQUFDLEVBQUMsQ0FBQztTQUNKO0lBQ0gsQ0FBQzs7Z0JBeEZGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsWUFBWTs7b0JBRXRCLFFBQVEsRUFBRSxpY0FhVDtpQkFDRjs7OztnQkFwQnVFLFNBQVM7OzsyQkFzQjlFLEtBQUs7OEJBR0wsS0FBSzs2QkFHTCxLQUFLOzRCQUdMLEtBQUs7MEJBR0wsS0FBSzsyQkFHTCxLQUFLOzZCQUdMLEtBQUs7d0JBU0wsTUFBTTt3QkFRTixNQUFNO3VCQVFOLE1BQU07MkJBR04sTUFBTTsyQkFHTixNQUFNOzBCQUdOLE1BQU07NEJBRU4sU0FBUyxTQUFDLFFBQVEsRUFBRSxFQUFFLE1BQU0sRUFBRSxJQUFJLEVBQUU7O0lBZ0J2QyxzQkFBQztDQUFBLEFBekZELElBeUZDO1NBdkVZLGVBQWU7OztJQUMxQixtQ0FDYzs7SUFFZCxzQ0FDZ0M7O0lBRWhDLHFDQUNzQjs7SUFFdEIsb0NBQ2tCOztJQUVsQixrQ0FDZ0I7O0lBRWhCLG1DQUNpQjs7SUFFakIscUNBQ21DOztJQVFuQyxnQ0FBMEQ7O0lBUTFELGdDQUEwRDs7SUFRMUQsK0JBQXlEOztJQUd6RCxtQ0FBNkQ7O0lBRzdELG1DQUE2RDs7SUFHN0Qsa0NBQTREOztJQUU1RCxvQ0FDeUM7Ozs7O0lBTTdCLG1DQUEyQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0LCBWaWV3Q2hpbGQsIEVsZW1lbnRSZWYsIFJlbmRlcmVyMiwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1idXR0b24nLFxyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogY29tcG9uZW50LW1heC1pbmxpbmUtZGVjbGFyYXRpb25zXHJcbiAgdGVtcGxhdGU6IGBcclxuICAgIDxidXR0b25cclxuICAgICAgI2J1dHRvblxyXG4gICAgICBbaWRdPVwiYnV0dG9uSWRcIlxyXG4gICAgICBbYXR0ci50eXBlXT1cImJ1dHRvblR5cGVcIlxyXG4gICAgICBbbmdDbGFzc109XCJidXR0b25DbGFzc1wiXHJcbiAgICAgIFtkaXNhYmxlZF09XCJsb2FkaW5nIHx8IGRpc2FibGVkXCJcclxuICAgICAgKGNsaWNrLnN0b3ApPVwiY2xpY2submV4dCgkZXZlbnQpOyBhYnBDbGljay5uZXh0KCRldmVudClcIlxyXG4gICAgICAoZm9jdXMpPVwiZm9jdXMubmV4dCgkZXZlbnQpOyBhYnBGb2N1cy5uZXh0KCRldmVudClcIlxyXG4gICAgICAoYmx1cik9XCJibHVyLm5leHQoJGV2ZW50KTsgYWJwQmx1ci5uZXh0KCRldmVudClcIlxyXG4gICAgPlxyXG4gICAgICA8aSBbbmdDbGFzc109XCJpY29uXCIgY2xhc3M9XCJtci0xXCI+PC9pPjxuZy1jb250ZW50PjwvbmctY29udGVudD5cclxuICAgIDwvYnV0dG9uPlxyXG4gIGAsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBCdXR0b25Db21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xyXG4gIEBJbnB1dCgpXHJcbiAgYnV0dG9uSWQgPSAnJztcclxuXHJcbiAgQElucHV0KClcclxuICBidXR0b25DbGFzcyA9ICdidG4gYnRuLXByaW1hcnknO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGJ1dHRvblR5cGUgPSAnYnV0dG9uJztcclxuXHJcbiAgQElucHV0KClcclxuICBpY29uQ2xhc3M6IHN0cmluZztcclxuXHJcbiAgQElucHV0KClcclxuICBsb2FkaW5nID0gZmFsc2U7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgZGlzYWJsZWQgPSBmYWxzZTtcclxuXHJcbiAgQElucHV0KClcclxuICBhdHRyaWJ1dGVzOiBBQlAuRGljdGlvbmFyeTxzdHJpbmc+O1xyXG5cclxuICAvKlxyXG4gICAqXHJcbiAgICpcclxuICAgKiBAZGVwcmVjYXRlZCB1c2UgYWJwQ2xpY2sgaW5zdGVhZFxyXG4gICAqL1xyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBjbGljayA9IG5ldyBFdmVudEVtaXR0ZXI8TW91c2VFdmVudD4oKTtcclxuXHJcbiAgLypcclxuICAgKlxyXG4gICAqXHJcbiAgICogQGRlcHJlY2F0ZWQgdXNlIGFicEZvY3VzIGluc3RlYWRcclxuICAgKi9cclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgZm9jdXMgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XHJcblxyXG4gIC8qXHJcbiAgICpcclxuICAgKlxyXG4gICAqIEBkZXByZWNhdGVkIHVzZSBhYnBCbHVyIGluc3RlYWRcclxuICAgKi9cclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgYmx1ciA9IG5ldyBFdmVudEVtaXR0ZXI8Rm9jdXNFdmVudD4oKTtcclxuXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGFicENsaWNrID0gbmV3IEV2ZW50RW1pdHRlcjxNb3VzZUV2ZW50PigpO1xyXG5cclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgYWJwRm9jdXMgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XHJcblxyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBhYnBCbHVyID0gbmV3IEV2ZW50RW1pdHRlcjxGb2N1c0V2ZW50PigpO1xyXG5cclxuICBAVmlld0NoaWxkKCdidXR0b24nLCB7IHN0YXRpYzogdHJ1ZSB9KVxyXG4gIGJ1dHRvblJlZjogRWxlbWVudFJlZjxIVE1MQnV0dG9uRWxlbWVudD47XHJcblxyXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gYCR7dGhpcy5sb2FkaW5nID8gJ2ZhIGZhLXNwaW5uZXIgZmEtc3BpbicgOiB0aGlzLmljb25DbGFzcyB8fCAnZC1ub25lJ31gO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpIHtcclxuICAgIGlmICh0aGlzLmF0dHJpYnV0ZXMpIHtcclxuICAgICAgT2JqZWN0LmtleXModGhpcy5hdHRyaWJ1dGVzKS5mb3JFYWNoKGtleSA9PiB7XHJcbiAgICAgICAgdGhpcy5yZW5kZXJlci5zZXRBdHRyaWJ1dGUodGhpcy5idXR0b25SZWYubmF0aXZlRWxlbWVudCwga2V5LCB0aGlzLmF0dHJpYnV0ZXNba2V5XSk7XHJcbiAgICAgIH0pO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=