/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, Input } from '@angular/core';
export class TableEmptyMessageComponent {
    constructor() {
        this.colspan = 2;
        this.localizationResource = 'AbpAccount';
        this.localizationProp = 'NoDataAvailableInDatatable';
    }
    /**
     * @return {?}
     */
    get emptyMessage() {
        return this.message || `${this.localizationResource}::${this.localizationProp}`;
    }
}
TableEmptyMessageComponent.decorators = [
    { type: Component, args: [{
                // tslint:disable-next-line: component-selector
                selector: '[abp-table-empty-message]',
                template: `
    <td class="text-center" [attr.colspan]="colspan">
      {{ emptyMessage | abpLocalization }}
    </td>
  `
            }] }
];
TableEmptyMessageComponent.propDecorators = {
    colspan: [{ type: Input }],
    message: [{ type: Input }],
    localizationResource: [{ type: Input }],
    localizationProp: [{ type: Input }]
};
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3RhYmxlLWVtcHR5LW1lc3NhZ2UvdGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBV3pELE1BQU0sT0FBTywwQkFBMEI7SUFUdkM7UUFXRSxZQUFPLEdBQUcsQ0FBQyxDQUFDO1FBTVoseUJBQW9CLEdBQUcsWUFBWSxDQUFDO1FBR3BDLHFCQUFnQixHQUFHLDRCQUE0QixDQUFDO0lBS2xELENBQUM7Ozs7SUFIQyxJQUFJLFlBQVk7UUFDZCxPQUFPLElBQUksQ0FBQyxPQUFPLElBQUksR0FBRyxJQUFJLENBQUMsb0JBQW9CLEtBQUssSUFBSSxDQUFDLGdCQUFnQixFQUFFLENBQUM7SUFDbEYsQ0FBQzs7O1lBeEJGLFNBQVMsU0FBQzs7Z0JBRVQsUUFBUSxFQUFFLDJCQUEyQjtnQkFDckMsUUFBUSxFQUFFOzs7O0dBSVQ7YUFDRjs7O3NCQUVFLEtBQUs7c0JBR0wsS0FBSzttQ0FHTCxLQUFLOytCQUdMLEtBQUs7Ozs7SUFUTiw2Q0FDWTs7SUFFWiw2Q0FDZ0I7O0lBRWhCLDBEQUNvQzs7SUFFcEMsc0RBQ2dEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50LCBPbkluaXQsIElucHV0IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGNvbXBvbmVudC1zZWxlY3RvclxyXG4gIHNlbGVjdG9yOiAnW2FicC10YWJsZS1lbXB0eS1tZXNzYWdlXScsXHJcbiAgdGVtcGxhdGU6IGBcclxuICAgIDx0ZCBjbGFzcz1cInRleHQtY2VudGVyXCIgW2F0dHIuY29sc3Bhbl09XCJjb2xzcGFuXCI+XHJcbiAgICAgIHt7IGVtcHR5TWVzc2FnZSB8IGFicExvY2FsaXphdGlvbiB9fVxyXG4gICAgPC90ZD5cclxuICBgXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBUYWJsZUVtcHR5TWVzc2FnZUNvbXBvbmVudCB7XHJcbiAgQElucHV0KClcclxuICBjb2xzcGFuID0gMjtcclxuXHJcbiAgQElucHV0KClcclxuICBtZXNzYWdlOiBzdHJpbmc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgbG9jYWxpemF0aW9uUmVzb3VyY2UgPSAnQWJwQWNjb3VudCc7XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgbG9jYWxpemF0aW9uUHJvcCA9ICdOb0RhdGFBdmFpbGFibGVJbkRhdGF0YWJsZSc7XHJcblxyXG4gIGdldCBlbXB0eU1lc3NhZ2UoKTogc3RyaW5nIHtcclxuICAgIHJldHVybiB0aGlzLm1lc3NhZ2UgfHwgYCR7dGhpcy5sb2NhbGl6YXRpb25SZXNvdXJjZX06OiR7dGhpcy5sb2NhbGl6YXRpb25Qcm9wfWA7XHJcbiAgfVxyXG59XHJcbiJdfQ==