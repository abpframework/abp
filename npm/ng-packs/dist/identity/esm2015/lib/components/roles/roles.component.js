/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { FormBuilder, Validators } from '@angular/forms';
import { IdentityState } from '../../states/identity.state';
import { IdentityUpdateRole, IdentityAddRole, IdentityDeleteRole, IdentityGetRoleById, } from '../../actions/identity.actions';
import { pluck } from 'rxjs/operators';
import { ConfirmationService } from '@abp/ng.theme.shared';
export class RolesComponent {
    /**
     * @param {?} confirmationService
     * @param {?} fb
     * @param {?} store
     */
    constructor(confirmationService, fb, store) {
        this.confirmationService = confirmationService;
        this.fb = fb;
        this.store = store;
        this.visiblePermissions = false;
    }
    /**
     * @return {?}
     */
    createForm() {
        this.form = this.fb.group({
            name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
            isDefault: [this.selected.isDefault || false],
            isPublic: [this.selected.isPublic || false],
        });
    }
    /**
     * @return {?}
     */
    openModal() {
        this.createForm();
        this.isModalVisible = true;
    }
    /**
     * @return {?}
     */
    onAdd() {
        this.selected = (/** @type {?} */ ({}));
        this.openModal();
    }
    /**
     * @param {?} id
     * @return {?}
     */
    onEdit(id) {
        this.store
            .dispatch(new IdentityGetRoleById(id))
            .pipe(pluck('IdentityState', 'selectedRole'))
            .subscribe((/**
         * @param {?} selectedRole
         * @return {?}
         */
        selectedRole => {
            this.selected = selectedRole;
            this.openModal();
        }));
    }
    /**
     * @return {?}
     */
    save() {
        if (!this.form.valid)
            return;
        this.store
            .dispatch(this.selected.id
            ? new IdentityUpdateRole(Object.assign({}, this.form.value, { id: this.selected.id }))
            : new IdentityAddRole(this.form.value))
            .subscribe((/**
         * @return {?}
         */
        () => {
            this.isModalVisible = false;
        }));
    }
    /**
     * @param {?} id
     * @param {?} name
     * @return {?}
     */
    delete(id, name) {
        this.confirmationService
            .warn('AbpIdentity::RoleDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
            messageLocalizationParams: [name],
        })
            .subscribe((/**
         * @param {?} status
         * @return {?}
         */
        (status) => {
            if (status === "confirm" /* confirm */) {
                this.store.dispatch(new IdentityDeleteRole(id));
            }
        }));
    }
}
RolesComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-roles',
                template: "<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">{{ 'AbpIdentity::Roles' | abpLocalization }}</h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button id=\"create-role\" class=\"btn btn-primary\" type=\"button\" (click)=\"onAdd()\">\n          <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewRole' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"dt.filterGlobal($event.target.value, 'contains')\"\n      /></label>\n    </div>\n    <p-table #dt [value]=\"roles$ | async\" [globalFilterFields]=\"['name']\" [paginator]=\"true\" [rows]=\"10\">\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::RoleName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)=\"onEdit(data.id)\">{{ 'AbpIdentity::Edit' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)=\"providerKey = data.name; visiblePermissions = true\">\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)=\"delete(data.id, data.name)\">\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" *ngIf=\"isModalVisible\" [centered]=\"true\">\n  <ng-template #abpHeader>\n    <h3>{{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewRole') | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <form [formGroup]=\"form\">\n      <div class=\"form-group\">\n        <label for=\"role-name\">{{ 'AbpIdentity::RoleName' | abpLocalization }}</label\n        ><span> * </span>\n        <input type=\"text\" id=\"role-name\" class=\"form-control\" formControlName=\"name\" />\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-default\" class=\"custom-control-input\" formControlName=\"isDefault\" />\n        <label class=\"custom-control-label\" for=\"role-is-default\">{{\n          'AbpIdentity::DisplayName:IsDefault' | abpLocalization\n        }}</label>\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-public\" class=\"custom-control-input\" formControlName=\"isPublic\" />\n        <label class=\"custom-control-label\" for=\"role-is-public\">{{\n          'AbpIdentity::DisplayName:IsPublic' | abpLocalization\n        }}</label>\n      </div>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"save()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n\n<abp-permission-management\n  [(visible)]=\"visiblePermissions\"\n  providerName=\"Role\"\n  [providerKey]=\"providerKey\"\n></abp-permission-management>\n"
            }] }
];
/** @nocollapse */
RolesComponent.ctorParameters = () => [
    { type: ConfirmationService },
    { type: FormBuilder },
    { type: Store }
];
RolesComponent.propDecorators = {
    modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
};
tslib_1.__decorate([
    Select(IdentityState.getRoles),
    tslib_1.__metadata("design:type", Observable)
], RolesComponent.prototype, "roles$", void 0);
if (false) {
    /** @type {?} */
    RolesComponent.prototype.roles$;
    /** @type {?} */
    RolesComponent.prototype.form;
    /** @type {?} */
    RolesComponent.prototype.selected;
    /** @type {?} */
    RolesComponent.prototype.isModalVisible;
    /** @type {?} */
    RolesComponent.prototype.visiblePermissions;
    /** @type {?} */
    RolesComponent.prototype.providerKey;
    /** @type {?} */
    RolesComponent.prototype.modalContent;
    /**
     * @type {?}
     * @private
     */
    RolesComponent.prototype.confirmationService;
    /**
     * @type {?}
     * @private
     */
    RolesComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    RolesComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm9sZXMuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3JvbGVzL3JvbGVzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNsRSxPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLGFBQWEsRUFBRSxNQUFNLDZCQUE2QixDQUFDO0FBRTVELE9BQU8sRUFDTCxrQkFBa0IsRUFDbEIsZUFBZSxFQUNmLGtCQUFrQixFQUNsQixtQkFBbUIsR0FDcEIsTUFBTSxnQ0FBZ0MsQ0FBQztBQUN4QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkMsT0FBTyxFQUFFLG1CQUFtQixFQUFXLE1BQU0sc0JBQXNCLENBQUM7QUFNcEUsTUFBTSxPQUFPLGNBQWM7Ozs7OztJQWlCekIsWUFBb0IsbUJBQXdDLEVBQVUsRUFBZSxFQUFVLEtBQVk7UUFBdkYsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUFVLE9BQUUsR0FBRixFQUFFLENBQWE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBUDNHLHVCQUFrQixHQUFZLEtBQUssQ0FBQztJQU8wRSxDQUFDOzs7O0lBRS9HLFVBQVU7UUFDUixJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDO1lBQ3hCLElBQUksRUFBRSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxJQUFJLEVBQUUsRUFBRSxDQUFD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aWxkIH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgRm9ybUJ1aWxkZXIsIEZvcm1Hcm91cCwgVmFsaWRhdG9ycyB9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcbmltcG9ydCB7IElkZW50aXR5U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMvaWRlbnRpdHkuc3RhdGUnO1xuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi8uLi9tb2RlbHMvaWRlbnRpdHknO1xuaW1wb3J0IHtcbiAgSWRlbnRpdHlVcGRhdGVSb2xlLFxuICBJZGVudGl0eUFkZFJvbGUsXG4gIElkZW50aXR5RGVsZXRlUm9sZSxcbiAgSWRlbnRpdHlHZXRSb2xlQnlJZCxcbn0gZnJvbSAnLi4vLi4vYWN0aW9ucy9pZGVudGl0eS5hY3Rpb25zJztcbmltcG9ydCB7IHBsdWNrIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSwgVG9hc3RlciB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXJvbGVzJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3JvbGVzLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgUm9sZXNDb21wb25lbnQge1xuICBAU2VsZWN0KElkZW50aXR5U3RhdGUuZ2V0Um9sZXMpXG4gIHJvbGVzJDogT2JzZXJ2YWJsZTxJZGVudGl0eS5Sb2xlSXRlbVtdPjtcblxuICBmb3JtOiBGb3JtR3JvdXA7XG5cbiAgc2VsZWN0ZWQ6IElkZW50aXR5LlJvbGVJdGVtO1xuXG4gIGlzTW9kYWxWaXNpYmxlOiBib29sZWFuO1xuXG4gIHZpc2libGVQZXJtaXNzaW9uczogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG5cbiAgQFZpZXdDaGlsZCgnbW9kYWxDb250ZW50JywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIG1vZGFsQ29udGVudDogVGVtcGxhdGVSZWY8YW55PjtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UsIHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBjcmVhdGVGb3JtKCkge1xuICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgbmFtZTogW3RoaXMuc2VsZWN0ZWQubmFtZSB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICAgIGlzRGVmYXVsdDogW3RoaXMuc2VsZWN0ZWQuaXNEZWZhdWx0IHx8IGZhbHNlXSxcbiAgICAgIGlzUHVibGljOiBbdGhpcy5zZWxlY3RlZC5pc1B1YmxpYyB8fCBmYWxzZV0sXG4gICAgfSk7XG4gIH1cblxuICBvcGVuTW9kYWwoKSB7XG4gICAgdGhpcy5jcmVhdGVGb3JtKCk7XG4gICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IHRydWU7XG4gIH1cblxuICBvbkFkZCgpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0ge30gYXMgSWRlbnRpdHkuUm9sZUl0ZW07XG4gICAgdGhpcy5vcGVuTW9kYWwoKTtcbiAgfVxuXG4gIG9uRWRpdChpZDogc3RyaW5nKSB7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBJZGVudGl0eUdldFJvbGVCeUlkKGlkKSlcbiAgICAgIC5waXBlKHBsdWNrKCdJZGVudGl0eVN0YXRlJywgJ3NlbGVjdGVkUm9sZScpKVxuICAgICAgLnN1YnNjcmliZShzZWxlY3RlZFJvbGUgPT4ge1xuICAgICAgICB0aGlzLnNlbGVjdGVkID0gc2VsZWN0ZWRSb2xlO1xuICAgICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGlmICghdGhpcy5mb3JtLnZhbGlkKSByZXR1cm47XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIHRoaXMuc2VsZWN0ZWQuaWRcbiAgICAgICAgICA/IG5ldyBJZGVudGl0eVVwZGF0ZVJvbGUoeyAuLi50aGlzLmZvcm0udmFsdWUsIGlkOiB0aGlzLnNlbGVjdGVkLmlkIH0pXG4gICAgICAgICAgOiBuZXcgSWRlbnRpdHlBZGRSb2xlKHRoaXMuZm9ybS52YWx1ZSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgfSk7XG4gIH1cblxuICBkZWxldGUoaWQ6IHN0cmluZywgbmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybignQWJwSWRlbnRpdHk6OlJvbGVEZWxldGlvbkNvbmZpcm1hdGlvbk1lc3NhZ2UnLCAnQWJwSWRlbnRpdHk6OkFyZVlvdVN1cmUnLCB7XG4gICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFtuYW1lXSxcbiAgICAgIH0pXG4gICAgICAuc3Vic2NyaWJlKChzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSA9PiB7XG4gICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBJZGVudGl0eURlbGV0ZVJvbGUoaWQpKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cbn1cbiJdfQ==