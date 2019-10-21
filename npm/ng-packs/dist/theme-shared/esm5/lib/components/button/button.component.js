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
                    template: "\n    <button\n      #button\n      [attr.type]=\"type\"\n      [ngClass]=\"buttonClass\"\n      [disabled]=\"loading || disabled\"\n      (click)=\"click.emit($event)\"\n      (focus)=\"focus.emit($event)\"\n      (blur)=\"blur.emit($event)\"\n    >\n      <i [ngClass]=\"icon\" class=\"mr-1\"></i><ng-content></ng-content>\n    </button>\n  "
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLEtBQUssRUFBRSxNQUFNLEVBQUUsU0FBUyxFQUFFLFVBQVUsRUFBRSxTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFHakg7SUF5REUseUJBQW9CLFFBQW1CO1FBQW5CLGFBQVEsR0FBUixRQUFRLENBQVc7UUF0Q3ZDLGdCQUFXLEdBQUcsaUJBQWlCLENBQUM7UUFTaEMsWUFBTyxHQUFHLEtBQUssQ0FBQztRQUdoQixhQUFRLEdBQUcsS0FBSyxDQUFDOztRQU1FLFVBQUssR0FBRyxJQUFJLFlBQVksRUFBYyxDQUFDOztRQUd2QyxVQUFLLEdBQUcsSUFBSSxZQUFZLEVBQWMsQ0FBQzs7UUFHdkMsU0FBSSxHQUFHLElBQUksWUFBWSxFQUFjLENBQUM7Ozs7UUFRaEQsU0FBSSxHQUFHLFFBQVEsQ0FBQztJQU1pQixDQUFDO0lBSjNDLHNCQUFJLGlDQUFJOzs7O1FBQVI7WUFDRSxPQUFPLE1BQUcsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDLENBQUMsdUJBQXVCLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksUUFBUSxDQUFFLENBQUM7UUFDbEYsQ0FBQzs7O09BQUE7Ozs7SUFJRCxrQ0FBUTs7O0lBQVI7UUFBQSxpQkFNQztRQUxDLElBQUksSUFBSSxDQUFDLFVBQVUsRUFBRTtZQUNuQixNQUFNLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsQ0FBQyxPQUFPOzs7O1lBQUMsVUFBQSxHQUFHO2dCQUN0QyxLQUFJLENBQUMsUUFBUSxDQUFDLFlBQVksQ0FBQyxLQUFJLENBQUMsU0FBUyxDQUFDLGFBQWEsRUFBRSxHQUFHLEVBQUUsS0FBSSxDQUFDLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO1lBQ3RGLENBQUMsRUFBQyxDQUFDO1NBQ0o7SUFDSCxDQUFDOztnQkFqRUYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxZQUFZOztvQkFFdEIsUUFBUSxFQUFFLHlWQVlUO2lCQUNGOzs7O2dCQW5CdUUsU0FBUzs7OzhCQXFCOUUsS0FBSzs2QkFHTCxLQUFLOzRCQUdMLEtBQUs7MEJBR0wsS0FBSzsyQkFHTCxLQUFLOzZCQUdMLEtBQUs7d0JBSUwsTUFBTTt3QkFHTixNQUFNO3VCQUdOLE1BQU07NEJBRU4sU0FBUyxTQUFDLFFBQVEsRUFBRSxFQUFFLE1BQU0sRUFBRSxJQUFJLEVBQUU7dUJBTXBDLEtBQUs7O0lBZVIsc0JBQUM7Q0FBQSxBQWxFRCxJQWtFQztTQWpEWSxlQUFlOzs7SUFDMUIsc0NBQ2dDOztJQUVoQyxxQ0FDVzs7SUFFWCxvQ0FDa0I7O0lBRWxCLGtDQUNnQjs7SUFFaEIsbUNBQ2lCOztJQUVqQixxQ0FDbUM7O0lBR25DLGdDQUEwRDs7SUFHMUQsZ0NBQTBEOztJQUcxRCwrQkFBeUQ7O0lBRXpELG9DQUN5Qzs7Ozs7SUFLekMsK0JBQXlCOzs7OztJQU1iLG1DQUEyQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgRXZlbnRFbWl0dGVyLCBJbnB1dCwgT3V0cHV0LCBWaWV3Q2hpbGQsIEVsZW1lbnRSZWYsIFJlbmRlcmVyMiwgT25Jbml0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtYnV0dG9uJyxcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtbWF4LWlubGluZS1kZWNsYXJhdGlvbnNcbiAgdGVtcGxhdGU6IGBcbiAgICA8YnV0dG9uXG4gICAgICAjYnV0dG9uXG4gICAgICBbYXR0ci50eXBlXT1cInR5cGVcIlxuICAgICAgW25nQ2xhc3NdPVwiYnV0dG9uQ2xhc3NcIlxuICAgICAgW2Rpc2FibGVkXT1cImxvYWRpbmcgfHwgZGlzYWJsZWRcIlxuICAgICAgKGNsaWNrKT1cImNsaWNrLmVtaXQoJGV2ZW50KVwiXG4gICAgICAoZm9jdXMpPVwiZm9jdXMuZW1pdCgkZXZlbnQpXCJcbiAgICAgIChibHVyKT1cImJsdXIuZW1pdCgkZXZlbnQpXCJcbiAgICA+XG4gICAgICA8aSBbbmdDbGFzc109XCJpY29uXCIgY2xhc3M9XCJtci0xXCI+PC9pPjxuZy1jb250ZW50PjwvbmctY29udGVudD5cbiAgICA8L2J1dHRvbj5cbiAgYCxcbn0pXG5leHBvcnQgY2xhc3MgQnV0dG9uQ29tcG9uZW50IGltcGxlbWVudHMgT25Jbml0IHtcbiAgQElucHV0KClcbiAgYnV0dG9uQ2xhc3MgPSAnYnRuIGJ0bi1wcmltYXJ5JztcblxuICBASW5wdXQoKVxuICBidXR0b25UeXBlOyAvLyBUT0RPOiBBZGQgaW5pdGlhbCB2YWx1ZS5cblxuICBASW5wdXQoKVxuICBpY29uQ2xhc3M6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBsb2FkaW5nID0gZmFsc2U7XG5cbiAgQElucHV0KClcbiAgZGlzYWJsZWQgPSBmYWxzZTtcblxuICBASW5wdXQoKVxuICBhdHRyaWJ1dGVzOiBBQlAuRGljdGlvbmFyeTxzdHJpbmc+O1xuXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogbm8tb3V0cHV0LW5hdGl2ZVxuICBAT3V0cHV0KCkgcmVhZG9ubHkgY2xpY2sgPSBuZXcgRXZlbnRFbWl0dGVyPE1vdXNlRXZlbnQ+KCk7XG5cbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBuby1vdXRwdXQtbmF0aXZlXG4gIEBPdXRwdXQoKSByZWFkb25seSBmb2N1cyA9IG5ldyBFdmVudEVtaXR0ZXI8Rm9jdXNFdmVudD4oKTtcblxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IG5vLW91dHB1dC1uYXRpdmVcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGJsdXIgPSBuZXcgRXZlbnRFbWl0dGVyPEZvY3VzRXZlbnQ+KCk7XG5cbiAgQFZpZXdDaGlsZCgnYnV0dG9uJywgeyBzdGF0aWM6IHRydWUgfSlcbiAgYnV0dG9uUmVmOiBFbGVtZW50UmVmPEhUTUxCdXR0b25FbGVtZW50PjtcblxuICAvKipcbiAgICogQGRlcHJlY2F0ZWQgVXNlIGJ1dHRvblR5cGUgaW5zdGVhZC4gVG8gYmUgZGVsZXRlZCBpbiB2MVxuICAgKi9cbiAgQElucHV0KCkgdHlwZSA9ICdidXR0b24nO1xuXG4gIGdldCBpY29uKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIGAke3RoaXMubG9hZGluZyA/ICdmYSBmYS1zcGlubmVyIGZhLXNwaW4nIDogdGhpcy5pY29uQ2xhc3MgfHwgJ2Qtbm9uZSd9YDtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMikge31cblxuICBuZ09uSW5pdCgpIHtcbiAgICBpZiAodGhpcy5hdHRyaWJ1dGVzKSB7XG4gICAgICBPYmplY3Qua2V5cyh0aGlzLmF0dHJpYnV0ZXMpLmZvckVhY2goa2V5ID0+IHtcbiAgICAgICAgdGhpcy5yZW5kZXJlci5zZXRBdHRyaWJ1dGUodGhpcy5idXR0b25SZWYubmF0aXZlRWxlbWVudCwga2V5LCB0aGlzLmF0dHJpYnV0ZXNba2V5XSk7XG4gICAgICB9KTtcbiAgICB9XG4gIH1cbn1cbiJdfQ==