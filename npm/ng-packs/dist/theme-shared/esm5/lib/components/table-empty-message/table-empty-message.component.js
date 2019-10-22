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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3RhYmxlLWVtcHR5LW1lc3NhZ2UvdGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRXpEO0lBQUE7UUFXRSxZQUFPLEdBQUcsQ0FBQyxDQUFDO1FBTVoseUJBQW9CLEdBQUcsWUFBWSxDQUFDO1FBR3BDLHFCQUFnQixHQUFHLDRCQUE0QixDQUFDO0lBS2xELENBQUM7SUFIQyxzQkFBSSxvREFBWTs7OztRQUFoQjtZQUNFLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBTyxJQUFJLENBQUMsb0JBQW9CLFVBQUssSUFBSSxDQUFDLGdCQUFrQixDQUFDO1FBQ2xGLENBQUM7OztPQUFBOztnQkF4QkYsU0FBUyxTQUFDOztvQkFFVCxRQUFRLEVBQUUsMkJBQTJCO29CQUNyQyxRQUFRLEVBQUUsd0hBSVQ7aUJBQ0Y7OzswQkFFRSxLQUFLOzBCQUdMLEtBQUs7dUNBR0wsS0FBSzttQ0FHTCxLQUFLOztJQU1SLGlDQUFDO0NBQUEsQUF6QkQsSUF5QkM7U0FoQlksMEJBQTBCOzs7SUFDckMsNkNBQ1k7O0lBRVosNkNBQ2dCOztJQUVoQiwwREFDb0M7O0lBRXBDLHNEQUNnRCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0LCBJbnB1dCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgLy8gdHNsaW50OmRpc2FibGUtbmV4dC1saW5lOiBjb21wb25lbnQtc2VsZWN0b3JcclxuICBzZWxlY3RvcjogJ1thYnAtdGFibGUtZW1wdHktbWVzc2FnZV0nLFxyXG4gIHRlbXBsYXRlOiBgXHJcbiAgICA8dGQgY2xhc3M9XCJ0ZXh0LWNlbnRlclwiIFthdHRyLmNvbHNwYW5dPVwiY29sc3BhblwiPlxyXG4gICAgICB7eyBlbXB0eU1lc3NhZ2UgfCBhYnBMb2NhbGl6YXRpb24gfX1cclxuICAgIDwvdGQ+XHJcbiAgYFxyXG59KVxyXG5leHBvcnQgY2xhc3MgVGFibGVFbXB0eU1lc3NhZ2VDb21wb25lbnQge1xyXG4gIEBJbnB1dCgpXHJcbiAgY29sc3BhbiA9IDI7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgbWVzc2FnZTogc3RyaW5nO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGxvY2FsaXphdGlvblJlc291cmNlID0gJ0FicEFjY291bnQnO1xyXG5cclxuICBASW5wdXQoKVxyXG4gIGxvY2FsaXphdGlvblByb3AgPSAnTm9EYXRhQXZhaWxhYmxlSW5EYXRhdGFibGUnO1xyXG5cclxuICBnZXQgZW1wdHlNZXNzYWdlKCk6IHN0cmluZyB7XHJcbiAgICByZXR1cm4gdGhpcy5tZXNzYWdlIHx8IGAke3RoaXMubG9jYWxpemF0aW9uUmVzb3VyY2V9Ojoke3RoaXMubG9jYWxpemF0aW9uUHJvcH1gO1xyXG4gIH1cclxufVxyXG4iXX0=