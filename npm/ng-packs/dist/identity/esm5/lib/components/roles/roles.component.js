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
var RolesComponent = /** @class */ (function () {
    function RolesComponent(confirmationService, fb, store) {
        this.confirmationService = confirmationService;
        this.fb = fb;
        this.store = store;
        this.visiblePermissions = false;
    }
    /**
     * @return {?}
     */
    RolesComponent.prototype.createForm = /**
     * @return {?}
     */
    function () {
        this.form = this.fb.group({
            name: [this.selected.name || '', [Validators.required, Validators.maxLength(256)]],
            isDefault: [this.selected.isDefault || false],
            isPublic: [this.selected.isPublic || false],
        });
    };
    /**
     * @return {?}
     */
    RolesComponent.prototype.openModal = /**
     * @return {?}
     */
    function () {
        this.createForm();
        this.isModalVisible = true;
    };
    /**
     * @return {?}
     */
    RolesComponent.prototype.onAdd = /**
     * @return {?}
     */
    function () {
        this.selected = (/** @type {?} */ ({}));
        this.openModal();
    };
    /**
     * @param {?} id
     * @return {?}
     */
    RolesComponent.prototype.onEdit = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        var _this = this;
        this.store
            .dispatch(new IdentityGetRoleById(id))
            .pipe(pluck('IdentityState', 'selectedRole'))
            .subscribe((/**
         * @param {?} selectedRole
         * @return {?}
         */
        function (selectedRole) {
            _this.selected = selectedRole;
            _this.openModal();
        }));
    };
    /**
     * @return {?}
     */
    RolesComponent.prototype.save = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (!this.form.valid)
            return;
        this.store
            .dispatch(this.selected.id
            ? new IdentityUpdateRole(tslib_1.__assign({}, this.form.value, { id: this.selected.id }))
            : new IdentityAddRole(this.form.value))
            .subscribe((/**
         * @return {?}
         */
        function () {
            _this.isModalVisible = false;
        }));
    };
    /**
     * @param {?} id
     * @param {?} name
     * @return {?}
     */
    RolesComponent.prototype.delete = /**
     * @param {?} id
     * @param {?} name
     * @return {?}
     */
    function (id, name) {
        var _this = this;
        this.confirmationService
            .warn('AbpIdentity::RoleDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
            messageLocalizationParams: [name],
        })
            .subscribe((/**
         * @param {?} status
         * @return {?}
         */
        function (status) {
            if (status === "confirm" /* confirm */) {
                _this.store.dispatch(new IdentityDeleteRole(id));
            }
        }));
    };
    RolesComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-roles',
                    template: "<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">{{ 'AbpIdentity::Roles' | abpLocalization }}</h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button id=\"create-role\" class=\"btn btn-primary\" type=\"button\" (click)=\"onAdd()\">\n          <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewRole' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"dt.filterGlobal($event.target.value, 'contains')\"\n      /></label>\n    </div>\n    <p-table #dt [value]=\"roles$ | async\" [globalFilterFields]=\"['name']\" [paginator]=\"true\" [rows]=\"10\">\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::RoleName' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)=\"onEdit(data.id)\">{{ 'AbpIdentity::Edit' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)=\"providerKey = data.name; visiblePermissions = true\">\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)=\"delete(data.id, data.name)\">\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal size=\"md\" [(visible)]=\"isModalVisible\" *ngIf=\"isModalVisible\" [centered]=\"true\">\n  <ng-template #abpHeader>\n    <h3>{{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewRole') | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <form [formGroup]=\"form\">\n      <div class=\"form-group\">\n        <label for=\"role-name\">{{ 'AbpIdentity::RoleName' | abpLocalization }}</label\n        ><span> * </span>\n        <input type=\"text\" id=\"role-name\" class=\"form-control\" formControlName=\"name\" />\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-default\" class=\"custom-control-input\" formControlName=\"isDefault\" />\n        <label class=\"custom-control-label\" for=\"role-is-default\">{{\n          'AbpIdentity::DisplayName:IsDefault' | abpLocalization\n        }}</label>\n      </div>\n\n      <div class=\"custom-checkbox custom-control mb-2\">\n        <input type=\"checkbox\" id=\"role-is-public\" class=\"custom-control-input\" formControlName=\"isPublic\" />\n        <label class=\"custom-control-label\" for=\"role-is-public\">{{\n          'AbpIdentity::DisplayName:IsPublic' | abpLocalization\n        }}</label>\n      </div>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"save()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n\n<abp-permission-management\n  [(visible)]=\"visiblePermissions\"\n  providerName=\"Role\"\n  [providerKey]=\"providerKey\"\n></abp-permission-management>\n"
                }] }
    ];
    /** @nocollapse */
    RolesComponent.ctorParameters = function () { return [
        { type: ConfirmationService },
        { type: FormBuilder },
        { type: Store }
    ]; };
    RolesComponent.propDecorators = {
        modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
    };
    tslib_1.__decorate([
        Select(IdentityState.getRoles),
        tslib_1.__metadata("design:type", Observable)
    ], RolesComponent.prototype, "roles$", void 0);
    return RolesComponent;
}());
export { RolesComponent };
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm9sZXMuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3JvbGVzL3JvbGVzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFFLFNBQVMsRUFBRSxNQUFNLGVBQWUsQ0FBQztBQUNsRSxPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDcEUsT0FBTyxFQUFFLGFBQWEsRUFBRSxNQUFNLDZCQUE2QixDQUFDO0FBRTVELE9BQU8sRUFDTCxrQkFBa0IsRUFDbEIsZUFBZSxFQUNmLGtCQUFrQixFQUNsQixtQkFBbUIsR0FDcEIsTUFBTSxnQ0FBZ0MsQ0FBQztBQUN4QyxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDdkMsT0FBTyxFQUFFLG1CQUFtQixFQUFXLE1BQU0sc0JBQXNCLENBQUM7QUFFcEU7SUFxQkUsd0JBQW9CLG1CQUF3QyxFQUFVLEVBQWUsRUFBVSxLQUFZO1FBQXZGLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7UUFBVSxPQUFFLEdBQUYsRUFBRSxDQUFhO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQVAzRyx1QkFBa0IsR0FBWSxLQUFLLENBQUM7SUFPMEUsQ0FBQzs7OztJQUUvRyxtQ0FBVTs7O0lBQVY7UUFDRSxJQUFJLENBQUMsSUFBSSxHQUFHLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUFDO1lBQ3hCLElBQUksRUFBRSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsSUFBSSxJQUFJLEVBQUUsRUFBRSxDQUFDLFVBQVUsQ0FBQyxR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QUUsNEJBQXVCOzs7OztJQUFFLCtCQUFvQiIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCwgVGVtcGxhdGVSZWYsIFZpZXdDaGlsZCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMgfSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBJZGVudGl0eVN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzL2lkZW50aXR5LnN0YXRlJztcbmltcG9ydCB7IElkZW50aXR5IH0gZnJvbSAnLi4vLi4vbW9kZWxzL2lkZW50aXR5JztcbmltcG9ydCB7XG4gIElkZW50aXR5VXBkYXRlUm9sZSxcbiAgSWRlbnRpdHlBZGRSb2xlLFxuICBJZGVudGl0eURlbGV0ZVJvbGUsXG4gIElkZW50aXR5R2V0Um9sZUJ5SWQsXG59IGZyb20gJy4uLy4uL2FjdGlvbnMvaWRlbnRpdHkuYWN0aW9ucyc7XG5pbXBvcnQgeyBwbHVjayB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UsIFRvYXN0ZXIgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1yb2xlcycsXG4gIHRlbXBsYXRlVXJsOiAnLi9yb2xlcy5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFJvbGVzQ29tcG9uZW50IHtcbiAgQFNlbGVjdChJZGVudGl0eVN0YXRlLmdldFJvbGVzKVxuICByb2xlcyQ6IE9ic2VydmFibGU8SWRlbnRpdHkuUm9sZUl0ZW1bXT47XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIHNlbGVjdGVkOiBJZGVudGl0eS5Sb2xlSXRlbTtcblxuICBpc01vZGFsVmlzaWJsZTogYm9vbGVhbjtcblxuICB2aXNpYmxlUGVybWlzc2lvbnM6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBwcm92aWRlcktleTogc3RyaW5nO1xuXG4gIEBWaWV3Q2hpbGQoJ21vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBtb2RhbENvbnRlbnQ6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlLCBwcml2YXRlIGZiOiBGb3JtQnVpbGRlciwgcHJpdmF0ZSBzdG9yZTogU3RvcmUpIHt9XG5cbiAgY3JlYXRlRm9ybSgpIHtcbiAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIG5hbWU6IFt0aGlzLnNlbGVjdGVkLm5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLnJlcXVpcmVkLCBWYWxpZGF0b3JzLm1heExlbmd0aCgyNTYpXV0sXG4gICAgICBpc0RlZmF1bHQ6IFt0aGlzLnNlbGVjdGVkLmlzRGVmYXVsdCB8fCBmYWxzZV0sXG4gICAgICBpc1B1YmxpYzogW3RoaXMuc2VsZWN0ZWQuaXNQdWJsaWMgfHwgZmFsc2VdLFxuICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIHRoaXMuY3JlYXRlRm9ybSgpO1xuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgb25BZGQoKSB7XG4gICAgdGhpcy5zZWxlY3RlZCA9IHt9IGFzIElkZW50aXR5LlJvbGVJdGVtO1xuICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gIH1cblxuICBvbkVkaXQoaWQ6IHN0cmluZykge1xuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChuZXcgSWRlbnRpdHlHZXRSb2xlQnlJZChpZCkpXG4gICAgICAucGlwZShwbHVjaygnSWRlbnRpdHlTdGF0ZScsICdzZWxlY3RlZFJvbGUnKSlcbiAgICAgIC5zdWJzY3JpYmUoc2VsZWN0ZWRSb2xlID0+IHtcbiAgICAgICAgdGhpcy5zZWxlY3RlZCA9IHNlbGVjdGVkUm9sZTtcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgc2F2ZSgpIHtcbiAgICBpZiAoIXRoaXMuZm9ybS52YWxpZCkgcmV0dXJuO1xuXG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKFxuICAgICAgICB0aGlzLnNlbGVjdGVkLmlkXG4gICAgICAgICAgPyBuZXcgSWRlbnRpdHlVcGRhdGVSb2xlKHsgLi4udGhpcy5mb3JtLnZhbHVlLCBpZDogdGhpcy5zZWxlY3RlZC5pZCB9KVxuICAgICAgICAgIDogbmV3IElkZW50aXR5QWRkUm9sZSh0aGlzLmZvcm0udmFsdWUpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSBmYWxzZTtcbiAgICAgIH0pO1xuICB9XG5cbiAgZGVsZXRlKGlkOiBzdHJpbmcsIG5hbWU6IHN0cmluZykge1xuICAgIHRoaXMuY29uZmlybWF0aW9uU2VydmljZVxuICAgICAgLndhcm4oJ0FicElkZW50aXR5OjpSb2xlRGVsZXRpb25Db25maXJtYXRpb25NZXNzYWdlJywgJ0FicElkZW50aXR5OjpBcmVZb3VTdXJlJywge1xuICAgICAgICBtZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zOiBbbmFtZV0sXG4gICAgICB9KVxuICAgICAgLnN1YnNjcmliZSgoc3RhdHVzOiBUb2FzdGVyLlN0YXR1cykgPT4ge1xuICAgICAgICBpZiAoc3RhdHVzID09PSBUb2FzdGVyLlN0YXR1cy5jb25maXJtKSB7XG4gICAgICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgSWRlbnRpdHlEZWxldGVSb2xlKGlkKSk7XG4gICAgICAgIH1cbiAgICAgIH0pO1xuICB9XG59XG4iXX0=