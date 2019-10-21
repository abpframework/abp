/**
 * @fileoverview added by tsickle
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3RhYmxlLWVtcHR5LW1lc3NhZ2UvdGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRXpEO0lBQUE7UUFXRSxZQUFPLEdBQUcsQ0FBQyxDQUFDO1FBTVoseUJBQW9CLEdBQUcsWUFBWSxDQUFDO1FBR3BDLHFCQUFnQixHQUFHLDRCQUE0QixDQUFDO0lBS2xELENBQUM7SUFIQyxzQkFBSSxvREFBWTs7OztRQUFoQjtZQUNFLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBTyxJQUFJLENBQUMsb0JBQW9CLFVBQUssSUFBSSxDQUFDLGdCQUFrQixDQUFDO1FBQ2xGLENBQUM7OztPQUFBOztnQkF4QkYsU0FBUyxTQUFDOztvQkFFVCxRQUFRLEVBQUUsMkJBQTJCO29CQUNyQyxRQUFRLEVBQUUsd0hBSVQ7aUJBQ0Y7OzswQkFFRSxLQUFLOzBCQUdMLEtBQUs7dUNBR0wsS0FBSzttQ0FHTCxLQUFLOztJQU1SLGlDQUFDO0NBQUEsQUF6QkQsSUF5QkM7U0FoQlksMEJBQTBCOzs7SUFDckMsNkNBQ1k7O0lBRVosNkNBQ2dCOztJQUVoQiwwREFDb0M7O0lBRXBDLHNEQUNnRCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0LCBJbnB1dCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuXG5AQ29tcG9uZW50KHtcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtc2VsZWN0b3JcbiAgc2VsZWN0b3I6ICdbYWJwLXRhYmxlLWVtcHR5LW1lc3NhZ2VdJyxcbiAgdGVtcGxhdGU6IGBcbiAgICA8dGQgY2xhc3M9XCJ0ZXh0LWNlbnRlclwiIFthdHRyLmNvbHNwYW5dPVwiY29sc3BhblwiPlxuICAgICAge3sgZW1wdHlNZXNzYWdlIHwgYWJwTG9jYWxpemF0aW9uIH19XG4gICAgPC90ZD5cbiAgYFxufSlcbmV4cG9ydCBjbGFzcyBUYWJsZUVtcHR5TWVzc2FnZUNvbXBvbmVudCB7XG4gIEBJbnB1dCgpXG4gIGNvbHNwYW4gPSAyO1xuXG4gIEBJbnB1dCgpXG4gIG1lc3NhZ2U6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBsb2NhbGl6YXRpb25SZXNvdXJjZSA9ICdBYnBBY2NvdW50JztcblxuICBASW5wdXQoKVxuICBsb2NhbGl6YXRpb25Qcm9wID0gJ05vRGF0YUF2YWlsYWJsZUluRGF0YXRhYmxlJztcblxuICBnZXQgZW1wdHlNZXNzYWdlKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMubWVzc2FnZSB8fCBgJHt0aGlzLmxvY2FsaXphdGlvblJlc291cmNlfTo6JHt0aGlzLmxvY2FsaXphdGlvblByb3B9YDtcbiAgfVxufVxuIl19