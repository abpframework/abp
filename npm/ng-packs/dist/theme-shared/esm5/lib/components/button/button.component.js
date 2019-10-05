/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input } from '@angular/core';
var ButtonComponent = /** @class */ (function () {
    function ButtonComponent() {
        this.buttonClass = 'btn btn-primary';
        this.type = 'button';
        this.loading = false;
        this.disabled = false;
    }
    Object.defineProperty(ButtonComponent.prototype, "icon", {
        get: /**
         * @return {?}
         */
        function () {
            return "" + (this.loading ? 'fa fa-pulse fa-spinner' : this.iconClass || 'd-none');
        },
        enumerable: true,
        configurable: true
    });
    ButtonComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-button',
                    template: "\n    <button [attr.type]=\"type\" [ngClass]=\"buttonClass\" [disabled]=\"loading || disabled\">\n      <i [ngClass]=\"icon\" class=\"mr-1\"></i><ng-content></ng-content>\n    </button>\n  "
                }] }
    ];
    ButtonComponent.propDecorators = {
        buttonClass: [{ type: Input }],
        type: [{ type: Input }],
        iconClass: [{ type: Input }],
        loading: [{ type: Input }],
        disabled: [{ type: Input }]
    };
    return ButtonComponent;
}());
export { ButtonComponent };
if (false) {
    /** @type {?} */
    ButtonComponent.prototype.buttonClass;
    /** @type {?} */
    ButtonComponent.prototype.type;
    /** @type {?} */
    ButtonComponent.prototype.iconClass;
    /** @type {?} */
    ButtonComponent.prototype.loading;
    /** @type {?} */
    ButtonComponent.prototype.disabled;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiYnV0dG9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvYnV0dG9uL2J1dHRvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRWpEO0lBQUE7UUFVRSxnQkFBVyxHQUFXLGlCQUFpQixDQUFDO1FBR3hDLFNBQUksR0FBVyxRQUFRLENBQUM7UUFNeEIsWUFBTyxHQUFZLEtBQUssQ0FBQztRQUd6QixhQUFRLEdBQVksS0FBSyxDQUFDO0lBSzVCLENBQUM7SUFIQyxzQkFBSSxpQ0FBSTs7OztRQUFSO1lBQ0UsT0FBTyxNQUFHLElBQUksQ0FBQyxPQUFPLENBQUMsQ0FBQyxDQUFDLHdCQUF3QixDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUyxJQUFJLFFBQVEsQ0FBRSxDQUFDO1FBQ25GLENBQUM7OztPQUFBOztnQkExQkYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxZQUFZO29CQUN0QixRQUFRLEVBQUUsK0xBSVQ7aUJBQ0Y7Ozs4QkFFRSxLQUFLO3VCQUdMLEtBQUs7NEJBR0wsS0FBSzswQkFHTCxLQUFLOzJCQUdMLEtBQUs7O0lBTVIsc0JBQUM7Q0FBQSxBQTNCRCxJQTJCQztTQW5CWSxlQUFlOzs7SUFDMUIsc0NBQ3dDOztJQUV4QywrQkFDd0I7O0lBRXhCLG9DQUNrQjs7SUFFbEIsa0NBQ3lCOztJQUV6QixtQ0FDMEIiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIElucHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1idXR0b24nLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxidXR0b24gW2F0dHIudHlwZV09XCJ0eXBlXCIgW25nQ2xhc3NdPVwiYnV0dG9uQ2xhc3NcIiBbZGlzYWJsZWRdPVwibG9hZGluZyB8fCBkaXNhYmxlZFwiPlxuICAgICAgPGkgW25nQ2xhc3NdPVwiaWNvblwiIGNsYXNzPVwibXItMVwiPjwvaT48bmctY29udGVudD48L25nLWNvbnRlbnQ+XG4gICAgPC9idXR0b24+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIEJ1dHRvbkNvbXBvbmVudCB7XG4gIEBJbnB1dCgpXG4gIGJ1dHRvbkNsYXNzOiBzdHJpbmcgPSAnYnRuIGJ0bi1wcmltYXJ5JztcblxuICBASW5wdXQoKVxuICB0eXBlOiBzdHJpbmcgPSAnYnV0dG9uJztcblxuICBASW5wdXQoKVxuICBpY29uQ2xhc3M6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBsb2FkaW5nOiBib29sZWFuID0gZmFsc2U7XG5cbiAgQElucHV0KClcbiAgZGlzYWJsZWQ6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBnZXQgaWNvbigpOiBzdHJpbmcge1xuICAgIHJldHVybiBgJHt0aGlzLmxvYWRpbmcgPyAnZmEgZmEtcHVsc2UgZmEtc3Bpbm5lcicgOiB0aGlzLmljb25DbGFzcyB8fCAnZC1ub25lJ31gO1xuICB9XG59XG4iXX0=