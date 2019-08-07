/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfirmationService } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { validatePassword } from '@ngx-validate/core';
import { Select, Store } from '@ngxs/store';
import { combineLatest, Observable, Subject } from 'rxjs';
import { debounceTime, filter, map, pluck, take } from 'rxjs/operators';
import snq from 'snq';
import { IdentityAddUser, IdentityDeleteUser, IdentityGetUserById, IdentityGetUserRoles, IdentityGetUsers, IdentityUpdateUser, } from '../../actions/identity.actions';
import { IdentityState } from '../../states/identity.state';
var UsersComponent = /** @class */ (function () {
    function UsersComponent(confirmationService, fb, store) {
        this.confirmationService = confirmationService;
        this.fb = fb;
        this.store = store;
        this.visiblePermissions = false;
        this.pageQuery = {
            sorting: 'userName',
        };
        this.loading = false;
        this.search$ = new Subject();
        this.trackByFn = (/**
         * @param {?} index
         * @param {?} item
         * @return {?}
         */
        function (index, item) { return Object.keys(item)[0] || index; });
    }
    Object.defineProperty(UsersComponent.prototype, "roleGroups", {
        get: /**
         * @return {?}
         */
        function () {
            var _this = this;
            return snq((/**
             * @return {?}
             */
            function () { return (/** @type {?} */ (((/** @type {?} */ (_this.form.get('roleNames')))).controls)); }), []);
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    UsersComponent.prototype.ngOnInit = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.search$.pipe(debounceTime(300)).subscribe((/**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            _this.pageQuery.filter = value;
            _this.get();
        }));
    };
    /**
     * @return {?}
     */
    UsersComponent.prototype.buildForm = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.roles = this.store.selectSnapshot(IdentityState.getRoles);
        this.form = this.fb.group({
            password: [
                '',
                [
                    Validators.required,
                    Validators.maxLength(32),
                    Validators.minLength(6),
                    validatePassword(['small', 'capital', 'number', 'special']),
                ],
            ],
            userName: [this.selected.userName || '', [Validators.required, Validators.maxLength(256)]],
            email: [this.selected.email || '', [Validators.required, Validators.email, Validators.maxLength(256)]],
            name: [this.selected.name || '', [Validators.maxLength(64)]],
            surname: [this.selected.surname || '', [Validators.maxLength(64)]],
            phoneNumber: [this.selected.phoneNumber || '', [Validators.maxLength(16)]],
            lockoutEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
            twoFactorEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
            roleNames: this.fb.array(this.roles.map((/**
             * @param {?} role
             * @return {?}
             */
            function (role) {
                var _a;
                return _this.fb.group((_a = {},
                    _a[role.name] = [!!snq((/**
                         * @return {?}
                         */
                        function () { return _this.selectedUserRoles.find((/**
                         * @param {?} userRole
                         * @return {?}
                         */
                        function (userRole) { return userRole.id === role.id; })); }))],
                    _a));
            }))),
        });
    };
    /**
     * @return {?}
     */
    UsersComponent.prototype.openModal = /**
     * @return {?}
     */
    function () {
        this.buildForm();
        this.isModalVisible = true;
    };
    /**
     * @return {?}
     */
    UsersComponent.prototype.onAdd = /**
     * @return {?}
     */
    function () {
        this.selected = (/** @type {?} */ ({}));
        this.selectedUserRoles = (/** @type {?} */ ([]));
        this.openModal();
    };
    /**
     * @param {?} id
     * @return {?}
     */
    UsersComponent.prototype.onEdit = /**
     * @param {?} id
     * @return {?}
     */
    function (id) {
        var _this = this;
        combineLatest([this.store.dispatch(new IdentityGetUserById(id)), this.store.dispatch(new IdentityGetUserRoles(id))])
            .pipe(filter((/**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var _b = tslib_1.__read(_a, 2), res1 = _b[0], res2 = _b[1];
            return res1 && res2;
        })), map((/**
         * @param {?} __0
         * @return {?}
         */
        function (_a) {
            var _b = tslib_1.__read(_a, 2), state = _b[0], _ = _b[1];
            return state;
        })), pluck('IdentityState'), take(1))
            .subscribe((/**
         * @param {?} state
         * @return {?}
         */
        function (state) {
            _this.selected = state.selectedUser;
            _this.selectedUserRoles = state.selectedUserRoles;
            _this.openModal();
        }));
    };
    /**
     * @return {?}
     */
    UsersComponent.prototype.save = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (!this.form.valid)
            return;
        var roleNames = this.form.value.roleNames;
        /** @type {?} */
        var mappedRoleNames = snq((/**
         * @return {?}
         */
        function () { return roleNames.filter((/**
         * @param {?} role
         * @return {?}
         */
        function (role) { return !!role[Object.keys(role)[0]]; })).map((/**
         * @param {?} role
         * @return {?}
         */
        function (role) { return Object.keys(role)[0]; })); }), []);
        this.store
            .dispatch(this.selected.id
            ? new IdentityUpdateUser(tslib_1.__assign({}, this.form.value, { id: this.selected.id, roleNames: mappedRoleNames }))
            : new IdentityAddUser(tslib_1.__assign({}, this.form.value, { roleNames: mappedRoleNames })))
            .subscribe((/**
         * @return {?}
         */
        function () {
            _this.isModalVisible = false;
        }));
    };
    /**
     * @param {?} id
     * @param {?} userName
     * @return {?}
     */
    UsersComponent.prototype.delete = /**
     * @param {?} id
     * @param {?} userName
     * @return {?}
     */
    function (id, userName) {
        var _this = this;
        this.confirmationService
            .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
            messageLocalizationParams: [userName],
        })
            .subscribe((/**
         * @param {?} status
         * @return {?}
         */
        function (status) {
            if (status === "confirm" /* confirm */) {
                _this.store.dispatch(new IdentityDeleteUser(id));
            }
        }));
    };
    /**
     * @param {?} data
     * @return {?}
     */
    UsersComponent.prototype.onPageChange = /**
     * @param {?} data
     * @return {?}
     */
    function (data) {
        this.pageQuery.skipCount = data.first;
        this.pageQuery.maxResultCount = data.rows;
        this.get();
    };
    /**
     * @return {?}
     */
    UsersComponent.prototype.get = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.loading = true;
        this.store.dispatch(new IdentityGetUsers(this.pageQuery)).subscribe((/**
         * @return {?}
         */
        function () { return (_this.loading = false); }));
    };
    UsersComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-users',
                    template: "<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">{{ 'AbpIdentity::Users' | abpLocalization }}</h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          [abpPermission]=\"'AbpIdentity.Users.Create'\"\n          id=\"create-role\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"onAdd()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewUser' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          placeholder=\"Search\"\n          (input)=\"search$.next($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      [value]=\"data$ | async\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::UserName' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</th>\n          <th>{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)=\"onEdit(data.id)\">{{ 'AbpIdentity::Edit' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)=\"providerKey = data.id; visiblePermissions = true\">\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)=\"delete(data.id, data.userName)\">\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.userName }}</td>\n          <td>{{ data.email }}</td>\n          <td>{{ data.phoneNumber }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\" *ngIf=\"isModalVisible\">\n  <ng-template #abpHeader>\n    <h3>{{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewUser') | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <perfect-scrollbar class=\"ps-show-always\" style=\"max-height: 65vh;\">\n      <form [formGroup]=\"form\">\n        <ngb-tabset>\n          <ngb-tab [title]=\"'AbpIdentity::UserInformations' | abpLocalization\">\n            <ng-template ngbTabContent>\n              <div class=\"mt-2\">\n                <div class=\"form-group\">\n                  <label for=\"user-name\">{{ 'AbpIdentity::UserName' | abpLocalization }}</label\n                  ><span> * </span>\n                  <input type=\"text\" id=\"user-name\" class=\"form-control\" formControlName=\"userName\" />\n                </div>\n\n                <div class=\"form-group\">\n                  <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label>\n                  <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n                </div>\n\n                <div class=\"form-group\">\n                  <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label>\n                  <input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n                </div>\n\n                <div class=\"form-group\">\n                  <label for=\"password\">{{ 'AbpIdentity::Password' | abpLocalization }}</label\n                  ><span> * </span>\n                  <input\n                    type=\"password\"\n                    id=\"password\"\n                    autocomplete=\"new-password\"\n                    class=\"form-control\"\n                    formControlName=\"password\"\n                  />\n                </div>\n\n                <div class=\"form-group\">\n                  <label for=\"email\">{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</label\n                  ><span> * </span>\n                  <input type=\"text\" id=\"email\" class=\"form-control\" formControlName=\"email\" />\n                </div>\n\n                <div class=\"form-group\">\n                  <label for=\"phone-number\">{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</label>\n                  <input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n                </div>\n\n                <div class=\"custom-checkbox custom-control mb-2\">\n                  <input\n                    type=\"checkbox\"\n                    id=\"lockout-checkbox\"\n                    class=\"custom-control-input\"\n                    formControlName=\"lockoutEnabled\"\n                  />\n                  <label class=\"custom-control-label\" for=\"lockout-checkbox\">{{\n                    'AbpIdentity::DisplayName:LockoutEnabled' | abpLocalization\n                  }}</label>\n                </div>\n\n                <div class=\"custom-checkbox custom-control mb-2\">\n                  <input\n                    type=\"checkbox\"\n                    id=\"two-factor-checkbox\"\n                    class=\"custom-control-input\"\n                    formControlName=\"twoFactorEnabled\"\n                  />\n                  <label class=\"custom-control-label\" for=\"two-factor-checkbox\">{{\n                    'AbpIdentity::DisplayName:TwoFactorEnabled' | abpLocalization\n                  }}</label>\n                </div>\n              </div>\n            </ng-template>\n          </ngb-tab>\n          <ngb-tab [title]=\"'AbpIdentity::Roles' | abpLocalization\">\n            <ng-template ngbTabContent>\n              <div class=\"mt-2\">\n                <div\n                  *ngFor=\"let roleGroup of roleGroups; let i = index; trackBy: trackByFn\"\n                  class=\"custom-checkbox custom-control mb-2\"\n                >\n                  <input\n                    type=\"checkbox\"\n                    name=\"Roles[0].IsAssigned\"\n                    value=\"true\"\n                    class=\"custom-control-input\"\n                    [attr.id]=\"'roles-' + i\"\n                    [formControl]=\"roleGroup.controls[roles[i].name]\"\n                  />\n                  <label class=\"custom-control-label\" [attr.for]=\"'roles-' + i\">{{ roles[i].name }}</label>\n                </div>\n              </div>\n            </ng-template>\n          </ngb-tab>\n        </ngb-tabset>\n      </form>\n    </perfect-scrollbar>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <button type=\"button\" class=\"btn btn-primary\" (click)=\"save()\">\n      <i class=\"fa fa-check mr-1\"></i> <span>{{ 'AbpIdentity::Save' | abpLocalization }}</span>\n    </button>\n  </ng-template>\n</abp-modal>\n\n<abp-permission-management\n  [(visible)]=\"visiblePermissions\"\n  providerName=\"User\"\n  [providerKey]=\"providerKey\"\n></abp-permission-management>\n"
                }] }
    ];
    /** @nocollapse */
    UsersComponent.ctorParameters = function () { return [
        { type: ConfirmationService },
        { type: FormBuilder },
        { type: Store }
    ]; };
    UsersComponent.propDecorators = {
        modalContent: [{ type: ViewChild, args: ['modalContent', { static: false },] }]
    };
    tslib_1.__decorate([
        Select(IdentityState.getUsers),
        tslib_1.__metadata("design:type", Observable)
    ], UsersComponent.prototype, "data$", void 0);
    tslib_1.__decorate([
        Select(IdentityState.getUsersTotalCount),
        tslib_1.__metadata("design:type", Observable)
    ], UsersComponent.prototype, "totalCount$", void 0);
    return UsersComponent;
}());
export { UsersComponent };
if (false) {
    /** @type {?} */
    UsersComponent.prototype.data$;
    /** @type {?} */
    UsersComponent.prototype.totalCount$;
    /** @type {?} */
    UsersComponent.prototype.modalContent;
    /** @type {?} */
    UsersComponent.prototype.form;
    /** @type {?} */
    UsersComponent.prototype.selected;
    /** @type {?} */
    UsersComponent.prototype.selectedUserRoles;
    /** @type {?} */
    UsersComponent.prototype.roles;
    /** @type {?} */
    UsersComponent.prototype.visiblePermissions;
    /** @type {?} */
    UsersComponent.prototype.providerKey;
    /** @type {?} */
    UsersComponent.prototype.pageQuery;
    /** @type {?} */
    UsersComponent.prototype.isModalVisible;
    /** @type {?} */
    UsersComponent.prototype.loading;
    /** @type {?} */
    UsersComponent.prototype.search$;
    /** @type {?} */
    UsersComponent.prototype.trackByFn;
    /**
     * @type {?}
     * @private
     */
    UsersComponent.prototype.confirmationService;
    /**
     * @type {?}
     * @private
     */
    UsersComponent.prototype.fb;
    /**
     * @type {?}
     * @private
     */
    UsersComponent.prototype.store;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidXNlcnMuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3VzZXJzL3VzZXJzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQVUsV0FBVyxFQUFtQixTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDM0YsT0FBTyxFQUE4QixXQUFXLEVBQWEsVUFBVSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDaEcsT0FBTyxFQUFFLGdCQUFnQixFQUFFLE1BQU0sb0JBQW9CLENBQUM7QUFDdEQsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLGFBQWEsRUFBRSxVQUFVLEVBQUUsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzFELE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLEdBQUcsRUFBRSxLQUFLLEVBQUUsSUFBSSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDeEUsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFDTCxlQUFlLEVBQ2Ysa0JBQWtCLEVBQ2xCLG1CQUFtQixFQUNuQixvQkFBb0IsRUFDcEIsZ0JBQWdCLEVBQ2hCLGtCQUFrQixHQUNuQixNQUFNLGdDQUFnQyxDQUFDO0FBRXhDLE9BQU8sRUFBRSxhQUFhLEVBQUUsTUFBTSw2QkFBNkIsQ0FBQztBQUM1RDtJQTBDRSx3QkFBb0IsbUJBQXdDLEVBQVUsRUFBZSxFQUFVLEtBQVk7UUFBdkYsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUFVLE9BQUUsR0FBRixFQUFFLENBQWE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBcEIzRyx1QkFBa0IsR0FBWSxLQUFLLENBQUM7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QUMsT0FBTyxHQUFHLElBQUksQ0FBQztRQUNwQixJQUFJLENBQUMsS0FBSyxDQUFDLFFBQVEsQ0FBQyxJQUFJLGdCQUFnQixDQUFDLElBQUksQ0FBQyxTQUFTLENBQUMsQ0FBQyxDQUFDLFNBQVM7OztRQUFDLGNBQU0sT0FBQSxDQUFDLEtBQUksQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDLEVBQXRCLENBQXNCLEVBQUMsQ0FBQztJQUNwRyxDQUFDOztnQkE1SkYsU0FBUyxTQUFDO29CQUNULFFBQVEsRUFBRSxXQUFXO29CQUNyQiwwNVBBQXFDO2lCQUN0Qzs7OztnQkFyQlEsbUJBQW1CO2dCQUVTLFdBQVc7Z0JBRS9CLEtBQUs7OzsrQkF5Qm5CLFNBQVMsU0FBQyxjQUFjLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFOztJQUw1QztRQURDLE1BQU0sQ0FBQyxhQUFhLENBQUMsUUFBUSxDQUFDOzBDQUN4QixVQUFVO2lEQUFzQjtJQUd2QztRQURDLE1BQU0sQ0FBQyxhQUFhLENBQUMsa0JBQWtCLENBQUM7MENBQzVCLFVBQVU7dURBQVM7SUFvSmxDLHFCQUFDO0NBQUEsQUE3SkQsSUE2SkM7U0F6SlksY0FBYzs7O0lBQ3pCLCtCQUN1Qzs7SUFFdkMscUNBQ2dDOztJQUVoQyxzQ0FDK0I7O0lBRS9CLDhCQUFnQjs7SUFFaEIsa0NBQTRCOztJQUU1QiwyQ0FBdUM7O0lBRXZDLCtCQUEyQjs7SUFFM0IsNENBQW9DOztJQUVwQyxxQ0FBb0I7O0lBRXBCLG1DQUVFOztJQUVGLHdDQUF3Qjs7SUFFeEIsaUNBQXlCOztJQUV6QixpQ0FBZ0M7O0lBRWhDLG1DQUE2Rjs7Ozs7SUFNakYsNkNBQWdEOzs7OztJQUFFLDRCQUF1Qjs7Ozs7SUFBRSwrQkFBb0IiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQgeyBBQlAgfSBmcm9tICdAYWJwL25nLmNvcmUnO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSwgVG9hc3RlciB9IGZyb20gJ0BhYnAvbmcudGhlbWUuc2hhcmVkJztcbmltcG9ydCB7IENvbXBvbmVudCwgT25Jbml0LCBUZW1wbGF0ZVJlZiwgVHJhY2tCeUZ1bmN0aW9uLCBWaWV3Q2hpbGQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEFic3RyYWN0Q29udHJvbCwgRm9ybUFycmF5LCBGb3JtQnVpbGRlciwgRm9ybUdyb3VwLCBWYWxpZGF0b3JzIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgdmFsaWRhdGVQYXNzd29yZCB9IGZyb20gJ0BuZ3gtdmFsaWRhdGUvY29yZSc7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgY29tYmluZUxhdGVzdCwgT2JzZXJ2YWJsZSwgU3ViamVjdCB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZGVib3VuY2VUaW1lLCBmaWx0ZXIsIG1hcCwgcGx1Y2ssIHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQge1xuICBJZGVudGl0eUFkZFVzZXIsXG4gIElkZW50aXR5RGVsZXRlVXNlcixcbiAgSWRlbnRpdHlHZXRVc2VyQnlJZCxcbiAgSWRlbnRpdHlHZXRVc2VyUm9sZXMsXG4gIElkZW50aXR5R2V0VXNlcnMsXG4gIElkZW50aXR5VXBkYXRlVXNlcixcbn0gZnJvbSAnLi4vLi4vYWN0aW9ucy9pZGVudGl0eS5hY3Rpb25zJztcbmltcG9ydCB7IElkZW50aXR5IH0gZnJvbSAnLi4vLi4vbW9kZWxzL2lkZW50aXR5JztcbmltcG9ydCB7IElkZW50aXR5U3RhdGUgfSBmcm9tICcuLi8uLi9zdGF0ZXMvaWRlbnRpdHkuc3RhdGUnO1xuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXVzZXJzJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3VzZXJzLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgVXNlcnNDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBAU2VsZWN0KElkZW50aXR5U3RhdGUuZ2V0VXNlcnMpXG4gIGRhdGEkOiBPYnNlcnZhYmxlPElkZW50aXR5LlVzZXJJdGVtW10+O1xuXG4gIEBTZWxlY3QoSWRlbnRpdHlTdGF0ZS5nZXRVc2Vyc1RvdGFsQ291bnQpXG4gIHRvdGFsQ291bnQkOiBPYnNlcnZhYmxlPG51bWJlcj47XG5cbiAgQFZpZXdDaGlsZCgnbW9kYWxDb250ZW50JywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIG1vZGFsQ29udGVudDogVGVtcGxhdGVSZWY8YW55PjtcblxuICBmb3JtOiBGb3JtR3JvdXA7XG5cbiAgc2VsZWN0ZWQ6IElkZW50aXR5LlVzZXJJdGVtO1xuXG4gIHNlbGVjdGVkVXNlclJvbGVzOiBJZGVudGl0eS5Sb2xlSXRlbVtdO1xuXG4gIHJvbGVzOiBJZGVudGl0eS5Sb2xlSXRlbVtdO1xuXG4gIHZpc2libGVQZXJtaXNzaW9uczogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG5cbiAgcGFnZVF1ZXJ5OiBBQlAuUGFnZVF1ZXJ5UGFyYW1zID0ge1xuICAgIHNvcnRpbmc6ICd1c2VyTmFtZScsXG4gIH07XG5cbiAgaXNNb2RhbFZpc2libGU6IGJvb2xlYW47XG5cbiAgbG9hZGluZzogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIHNlYXJjaCQgPSBuZXcgU3ViamVjdDxzdHJpbmc+KCk7XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248QWJzdHJhY3RDb250cm9sPiA9IChpbmRleCwgaXRlbSkgPT4gT2JqZWN0LmtleXMoaXRlbSlbMF0gfHwgaW5kZXg7XG5cbiAgZ2V0IHJvbGVHcm91cHMoKTogRm9ybUdyb3VwW10ge1xuICAgIHJldHVybiBzbnEoKCkgPT4gKHRoaXMuZm9ybS5nZXQoJ3JvbGVOYW1lcycpIGFzIEZvcm1BcnJheSkuY29udHJvbHMgYXMgRm9ybUdyb3VwW10sIFtdKTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgY29uZmlybWF0aW9uU2VydmljZTogQ29uZmlybWF0aW9uU2VydmljZSwgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIG5nT25Jbml0KCkge1xuICAgIHRoaXMuc2VhcmNoJC5waXBlKGRlYm91bmNlVGltZSgzMDApKS5zdWJzY3JpYmUodmFsdWUgPT4ge1xuICAgICAgdGhpcy5wYWdlUXVlcnkuZmlsdGVyID0gdmFsdWU7XG4gICAgICB0aGlzLmdldCgpO1xuICAgIH0pO1xuICB9XG5cbiAgYnVpbGRGb3JtKCkge1xuICAgIHRoaXMucm9sZXMgPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KElkZW50aXR5U3RhdGUuZ2V0Um9sZXMpO1xuXG4gICAgdGhpcy5mb3JtID0gdGhpcy5mYi5ncm91cCh7XG4gICAgICBwYXNzd29yZDogW1xuICAgICAgICAnJyxcbiAgICAgICAgW1xuICAgICAgICAgIFZhbGlkYXRvcnMucmVxdWlyZWQsXG4gICAgICAgICAgVmFsaWRhdG9ycy5tYXhMZW5ndGgoMzIpLFxuICAgICAgICAgIFZhbGlkYXRvcnMubWluTGVuZ3RoKDYpLFxuICAgICAgICAgIHZhbGlkYXRlUGFzc3dvcmQoWydzbWFsbCcsICdjYXBpdGFsJywgJ251bWJlcicsICdzcGVjaWFsJ10pLFxuICAgICAgICBdLFxuICAgICAgXSxcbiAgICAgIHVzZXJOYW1lOiBbdGhpcy5zZWxlY3RlZC51c2VyTmFtZSB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICAgIGVtYWlsOiBbdGhpcy5zZWxlY3RlZC5lbWFpbCB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMuZW1haWwsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICAgIG5hbWU6IFt0aGlzLnNlbGVjdGVkLm5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLm1heExlbmd0aCg2NCldXSxcbiAgICAgIHN1cm5hbWU6IFt0aGlzLnNlbGVjdGVkLnN1cm5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLm1heExlbmd0aCg2NCldXSxcbiAgICAgIHBob25lTnVtYmVyOiBbdGhpcy5zZWxlY3RlZC5waG9uZU51bWJlciB8fCAnJywgW1ZhbGlkYXRvcnMubWF4TGVuZ3RoKDE2KV1dLFxuICAgICAgbG9ja291dEVuYWJsZWQ6IFt0aGlzLnNlbGVjdGVkLnR3b0ZhY3RvckVuYWJsZWQgfHwgKHRoaXMuc2VsZWN0ZWQuaWQgPyBmYWxzZSA6IHRydWUpXSxcbiAgICAgIHR3b0ZhY3RvckVuYWJsZWQ6IFt0aGlzLnNlbGVjdGVkLnR3b0ZhY3RvckVuYWJsZWQgfHwgKHRoaXMuc2VsZWN0ZWQuaWQgPyBmYWxzZSA6IHRydWUpXSxcbiAgICAgIHJvbGVOYW1lczogdGhpcy5mYi5hcnJheShcbiAgICAgICAgdGhpcy5yb2xlcy5tYXAocm9sZSA9PlxuICAgICAgICAgIHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgICAgICAgW3JvbGUubmFtZV06IFshIXNucSgoKSA9PiB0aGlzLnNlbGVjdGVkVXNlclJvbGVzLmZpbmQodXNlclJvbGUgPT4gdXNlclJvbGUuaWQgPT09IHJvbGUuaWQpKV0sXG4gICAgICAgICAgfSksXG4gICAgICAgICksXG4gICAgICApLFxuICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIHRoaXMuYnVpbGRGb3JtKCk7XG4gICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IHRydWU7XG4gIH1cblxuICBvbkFkZCgpIHtcbiAgICB0aGlzLnNlbGVjdGVkID0ge30gYXMgSWRlbnRpdHkuVXNlckl0ZW07XG4gICAgdGhpcy5zZWxlY3RlZFVzZXJSb2xlcyA9IFtdIGFzIElkZW50aXR5LlJvbGVJdGVtW107XG4gICAgdGhpcy5vcGVuTW9kYWwoKTtcbiAgfVxuXG4gIG9uRWRpdChpZDogc3RyaW5nKSB7XG4gICAgY29tYmluZUxhdGVzdChbdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgSWRlbnRpdHlHZXRVc2VyQnlJZChpZCkpLCB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBJZGVudGl0eUdldFVzZXJSb2xlcyhpZCkpXSlcbiAgICAgIC5waXBlKFxuICAgICAgICBmaWx0ZXIoKFtyZXMxLCByZXMyXSkgPT4gcmVzMSAmJiByZXMyKSxcbiAgICAgICAgbWFwKChbc3RhdGUsIF9dKSA9PiBzdGF0ZSksXG4gICAgICAgIHBsdWNrKCdJZGVudGl0eVN0YXRlJyksXG4gICAgICAgIHRha2UoMSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKChzdGF0ZTogSWRlbnRpdHkuU3RhdGUpID0+IHtcbiAgICAgICAgdGhpcy5zZWxlY3RlZCA9IHN0YXRlLnNlbGVjdGVkVXNlcjtcbiAgICAgICAgdGhpcy5zZWxlY3RlZFVzZXJSb2xlcyA9IHN0YXRlLnNlbGVjdGVkVXNlclJvbGVzO1xuICAgICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGlmICghdGhpcy5mb3JtLnZhbGlkKSByZXR1cm47XG5cbiAgICBjb25zdCB7IHJvbGVOYW1lcyB9ID0gdGhpcy5mb3JtLnZhbHVlO1xuICAgIGNvbnN0IG1hcHBlZFJvbGVOYW1lcyA9IHNucShcbiAgICAgICgpID0+IHJvbGVOYW1lcy5maWx0ZXIocm9sZSA9PiAhIXJvbGVbT2JqZWN0LmtleXMocm9sZSlbMF1dKS5tYXAocm9sZSA9PiBPYmplY3Qua2V5cyhyb2xlKVswXSksXG4gICAgICBbXSxcbiAgICApO1xuXG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKFxuICAgICAgICB0aGlzLnNlbGVjdGVkLmlkXG4gICAgICAgICAgPyBuZXcgSWRlbnRpdHlVcGRhdGVVc2VyKHtcbiAgICAgICAgICAgICAgLi4udGhpcy5mb3JtLnZhbHVlLFxuICAgICAgICAgICAgICBpZDogdGhpcy5zZWxlY3RlZC5pZCxcbiAgICAgICAgICAgICAgcm9sZU5hbWVzOiBtYXBwZWRSb2xlTmFtZXMsXG4gICAgICAgICAgICB9KVxuICAgICAgICAgIDogbmV3IElkZW50aXR5QWRkVXNlcih7XG4gICAgICAgICAgICAgIC4uLnRoaXMuZm9ybS52YWx1ZSxcbiAgICAgICAgICAgICAgcm9sZU5hbWVzOiBtYXBwZWRSb2xlTmFtZXMsXG4gICAgICAgICAgICB9KSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICB9KTtcbiAgfVxuXG4gIGRlbGV0ZShpZDogc3RyaW5nLCB1c2VyTmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybignQWJwSWRlbnRpdHk6OlVzZXJEZWxldGlvbkNvbmZpcm1hdGlvbk1lc3NhZ2UnLCAnQWJwSWRlbnRpdHk6OkFyZVlvdVN1cmUnLCB7XG4gICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFt1c2VyTmFtZV0sXG4gICAgICB9KVxuICAgICAgLnN1YnNjcmliZSgoc3RhdHVzOiBUb2FzdGVyLlN0YXR1cykgPT4ge1xuICAgICAgICBpZiAoc3RhdHVzID09PSBUb2FzdGVyLlN0YXR1cy5jb25maXJtKSB7XG4gICAgICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgSWRlbnRpdHlEZWxldGVVc2VyKGlkKSk7XG4gICAgICAgIH1cbiAgICAgIH0pO1xuICB9XG5cbiAgb25QYWdlQ2hhbmdlKGRhdGEpIHtcbiAgICB0aGlzLnBhZ2VRdWVyeS5za2lwQ291bnQgPSBkYXRhLmZpcnN0O1xuICAgIHRoaXMucGFnZVF1ZXJ5Lm1heFJlc3VsdENvdW50ID0gZGF0YS5yb3dzO1xuXG4gICAgdGhpcy5nZXQoKTtcbiAgfVxuXG4gIGdldCgpIHtcbiAgICB0aGlzLmxvYWRpbmcgPSB0cnVlO1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IElkZW50aXR5R2V0VXNlcnModGhpcy5wYWdlUXVlcnkpKS5zdWJzY3JpYmUoKCkgPT4gKHRoaXMubG9hZGluZyA9IGZhbHNlKSk7XG4gIH1cbn1cbiJdfQ==