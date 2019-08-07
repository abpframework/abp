/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
export class TenantBoxComponent {
    /**
     * @param {?} modalService
     * @param {?} fb
     */
    constructor(modalService, fb) {
        this.modalService = modalService;
        this.fb = fb;
    }
    /**
     * @return {?}
     */
    createForm() {
        this.form = this.fb.group({
            name: [this.selected.name],
        });
    }
    /**
     * @return {?}
     */
    openModal() {
        this.createForm();
        this.modalService.open(this.modalContent);
    }
    /**
     * @return {?}
     */
    onSwitch() {
        this.selected = (/** @type {?} */ ({}));
        this.openModal();
    }
    /**
     * @return {?}
     */
    save() {
        this.selected = this.form.value;
        this.modalService.dismissAll();
    }
}
TenantBoxComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-tenant-box',
                template: "<div\n  class=\"tenant-switch-box\"\n  style=\"background-color: #eee; margin-bottom: 20px; color: #000; padding: 10px; text-align: center;\"\n>\n  <span style=\"color: #666;\">{{ 'AbpUiMultiTenancy::Tenant' | abpLocalization }}: </span>\n  <strong>\n    <i>{{ selected?.name ? selected.name : ('AbpUiMultiTenancy::NotSelected' | abpLocalization) }}</i>\n  </strong>\n  (<a id=\"abp-tenant-switch-link\" style=\"color: #333; cursor: pointer\" (click)=\"onSwitch()\">{{\n    'AbpUiMultiTenancy::Switch' | abpLocalization\n  }}</a\n  >)\n</div>\n\n<ng-template #modalContent let-modal>\n  <div class=\"modal-header\">\n    <h5 class=\"modal-title\" id=\"modal-basic-title\">\n      SwitchTenant\n    </h5>\n    <button type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"modal.dismiss()\">\n      <span aria-hidden=\"true\">&times;</span>\n    </button>\n  </div>\n  <form [formGroup]=\"form\" (ngSubmit)=\"save()\">\n    <div class=\"modal-body\">\n      <div class=\"mt-2\">\n        <div class=\"form-group\">\n          <label for=\"name\">{{ 'AbpUiMultiTenancy::Name' | abpLocalization }}</label>\n          <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n        </div>\n        <p>{{ 'AbpUiMultiTenancy::SwitchTenantHint' | abpLocalization }}</p>\n      </div>\n    </div>\n\n    <div class=\"modal-footer\">\n      <button type=\"button\" class=\"btn btn-secondary\" data-dismiss=\"modal\" (click)=\"modal.close()\">\n        {{ 'AbpTenantManagement::Cancel' | abpLocalization }}\n      </button>\n      <button type=\"submit\" class=\"btn btn-primary\">\n        <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpTenantManagement::Save' | abpLocalization }}</span>\n      </button>\n    </div>\n  </form>\n</ng-template>\n"
            }] }
];
/** @nocollapse */
TenantBoxComponent.ctorParameters = () => [
    { type: NgbModal },
    { type: FormBuilder }
];
TenantBoxComponent.propDecorators = {
    modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
};
if (false) {
    /** @type {?} */
    TenantBoxComponent.prototype.form;
    /** @type {?} */
    TenantBoxComponent.prototype.selected;
    /** @type {?} */
    TenantBoxComponent.prototype.modalContent;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.modalService;
    /**
     * @type {?}
     * @private
     */
    TenantBoxComponent.prototype.fb;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidGVuYW50LWJveC5jb21wb25lbnQuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9AYWJwL25nLmFjY291bnQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy90ZW5hbnQtYm94L3RlbmFudC1ib3guY29tcG9uZW50LnRzIl0sIm5hbWVzIjpbXSwibWFwcGluZ3MiOiI7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLFdBQVcsRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbEUsT0FBTyxFQUFFLFdBQVcsRUFBYSxNQUFNLGdCQUFnQixDQUFDO0FBQ3hELE9BQU8sRUFBRSxRQUFRLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQU90RCxNQUFNLE9BQU8sa0JBQWtCOzs7OztJQUM3QixZQUFvQixZQUFzQixFQUFVLEVBQWU7UUFBL0MsaUJBQVksR0FBWixZQUFZLENBQVU7UUFBVSxPQUFFLEdBQUYsRUFBRSxDQUFhO0lBQUcsQ0FBQzs7OztJQVN2RSxVQUFVO1FBQ1IsSUFBSSxDQUFDLElBQUksR0FBRyxJQUFJLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQztZQUN4QixJQUFJLEVBQUUsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksQ0FBQztTQUMzQixDQUFDLENBQUM7SUFDTCxDQUFDOzs7O0lBRUQsU0FBUztRQUNQLElBQUksQ0FBQyxVQUFVLEVBQUUsQ0FBQztRQUNsQixJQUFJLENBQUMsWUFBWSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsWUFBWSxDQUFDLENBQUM7SUFDNUMsQ0FBQzs7OztJQUVELFFBQVE7UUFDTixJQUFJLENBQUMsUUFBUSxHQUFHLG1CQUFBLEVBQUUsRUFBaUIsQ0FBQztRQUNwQyxJQUFJLENBQUMsU0FBUyxFQUFFLENBQUM7SUFDbkIsQ0FBQzs7OztJQUVELElBQUk7UUFDRixJQUFJLENBQUMsUUFBUSxHQUFHLElBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxDQUFDO1FBQ2hDLElBQUksQ0FBQyxZQUFZLENBQUMsVUFBVSxFQUFFLENBQUM7SUFDakMsQ0FBQzs7O1lBakNGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsZ0JBQWdCO2dCQUMxQiwydkRBQTBDO2FBQzNDOzs7O1lBTlEsUUFBUTtZQURSLFdBQVc7OzsyQkFlakIsU0FBUyxTQUFDLGNBQWMsRUFBRSxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUU7Ozs7SUFKNUMsa0NBQWdCOztJQUVoQixzQ0FBd0I7O0lBRXhCLDBDQUMrQjs7Ozs7SUFQbkIsMENBQThCOzs7OztJQUFFLGdDQUF1QiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgVGVtcGxhdGVSZWYsIFZpZXdDaGlsZCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IE5nYk1vZGFsIH0gZnJvbSAnQG5nLWJvb3RzdHJhcC9uZy1ib290c3RyYXAnO1xuaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXRlbmFudC1ib3gnLFxuICB0ZW1wbGF0ZVVybDogJy4vdGVuYW50LWJveC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFRlbmFudEJveENvbXBvbmVudCB7XG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgbW9kYWxTZXJ2aWNlOiBOZ2JNb2RhbCwgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIpIHt9XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIHNlbGVjdGVkOiBBQlAuQmFzaWNJdGVtO1xuXG4gIEBWaWV3Q2hpbGQoJ21vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBtb2RhbENvbnRlbnQ6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgY3JlYXRlRm9ybSgpIHtcbiAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIG5hbWU6IFt0aGlzLnNlbGVjdGVkLm5hbWVdLFxuICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIHRoaXMuY3JlYXRlRm9ybSgpO1xuICAgIHRoaXMubW9kYWxTZXJ2aWNlLm9wZW4odGhpcy5tb2RhbENvbnRlbnQpO1xuICB9XG5cbiAgb25Td2l0Y2goKSB7XG4gICAgdGhpcy5zZWxlY3RlZCA9IHt9IGFzIEFCUC5CYXNpY0l0ZW07XG4gICAgdGhpcy5vcGVuTW9kYWwoKTtcbiAgfVxuXG4gIHNhdmUoKSB7XG4gICAgdGhpcy5zZWxlY3RlZCA9IHRoaXMuZm9ybS52YWx1ZTtcbiAgICB0aGlzLm1vZGFsU2VydmljZS5kaXNtaXNzQWxsKCk7XG4gIH1cbn1cbiJdfQ==