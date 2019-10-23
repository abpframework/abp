/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, EventEmitter, Input, Output, ViewChild, ElementRef, Renderer2 } from '@angular/core';
var ButtonComponent = /** @class */ (function () {
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
                    template: "\n    <button\n      #button\n      [attr.type]=\"buttonType || type\"\n      [ngClass]=\"buttonClass\"\n      [disabled]=\"loading || disabled\"\n      (click)=\"click.emit($event)\"\n      (focus)=\"focus.emit($event)\"\n      (blur)=\"blur.emit($event)\"\n    >\n      <i [ngClass]=\"icon\" class=\"mr-1\"></i><ng-content></ng-content>\n    </button>\n  "
                }] }
    ];
    /** @nocollapse */
    ButtonComponent.ctorParameters = function () { return [
        { type: Renderer2 }
    ]; };
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
    return ButtonComponent;
}());
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFHakg7SUF5REUseUJBQW9CLFFBQW1CO1FBQW5CLGFBQVEsR0FBUixRQUFRLENBQVc7UUF0Q3ZDLGdCQUFXLEdBQUcsaUJBQWlCLENBQUM7UUFTaEMsWUFBTyxHQUFHLEtBQUssQ0FBQztRQUdoQixhQUFRLEdBQUcsS0FBSyxDQUFDOztRQU1FLFVBQUssR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOztRQUd2QyxVQUFLLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7UUFHdkMsU0FBSSxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7Ozs7UUFRaEQsU0FBSSxHQUFHLFFBQVEsQ0FBQztJQU1pQixDQUFDO0lBSjNDLHNCQUFJLGlDQUFJOzs7O1FBQVI7WUFDRSxPQUFPLE1BQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsdUJBQXVCLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksUUFBUSxDQUFFLENBQUM7UUFDbEYsQ0FBQzs7O09BQUE7Ozs7SUFJRCxrQ0FBUTs7O0lBQVI7UUFBQSxpQkFNQztRQUxDLElBQUksSUFBSSxDQUFDLFVBQVUsRUFBRTtZQUNuQixNQUFNLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsQ0FBQyxPQUFPOzs7O1lBQUMsVUFBQSxHQUFHO2dCQUN0QyxLQUFJLENBQUMsUUFBUSxDQUFDLFlBQVksQ0FBQyxLQUFJLENBQUMsU0FBUyxDQUFDLGFBQWEsRUFBRSxHQUFHLEVBQUUsS0FBSSxDQUFDLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO1lBQ3RGLENBQUMsRUFBQyxDQUFDO1NBQ0o7SUFDSCxDQUFDOztnQkFqRUYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxZQUFZOztvQkFFdEIsUUFBUSxFQUFFLHVXQVlUO2lCQUNGOzs7O2dCQW5CdUUsU0FBUzs7OzhCQXFCOUUsS0FBSzs2QkFHTCxLQUFLOzRCQUdMLEtBQUs7MEJBR0wsS0FBSzsyQkFHTCxLQUFLOzZCQUdMLEtBQUs7d0JBSUwsTUFBTTt3QkFHTixNQUFNO3VCQUdOLE1BQU07NEJBRU4sU0FBUyxTQUFDLFFBQVEsRUFBRSxFQUFFLE1BQU0sRUFBRSxJQUFJLEVBQUU7dUJBTXBDLEtBQUs7O0lBZVIsc0JBQUM7Q0FBQSxBQWxFRCxJQWtFQztTQWpEWSxlQUFlOzs7SUFDMUIsc0NBQ2dDOztJQUVoQyxxQ0FDVzs7SUFFWCxvQ0FDa0I7O0lBRWxCLGtDQUNnQjs7SUFFaEIsbUNBQ2lCOztJQUVqQixxQ0FDbUM7O0lBR25DLGdDQUEwRDs7SUFHMUQsZ0NBQTBEOztJQUcxRCwrQkFBeUQ7O0lBRXpELG9DQUN5Qzs7Ozs7SUFLekMsK0JBQXlCOzs7OztJQU1iLG1DQUEyQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0LCBWaWV3Q2hpbGQsIEVsZW1lbnRSZWYsIFJlbmRlcmVyMiwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IEFCUCB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1idXR0b24nLFxyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogY29tcG9uZW50LW1heC1pbmxpbmUtZGVjbGFyYXRpb25zXHJcbiAgdGVtcGxhdGU6IGBcclxuICAgIDxidXR0b25cclxuICAgICAgI2J1dHRvblxyXG4gICAgICBbYXR0ci50eXBlXT1cImJ1dHRvblR5cGUgfHwgdHlwZVwiXHJcbiAgICAgIFtuZ0NsYXNzXT1cImJ1dHRvbkNsYXNzXCJcclxuICAgICAgW2Rpc2FibGVkXT1cImxvYWRpbmcgfHwgZGlzYWJsZWRcIlxyXG4gICAgICAoY2xpY2spPVwiY2xpY2suZW1pdCgkZXZlbnQpXCJcclxuICAgICAgKGZvY3VzKT1cImZvY3VzLmVtaXQoJGV2ZW50KVwiXHJcbiAgICAgIChibHVyKT1cImJsdXIuZW1pdCgkZXZlbnQpXCJcclxuICAgID5cclxuICAgICAgPGkgW25nQ2xhc3NdPVwiaWNvblwiIGNsYXNzPVwibXItMVwiPjwvaT48bmctY29udGVudD48L25nLWNvbnRlbnQ+XHJcbiAgICA8L2J1dHRvbj5cclxuICBgLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgQnV0dG9uQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcclxuICBASW5wdXQoKVxyXG4gIGJ1dHRvbkNsYXNzID0gJ2J0biBidG4tcHJpbWFyeSc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgYnV0dG9uVHlwZTsgLy8gVE9ETzogQWRkIGluaXRpYWwgdmFsdWUuXHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgaWNvbkNsYXNzOiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgbG9hZGluZyA9IGZhbHNlO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGRpc2FibGVkID0gZmFsc2U7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgYXR0cmlidXRlczogQUJQLkRpY3Rpb25hcnk8c3RyaW5nPjtcclxuXHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGNsaWNrID0gbmV3IEV2ZW50RW1pdHRlcjxNb3VzZUV2ZW50PigpO1xyXG5cclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgZm9jdXMgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XHJcblxyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBibHVyID0gbmV3IEV2ZW50RW1pdHRlcjxGb2N1c0V2ZW50PigpO1xyXG5cclxuICBAVmlld0NoaWxkKCdidXR0b24nLCB7IHN0YXRpYzogdHJ1ZSB9KVxyXG4gIGJ1dHRvblJlZjogRWxlbWVudFJlZjxIVE1MQnV0dG9uRWxlbWVudD47XHJcblxyXG4gIC8qKlxyXG4gICAqIEBkZXByZWNhdGVkIFVzZSBidXR0b25UeXBlIGluc3RlYWQuIFRvIGJlIGRlbGV0ZWQgaW4gdjFcclxuICAgKi9cclxuICBASW5wdXQoKSB0eXBlID0gJ2J1dHRvbic7XHJcblxyXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gYCR7dGhpcy5sb2FkaW5nID8gJ2ZhIGZhLXNwaW5uZXIgZmEtc3BpbicgOiB0aGlzLmljb25DbGFzcyB8fCAnZC1ub25lJ31gO1xyXG4gIH1cclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyKSB7fVxyXG5cclxuICBuZ09uSW5pdCgpIHtcclxuICAgIGlmICh0aGlzLmF0dHJpYnV0ZXMpIHtcclxuICAgICAgT2JqZWN0LmtleXModGhpcy5hdHRyaWJ1dGVzKS5mb3JFYWNoKGtleSA9PiB7XHJcbiAgICAgICAgdGhpcy5yZW5kZXJlci5zZXRBdHRyaWJ1dGUodGhpcy5idXR0b25SZWYubmF0aXZlRWxlbWVudCwga2V5LCB0aGlzLmF0dHJpYnV0ZXNba2V5XSk7XHJcbiAgICAgIH0pO1xyXG4gICAgfVxyXG4gIH1cclxufVxyXG4iXX0=