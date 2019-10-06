/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLnRoZW1lLnNoYXJlZC8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3RhYmxlLWVtcHR5LW1lc3NhZ2UvdGFibGUtZW1wdHktbWVzc2FnZS5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQVUsS0FBSyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBVXpELE1BQU0sT0FBTywwQkFBMEI7SUFSdkM7UUFVRSxZQUFPLEdBQUcsQ0FBQyxDQUFDO1FBTVoseUJBQW9CLEdBQVcsWUFBWSxDQUFDO1FBRzVDLHFCQUFnQixHQUFXLDRCQUE0QixDQUFDO0lBSzFELENBQUM7Ozs7SUFIQyxJQUFJLFlBQVk7UUFDZCxPQUFPLElBQUksQ0FBQyxPQUFPLElBQUksR0FBRyxJQUFJLENBQUMsb0JBQW9CLEtBQUssSUFBSSxDQUFDLGdCQUFnQixFQUFFLENBQUM7SUFDbEYsQ0FBQzs7O1lBdkJGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsMkJBQTJCO2dCQUNyQyxRQUFRLEVBQUU7Ozs7R0FJVDthQUNGOzs7c0JBRUUsS0FBSztzQkFHTCxLQUFLO21DQUdMLEtBQUs7K0JBR0wsS0FBSzs7OztJQVROLDZDQUNZOztJQUVaLDZDQUNnQjs7SUFFaEIsMERBQzRDOztJQUU1QyxzREFDd0QiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBDb21wb25lbnQsIE9uSW5pdCwgSW5wdXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnW2FicC10YWJsZS1lbXB0eS1tZXNzYWdlXScsXG4gIHRlbXBsYXRlOiBgXG4gICAgPHRkIGNsYXNzPVwidGV4dC1jZW50ZXJcIiBbYXR0ci5jb2xzcGFuXT1cImNvbHNwYW5cIj5cbiAgICAgIHt7IGVtcHR5TWVzc2FnZSB8IGFicExvY2FsaXphdGlvbiB9fVxuICAgIDwvdGQ+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIFRhYmxlRW1wdHlNZXNzYWdlQ29tcG9uZW50IHtcbiAgQElucHV0KClcbiAgY29sc3BhbiA9IDI7XG5cbiAgQElucHV0KClcbiAgbWVzc2FnZTogc3RyaW5nO1xuXG4gIEBJbnB1dCgpXG4gIGxvY2FsaXphdGlvblJlc291cmNlOiBzdHJpbmcgPSAnQWJwQWNjb3VudCc7XG5cbiAgQElucHV0KClcbiAgbG9jYWxpemF0aW9uUHJvcDogc3RyaW5nID0gJ05vRGF0YUF2YWlsYWJsZUluRGF0YXRhYmxlJztcblxuICBnZXQgZW1wdHlNZXNzYWdlKCk6IHN0cmluZyB7XG4gICAgcmV0dXJuIHRoaXMubWVzc2FnZSB8fCBgJHt0aGlzLmxvY2FsaXphdGlvblJlc291cmNlfTo6JHt0aGlzLmxvY2FsaXphdGlvblByb3B9YDtcbiAgfVxufVxuIl19