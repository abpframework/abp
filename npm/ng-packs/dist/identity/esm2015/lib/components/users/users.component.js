/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/users/users.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { ConfigState } from '@abp/ng.core';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormControl, } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, pluck, switchMap, take } from 'rxjs/operators';
import snq from 'snq';
import { CreateUser, DeleteUser, GetUserById, GetUserRoles, GetUsers, UpdateUser, GetRoles, } from '../../actions/identity.actions';
import { IdentityState } from '../../states/identity.state';
import { validatePassword } from '@ngx-validate/core';
export class UsersComponent {
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
        this.pageQuery = {};
        this.loading = false;
        this.modalBusy = false;
        this.sortOrder = '';
        this.sortKey = '';
        this.passwordRulesArr = (/** @type {?} */ ([]));
        this.requiredPasswordLength = 1;
        this.trackByFn = (/**
         * @param {?} index
         * @param {?} item
         * @return {?}
         */
        (index, item) => Object.keys(item)[0] || index);
    }
    /**
     * @return {?}
     */
    get roleGroups() {
        return snq((/**
         * @return {?}
         */
        () => (/** @type {?} */ (((/** @type {?} */ (this.form.get('roleNames')))).controls))), []);
    }
    /**
     * @return {?}
     */
    ngOnInit() {
        this.get();
        /** @type {?} */
        const passwordRules = this.store.selectSnapshot(ConfigState.getSettings('Identity.Password'));
        if ((passwordRules['Abp.Identity.Password.RequireDigit'] || '').toLowerCase() === 'true') {
            this.passwordRulesArr.push('number');
        }
        if ((passwordRules['Abp.Identity.Password.RequireLowercase'] || '').toLowerCase() === 'true') {
            this.passwordRulesArr.push('small');
        }
        if ((passwordRules['Abp.Identity.Password.RequireUppercase'] || '').toLowerCase() === 'true') {
            this.passwordRulesArr.push('capital');
        }
        if (+(passwordRules['Abp.Identity.Password.RequiredUniqueChars'] || 0) > 0) {
            this.passwordRulesArr.push('special');
        }
        if (Number.isInteger(+passwordRules['Abp.Identity.Password.RequiredLength'])) {
            this.requiredPasswordLength = +passwordRules['Abp.Identity.Password.RequiredLength'];
        }
    }
    /**
     * @param {?} value
     * @return {?}
     */
    onSearch(value) {
        this.pageQuery.filter = value;
        this.get();
    }
    /**
     * @return {?}
     */
    buildForm() {
        this.store.dispatch(new GetRoles()).subscribe((/**
         * @return {?}
         */
        () => {
            this.roles = this.store.selectSnapshot(IdentityState.getRoles);
            this.form = this.fb.group({
                userName: [this.selected.userName || '', [Validators.required, Validators.maxLength(256)]],
                email: [
                    this.selected.email || '',
                    [Validators.required, Validators.email, Validators.maxLength(256)],
                ],
                name: [this.selected.name || '', [Validators.maxLength(64)]],
                surname: [this.selected.surname || '', [Validators.maxLength(64)]],
                phoneNumber: [this.selected.phoneNumber || '', [Validators.maxLength(16)]],
                lockoutEnabled: [this.selected.lockoutEnabled || (this.selected.id ? false : true)],
                twoFactorEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
                roleNames: this.fb.array(this.roles.map((/**
                 * @param {?} role
                 * @return {?}
                 */
                role => this.fb.group({
                    [role.name]: [
                        !!snq((/**
                         * @return {?}
                         */
                        () => this.selectedUserRoles.find((/**
                         * @param {?} userRole
                         * @return {?}
                         */
                        userRole => userRole.id === role.id)))),
                    ],
                })))),
            });
            /** @type {?} */
            const passwordValidators = [
                validatePassword(this.passwordRulesArr),
                Validators.minLength(this.requiredPasswordLength),
                Validators.maxLength(32),
            ];
            this.form.addControl('password', new FormControl('', [...passwordValidators]));
            if (!this.selected.userName) {
                this.form.get('password').setValidators([...passwordValidators, Validators.required]);
                this.form.get('password').updateValueAndValidity();
            }
        }));
    }
    /**
     * @return {?}
     */
    openModal() {
        this.buildForm();
        this.isModalVisible = true;
    }
    /**
     * @return {?}
     */
    add() {
        this.selected = (/** @type {?} */ ({}));
        this.selectedUserRoles = (/** @type {?} */ ([]));
        this.openModal();
    }
    /**
     * @param {?} id
     * @return {?}
     */
    edit(id) {
        this.store
            .dispatch(new GetUserById(id))
            .pipe(switchMap((/**
         * @return {?}
         */
        () => this.store.dispatch(new GetUserRoles(id)))), pluck('IdentityState'), take(1))
            .subscribe((/**
         * @param {?} state
         * @return {?}
         */
        (state) => {
            this.selected = state.selectedUser;
            this.selectedUserRoles = state.selectedUserRoles;
            this.openModal();
        }));
    }
    /**
     * @return {?}
     */
    save() {
        if (!this.form.valid || this.modalBusy)
            return;
        this.modalBusy = true;
        const { roleNames } = this.form.value;
        /** @type {?} */
        const mappedRoleNames = snq((/**
         * @return {?}
         */
        () => roleNames.filter((/**
         * @param {?} role
         * @return {?}
         */
        role => !!role[Object.keys(role)[0]])).map((/**
         * @param {?} role
         * @return {?}
         */
        role => Object.keys(role)[0]))), []);
        this.store
            .dispatch(this.selected.id
            ? new UpdateUser(Object.assign({}, this.selected, this.form.value, { id: this.selected.id, roleNames: mappedRoleNames }))
            : new CreateUser(Object.assign({}, this.form.value, { roleNames: mappedRoleNames })))
            .pipe(finalize((/**
         * @return {?}
         */
        () => (this.modalBusy = false))))
            .subscribe((/**
         * @return {?}
         */
        () => {
            this.isModalVisible = false;
            this.get();
        }));
    }
    /**
     * @param {?} id
     * @param {?} userName
     * @return {?}
     */
    delete(id, userName) {
        this.confirmationService
            .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
            messageLocalizationParams: [userName],
        })
            .subscribe((/**
         * @param {?} status
         * @return {?}
         */
        (status) => {
            if (status === "confirm" /* confirm */) {
                this.store.dispatch(new DeleteUser(id)).subscribe((/**
                 * @return {?}
                 */
                () => this.get()));
            }
        }));
    }
    /**
     * @param {?} data
     * @return {?}
     */
    onPageChange(data) {
        this.pageQuery.skipCount = data.first;
        this.pageQuery.maxResultCount = data.rows;
        this.get();
    }
    /**
     * @return {?}
     */
    get() {
        this.loading = true;
        this.store
            .dispatch(new GetUsers(this.pageQuery))
            .pipe(finalize((/**
         * @return {?}
         */
        () => (this.loading = false))))
            .subscribe();
    }
}
UsersComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-users',
                template: "<div id=\"identity-roles-wrapper\" class=\"card\">\n  <div class=\"card-header\">\n    <div class=\"row\">\n      <div class=\"col col-md-6\">\n        <h5 class=\"card-title\">{{ 'AbpIdentity::Users' | abpLocalization }}</h5>\n      </div>\n      <div class=\"text-right col col-md-6\">\n        <button\n          *abpPermission=\"'AbpIdentity.Users.Create'\"\n          id=\"create-role\"\n          class=\"btn btn-primary\"\n          type=\"button\"\n          (click)=\"add()\"\n        >\n          <i class=\"fa fa-plus mr-1\"></i> <span>{{ 'AbpIdentity::NewUser' | abpLocalization }}</span>\n        </button>\n      </div>\n    </div>\n  </div>\n  <div class=\"card-body\">\n    <div id=\"data-tables-table-filter\" class=\"data-tables-filter\">\n      <label\n        ><input\n          type=\"search\"\n          class=\"form-control form-control-sm\"\n          [placeholder]=\"'AbpUi::PagerSearch' | abpLocalization\"\n          (input.debounce)=\"onSearch($event.target.value)\"\n      /></label>\n    </div>\n    <p-table\n      *ngIf=\"[150, 250, 250, 250] as columnWidths\"\n      [value]=\"data$ | async\"\n      [abpTableSort]=\"{ key: sortKey, order: sortOrder }\"\n      [lazy]=\"true\"\n      [lazyLoadOnInit]=\"false\"\n      [paginator]=\"true\"\n      [rows]=\"10\"\n      [totalRecords]=\"totalCount$ | async\"\n      [loading]=\"loading\"\n      [resizableColumns]=\"true\"\n      [scrollable]=\"true\"\n      (onLazyLoad)=\"onPageChange($event)\"\n    >\n      <ng-template pTemplate=\"colgroup\">\n        <colgroup>\n          <col *ngFor=\"let width of columnWidths\" [ngStyle]=\"{ 'width.px': width || undefined }\" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate=\"emptymessage\" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]=\"columnWidths.length\"\n          localizationResource=\"AbpIdentity\"\n          localizationProp=\"NoDataAvailableInDatatable\"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate=\"header\">\n        <tr>\n          <th>{{ 'AbpIdentity::Actions' | abpLocalization }}</th>\n          <th pResizableColumn (click)=\"sortOrderIcon.sort('userName')\">\n            {{ 'AbpIdentity::UserName' | abpLocalization }}\n            <abp-sort-order-icon #sortOrderIcon key=\"userName\" [(selectedKey)]=\"sortKey\" [(order)]=\"sortOrder\">\n            </abp-sort-order-icon>\n          </th>\n          <th pResizableColumn (click)=\"sortOrderIcon.sort('email')\">\n            {{ 'AbpIdentity::EmailAddress' | abpLocalization }}\n            <abp-sort-order-icon key=\"email\" [(selectedKey)]=\"sortKey\" [(order)]=\"sortOrder\"></abp-sort-order-icon>\n          </th>\n          <th pResizableColumn (click)=\"sortOrderIcon.sort('phoneNumber')\">\n            {{ 'AbpIdentity::PhoneNumber' | abpLocalization }}\n            <abp-sort-order-icon key=\"phoneNumber\" [(selectedKey)]=\"sortKey\" [(order)]=\"sortOrder\">\n            </abp-sort-order-icon>\n          </th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate=\"body\" let-data>\n        <tr>\n          <td class=\"text-center\">\n            <div ngbDropdown container=\"body\" class=\"d-inline-block\">\n              <button\n                class=\"btn btn-primary btn-sm dropdown-toggle\"\n                data-toggle=\"dropdown\"\n                aria-haspopup=\"true\"\n                ngbDropdownToggle\n              >\n                <i class=\"fa fa-cog mr-1\"></i>{{ 'AbpIdentity::Actions' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button *abpPermission=\"'AbpIdentity.Users.Update'\" ngbDropdownItem (click)=\"edit(data.id)\">\n                  {{ 'AbpIdentity::Edit' | abpLocalization }}\n                </button>\n                <button\n                  *abpPermission=\"'AbpIdentity.Users.ManagePermissions'\"\n                  ngbDropdownItem\n                  (click)=\"providerKey = data.id; visiblePermissions = true\"\n                >\n                  {{ 'AbpIdentity::Permissions' | abpLocalization }}\n                </button>\n                <button\n                  *abpPermission=\"'AbpIdentity.Users.Delete'\"\n                  ngbDropdownItem\n                  (click)=\"delete(data.id, data.userName)\"\n                >\n                  {{ 'AbpIdentity::Delete' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.userName }}</td>\n          <td>{{ data.email }}</td>\n          <td>{{ data.phoneNumber }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal [(visible)]=\"isModalVisible\" [busy]=\"modalBusy\" (disappear)=\"form = null\">\n  <ng-template #abpHeader>\n    <h3>{{ (selected?.id ? 'AbpIdentity::Edit' : 'AbpIdentity::NewUser') | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <ng-template #loaderRef\n      ><div class=\"text-center\"><i class=\"fa fa-pulse fa-spinner\"></i></div\n    ></ng-template>\n\n    <form *ngIf=\"form; else loaderRef\" [formGroup]=\"form\" (ngSubmit)=\"save()\">\n      <ngb-tabset>\n        <ngb-tab [title]=\"'AbpIdentity::UserInformations' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2 fade-in-top\">\n              <div class=\"form-group\">\n                <label for=\"user-name\">{{ 'AbpIdentity::UserName' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"user-name\" class=\"form-control\" formControlName=\"userName\" autofocus />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"name\">{{ 'AbpIdentity::DisplayName:Name' | abpLocalization }}</label>\n                <input type=\"text\" id=\"name\" class=\"form-control\" formControlName=\"name\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"surname\">{{ 'AbpIdentity::DisplayName:Surname' | abpLocalization }}</label>\n                <input type=\"text\" id=\"surname\" class=\"form-control\" formControlName=\"surname\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"password\">{{ 'AbpIdentity::Password' | abpLocalization }}</label\n                ><span *ngIf=\"!selected.userName\"> * </span>\n                <input\n                  type=\"password\"\n                  id=\"password\"\n                  autocomplete=\"new-password\"\n                  class=\"form-control\"\n                  formControlName=\"password\"\n                />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"email\">{{ 'AbpIdentity::EmailAddress' | abpLocalization }}</label\n                ><span> * </span>\n                <input type=\"text\" id=\"email\" class=\"form-control\" formControlName=\"email\" />\n              </div>\n\n              <div class=\"form-group\">\n                <label for=\"phone-number\">{{ 'AbpIdentity::PhoneNumber' | abpLocalization }}</label>\n                <input type=\"text\" id=\"phone-number\" class=\"form-control\" formControlName=\"phoneNumber\" />\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"lockout-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"lockoutEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"lockout-checkbox\">{{\n                  'AbpIdentity::DisplayName:LockoutEnabled' | abpLocalization\n                }}</label>\n              </div>\n\n              <div class=\"custom-checkbox custom-control mb-2\">\n                <input\n                  type=\"checkbox\"\n                  id=\"two-factor-checkbox\"\n                  class=\"custom-control-input\"\n                  formControlName=\"twoFactorEnabled\"\n                />\n                <label class=\"custom-control-label\" for=\"two-factor-checkbox\">{{\n                  'AbpIdentity::DisplayName:TwoFactorEnabled' | abpLocalization\n                }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n        <ngb-tab [title]=\"'AbpIdentity::Roles' | abpLocalization\">\n          <ng-template ngbTabContent>\n            <div class=\"mt-2 fade-in-top\">\n              <div\n                *ngFor=\"let roleGroup of roleGroups; let i = index; trackBy: trackByFn\"\n                class=\"custom-checkbox custom-control mb-2\"\n              >\n                <input\n                  type=\"checkbox\"\n                  name=\"Roles[0].IsAssigned\"\n                  value=\"true\"\n                  class=\"custom-control-input\"\n                  [attr.id]=\"'roles-' + i\"\n                  [formControl]=\"roleGroup.controls[roles[i].name]\"\n                />\n                <label class=\"custom-control-label\" [attr.for]=\"'roles-' + i\">{{ roles[i].name }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n      </ngb-tabset>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button type=\"button\" class=\"btn btn-secondary\" #abpClose>\n      {{ 'AbpIdentity::Cancel' | abpLocalization }}\n    </button>\n    <abp-button iconClass=\"fa fa-check\" (click)=\"save()\" [disabled]=\"form?.invalid\">{{\n      'AbpIdentity::Save' | abpLocalization\n    }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<abp-permission-management [(visible)]=\"visiblePermissions\" providerName=\"U\" [providerKey]=\"providerKey\">\n</abp-permission-management>\n"
            }] }
];
/** @nocollapse */
UsersComponent.ctorParameters = () => [
    { type: ConfirmationService },
    { type: FormBuilder },
    { type: Store }
];
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
    UsersComponent.prototype.passwordRulesArr;
    /** @type {?} */
    UsersComponent.prototype.requiredPasswordLength;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidXNlcnMuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3VzZXJzL3VzZXJzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7Ozs7QUFBQSxPQUFPLEVBQU8sV0FBVyxFQUFFLE1BQU0sY0FBYyxDQUFDO0FBQ2hELE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFtQixTQUFTLEVBQVUsTUFBTSxlQUFlLENBQUM7QUFDM0YsT0FBTyxFQUdMLFdBQVcsRUFFWCxVQUFVLEVBQ1YsV0FBVyxHQUNaLE1BQU0sZ0JBQWdCLENBQUM7QUFDeEIsT0FBTyxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUUsTUFBTSxhQUFhLENBQUM7QUFDNUMsT0FBTyxFQUFFLFVBQVUsRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNsQyxPQUFPLEVBQUUsUUFBUSxFQUFFLEtBQUssRUFBRSxTQUFTLEVBQUUsSUFBSSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDbEUsT0FBTyxHQUFHLE1BQU0sS0FBSyxDQUFDO0FBQ3RCLE9BQU8sRUFDTCxVQUFVLEVBQ1YsVUFBVSxFQUNWLFdBQVcsRUFDWCxZQUFZLEVBQ1osUUFBUSxFQUNSLFVBQVUsRUFDVixRQUFRLEdBQ1QsTUFBTSxnQ0FBZ0MsQ0FBQztBQUV4QyxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0sNkJBQTZCLENBQUM7QUFDNUQsT0FBTyxFQUFpQixnQkFBZ0IsRUFBRSxNQUFNLG9CQUFvQixDQUFDO0FBS3JFLE1BQU0sT0FBTyxjQUFjOzs7Ozs7SUE0Q3pCLFlBQ1UsbUJBQXdDLEVBQ3hDLEVBQWUsRUFDZixLQUFZO1FBRlosd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUN4QyxPQUFFLEdBQUYsRUFBRSxDQUFhO1FBQ2YsVUFBSyxHQUFMLEtBQUssQ0FBTztRQTdCdEIsdUJBQWtCLEdBQUcsS0FBSyxDQUFDO1FBSTNCLGNBQVMsR0FBd0IsRUFBRSxDQUFDO1FBSXBDLFlBQU8sR0FBRyxLQUFLLENBQUM7UUFFaEIsY0FBUyxHQUFHLEtBQUssQ0FBQztRQUVsQixjQUFTLEdBQUcsRUFBRSxDQUFDO1FBRWYsWUFBTyxHQUFHLEVBQUUsQ0FBQztRQUViLHFCQUFnQixHQUFHLG1CQUFBLEVBQUUsRUFBaUIsQ0FBQztRQUV2QywyQkFBc0IsR0FBRyxDQUFDLENBQUM7UUFFM0IsY0FBUzs7Ozs7UUFBcUMsQ0FBQyxLQUFLLEVBQUUsSUFBSSxFQUFFLEVBQUUsQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEtBQUssRUFBQztJQVUxRixDQUFDOzs7O0lBUkosSUFBSSxVQUFVO1FBQ1osT0FBTyxHQUFHOzs7UUFBQyxHQUFHLEVBQUUsQ0FBQyxtQkFBQSxDQUFDLG1CQUFBLElBQUksQ0FBQyxJQUFJLENBQUMsR0FBRyxDQUFDLFdBQVcsQ0FBQyxFQUFhLENBQUMsQ0FBQyxRQUFRLEVBQWUsR0FBRSxFQUFFLENBQUMsQ0FBQztJQUMxRixDQUFDOzs7O0lBUUQsUUFBUTtRQUNOLElBQUksQ0FBQyxHQUFHLEVBQUUsQ0FBQzs7Y0FFTCxhQUFhLEdBQTJCLElBQUksQ0FBQyxLQUFLLENBQUMsY0FBYyxDQUNyRSxXQUFXLENBQUMsV0FBVyxDQUFDLG1CQUFtQixDQUFDLENBQzdDO1FBRUQsSUFBSSxDQUFDLGFBQWEsQ0FBQyxvQ0FBb0MsQ0FBQyxJQUFJLEVBQUUsQ0FBQyxDQUFDLFdBQVcsRUFBRSxLQUFLLE1BQU0sRUFBRTtZQUN4RixJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxDQUFDO1NBQ3RDO1FBRUQsSUFBSSxDQUFDLGFBQWEsQ0FBQyx3Q0FBd0MsQ0FBQyxJQUFJLEVBQUUsQ0FBQyxDQUFDLFdBQVcsRUFBRSxLQUFLLE1BQU0sRUFBRTtZQUM1RixJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLE9BQU8sQ0FBQyxDQUFDO1NBQ3JDO1FBRUQsSUFBSSxDQUFDLGFBQWEsQ0FBQyx3Q0FBd0MsQ0FBQyxJQUFJLEVBQUUsQ0FBQyxDQUFDLFdBQVcsRUFBRSxLQUFLLE1BQU0sRUFBRTtZQUM1RixJQUFJLENBQUMsZ0JBQWdCLENBQUMsSUFBSSxDQUFDLFNBQVMsQ0FBQyxDQUFDO1NBQ3ZDO1FBRUQsSUFBSSxDQUFDLENBQUMsYUFBYSxDQUFDLDJDQUEyQyxDQUFDLElBQUksQ0FBQyxDQUFDLEdBQUcsQ0FBQyxFQUFFO1lBQzFFLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUM7U0FDdkM7UUFFRCxJQUFJLE1BQU0sQ0FBQyxTQUFTLENBQUMsQ0FBQyxhQUFhLENBQUMsc0NBQXNDLENBQUMsQ0FBQyxFQUFFO1lBQzVFLElBQUksQ0FBQyxzQkFBc0IsR0FBRyxDQUFDLGFBQWEsQ0FBQyxzQ0FBc0MsQ0FBQyxDQUFDO1NBQ3RGO0lBQ0gsQ0FBQzs7Ozs7SUFFRCxRQUFRLENBQUMsS0FBSztRQUNaLElBQUksQ0FBQyxTQUFTLENBQUMsTUFBTSxHQUFHLEtBQUssQ0FBQztRQUM5QixJQUFJLENBQUMsR0FBRyxFQUFFLENBQUM7SUFDYixDQUFDOzs7O0lBRUQsU0FBUztRQUNQLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksUUFBUSxFQUFFLENBQUMsQ0FBQyxTQUFTOzs7UUFBQyxHQUFHLEVBQUU7WUFDakQsSUFBSSxDQUFDLEtBQUssR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDLGNBQWMsQ0FBQyxhQUFhLENBQUMsUUFBUSxDQUFDLENBQUM7WUFDL0QsSUFBSSxDQUFDLElBQUksR0FBRyxJQUFJLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQztnQkFDeEIsUUFBUSxFQUFFLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxRQUFRLElBQUksRUFBRSxFQUFFLENBQUMsVUFBVSxDQUFDLFFBQVEsRUFBRSxVQUFVLENBQUMsU0FBUyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7Z0JBQzFGLEtBQUssRUFBRTtvQkFDTCxJQUFJLENBQUMsUUFBUSxDQUFDLEtBQUssSUFBSSxFQUFFO29CQUN6QixDQUFDLFVBQVUsQ0FBQyxRQUFRLEVBQUUsVUFBVSxDQUFDLEtBQUssRUFBRSxVQUFVLENBQUMsU0FBUyxDQUFDLEdBQUcsQ0FBQyxDQUFDO2lCQUNuRTtnQkFDRCxJQUFJLEVBQUUsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksSUFBSSxFQUFFLEVBQUUsQ0FBQyxVQUFVLENBQUMsU0FBUyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUM7Z0JBQzVELE9BQU8sRUFBRSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsT0FBTyxJQUFJLEVBQUUsRUFBRSxDQUFDLFVBQVUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQztnQkFDbEUsV0FBVyxFQUFFLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxXQUFXLElBQUksRUFBRSxFQUFFLENBQUMsVUFBVSxDQUFDLFNBQVMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDO2dCQUMxRSxjQUFjLEVBQUUsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLGNBQWMsSUFBSSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxLQUFLLENBQUMsQ0FBQyxDQUFDLElBQUksQ0FBQyxDQUFDO2dCQUNuRixnQkFBZ0IsRUFBRSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsZ0JBQWdCLElBQUksQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsS0FBSyxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsQ0FBQztnQkFDdkYsU0FBUyxFQUFFLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUN0QixJQUFJLENBQUMsS0FBSyxDQUFDLEdBQUc7Ozs7Z0JBQUMsSUFBSSxDQUFDLEVBQUUsQ0FDcEIsSUFBSSxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUM7b0JBQ1osQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLEVBQUU7d0JBQ1gsQ0FBQyxDQUFDLEdBQUc7Ozt3QkFBQyxHQUFHLEVBQUUsQ0FBQyxJQUFJLENBQUMsaUJBQWlCLENBQUMsSUFBSTs7Ozt3QkFBQyxRQUFRLENBQUMsRUFBRSxDQUFDLFFBQVEsQ0FBQyxFQUFFLEtBQUssSUFBSSxDQUFDLEVBQUUsRUFBQyxFQUFDO3FCQUM5RTtpQkFDRixDQUFDLEVBQ0gsQ0FDRjthQUNGLENBQUMsQ0FBQzs7a0JBRUcsa0JBQWtCLEdBQUc7Z0JBQ3pCLGdCQUFnQixDQUFDLElBQUksQ0FBQyxnQkFBZ0IsQ0FBQztnQkFDdkMsVUFBVSxDQUFDLFNBQVMsQ0FBQyxJQUFJLENBQUMsc0JBQXNCLENBQUM7Z0JBQ2pELFVBQVUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDO2FBQ3pCO1lBRUQsSUFBSSxDQUFDLElBQUksQ0FBQyxVQUFVLENBQUMsVUFBVSxFQUFFLElBQUksV0FBVyxDQUFDLEVBQUUsRUFBRSxDQUFDLEdBQUcsa0JBQWtCLENBQUMsQ0FBQyxDQUFDLENBQUM7WUFFL0UsSUFBSSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsUUFBUSxFQUFFO2dCQUMzQixJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsQ0FBQyxhQUFhLENBQUMsQ0FBQyxHQUFHLGtCQUFrQixFQUFFLFVBQVUsQ0FBQyxRQUFRLENBQUMsQ0FBQyxDQUFDO2dCQUN0RixJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsQ0FBQyxzQkFBc0IsRUFBRSxDQUFDO2FBQ3BEO1FBQ0gsQ0FBQyxFQUFDLENBQUM7SUFDTCxDQUFDOzs7O0lBRUQsU0FBUztRQUNQLElBQUksQ0FBQyxTQUFTLEVBQUUsQ0FBQztRQUNqQixJQUFJLENBQUMsY0FBYyxHQUFHLElBQUksQ0FBQztJQUM3QixDQUFDOzs7O0lBRUQsR0FBRztRQUNELElBQUksQ0FBQyxRQUFRLEdBQUcsbUJBQUEsRUFBRSxFQUFxQixDQUFDO1FBQ3hDLElBQUksQ0FBQyxpQkFBaUIsR0FBRyxtQkFBQSxFQUFFLEVBQXVCLENBQUM7UUFDbkQsSUFBSSxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQ25CLENBQUM7Ozs7O0lBRUQsSUFBSSxDQUFDLEVBQVU7UUFDYixJQUFJLENBQUMsS0FBSzthQUNQLFFBQVEsQ0FBQyxJQUFJLFdBQVcsQ0FBQyxFQUFFLENBQUMsQ0FBQzthQUM3QixJQUFJLENBQ0gsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxZQUFZLENBQUMsRUFBRSxDQUFDLENBQUMsRUFBQyxFQUMxRCxLQUFLLENBQUMsZUFBZSxDQUFDLEVBQ3RCLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FDUjthQUNBLFNBQVM7Ozs7UUFBQyxDQUFDLEtBQXFCLEVBQUUsRUFBRTtZQUNuQyxJQUFJLENBQUMsUUFBUSxHQUFHLEtBQUssQ0FBQyxZQUFZLENBQUM7WUFDbkMsSUFBSSxDQUFDLGlCQUFpQixHQUFHLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQztZQUNqRCxJQUFJLENBQUMsU0FBUyxFQUFFLENBQUM7UUFDbkIsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsSUFBSTtRQUNGLElBQUksQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLEtBQUssSUFBSSxJQUFJLENBQUMsU0FBUztZQUFFLE9BQU87UUFDL0MsSUFBSSxDQUFDLFNBQVMsR0FBRyxJQUFJLENBQUM7Y0FFaEIsRUFBRSxTQUFTLEVBQUUsR0FBRyxJQUFJLENBQUMsSUFBSSxDQUFDLEtBQUs7O2NBQy9CLGVBQWUsR0FBRyxHQUFHOzs7UUFDekIsR0FBRyxFQUFFLENBQ0gsU0FBUyxDQUFDLE1BQU07Ozs7UUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxJQUFJLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxFQUFDLENBQUMsR0FBRzs7OztRQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsTUFBTSxDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxHQUMxRixFQUFFLENBQ0g7UUFFRCxJQUFJLENBQUMsS0FBSzthQUNQLFFBQVEsQ0FDUCxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQUU7WUFDZCxDQUFDLENBQUMsSUFBSSxVQUFVLG1CQUNULElBQUksQ0FBQyxRQUFRLEVBQ2IsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLElBQ2xCLEVBQUUsRUFBRSxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQUUsRUFDcEIsU0FBUyxFQUFFLGVBQWUsSUFDMUI7WUFDSixDQUFDLENBQUMsSUFBSSxVQUFVLG1CQUNULElBQUksQ0FBQyxJQUFJLENBQUMsS0FBSyxJQUNsQixTQUFTLEVBQUUsZUFBZSxJQUMxQixDQUNQO2FBQ0EsSUFBSSxDQUFDLFFBQVE7OztRQUFDLEdBQUcsRUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVMsR0FBRyxLQUFLLENBQUMsRUFBQyxDQUFDO2FBQzlDLFNBQVM7OztRQUFDLEdBQUcsRUFBRTtZQUNkLElBQUksQ0FBQyxjQUFjLEdBQUcsS0FBSyxDQUFDO1lBQzVCLElBQUksQ0FBQyxHQUFHLEVBQUUsQ0FBQztRQUNiLENBQUMsRUFBQyxDQUFDO0lBQ1AsQ0FBQzs7Ozs7O0lBRUQsTUFBTSxDQUFDLEVBQVUsRUFBRSxRQUFnQjtRQUNqQyxJQUFJLENBQUMsbUJBQW1CO2FBQ3JCLElBQUksQ0FBQyw4Q0FBOEMsRUFBRSx5QkFBeUIsRUFBRTtZQUMvRSx5QkFBeUIsRUFBRSxDQUFDLFFBQVEsQ0FBQztTQUN0QyxDQUFDO2FBQ0QsU0FBUzs7OztRQUFDLENBQUMsTUFBc0IsRUFBRSxFQUFFO1lBQ3BDLElBQUksTUFBTSw0QkFBMkIsRUFBRTtnQkFDckMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxVQUFVLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxTQUFTOzs7Z0JBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEdBQUcsRUFBRSxFQUFDLENBQUM7YUFDckU7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7O0lBRUQsWUFBWSxDQUFDLElBQUk7UUFDZixJQUFJLENBQUMsU0FBUyxDQUFDLFNBQVMsR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDO1FBQ3RDLElBQUksQ0FBQyxTQUFTLENBQUMsY0FBYyxHQUFHLElBQUksQ0FBQyxJQUFJLENBQUM7UUFFMUMsSUFBSSxDQUFDLEdBQUcsRUFBRSxDQUFDO0lBQ2IsQ0FBQzs7OztJQUVELEdBQUc7UUFDRCxJQUFJLENBQUMsT0FBTyxHQUFHLElBQUksQ0FBQztRQUNwQixJQUFJLENBQUMsS0FBSzthQUNQLFFBQVEsQ0FBQyxJQUFJLFFBQVEsQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUM7YUFDdEMsSUFBSSxDQUFDLFFBQVE7OztRQUFDLEdBQUcsRUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUMsRUFBQyxDQUFDO2FBQzVDLFNBQVMsRUFBRSxDQUFDO0lBQ2pCLENBQUM7OztZQWxORixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLFdBQVc7Z0JBQ3JCLHltVEFBcUM7YUFDdEM7Ozs7WUE3QlEsbUJBQW1CO1lBSzFCLFdBQVc7WUFLSSxLQUFLOzs7MkJBMkJuQixTQUFTLFNBQUMsY0FBYyxFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRTs7QUFMNUM7SUFEQyxNQUFNLENBQUMsYUFBYSxDQUFDLFFBQVEsQ0FBQztzQ0FDeEIsVUFBVTs2Q0FBc0I7QUFHdkM7SUFEQyxNQUFNLENBQUMsYUFBYSxDQUFDLGtCQUFrQixDQUFDO3NDQUM1QixVQUFVO21EQUFTOzs7SUFKaEMsK0JBQ3VDOztJQUV2QyxxQ0FDZ0M7O0lBRWhDLHNDQUMrQjs7SUFFL0IsOEJBQWdCOztJQUVoQixrQ0FBNEI7O0lBRTVCLDJDQUF1Qzs7SUFFdkMsK0JBQTJCOztJQUUzQiw0Q0FBMkI7O0lBRTNCLHFDQUFvQjs7SUFFcEIsbUNBQW9DOztJQUVwQyx3Q0FBd0I7O0lBRXhCLGlDQUFnQjs7SUFFaEIsbUNBQWtCOztJQUVsQixtQ0FBZTs7SUFFZixpQ0FBYTs7SUFFYiwwQ0FBdUM7O0lBRXZDLGdEQUEyQjs7SUFFM0IsbUNBQTZGOzs7OztJQU8zRiw2Q0FBZ0Q7Ozs7O0lBQ2hELDRCQUF1Qjs7Ozs7SUFDdkIsK0JBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQLCBDb25maWdTdGF0ZSB9IGZyb20gJ0BhYnAvbmcuY29yZSc7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlLCBUb2FzdGVyIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBUZW1wbGF0ZVJlZiwgVHJhY2tCeUZ1bmN0aW9uLCBWaWV3Q2hpbGQsIE9uSW5pdCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHtcbiAgQWJzdHJhY3RDb250cm9sLFxuICBGb3JtQXJyYXksXG4gIEZvcm1CdWlsZGVyLFxuICBGb3JtR3JvdXAsXG4gIFZhbGlkYXRvcnMsXG4gIEZvcm1Db250cm9sLFxufSBmcm9tICdAYW5ndWxhci9mb3Jtcyc7XG5pbXBvcnQgeyBTZWxlY3QsIFN0b3JlIH0gZnJvbSAnQG5neHMvc3RvcmUnO1xuaW1wb3J0IHsgT2JzZXJ2YWJsZSB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZmluYWxpemUsIHBsdWNrLCBzd2l0Y2hNYXAsIHRha2UgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgc25xIGZyb20gJ3NucSc7XG5pbXBvcnQge1xuICBDcmVhdGVVc2VyLFxuICBEZWxldGVVc2VyLFxuICBHZXRVc2VyQnlJZCxcbiAgR2V0VXNlclJvbGVzLFxuICBHZXRVc2VycyxcbiAgVXBkYXRlVXNlcixcbiAgR2V0Um9sZXMsXG59IGZyb20gJy4uLy4uL2FjdGlvbnMvaWRlbnRpdHkuYWN0aW9ucyc7XG5pbXBvcnQgeyBJZGVudGl0eSB9IGZyb20gJy4uLy4uL21vZGVscy9pZGVudGl0eSc7XG5pbXBvcnQgeyBJZGVudGl0eVN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzL2lkZW50aXR5LnN0YXRlJztcbmltcG9ydCB7IFBhc3N3b3JkUnVsZXMsIHZhbGlkYXRlUGFzc3dvcmQgfSBmcm9tICdAbmd4LXZhbGlkYXRlL2NvcmUnO1xuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXVzZXJzJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3VzZXJzLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgVXNlcnNDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBAU2VsZWN0KElkZW50aXR5U3RhdGUuZ2V0VXNlcnMpXG4gIGRhdGEkOiBPYnNlcnZhYmxlPElkZW50aXR5LlVzZXJJdGVtW10+O1xuXG4gIEBTZWxlY3QoSWRlbnRpdHlTdGF0ZS5nZXRVc2Vyc1RvdGFsQ291bnQpXG4gIHRvdGFsQ291bnQkOiBPYnNlcnZhYmxlPG51bWJlcj47XG5cbiAgQFZpZXdDaGlsZCgnbW9kYWxDb250ZW50JywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIG1vZGFsQ29udGVudDogVGVtcGxhdGVSZWY8YW55PjtcblxuICBmb3JtOiBGb3JtR3JvdXA7XG5cbiAgc2VsZWN0ZWQ6IElkZW50aXR5LlVzZXJJdGVtO1xuXG4gIHNlbGVjdGVkVXNlclJvbGVzOiBJZGVudGl0eS5Sb2xlSXRlbVtdO1xuXG4gIHJvbGVzOiBJZGVudGl0eS5Sb2xlSXRlbVtdO1xuXG4gIHZpc2libGVQZXJtaXNzaW9ucyA9IGZhbHNlO1xuXG4gIHByb3ZpZGVyS2V5OiBzdHJpbmc7XG5cbiAgcGFnZVF1ZXJ5OiBBQlAuUGFnZVF1ZXJ5UGFyYW1zID0ge307XG5cbiAgaXNNb2RhbFZpc2libGU6IGJvb2xlYW47XG5cbiAgbG9hZGluZyA9IGZhbHNlO1xuXG4gIG1vZGFsQnVzeSA9IGZhbHNlO1xuXG4gIHNvcnRPcmRlciA9ICcnO1xuXG4gIHNvcnRLZXkgPSAnJztcblxuICBwYXNzd29yZFJ1bGVzQXJyID0gW10gYXMgUGFzc3dvcmRSdWxlcztcblxuICByZXF1aXJlZFBhc3N3b3JkTGVuZ3RoID0gMTtcblxuICB0cmFja0J5Rm46IFRyYWNrQnlGdW5jdGlvbjxBYnN0cmFjdENvbnRyb2w+ID0gKGluZGV4LCBpdGVtKSA9PiBPYmplY3Qua2V5cyhpdGVtKVswXSB8fCBpbmRleDtcblxuICBnZXQgcm9sZUdyb3VwcygpOiBGb3JtR3JvdXBbXSB7XG4gICAgcmV0dXJuIHNucSgoKSA9PiAodGhpcy5mb3JtLmdldCgncm9sZU5hbWVzJykgYXMgRm9ybUFycmF5KS5jb250cm9scyBhcyBGb3JtR3JvdXBbXSwgW10pO1xuICB9XG5cbiAgY29uc3RydWN0b3IoXG4gICAgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlLFxuICAgIHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLFxuICAgIHByaXZhdGUgc3RvcmU6IFN0b3JlLFxuICApIHt9XG5cbiAgbmdPbkluaXQoKSB7XG4gICAgdGhpcy5nZXQoKTtcblxuICAgIGNvbnN0IHBhc3N3b3JkUnVsZXM6IEFCUC5EaWN0aW9uYXJ5PHN0cmluZz4gPSB0aGlzLnN0b3JlLnNlbGVjdFNuYXBzaG90KFxuICAgICAgQ29uZmlnU3RhdGUuZ2V0U2V0dGluZ3MoJ0lkZW50aXR5LlBhc3N3b3JkJyksXG4gICAgKTtcblxuICAgIGlmICgocGFzc3dvcmRSdWxlc1snQWJwLklkZW50aXR5LlBhc3N3b3JkLlJlcXVpcmVEaWdpdCddIHx8ICcnKS50b0xvd2VyQ2FzZSgpID09PSAndHJ1ZScpIHtcbiAgICAgIHRoaXMucGFzc3dvcmRSdWxlc0Fyci5wdXNoKCdudW1iZXInKTtcbiAgICB9XG5cbiAgICBpZiAoKHBhc3N3b3JkUnVsZXNbJ0FicC5JZGVudGl0eS5QYXNzd29yZC5SZXF1aXJlTG93ZXJjYXNlJ10gfHwgJycpLnRvTG93ZXJDYXNlKCkgPT09ICd0cnVlJykge1xuICAgICAgdGhpcy5wYXNzd29yZFJ1bGVzQXJyLnB1c2goJ3NtYWxsJyk7XG4gICAgfVxuXG4gICAgaWYgKChwYXNzd29yZFJ1bGVzWydBYnAuSWRlbnRpdHkuUGFzc3dvcmQuUmVxdWlyZVVwcGVyY2FzZSddIHx8ICcnKS50b0xvd2VyQ2FzZSgpID09PSAndHJ1ZScpIHtcbiAgICAgIHRoaXMucGFzc3dvcmRSdWxlc0Fyci5wdXNoKCdjYXBpdGFsJyk7XG4gICAgfVxuXG4gICAgaWYgKCsocGFzc3dvcmRSdWxlc1snQWJwLklkZW50aXR5LlBhc3N3b3JkLlJlcXVpcmVkVW5pcXVlQ2hhcnMnXSB8fCAwKSA+IDApIHtcbiAgICAgIHRoaXMucGFzc3dvcmRSdWxlc0Fyci5wdXNoKCdzcGVjaWFsJyk7XG4gICAgfVxuXG4gICAgaWYgKE51bWJlci5pc0ludGVnZXIoK3Bhc3N3b3JkUnVsZXNbJ0FicC5JZGVudGl0eS5QYXNzd29yZC5SZXF1aXJlZExlbmd0aCddKSkge1xuICAgICAgdGhpcy5yZXF1aXJlZFBhc3N3b3JkTGVuZ3RoID0gK3Bhc3N3b3JkUnVsZXNbJ0FicC5JZGVudGl0eS5QYXNzd29yZC5SZXF1aXJlZExlbmd0aCddO1xuICAgIH1cbiAgfVxuXG4gIG9uU2VhcmNoKHZhbHVlKSB7XG4gICAgdGhpcy5wYWdlUXVlcnkuZmlsdGVyID0gdmFsdWU7XG4gICAgdGhpcy5nZXQoKTtcbiAgfVxuXG4gIGJ1aWxkRm9ybSgpIHtcbiAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBHZXRSb2xlcygpKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgdGhpcy5yb2xlcyA9IHRoaXMuc3RvcmUuc2VsZWN0U25hcHNob3QoSWRlbnRpdHlTdGF0ZS5nZXRSb2xlcyk7XG4gICAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgICAgdXNlck5hbWU6IFt0aGlzLnNlbGVjdGVkLnVzZXJOYW1lIHx8ICcnLCBbVmFsaWRhdG9ycy5yZXF1aXJlZCwgVmFsaWRhdG9ycy5tYXhMZW5ndGgoMjU2KV1dLFxuICAgICAgICBlbWFpbDogW1xuICAgICAgICAgIHRoaXMuc2VsZWN0ZWQuZW1haWwgfHwgJycsXG4gICAgICAgICAgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMuZW1haWwsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildLFxuICAgICAgICBdLFxuICAgICAgICBuYW1lOiBbdGhpcy5zZWxlY3RlZC5uYW1lIHx8ICcnLCBbVmFsaWRhdG9ycy5tYXhMZW5ndGgoNjQpXV0sXG4gICAgICAgIHN1cm5hbWU6IFt0aGlzLnNlbGVjdGVkLnN1cm5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLm1heExlbmd0aCg2NCldXSxcbiAgICAgICAgcGhvbmVOdW1iZXI6IFt0aGlzLnNlbGVjdGVkLnBob25lTnVtYmVyIHx8ICcnLCBbVmFsaWRhdG9ycy5tYXhMZW5ndGgoMTYpXV0sXG4gICAgICAgIGxvY2tvdXRFbmFibGVkOiBbdGhpcy5zZWxlY3RlZC5sb2Nrb3V0RW5hYmxlZCB8fCAodGhpcy5zZWxlY3RlZC5pZCA/IGZhbHNlIDogdHJ1ZSldLFxuICAgICAgICB0d29GYWN0b3JFbmFibGVkOiBbdGhpcy5zZWxlY3RlZC50d29GYWN0b3JFbmFibGVkIHx8ICh0aGlzLnNlbGVjdGVkLmlkID8gZmFsc2UgOiB0cnVlKV0sXG4gICAgICAgIHJvbGVOYW1lczogdGhpcy5mYi5hcnJheShcbiAgICAgICAgICB0aGlzLnJvbGVzLm1hcChyb2xlID0+XG4gICAgICAgICAgICB0aGlzLmZiLmdyb3VwKHtcbiAgICAgICAgICAgICAgW3JvbGUubmFtZV06IFtcbiAgICAgICAgICAgICAgICAhIXNucSgoKSA9PiB0aGlzLnNlbGVjdGVkVXNlclJvbGVzLmZpbmQodXNlclJvbGUgPT4gdXNlclJvbGUuaWQgPT09IHJvbGUuaWQpKSxcbiAgICAgICAgICAgICAgXSxcbiAgICAgICAgICAgIH0pLFxuICAgICAgICAgICksXG4gICAgICAgICksXG4gICAgICB9KTtcblxuICAgICAgY29uc3QgcGFzc3dvcmRWYWxpZGF0b3JzID0gW1xuICAgICAgICB2YWxpZGF0ZVBhc3N3b3JkKHRoaXMucGFzc3dvcmRSdWxlc0FyciksXG4gICAgICAgIFZhbGlkYXRvcnMubWluTGVuZ3RoKHRoaXMucmVxdWlyZWRQYXNzd29yZExlbmd0aCksXG4gICAgICAgIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDMyKSxcbiAgICAgIF07XG5cbiAgICAgIHRoaXMuZm9ybS5hZGRDb250cm9sKCdwYXNzd29yZCcsIG5ldyBGb3JtQ29udHJvbCgnJywgWy4uLnBhc3N3b3JkVmFsaWRhdG9yc10pKTtcblxuICAgICAgaWYgKCF0aGlzLnNlbGVjdGVkLnVzZXJOYW1lKSB7XG4gICAgICAgIHRoaXMuZm9ybS5nZXQoJ3Bhc3N3b3JkJykuc2V0VmFsaWRhdG9ycyhbLi4ucGFzc3dvcmRWYWxpZGF0b3JzLCBWYWxpZGF0b3JzLnJlcXVpcmVkXSk7XG4gICAgICAgIHRoaXMuZm9ybS5nZXQoJ3Bhc3N3b3JkJykudXBkYXRlVmFsdWVBbmRWYWxpZGl0eSgpO1xuICAgICAgfVxuICAgIH0pO1xuICB9XG5cbiAgb3Blbk1vZGFsKCkge1xuICAgIHRoaXMuYnVpbGRGb3JtKCk7XG4gICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IHRydWU7XG4gIH1cblxuICBhZGQoKSB7XG4gICAgdGhpcy5zZWxlY3RlZCA9IHt9IGFzIElkZW50aXR5LlVzZXJJdGVtO1xuICAgIHRoaXMuc2VsZWN0ZWRVc2VyUm9sZXMgPSBbXSBhcyBJZGVudGl0eS5Sb2xlSXRlbVtdO1xuICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gIH1cblxuICBlZGl0KGlkOiBzdHJpbmcpIHtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFVzZXJCeUlkKGlkKSlcbiAgICAgIC5waXBlKFxuICAgICAgICBzd2l0Y2hNYXAoKCkgPT4gdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgR2V0VXNlclJvbGVzKGlkKSkpLFxuICAgICAgICBwbHVjaygnSWRlbnRpdHlTdGF0ZScpLFxuICAgICAgICB0YWtlKDEpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoc3RhdGU6IElkZW50aXR5LlN0YXRlKSA9PiB7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWQgPSBzdGF0ZS5zZWxlY3RlZFVzZXI7XG4gICAgICAgIHRoaXMuc2VsZWN0ZWRVc2VyUm9sZXMgPSBzdGF0ZS5zZWxlY3RlZFVzZXJSb2xlcztcbiAgICAgICAgdGhpcy5vcGVuTW9kYWwoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgc2F2ZSgpIHtcbiAgICBpZiAoIXRoaXMuZm9ybS52YWxpZCB8fCB0aGlzLm1vZGFsQnVzeSkgcmV0dXJuO1xuICAgIHRoaXMubW9kYWxCdXN5ID0gdHJ1ZTtcblxuICAgIGNvbnN0IHsgcm9sZU5hbWVzIH0gPSB0aGlzLmZvcm0udmFsdWU7XG4gICAgY29uc3QgbWFwcGVkUm9sZU5hbWVzID0gc25xKFxuICAgICAgKCkgPT5cbiAgICAgICAgcm9sZU5hbWVzLmZpbHRlcihyb2xlID0+ICEhcm9sZVtPYmplY3Qua2V5cyhyb2xlKVswXV0pLm1hcChyb2xlID0+IE9iamVjdC5rZXlzKHJvbGUpWzBdKSxcbiAgICAgIFtdLFxuICAgICk7XG5cbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2goXG4gICAgICAgIHRoaXMuc2VsZWN0ZWQuaWRcbiAgICAgICAgICA/IG5ldyBVcGRhdGVVc2VyKHtcbiAgICAgICAgICAgICAgLi4udGhpcy5zZWxlY3RlZCxcbiAgICAgICAgICAgICAgLi4udGhpcy5mb3JtLnZhbHVlLFxuICAgICAgICAgICAgICBpZDogdGhpcy5zZWxlY3RlZC5pZCxcbiAgICAgICAgICAgICAgcm9sZU5hbWVzOiBtYXBwZWRSb2xlTmFtZXMsXG4gICAgICAgICAgICB9KVxuICAgICAgICAgIDogbmV3IENyZWF0ZVVzZXIoe1xuICAgICAgICAgICAgICAuLi50aGlzLmZvcm0udmFsdWUsXG4gICAgICAgICAgICAgIHJvbGVOYW1lczogbWFwcGVkUm9sZU5hbWVzLFxuICAgICAgICAgICAgfSksXG4gICAgICApXG4gICAgICAucGlwZShmaW5hbGl6ZSgoKSA9PiAodGhpcy5tb2RhbEJ1c3kgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSBmYWxzZTtcbiAgICAgICAgdGhpcy5nZXQoKTtcbiAgICAgIH0pO1xuICB9XG5cbiAgZGVsZXRlKGlkOiBzdHJpbmcsIHVzZXJOYW1lOiBzdHJpbmcpIHtcbiAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2VcbiAgICAgIC53YXJuKCdBYnBJZGVudGl0eTo6VXNlckRlbGV0aW9uQ29uZmlybWF0aW9uTWVzc2FnZScsICdBYnBJZGVudGl0eTo6QXJlWW91U3VyZScsIHtcbiAgICAgICAgbWVzc2FnZUxvY2FsaXphdGlvblBhcmFtczogW3VzZXJOYW1lXSxcbiAgICAgIH0pXG4gICAgICAuc3Vic2NyaWJlKChzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSA9PiB7XG4gICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBEZWxldGVVc2VyKGlkKSkuc3Vic2NyaWJlKCgpID0+IHRoaXMuZ2V0KCkpO1xuICAgICAgICB9XG4gICAgICB9KTtcbiAgfVxuXG4gIG9uUGFnZUNoYW5nZShkYXRhKSB7XG4gICAgdGhpcy5wYWdlUXVlcnkuc2tpcENvdW50ID0gZGF0YS5maXJzdDtcbiAgICB0aGlzLnBhZ2VRdWVyeS5tYXhSZXN1bHRDb3VudCA9IGRhdGEucm93cztcblxuICAgIHRoaXMuZ2V0KCk7XG4gIH1cblxuICBnZXQoKSB7XG4gICAgdGhpcy5sb2FkaW5nID0gdHJ1ZTtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFVzZXJzKHRoaXMucGFnZVF1ZXJ5KSlcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLmxvYWRpbmcgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgpO1xuICB9XG59XG4iXX0=