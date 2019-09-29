/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3RhYmxlLWVtcHR5LW1lc3NhZ2UvdGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBRXpEO0lBQUE7UUFVRSxZQUFPLEdBQUcsQ0FBQyxDQUFDO1FBTVoseUJBQW9CLEdBQVcsWUFBWSxDQUFDO1FBRzVDLHFCQUFnQixHQUFXLDRCQUE0QixDQUFDO0lBSzFELENBQUM7SUFIQyxzQkFBSSxvREFBWTs7OztRQUFoQjtZQUNFLE9BQU8sSUFBSSxDQUFDLE9BQU8sSUFBTyxJQUFJLENBQUMsb0JBQW9CLFVBQUssSUFBSSxDQUFDLGdCQUFrQixDQUFDO1FBQ2xGLENBQUM7OztPQUFBOztnQkF2QkYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSwyQkFBMkI7b0JBQ3JDLFFBQVEsRUFBRSx3SEFJVDtpQkFDRjs7OzBCQUVFLEtBQUs7MEJBR0wsS0FBSzt1Q0FHTCxLQUFLO21DQUdMLEtBQUs7O0lBTVIsaUNBQUM7Q0FBQSxBQXhCRCxJQXdCQztTQWhCWSwwQkFBMEI7OztJQUNyQyw2Q0FDWTs7SUFFWiw2Q0FDZ0I7O0lBRWhCLDBEQUM0Qzs7SUFFNUMsc0RBQ3dEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQsIElucHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ1thYnAtdGFibGUtZW1wdHktbWVzc2FnZV0nLFxuICB0ZW1wbGF0ZTogYFxuICAgIDx0ZCBjbGFzcz1cInRleHQtY2VudGVyXCIgW2F0dHIuY29sc3Bhbl09XCJjb2xzcGFuXCI+XG4gICAgICB7eyBlbXB0eU1lc3NhZ2UgfCBhYnBMb2NhbGl6YXRpb24gfX1cbiAgICA8L3RkPlxuICBgLFxufSlcbmV4cG9ydCBjbGFzcyBUYWJsZUVtcHR5TWVzc2FnZUNvbXBvbmVudCB7XG4gIEBJbnB1dCgpXG4gIGNvbHNwYW4gPSAyO1xuXG4gIEBJbnB1dCgpXG4gIG1lc3NhZ2U6IHN0cmluZztcblxuICBASW5wdXQoKVxuICBsb2NhbGl6YXRpb25SZXNvdXJjZTogc3RyaW5nID0gJ0FicEFjY291bnQnO1xuXG4gIEBJbnB1dCgpXG4gIGxvY2FsaXphdGlvblByb3A6IHN0cmluZyA9ICdOb0RhdGFBdmFpbGFibGVJbkRhdGF0YWJsZSc7XG5cbiAgZ2V0IGVtcHR5TWVzc2FnZSgpOiBzdHJpbmcge1xuICAgIHJldHVybiB0aGlzLm1lc3NhZ2UgfHwgYCR7dGhpcy5sb2NhbGl6YXRpb25SZXNvdXJjZX06OiR7dGhpcy5sb2NhbGl6YXRpb25Qcm9wfWA7XG4gIH1cbn1cbiJdfQ==