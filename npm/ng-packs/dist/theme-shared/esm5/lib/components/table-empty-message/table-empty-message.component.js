/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/table-empty-message/table-empty-message.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input } from '@angular/core';
var TableEmptyMessageComponent = /** @class */ (function () {
    function TableEmptyMessageComponent() {
        this.colspan = 2;
        this.localizationResource = 'AbpAccount';
        this.localizationProp = 'NoDataAvailableInDatatable';
    }
    Object.defineProperty(TableEmptyMessageComponent.prototype, "emptyMessage", {
        get: /**
         * @return {?}
         */
        function () {
            return this.message || this.localizationResource + "::" + this.localizationProp;
        },
        enumerable: true,
        configurable: true
    });
    TableEmptyMessageComponent.decorators = [
        { type: Component, args: [{
                    // tslint:disable-next-line: component-selector
                    selector: '[abp-table-empty-message]',
                    template: "\n    <td class=\"text-center\" [attr.colspan]=\"colspan\">\n      {{ emptyMessage | abpLocalization }}\n    </td>\n  "
                }] }
    ];
    TableEmptyMessageComponent.propDecorators = {
        colspan: [{ type: Input }],
        message: [{ type: Input }],
        localizationResource: [{ type: Input }],
        localizationProp: [{ type: Input }]
    };
    return TableEmptyMessageComponent;
}());
export { TableEmptyMessageComponent };
if (false) {
    /** @type {?} */
    TableEmptyMessageComponent.prototype.colspan;
    /** @type {?} */
    TableEmptyMessageComponent.prototype.message;
    /** @type {?} */
    TableEmptyMessageComponent.prototype.localizationResource;
    /** @type {?} */
    TableEmptyMessageComponent.prototype.localizationProp;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3RhYmxlLWVtcHR5LW1lc3NhZ2UvdGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFVLEtBQUssRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUV6RDtJQUFBO1FBV0UsWUFBTyxHQUFHLENBQUMsQ0FBQztRQU1aLHlCQUFvQixHQUFHLFlBQVksQ0FBQztRQUdwQyxxQkFBZ0IsR0FBRyw0QkFBNEIsQ0FBQztJQUtsRCxDQUFDO0lBSEMsc0JBQUksb0RBQVk7Ozs7UUFBaEI7WUFDRSxPQUFPLElBQUksQ0FBQyxPQUFPLElBQU8sSUFBSSxDQUFDLG9CQUFvQixVQUFLLElBQUksQ0FBQyxnQkFBa0IsQ0FBQztRQUNsRixDQUFDOzs7T0FBQTs7Z0JBeEJGLFNBQVMsU0FBQzs7b0JBRVQsUUFBUSxFQUFFLDJCQUEyQjtvQkFDckMsUUFBUSxFQUFFLHdIQUlUO2lCQUNGOzs7MEJBRUUsS0FBSzswQkFHTCxLQUFLO3VDQUdMLEtBQUs7bUNBR0wsS0FBSzs7SUFNUixpQ0FBQztDQUFBLEFBekJELElBeUJDO1NBaEJZLDBCQUEwQjs7O0lBQ3JDLDZDQUNZOztJQUVaLDZDQUNnQjs7SUFFaEIsMERBQ29DOztJQUVwQyxzREFDZ0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCwgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogY29tcG9uZW50LXNlbGVjdG9yXG4gIHNlbGVjdG9yOiAnW2FicC10YWJsZS1lbXB0eS1tZXNzYWdlXScsXG4gIHRlbXBsYXRlOiBgXG4gICAgPHRkIGNsYXNzPVwidGV4dC1jZW50ZXJcIiBbYXR0ci5jb2xzcGFuXT1cImNvbHNwYW5cIj5cbiAgICAgIHt7IGVtcHR5TWVzc2FnZSB8IGFicExvY2FsaXphdGlvbiB9fVxuICAgIDwvdGQ+XG4gIGBcbn0pXG5leHBvcnQgY2xhc3MgVGFibGVFbXB0eU1lc3NhZ2VDb21wb25lbnQge1xuICBASW5wdXQoKVxuICBjb2xzcGFuID0gMjtcblxuICBASW5wdXQoKVxuICBtZXNzYWdlOiBzdHJpbmc7XG5cbiAgQElucHV0KClcbiAgbG9jYWxpemF0aW9uUmVzb3VyY2UgPSAnQWJwQWNjb3VudCc7XG5cbiAgQElucHV0KClcbiAgbG9jYWxpemF0aW9uUHJvcCA9ICdOb0RhdGFBdmFpbGFibGVJbkRhdGF0YWJsZSc7XG5cbiAgZ2V0IGVtcHR5TWVzc2FnZSgpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLm1lc3NhZ2UgfHwgYCR7dGhpcy5sb2NhbGl6YXRpb25SZXNvdXJjZX06OiR7dGhpcy5sb2NhbGl6YXRpb25Qcm9wfWA7XG4gIH1cbn1cbiJdfQ==