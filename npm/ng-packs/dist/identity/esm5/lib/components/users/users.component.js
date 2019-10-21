/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfirmationService } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormControl } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, pluck, switchMap, take } from 'rxjs/operators';
import snq from 'snq';
import { CreateUser, DeleteUser, GetUserById, GetUserRoles, GetUsers, UpdateUser, GetRoles, } from '../../actions/identity.actions';
import { IdentityState } from '../../states/identity.state';
var UsersComponent = /** @class */ (function () {
    function UsersComponent(confirmationService, fb, store) {
        this.confirmationService = confirmationService;
        this.fb = fb;
        this.store = store;
        this.visiblePermissions = false;
        this.pageQuery = {};
        this.loading = false;
        this.modalBusy = false;
        this.sortOrder = '';
        this.sortKey = '';
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
        this.get();
    };
    /**
     * @param {?} value
     * @return {?}
     */
    UsersComponent.prototype.onSearch = /**
     * @param {?} value
     * @return {?}
     */
    function (value) {
        this.pageQuery.filter = value;
        this.get();
    };
    /**
     * @return {?}
     */
    UsersComponent.prototype.buildForm = /**
     * @return {?}
     */
    function () {
        var _this = this;
        this.store.dispatch(new GetRoles()).subscribe((/**
         * @return {?}
         */
        function () {
            _this.roles = _this.store.selectSnapshot(IdentityState.getRoles);
            _this.form = _this.fb.group({
                userName: [_this.selected.userName || '', [Validators.required, Validators.maxLength(256)]],
                email: [_this.selected.email || '', [Validators.required, Validators.email, Validators.maxLength(256)]],
                name: [_this.selected.name || '', [Validators.maxLength(64)]],
                surname: [_this.selected.surname || '', [Validators.maxLength(64)]],
                phoneNumber: [_this.selected.phoneNumber || '', [Validators.maxLength(16)]],
                lockoutEnabled: [_this.selected.twoFactorEnabled || (_this.selected.id ? false : true)],
                twoFactorEnabled: [_this.selected.twoFactorEnabled || (_this.selected.id ? false : true)],
                roleNames: _this.fb.array(_this.roles.map((/**
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
            if (!_this.selected.userName) {
                _this.form.addControl('password', new FormControl('', [Validators.required, Validators.maxLength(32)]));
            }
            else {
                _this.form.addControl('password', new FormControl('', [Validators.maxLength(32)]));
            }
        }));
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
        this.store
            .dispatch(new GetUserById(id))
            .pipe(switchMap((/**
         * @return {?}
         */
        function () { return _this.store.dispatch(new GetUserRoles(id)); })), pluck('IdentityState'), take(1))
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
        this.modalBusy = true;
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
            ? new UpdateUser(tslib_1.__assign({}, this.form.value, { id: this.selected.id, roleNames: mappedRoleNames }))
            : new CreateUser(tslib_1.__assign({}, this.form.value, { roleNames: mappedRoleNames })))
            .subscribe((/**
         * @return {?}
         */
        function () {
            _this.modalBusy = false;
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
                _this.store.dispatch(new DeleteUser(id));
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
        this.store
            .dispatch(new GetUsers(this.pageQuery))
            .pipe(finalize((/**
         * @return {?}
         */
        function () { return (_this.loading = false); })))
            .subscribe();
    };
    UsersComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-users',
                    template: "<div class=\"row entry-row\">\n  <div class=\"col-auto\">\n    <h1 class=\"content-header-title\">{{ 'AbpIdentity::Users' | abpLocalization }}</h1>\n  </div>\n  <div class=\"col\">\n    <div class=\"text-lg-right pt-2\" id=\"AbpContentToolbar\">\n      <button\n        [abpPermission]=\"'AbpIdentity.Users.Create'\"\n        id=\"create-role\"\n        class=\"btn btn-primary\"\n        type=\"button\"\n        (click)=\"onAdd()\"\n      >\n        <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewUser' | abpLocalization }}</span>\n      </button>\n    </div>\n  </div>\n</div>\n\n<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\n          (input.debounce)=\"onSearch($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      *ngIf=\"[150, 250, 250, 250] as columnWidths\"\n      [value]=\"data$ | async\"\n      [abpTableSort]=\"{ key: sortKey, order: sortOrder }\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      [resizableColumns]=\"true\"\n      [scrollable]=\"true\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"colgroup\">\n        <colgroup>\n          <col *ngFor=\"let width of columnWidths\" [ngStyle]=\"{ 'width.px': width || undefined }\" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate=\"emptymessage\" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]=\"columnWidths.length\"\n          localizationResource=\"AbpIdentity\"\n          localizationProp=\"NoDataAvailableInDatatable\"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th pResizableColumn (click)=\"sortOrderIcon.sort('userName')\">\n            {{ 'AbpIdentity::UserName' | abpLocalization }}\n            <abp-sort-order-icon #sortOrderIcon key=\"userName\" [(selectedKey)]=\"sortKey\" [(order)]=\"sortOrder\">\n            </abp-sort-order-icon>\n          </th>\n          <th pResizableColumn (click)=\"sortOrderIcon.sort('email')\">\n            {{ 'AbpIdentity::EmailAddress' | abpLocalization }}\n            <abp-sort-order-icon key=\"email\" [(selectedKey)]=\"sortKey\" [(order)]=\"sortOrder\"></abp-sort-order-icon>\n          </th>\n          <th pResizableColumn (click)=\"sortOrderIcon.sort('phoneNumber')\">\n            {{ 'AbpIdentity::PhoneNumber' | abpLocalization }}\n            <abp-sort-order-icon key=\"phoneNumber\" [(selectedKey)]=\"sortKey\" [(order)]=\"sortOrder\">\n            </abp-sort-order-icon>\n          </th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td class=\"text-center\">\n            <div ngbDropdown container=\"body\" class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button\n                  [abpPermission]=\"'AbpIdentity.Users.Update'\"\n                  ngbDropdownItem\n                  (click)=\"onEdit(data.id)\"\n                >\n                  {{ 'AbpIdentity::Edit' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpIdentity.Users.ManagePermissions'\"\n                  ngbDropdownItem\n                  (click)=\"providerKey = data.id; visiblePermissions = true\"\n                >\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]=\"'AbpIdentity.Users.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.userName)\"\n                >\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.userName }}</td>\n          <td>{{ data.email }}</td>\n          <td>{{ data.phoneNumber }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\" [busy]=\"modalBusy\">\n  <ng-template #abpHeader>\n    <h3>{{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewUser') | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-template #loaderRef\n      ><div class=\"text-center\"><i class=\"fa fa-pulse fa-spinner\"></i></div\n    ></ng-template>\n\n    <form *ngIf=\"form; else loaderRef\" [formGroup]=\"form\" (ngSubmit)=\"save()\">\n      <ngb-tabset>\n        <ngb-tab [title]=\"'AbpIdentity::UserInformations' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2 fade-in-top\">\n              <div class=\"form-group\">\n                <label for=\"user-name\">{{ 'AbpIdentity::UserName' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"user-name\" class=\"form-control\" formControlName=\"userName\" autofocus />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label>\n                <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label>\n                <input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"password\">{{ 'AbpIdentity::Password' | abpLocalization }}</label\n                ><span *ngIf=\"!selected.userName\"> * </span>\n                <input\n                  type=\"password\"\n                  id=\"password\"\n                  autocomplete=\"new-password\"\n                  class=\"form-control\"\n                  formControlName=\"password\"\n                />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"email\">{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"email\" class=\"form-control\" formControlName=\"email\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"phone-number\">{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</label>\n                <input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"lockout-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"lockoutEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"lockout-checkbox\">{{\n                  'AbpIdentity::DisplayName:LockoutEnabled' | abpLocalization\n                }}</label>\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"two-factor-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"twoFactorEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"two-factor-checkbox\">{{\n                  'AbpIdentity::DisplayName:TwoFactorEnabled' | abpLocalization\n                }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n        <ngb-tab [title]=\"'AbpIdentity::Roles' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2 fade-in-top\">\n              <div\n                *ngFor=\"let roleGroup of roleGroups; let i = index; trackBy: trackByFn\"\n                class=\"custom-checkbox custom-control mb-2\"\n              >\n                <input\n                  type=\"checkbox\"\n                  name=\"Roles[0].IsAssigned\"\n                  value=\"true\"\n                  class=\"custom-control-input\"\n                  [attr.id]=\"'roles-' + i\"\n                  [formControl]=\"roleGroup.controls[roles[i].name]\"\n                />\n                <label class=\"custom-control-label\" [attr.for]=\"'roles-' + i\">{{ roles[i].name }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n      </ngb-tabset>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\" [disabled]=\"form?.invalid\">{{\n      'AbpIdentity::Save' | abpLocalization\n    }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<abp-permission-management [(visible)]=\"visiblePermissions\" providerName=\"U\" [providerKey]=\"providerKey\">\n</abp-permission-management>\n"
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
    UsersComponent.prototype.modalBusy;
    /** @type {?} */
    UsersComponent.prototype.sortOrder;
    /** @type {?} */
    UsersComponent.prototype.sortKey;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidXNlcnMuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3VzZXJzL3VzZXJzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFtQixTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDM0YsT0FBTyxFQUE4QixXQUFXLEVBQWEsVUFBVSxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQzdHLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQUUsU0FBUyxFQUFFLElBQUksRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2xFLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUN0QixPQUFPLEVBQ0wsVUFBVSxFQUNWLFVBQVUsRUFDVixXQUFXLEVBQ1gsWUFBWSxFQUNaLFFBQVEsRUFDUixVQUFVLEVBQ1YsUUFBUSxHQUNULE1BQU0sZ0NBQWdDLENBQUM7QUFFeEMsT0FBTyxFQUFFLGFBQWEsRUFBRSxNQUFNLDZCQUE2QixDQUFDO0FBQzVEO0lBNENFLHdCQUFvQixtQkFBd0MsRUFBVSxFQUFlLEVBQVUsS0FBWTtRQUF2Rix3QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCO1FBQVUsT0FBRSxHQUFGLEVBQUUsQ0FBYTtRQUFVLFVBQUssR0FBTCxLQUFLLENBQU87UUF0QjNHLHVCQUFrQixHQUFHLEtBQUssQ0FBQztRQUkzQixjQUFTLEdBQXdCLEVBQUUsQ0FBQztRQUlwQyxZQUFPLEdBQUcsS0FBSyxDQUFDO1FBRWhCLGNBQVMsR0FBRyxL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b25maXJtYXRpb25TZXJ2aWNlLCBUb2FzdGVyIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBUZW1wbGF0ZVJlZiwgVHJhY2tCeUZ1bmN0aW9uLCBWaWV3Q2hpbGQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWJzdHJhY3RDb250cm9sLCBGb3JtQXJyYXksIEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMsIEZvcm1Db250cm9sIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGZpbmFsaXplLCBwbHVjaywgc3dpdGNoTWFwLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHtcbiAgQ3JlYXRlVXNlcixcbiAgRGVsZXRlVXNlcixcbiAgR2V0VXNlckJ5SWQsXG4gIEdldFVzZXJSb2xlcyxcbiAgR2V0VXNlcnMsXG4gIFVwZGF0ZVVzZXIsXG4gIEdldFJvbGVzLFxufSBmcm9tICcuLi8uLi9hY3Rpb25zL2lkZW50aXR5LmFjdGlvbnMnO1xuaW1wb3J0IHsgSWRlbnRpdHkgfSBmcm9tICcuLi8uLi9tb2RlbHMvaWRlbnRpdHknO1xuaW1wb3J0IHsgSWRlbnRpdHlTdGF0ZSB9IGZyb20gJy4uLy4uL3N0YXRlcy9pZGVudGl0eS5zdGF0ZSc7XG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtdXNlcnMnLFxuICB0ZW1wbGF0ZVVybDogJy4vdXNlcnMuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBVc2Vyc0NvbXBvbmVudCBpbXBsZW1lbnRzIE9uSW5pdCB7XG4gIEBTZWxlY3QoSWRlbnRpdHlTdGF0ZS5nZXRVc2VycylcbiAgZGF0YSQ6IE9ic2VydmFibGU8SWRlbnRpdHkuVXNlckl0ZW1bXT47XG5cbiAgQFNlbGVjdChJZGVudGl0eVN0YXRlLmdldFVzZXJzVG90YWxDb3VudClcbiAgdG90YWxDb3VudCQ6IE9ic2VydmFibGU8bnVtYmVyPjtcblxuICBAVmlld0NoaWxkKCdtb2RhbENvbnRlbnQnLCB7IHN0YXRpYzogZmFsc2UgfSlcbiAgbW9kYWxDb250ZW50OiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIGZvcm06IEZvcm1Hcm91cDtcblxuICBzZWxlY3RlZDogSWRlbnRpdHkuVXNlckl0ZW07XG5cbiAgc2VsZWN0ZWRVc2VyUm9sZXM6IElkZW50aXR5LlJvbGVJdGVtW107XG5cbiAgcm9sZXM6IElkZW50aXR5LlJvbGVJdGVtW107XG5cbiAgdmlzaWJsZVBlcm1pc3Npb25zID0gZmFsc2U7XG5cbiAgcHJvdmlkZXJLZXk6IHN0cmluZztcblxuICBwYWdlUXVlcnk6IEFCUC5QYWdlUXVlcnlQYXJhbXMgPSB7fTtcblxuICBpc01vZGFsVmlzaWJsZTogYm9vbGVhbjtcblxuICBsb2FkaW5nID0gZmFsc2U7XG5cbiAgbW9kYWxCdXN5ID0gZmFsc2U7XG5cbiAgc29ydE9yZGVyID0gJyc7XG5cbiAgc29ydEtleSA9ICcnO1xuXG4gIHRyYWNrQnlGbjogVHJhY2tCeUZ1bmN0aW9uPEFic3RyYWN0Q29udHJvbD4gPSAoaW5kZXgsIGl0ZW0pID0+IE9iamVjdC5rZXlzKGl0ZW0pWzBdIHx8IGluZGV4O1xuXG4gIGdldCByb2xlR3JvdXBzKCk6IEZvcm1Hcm91cFtdIHtcbiAgICByZXR1cm4gc25xKCgpID0+ICh0aGlzLmZvcm0uZ2V0KCdyb2xlTmFtZXMnKSBhcyBGb3JtQXJyYXkpLmNvbnRyb2xzIGFzIEZvcm1Hcm91cFtdLCBbXSk7XG4gIH1cblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UsIHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBuZ09uSW5pdCgpIHtcbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgb25TZWFyY2godmFsdWUpIHtcbiAgICB0aGlzLnBhZ2VRdWVyeS5maWx0ZXIgPSB2YWx1ZTtcbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgYnVpbGRGb3JtKCkge1xuICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IEdldFJvbGVzKCkpLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICB0aGlzLnJvbGVzID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChJZGVudGl0eVN0YXRlLmdldFJvbGVzKTtcbiAgICAgIHRoaXMuZm9ybSA9IHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgICB1c2VyTmFtZTogW3RoaXMuc2VsZWN0ZWQudXNlck5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLnJlcXVpcmVkLCBWYWxpZGF0b3JzLm1heExlbmd0aCgyNTYpXV0sXG4gICAgICAgIGVtYWlsOiBbdGhpcy5zZWxlY3RlZC5lbWFpbCB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMuZW1haWwsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICAgICAgbmFtZTogW3RoaXMuc2VsZWN0ZWQubmFtZSB8fCAnJywgW1ZhbGlkYXRvcnMubWF4TGVuZ3RoKDY0KV1dLFxuICAgICAgICBzdXJuYW1lOiBbdGhpcy5zZWxlY3RlZC5zdXJuYW1lIHx8ICcnLCBbVmFsaWRhdG9ycy5tYXhMZW5ndGgoNjQpXV0sXG4gICAgICAgIHBob25lTnVtYmVyOiBbdGhpcy5zZWxlY3RlZC5waG9uZU51bWJlciB8fCAnJywgW1ZhbGlkYXRvcnMubWF4TGVuZ3RoKDE2KV1dLFxuICAgICAgICBsb2Nrb3V0RW5hYmxlZDogW3RoaXMuc2VsZWN0ZWQudHdvRmFjdG9yRW5hYmxlZCB8fCAodGhpcy5zZWxlY3RlZC5pZCA/IGZhbHNlIDogdHJ1ZSldLFxuICAgICAgICB0d29GYWN0b3JFbmFibGVkOiBbdGhpcy5zZWxlY3RlZC50d29GYWN0b3JFbmFibGVkIHx8ICh0aGlzLnNlbGVjdGVkLmlkID8gZmFsc2UgOiB0cnVlKV0sXG4gICAgICAgIHJvbGVOYW1lczogdGhpcy5mYi5hcnJheShcbiAgICAgICAgICB0aGlzLnJvbGVzLm1hcChyb2xlID0+XG4gICAgICAgICAgICB0aGlzLmZiLmdyb3VwKHtcbiAgICAgICAgICAgICAgW3JvbGUubmFtZV06IFshIXNucSgoKSA9PiB0aGlzLnNlbGVjdGVkVXNlclJvbGVzLmZpbmQodXNlclJvbGUgPT4gdXNlclJvbGUuaWQgPT09IHJvbGUuaWQpKV0sXG4gICAgICAgICAgICB9KSxcbiAgICAgICAgICApLFxuICAgICAgICApLFxuICAgICAgfSk7XG5cbiAgICAgIGlmICghdGhpcy5zZWxlY3RlZC51c2VyTmFtZSkge1xuICAgICAgICB0aGlzLmZvcm0uYWRkQ29udHJvbCgncGFzc3dvcmQnLCBuZXcgRm9ybUNvbnRyb2woJycsIFtWYWxpZGF0b3JzLnJlcXVpcmVkLCBWYWxpZGF0b3JzLm1heExlbmd0aCgzMildKSk7XG4gICAgICB9IGVsc2Uge1xuICAgICAgICB0aGlzLmZvcm0uYWRkQ29udHJvbCgncGFzc3dvcmQnLCBuZXcgRm9ybUNvbnRyb2woJycsIFtWYWxpZGF0b3JzLm1heExlbmd0aCgzMildKSk7XG4gICAgICB9XG4gICAgfSk7XG4gIH1cblxuICBvcGVuTW9kYWwoKSB7XG4gICAgdGhpcy5idWlsZEZvcm0oKTtcbiAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gdHJ1ZTtcbiAgfVxuXG4gIG9uQWRkKCkge1xuICAgIHRoaXMuc2VsZWN0ZWQgPSB7fSBhcyBJZGVudGl0eS5Vc2VySXRlbTtcbiAgICB0aGlzLnNlbGVjdGVkVXNlclJvbGVzID0gW10gYXMgSWRlbnRpdHkuUm9sZUl0ZW1bXTtcbiAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICB9XG5cbiAgb25FZGl0KGlkOiBzdHJpbmcpIHtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFVzZXJCeUlkKGlkKSlcbiAgICAgIC5waXBlKFxuICAgICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgR2V0VXNlclJvbGVzKGlkKSkpLFxuICAgICAgICBwbHVjaygnSWRlbnRpdHlTdGF0ZScpLFxuICAgICAgICB0YWtlKDEpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoc3RhdGU6IElkZW50aXR5LlN0YXRlKSA9PiB7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzdGF0ZS5zZWxlY3RlZFVzZXI7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWRVc2VyUm9sZXMgPSBzdGF0ZS5zZWxlY3RlZFVzZXJSb2xlcztcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgc2F2ZSgpIHtcbiAgICBpZiAoIXRoaXMuZm9ybS52YWxpZCkgcmV0dXJuO1xuICAgIHRoaXMubW9kYWxCdXN5ID0gdHJ1ZTtcblxuICAgIGNvbnN0IHsgcm9sZU5hbWVzIH0gPSB0aGlzLmZvcm0udmFsdWU7XG4gICAgY29uc3QgbWFwcGVkUm9sZU5hbWVzID0gc25xKFxuICAgICAgKCkgPT4gcm9sZU5hbWVzLmZpbHRlcihyb2xlID0+ICEhcm9sZVtPYmplY3Qua2V5cyhyb2xlKVswXV0pLm1hcChyb2xlID0+IE9iamVjdC5rZXlzKHJvbGUpWzBdKSxcbiAgICAgIFtdLFxuICAgICk7XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIHRoaXMuc2VsZWN0ZWQuaWRcbiAgICAgICAgICA/IG5ldyBVcGRhdGVVc2VyKHtcbiAgICAgICAgICAgICAgLi4udGhpcy5mb3JtLnZhbHVlLFxuICAgICAgICAgICAgICBpZDogdGhpcy5zZWxlY3RlZC5pZCxcbiAgICAgICAgICAgICAgcm9sZU5hbWVzOiBtYXBwZWRSb2xlTmFtZXMsXG4gICAgICAgICAgICB9KVxuICAgICAgICAgIDogbmV3IENyZWF0ZVVzZXIoe1xuICAgICAgICAgICAgICAuLi50aGlzLmZvcm0udmFsdWUsXG4gICAgICAgICAgICAgIHJvbGVOYW1lczogbWFwcGVkUm9sZU5hbWVzLFxuICAgICAgICAgICAgfSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5tb2RhbEJ1c3kgPSBmYWxzZTtcbiAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgfSk7XG4gIH1cblxuICBkZWxldGUoaWQ6IHN0cmluZywgdXNlck5hbWU6IHN0cmluZykge1xuICAgIHRoaXMuY29uZmlybWF0aW9uU2VydmljZVxuICAgICAgLndhcm4oJ0FicElkZW50aXR5OjpVc2VyRGVsZXRpb25Db25maXJtYXRpb25NZXNzYWdlJywgJ0FicElkZW50aXR5OjpBcmVZb3VTdXJlJywge1xuICAgICAgICBtZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zOiBbdXNlck5hbWVdLFxuICAgICAgfSlcbiAgICAgIC5zdWJzY3JpYmUoKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpID0+IHtcbiAgICAgICAgaWYgKHN0YXR1cyA9PT0gVG9hc3Rlci5TdGF0dXMuY29uZmlybSkge1xuICAgICAgICAgIHRoaXMuc3RvcmUuZGlzcGF0Y2gobmV3IERlbGV0ZVVzZXIoaWQpKTtcbiAgICAgICAgfVxuICAgICAgfSk7XG4gIH1cblxuICBvblBhZ2VDaGFuZ2UoZGF0YSkge1xuICAgIHRoaXMucGFnZVF1ZXJ5LnNraXBDb3VudCA9IGRhdGEuZmlyc3Q7XG4gICAgdGhpcy5wYWdlUXVlcnkubWF4UmVzdWx0Q291bnQgPSBkYXRhLnJvd3M7XG5cbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgZ2V0KCkge1xuICAgIHRoaXMubG9hZGluZyA9IHRydWU7XG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKG5ldyBHZXRVc2Vycyh0aGlzLnBhZ2VRdWVyeSkpXG4gICAgICAucGlwZShmaW5hbGl6ZSgoKSA9PiAodGhpcy5sb2FkaW5nID0gZmFsc2UpKSlcbiAgICAgIC5zdWJzY3JpYmUoKTtcbiAgfVxufVxuIl19