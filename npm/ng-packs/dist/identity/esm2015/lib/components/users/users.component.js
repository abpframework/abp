/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import { ConfirmationService } from '@abp/ng.theme.shared';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, Validators, FormControl } from '@angular/forms';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, pluck, switchMap, take } from 'rxjs/operators';
import snq from 'snq';
import {
  CreateUser,
  DeleteUser,
  GetUserById,
  GetUserRoles,
  GetUsers,
  UpdateUser,
} from '../../actions/identity.actions';
import { IdentityState } from '../../states/identity.state';
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
    this.trackByFn
    /**
     * @param {?} index
     * @param {?} item
     * @return {?}
     */ = (index, item) => Object.keys(item)[0] || index;
  }
  /**
   * @return {?}
   */
  get roleGroups() {
    return snq(
      /**
       * @return {?}
       */
      () => /** @type {?} */ (/** @type {?} */ (this.form.get('roleNames')).controls),
      [],
    );
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
    this.roles = this.store.selectSnapshot(IdentityState.getRoles);
    this.form = this.fb.group({
      userName: [this.selected.userName || '', [Validators.required, Validators.maxLength(256)]],
      email: [this.selected.email || '', [Validators.required, Validators.email, Validators.maxLength(256)]],
      name: [this.selected.name || '', [Validators.maxLength(64)]],
      surname: [this.selected.surname || '', [Validators.maxLength(64)]],
      phoneNumber: [this.selected.phoneNumber || '', [Validators.maxLength(16)]],
      lockoutEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
      twoFactorEnabled: [this.selected.twoFactorEnabled || (this.selected.id ? false : true)],
      roleNames: this.fb.array(
        this.roles.map(
          /**
           * @param {?} role
           * @return {?}
           */
          role =>
            this.fb.group({
              [role.name]: [
                !!snq(
                  /**
                   * @return {?}
                   */
                  () =>
                    this.selectedUserRoles.find(
                      /**
                       * @param {?} userRole
                       * @return {?}
                       */
                      userRole => userRole.id === role.id,
                    ),
                ),
              ],
            }),
        ),
      ),
    });
    if (!this.selected.userName) {
      this.form.addControl('password', new FormControl('', [Validators.required, Validators.maxLength(32)]));
    }
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
  onAdd() {
    this.selected = /** @type {?} */ ({});
    this.selectedUserRoles = /** @type {?} */ ([]);
    this.openModal();
  }
  /**
   * @param {?} id
   * @return {?}
   */
  onEdit(id) {
    this.store
      .dispatch(new GetUserById(id))
      .pipe(
        switchMap(
          /**
           * @return {?}
           */
          () => this.store.dispatch(new GetUserRoles(id)),
        ),
        pluck('IdentityState'),
        take(1),
      )
      .subscribe(
        /**
         * @param {?} state
         * @return {?}
         */
        state => {
          this.selected = state.selectedUser;
          this.selectedUserRoles = state.selectedUserRoles;
          this.openModal();
        },
      );
  }
  /**
   * @return {?}
   */
  save() {
    if (!this.form.valid) return;
    this.modalBusy = true;
    const { roleNames } = this.form.value;
    /** @type {?} */
    const mappedRoleNames = snq(
      /**
       * @return {?}
       */
      (() =>
        roleNames
          .filter(
            /**
             * @param {?} role
             * @return {?}
             */
            role => !!role[Object.keys(role)[0]],
          )
          .map(
            /**
             * @param {?} role
             * @return {?}
             */
            role => Object.keys(role)[0],
          )),
      [],
    );
    this.store
      .dispatch(
        this.selected.id
          ? new UpdateUser(Object.assign({}, this.form.value, { id: this.selected.id, roleNames: mappedRoleNames }))
          : new CreateUser(Object.assign({}, this.form.value, { roleNames: mappedRoleNames })),
      )
      .subscribe(
        /**
         * @return {?}
         */
        () => {
          this.modalBusy = false;
          this.isModalVisible = false;
        },
      );
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
      .subscribe(
        /**
         * @param {?} status
         * @return {?}
         */
        status => {
          if (status === 'confirm' /* confirm */) {
            this.store.dispatch(new DeleteUser(id));
          }
        },
      );
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
      .pipe(
        finalize(
          /**
           * @return {?}
           */
          () => (this.loading = false),
        ),
      )
      .subscribe();
  }
}
UsersComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-users',
        template:
          '<div class="row entry-row">\n  <div class="col-auto">\n    <h1 class="content-header-title">{{ \'AbpIdentity::Users\' | abpLocalization }}</h1>\n  </div>\n  <div class="col">\n    <div class="text-lg-right pt-2" id="AbpContentToolbar">\n      <button\n        [abpPermission]="\'AbpIdentity.Users.Create\'"\n        id="create-role"\n        class="btn btn-primary"\n        type="button"\n        (click)="onAdd()"\n      >\n        <i class="fa fa-plus mr-1"></i> <span>{{ \'AbpIdentity::NewUser\' | abpLocalization }}</span>\n      </button>\n    </div>\n  </div>\n</div>\n\n<div id="identity-roles-wrapper" class="card">\n  <div class="card-body">\n    <div id="data-tables-table-filter" class="data-tables-filter">\n      <label\n        ><input\n          type="search"\n          class="form-control form-control-sm"\n          [placeholder]="\'AbpUi::PagerSearch\' | abpLocalization"\n          (input.debounce)="onSearch($event.target.value)"\n      /></label>\n    </div>\n    <p-table\n      *ngIf="[130, 200, 200, 200] as columnWidths"\n      [value]="data$ | async"\n      [abpTableSort]="{ key: sortKey, order: sortOrder }"\n      [lazy]="true"\n      [lazyLoadOnInit]="false"\n      [paginator]="true"\n      [rows]="10"\n      [totalRecords]="totalCount$ | async"\n      [loading]="loading"\n      [resizableColumns]="true"\n      [scrollable]="true"\n      (onLazyLoad)="onPageChange($event)"\n    >\n      <ng-template pTemplate="colgroup">\n        <colgroup>\n          <col *ngFor="let width of columnWidths" [ngStyle]="{ \'width.px\': width }" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate="emptymessage" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]="columnWidths.length"\n          localizationResource="AbpIdentityServer"\n          localizationProp="NoDataAvailableInDatatable"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate="header">\n        <tr>\n          <th>{{ \'AbpIdentity::Actions\' | abpLocalization }}</th>\n          <th pResizableColumn (click)="sortOrderIcon.sort(\'userName\')">\n            {{ \'AbpIdentity::UserName\' | abpLocalization }}\n            <abp-sort-order-icon #sortOrderIcon key="userName" [(selectedKey)]="sortKey" [(order)]="sortOrder">\n            </abp-sort-order-icon>\n          </th>\n          <th pResizableColumn (click)="sortOrderIcon.sort(\'email\')">\n            {{ \'AbpIdentity::EmailAddress\' | abpLocalization }}\n            <abp-sort-order-icon key="email" [(selectedKey)]="sortKey" [(order)]="sortOrder"></abp-sort-order-icon>\n          </th>\n          <th pResizableColumn (click)="sortOrderIcon.sort(\'phoneNumber\')">\n            {{ \'AbpIdentity::PhoneNumber\' | abpLocalization }}\n            <abp-sort-order-icon key="phoneNumber" [(selectedKey)]="sortKey" [(order)]="sortOrder">\n            </abp-sort-order-icon>\n          </th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate="body" let-data>\n        <tr>\n          <td>\n            <div ngbDropdown container="body" class="d-inline-block">\n              <button\n                class="btn btn-primary btn-sm dropdown-toggle"\n                data-toggle="dropdown"\n                aria-haspopup="true"\n                ngbDropdownToggle\n              >\n                <i class="fa fa-cog mr-1"></i>{{ \'AbpIdentity::Actions\' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button ngbDropdownItem (click)="onEdit(data.id)">{{ \'AbpIdentity::Edit\' | abpLocalization }}</button>\n                <button ngbDropdownItem (click)="providerKey = data.id; visiblePermissions = true">\n                  {{ \'AbpIdentity::Permissions\' | abpLocalization }}\n                </button>\n                <button ngbDropdownItem (click)="delete(data.id, data.userName)">\n                  {{ \'AbpIdentity::Delete\' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.userName }}</td>\n          <td>{{ data.email }}</td>\n          <td>{{ data.phoneNumber }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal [(visible)]="isModalVisible" [busy]="modalBusy">\n  <ng-template #abpHeader>\n    <h3>{{ (selected?.id ? \'AbpIdentity::Edit\' : \'AbpIdentity::NewUser\') | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <form [formGroup]="form" (ngSubmit)="save()">\n      <ngb-tabset>\n        <ngb-tab [title]="\'AbpIdentity::UserInformations\' | abpLocalization">\n          <ng-template ngbTabContent>\n            <div class="mt-2 fade-in-top">\n              <div class="form-group">\n                <label for="user-name">{{ \'AbpIdentity::UserName\' | abpLocalization }}</label\n                ><span> * </span>\n                <input type="text" id="user-name" class="form-control" formControlName="userName" autofocus />\n              </div>\n\n              <div class="form-group">\n                <label for="name">{{ \'AbpIdentity::DisplayName:Name\' | abpLocalization }}</label>\n                <input type="text" id="name" class="form-control" formControlName="name" />\n              </div>\n\n              <div class="form-group">\n                <label for="surname">{{ \'AbpIdentity::DisplayName:Surname\' | abpLocalization }}</label>\n                <input type="text" id="surname" class="form-control" formControlName="surname" />\n              </div>\n\n              <div *ngIf="!selected.userName" class="form-group">\n                <label for="password">{{ \'AbpIdentity::Password\' | abpLocalization }}</label\n                ><span> * </span>\n                <input\n                  type="password"\n                  id="password"\n                  autocomplete="new-password"\n                  class="form-control"\n                  formControlName="password"\n                />\n              </div>\n\n              <div class="form-group">\n                <label for="email">{{ \'AbpIdentity::EmailAddress\' | abpLocalization }}</label\n                ><span> * </span>\n                <input type="text" id="email" class="form-control" formControlName="email" />\n              </div>\n\n              <div class="form-group">\n                <label for="phone-number">{{ \'AbpIdentity::PhoneNumber\' | abpLocalization }}</label>\n                <input type="text" id="phone-number" class="form-control" formControlName="phoneNumber" />\n              </div>\n\n              <div class="custom-checkbox custom-control mb-2">\n                <input\n                  type="checkbox"\n                  id="lockout-checkbox"\n                  class="custom-control-input"\n                  formControlName="lockoutEnabled"\n                />\n                <label class="custom-control-label" for="lockout-checkbox">{{\n                  \'AbpIdentity::DisplayName:LockoutEnabled\' | abpLocalization\n                }}</label>\n              </div>\n\n              <div class="custom-checkbox custom-control mb-2">\n                <input\n                  type="checkbox"\n                  id="two-factor-checkbox"\n                  class="custom-control-input"\n                  formControlName="twoFactorEnabled"\n                />\n                <label class="custom-control-label" for="two-factor-checkbox">{{\n                  \'AbpIdentity::DisplayName:TwoFactorEnabled\' | abpLocalization\n                }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n        <ngb-tab [title]="\'AbpIdentity::Roles\' | abpLocalization">\n          <ng-template ngbTabContent>\n            <div class="mt-2 fade-in-top">\n              <div\n                *ngFor="let roleGroup of roleGroups; let i = index; trackBy: trackByFn"\n                class="custom-checkbox custom-control mb-2"\n              >\n                <input\n                  type="checkbox"\n                  name="Roles[0].IsAssigned"\n                  value="true"\n                  class="custom-control-input"\n                  [attr.id]="\'roles-\' + i"\n                  [formControl]="roleGroup.controls[roles[i].name]"\n                />\n                <label class="custom-control-label" [attr.for]="\'roles-\' + i">{{ roles[i].name }}</label>\n              </div>\n            </div>\n          </ng-template>\n        </ngb-tab>\n      </ngb-tabset>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button type="button" class="btn btn-secondary" #abpClose>\n      {{ \'AbpIdentity::Cancel\' | abpLocalization }}\n    </button>\n    <abp-button iconClass="fa fa-check" (click)="save()" [disabled]="form.invalid">{{\n      \'AbpIdentity::Save\' | abpLocalization\n    }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<abp-permission-management [(visible)]="visiblePermissions" providerName="User" [providerKey]="providerKey">\n</abp-permission-management>\n',
      },
    ],
  },
];
/** @nocollapse */
UsersComponent.ctorParameters = () => [{ type: ConfirmationService }, { type: FormBuilder }, { type: Store }];
UsersComponent.propDecorators = {
  modalContent: [{ type: ViewChild, args: ['modalContent', { static: false }] }],
};
tslib_1.__decorate(
  [Select(IdentityState.getUsers), tslib_1.__metadata('design:type', Observable)],
  UsersComponent.prototype,
  'data$',
  void 0,
);
tslib_1.__decorate(
  [Select(IdentityState.getUsersTotalCount), tslib_1.__metadata('design:type', Observable)],
  UsersComponent.prototype,
  'totalCount$',
  void 0,
);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidXNlcnMuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3VzZXJzL3VzZXJzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFtQixTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkYsT0FBTyxFQUE4QixXQUFXLEVBQWEsVUFBVSxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQzdHLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQUUsU0FBUyxFQUFFLElBQUksRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2xFLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUN0QixPQUFPLEVBQ0wsVUFBVSxFQUNWLFVBQVUsRUFDVixXQUFXLEVBQ1gsWUFBWSxFQUNaLFFBQVEsRUFDUixVQUFVLEdBQ1gsTUFBTSxnQ0FBZ0MsQ0FBQztBQUV4QyxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0sNkJBQTZCLENBQUM7QUFLNUQsTUFBTSxPQUFPLGNBQWM7Ozs7OztJQXdDekIsWUFBb0IsbUJBQXdDLEVBQVUsRUFBZSxFQUFVLEtBQVk7UUFBdkYsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUFVLE9BQUUsR0FBRixFQUFFLENBQWE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBdEIzRyx1QkFBa0IsR0FBRyxLQUFLLENBQUM7UUFJM0IsY0FBUyxHQUF3QixFQUFFLENBQUM7UUFJcEMsWUFBTyxHQUFHLEtBQUssQ0FBQztRQUVoQixjQUFTLEdBQUcsS0FBSyxDQUFDO1FBRWxCLGNBQVMsR0FBRyxFQUFFLENBQUM7UUFFZixZQUFPLEdBQUcsRUFBRSxDQUFDO1FBRWIsY0FBUzs7Ozs7UUFBcUMsQ0FBQyxLQUFLLEVBQUUsSUFBSSxFQUFFLEVBQUUsQ0FBQyxNQUFNLENBQUMsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQyxJQUFJLEtBQUssRUFBQztJQU1pQixDQUFDOzs7O0lBSi9HLElBQUksVUFBVTtRQUNaLE9BQU8sR0FBRzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsbUJBQUEsQ0FBQyxtQkFBQSxJQUFJLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxXQUFXLENBQUMsRUFBYSxDQUFDLENBQUMsUUFBUSxFQUFlLEdBQUUsRUFBRSxDQUFDLENBQUM7SUFDMUYsQ0FBQzs7Ozs7SUFJRCxRQUFRLENBQUMsS0FBSztRQUNaLElBQUksQ0FBQyxTQUFTLENBQUMsTUFBTSxHQUFHLEtBQUssQ0FBQztRQUM5QixJQUFJLENBQUMsR0FBRyxFQUFFLENBQUM7SUFDYixDQUFDOzs7O0lBRUQsU0FBUztRQUNQLElBQUksQ0FBQyxLQUFLLEdBQUcsSUFBSSxDQUFDLEtBQUssQ0FBQyxjQUFjLENBQUMsYUFBYSxDQUFDLFFBQVEsQ0FBQyxDQUFDO1FBQy9ELElBQUksQ0FBQyxJQUFJLEdBQUcsSUFBSSxDQUFDLEVBQUUsQ0FBQyxLQUFLLENBQUM7WUFDeEIsUUFBUSxFQUFFLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxRQUFRLElBQUksRUFBRSxFQUFFLENBQUMsVUFBVSxDQUFDLFFBQVEsRUFBRSxVQUFVLENBQUMsU0FBUyxDQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7WUFDMUYsS0FBSyxFQUFFLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxLQUFLLElBQUksRUFBRSxFQUFFLENBQUMsVUFBVSxDQUFDLFFBQVEsRUFBRSxVQUFVLENBQUMsS0FBSyxFQUFFLFVBQVUsQ0FBQyxTQUFTLENBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztZQUN0RyxJQUFJLEVBQUUsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksSUFBSSxFQUFFLEVBQUUsQ0FBQyxVQUFVLENBQUMsU0FBUyxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUM7WUFDNUQsT0FBTyxFQUFFLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxPQUFPLElBQUksRUFBRSxFQUFFLENBQUMsVUFBVSxDQUFDLFNBQVMsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDO1lBQ2xFLFdBQVcsRUFBRSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxJQUFJLEVBQUUsRUFBRSxDQUFDLFVBQVUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQztZQUMxRSxjQUFjLEVBQUUsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLGdCQUFnQixJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUM7WUFDckYsZ0JBQWdCLEVBQUUsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLGdCQUFnQixJQUFJLENBQUMsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUFFLENBQUMsQ0FBQyxDQUFDLEtBQUssQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLENBQUM7WUFDdkYsU0FBUyxFQUFFLElBQUksQ0FBQyxFQUFFLENBQUMsS0FBSyxDQUN0QixJQUFJLENBQUMsS0FBSyxDQUFDLEdBQUc7Ozs7WUFBQyxJQUFJLENBQUMsRUFBRSxDQUNwQixJQUFJLENBQUMsRUFBRSxDQUFDLEtBQUssQ0FBQztnQkFDWixDQUFDLElBQUksQ0FBQyxJQUFJLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxHQUFHOzs7b0JBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLGlCQUFpQixDQUFDLElBQUk7Ozs7b0JBQUMsUUFBUSxDQUFDLEVBQUUsQ0FBQyxRQUFRLENBQUMsRUFBRSxLQUFLLElBQUksQ0FBQyxFQUFFLEVBQUMsRUFBQyxDQUFDO2FBQzdGLENBQUMsRUFDSCxDQUNGO1NBQ0YsQ0FBQyxDQUFDO1FBQ0gsSUFBSSxDQUFDLElBQUksQ0FBQyxRQUFRLENBQUMsUUFBUSxFQUFFO1lBQzNCLElBQUksQ0FBQyxJQUFJLENBQUMsVUFBVSxDQUFDLFVBQVUsRUFBRSxJQUFJLFdBQVcsQ0FBQyxFQUFFLEVBQUUsQ0FBQyxVQUFVLENBQUMsUUFBUSxFQUFFLFVBQVUsQ0FBQyxTQUFTLENBQUMsRUFBRSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUM7U0FDeEc7SUFDSCxDQUFDOzs7O0lBRUQsU0FBUztRQUNQLElBQUksQ0FBQyxTQUFTLEVBQUUsQ0FBQztRQUNqQixJQUFJLENBQUMsY0FBYyxHQUFHLElBQUksQ0FBQztJQUM3QixDQUFDOzs7O0lBRUQsS0FBSztRQUNILElBQUksQ0FBQyxRQUFRLEdBQUcsbUJBQUEsRUFBRSxFQUFxQixDQUFDO1FBQ3hDLElBQUksQ0FBQyxpQkFBaUIsR0FBRyxtQkFBQSxFQUFFLEVBQXVCLENBQUM7UUFDbkQsSUFBSSxDQUFDLFNBQVMsRUFBRSxDQUFDO0lBQ25CLENBQUM7Ozs7O0lBRUQsTUFBTSxDQUFDLEVBQVU7UUFDZixJQUFJLENBQUMsS0FBSzthQUNQLFFBQVEsQ0FBQyxJQUFJLFdBQVcsQ0FBQyxFQUFFLENBQUMsQ0FBQzthQUM3QixJQUFJLENBQ0gsU0FBUzs7O1FBQUMsR0FBRyxFQUFFLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxRQUFRLENBQUMsSUFBSSxZQUFZLENBQUMsRUFBRSxDQUFDLENBQUMsRUFBQyxFQUMxRCxLQUFLLENBQUMsZUFBZSxDQUFDLEVBQ3RCLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FDUjthQUNBLFNBQVM7Ozs7UUFBQyxDQUFDLEtBQXFCLEVBQUUsRUFBRTtZQUNuQyxJQUFJLENBQUMsUUFBUSxHQUFHLEtBQUssQ0FBQyxZQUFZLENBQUM7WUFDbkMsSUFBSSxDQUFDLGlCQUFpQixHQUFHLEtBQUssQ0FBQyxpQkFBaUIsQ0FBQztZQUNqRCxJQUFJLENBQUMsU0FBUyxFQUFFLENBQUM7UUFDbkIsQ0FBQyxFQUFDLENBQUM7SUFDUCxDQUFDOzs7O0lBRUQsSUFBSTtRQUNGLElBQUksQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLEtBQUs7WUFBRSxPQUFPO1FBQzdCLElBQUksQ0FBQyxTQUFTLEdBQUcsSUFBSSxDQUFDO2NBRWhCLEVBQUUsU0FBUyxFQUFFLEdBQUcsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLOztjQUMvQixlQUFlLEdBQUcsR0FBRzs7O1FBQ3pCLEdBQUcsRUFBRSxDQUFDLFNBQVMsQ0FBQyxNQUFNOzs7O1FBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUMsSUFBSSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLENBQUMsRUFBQyxDQUFDLEdBQUc7Ozs7UUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLE1BQU0sQ0FBQyxJQUFJLENBQUMsSUFBSSxDQUFDLENBQUMsQ0FBQyxDQUFDLEVBQUMsR0FDOUYsRUFBRSxDQUNIO1FBRUQsSUFBSSxDQUFDLEtBQUs7YUFDUCxRQUFRLENBQ1AsSUFBSSxDQUFDLFFBQVEsQ0FBQyxFQUFFO1lBQ2QsQ0FBQyxDQUFDLElBQUksVUFBVSxtQkFDVCxJQUFJLENBQUMsSUFBSSxDQUFDLEtBQUssSUFDbEIsRUFBRSxFQUFFLElBQUksQ0FBQyxRQUFRLENBQUMsRUFBRSxFQUNwQixTQUFTLEVBQUUsZUFBZSxJQUMxQjtZQUNKLENBQUMsQ0FBQyxJQUFJLFVBQVUsbUJBQ1QsSUFBSSxDQUFDLElBQUksQ0FBQyxLQUFLLElBQ2xCLFNBQVMsRUFBRSxlQUFlLElBQzFCLENBQ1A7YUFDQSxTQUFTOzs7UUFBQyxHQUFHLEVBQUU7WUFDZCxJQUFJLENBQUMsU0FBUyxHQUFHLEtBQUssQ0FBQztZQUN2QixJQUFJLENBQUMsY0FBYyxHQUFHLEtBQUssQ0FBQztRQUM5QixDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7OztJQUVELE1BQU0sQ0FBQyxFQUFVLEVBQUUsUUFBZ0I7UUFDakMsSUFBSSxDQUFDLG1CQUFtQjthQUNyQixJQUFJLENBQUMsOENBQThDLEVBQUUseUJBQXlCLEVBQUU7WUFDL0UseUJBQXlCLEVBQUUsQ0FBQyxRQUFRLENBQUM7U0FDdEMsQ0FBQzthQUNELFNBQVM7Ozs7UUFBQyxDQUFDLE1BQXNCLEVBQUUsRUFBRTtZQUNwQyxJQUFJLE1BQU0sNEJBQTJCLEVBQUU7Z0JBQ3JDLElBQUksQ0FBQyxLQUFLLENBQUMsUUFBUSxDQUFDLElBQUksVUFBVSxDQUFDLEVBQUUsQ0FBQyxDQUFDLENBQUM7YUFDekM7UUFDSCxDQUFDLEVBQUMsQ0FBQztJQUNQLENBQUM7Ozs7O0lBRUQsWUFBWSxDQUFDLElBQUk7UUFDZixJQUFJLENBQUMsU0FBUyxDQUFDLFNBQVMsR0FBRyxJQUFJLENBQUMsS0FBSyxDQUFDO1FBQ3RDLElBQUksQ0FBQyxTQUFTLENBQUMsY0FBYyxHQUFHLElBQUksQ0FBQyxJQUFJLENBQUM7UUFFMUMsSUFBSSxDQUFDLEdBQUcsRUFBRSxDQUFDO0lBQ2IsQ0FBQzs7OztJQUVELEdBQUc7UUFDRCxJQUFJLENBQUMsT0FBTyxHQUFHLElBQUksQ0FBQztRQUNwQixJQUFJLENBQUMsS0FBSzthQUNQLFFBQVEsQ0FBQyxJQUFJLFFBQVEsQ0FBQyxJQUFJLENBQUMsU0FBUyxDQUFDLENBQUM7YUFDdEMsSUFBSSxDQUFDLFFBQVE7OztRQUFDLEdBQUcsRUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUMsRUFBQyxDQUFDO2FBQzVDLFNBQVMsRUFBRSxDQUFDO0lBQ2pCLENBQUM7OztZQTFKRixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLFdBQVc7Z0JBQ3JCLHNqU0FBcUM7YUFDdEM7Ozs7WUFwQlEsbUJBQW1CO1lBRVMsV0FBVztZQUMvQixLQUFLOzs7MkJBeUJuQixTQUFTLFNBQUMsY0FBYyxFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRTs7QUFMNUM7SUFEQyxNQUFNLENBQUMsYUFBYSxDQUFDLFFBQVEsQ0FBQztzQ0FDeEIsVUFBVTs2Q0FBc0I7QUFHdkM7SUFEQyxNQUFNLENBQUMsYUFBYSxDQUFDLGtCQUFrQixDQUFDO3NDQUM1QixVQUFVO21EQUFTOzs7SUFKaEMsK0JBQ3VDOztJQUV2QyxxQ0FDZ0M7O0lBRWhDLHNDQUMrQjs7SUFFL0IsOEJBQWdCOztJQUVoQixrQ0FBNEI7O0lBRTVCLDJDQUF1Qzs7SUFFdkMsK0JBQTJCOztJQUUzQiw0Q0FBMkI7O0lBRTNCLHFDQUFvQjs7SUFFcEIsbUNBQW9DOztJQUVwQyx3Q0FBd0I7O0lBRXhCLGlDQUFnQjs7SUFFaEIsbUNBQWtCOztJQUVsQixtQ0FBZTs7SUFFZixpQ0FBYTs7SUFFYixtQ0FBNkY7Ozs7O0lBTWpGLDZDQUFnRDs7Ozs7SUFBRSw0QkFBdUI7Ozs7O0lBQUUsK0JBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UsIFRvYXN0ZXIgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBDb21wb25lbnQsIFRlbXBsYXRlUmVmLCBUcmFja0J5RnVuY3Rpb24sIFZpZXdDaGlsZCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWJzdHJhY3RDb250cm9sLCBGb3JtQXJyYXksIEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMsIEZvcm1Db250cm9sIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGZpbmFsaXplLCBwbHVjaywgc3dpdGNoTWFwLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHtcbiAgQ3JlYXRlVXNlcixcbiAgRGVsZXRlVXNlcixcbiAgR2V0VXNlckJ5SWQsXG4gIEdldFVzZXJSb2xlcyxcbiAgR2V0VXNlcnMsXG4gIFVwZGF0ZVVzZXIsXG59IGZyb20gJy4uLy4uL2FjdGlvbnMvaWRlbnRpdHkuYWN0aW9ucyc7XG5pbXBvcnQgeyBJZGVudGl0eSB9IGZyb20gJy4uLy4uL21vZGVscy9pZGVudGl0eSc7XG5pbXBvcnQgeyBJZGVudGl0eVN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzL2lkZW50aXR5LnN0YXRlJztcbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC11c2VycycsXG4gIHRlbXBsYXRlVXJsOiAnLi91c2Vycy5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFVzZXJzQ29tcG9uZW50IHtcbiAgQFNlbGVjdChJZGVudGl0eVN0YXRlLmdldFVzZXJzKVxuICBkYXRhJDogT2JzZXJ2YWJsZTxJZGVudGl0eS5Vc2VySXRlbVtdPjtcblxuICBAU2VsZWN0KElkZW50aXR5U3RhdGUuZ2V0VXNlcnNUb3RhbENvdW50KVxuICB0b3RhbENvdW50JDogT2JzZXJ2YWJsZTxudW1iZXI+O1xuXG4gIEBWaWV3Q2hpbGQoJ21vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBtb2RhbENvbnRlbnQ6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIHNlbGVjdGVkOiBJZGVudGl0eS5Vc2VySXRlbTtcblxuICBzZWxlY3RlZFVzZXJSb2xlczogSWRlbnRpdHkuUm9sZUl0ZW1bXTtcblxuICByb2xlczogSWRlbnRpdHkuUm9sZUl0ZW1bXTtcblxuICB2aXNpYmxlUGVybWlzc2lvbnMgPSBmYWxzZTtcblxuICBwcm92aWRlcktleTogc3RyaW5nO1xuXG4gIHBhZ2VRdWVyeTogQUJQLlBhZ2VRdWVyeVBhcmFtcyA9IHt9O1xuXG4gIGlzTW9kYWxWaXNpYmxlOiBib29sZWFuO1xuXG4gIGxvYWRpbmcgPSBmYWxzZTtcblxuICBtb2RhbEJ1c3kgPSBmYWxzZTtcblxuICBzb3J0T3JkZXIgPSAnJztcblxuICBzb3J0S2V5ID0gJyc7XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248QWJzdHJhY3RDb250cm9sPiA9IChpbmRleCwgaXRlbSkgPT4gT2JqZWN0LmtleXMoaXRlbSlbMF0gfHwgaW5kZXg7XG5cbiAgZ2V0IHJvbGVHcm91cHMoKTogRm9ybUdyb3VwW10ge1xuICAgIHJldHVybiBzbnEoKCkgPT4gKHRoaXMuZm9ybS5nZXQoJ3JvbGVOYW1lcycpIGFzIEZvcm1BcnJheSkuY29udHJvbHMgYXMgRm9ybUdyb3VwW10sIFtdKTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgY29uZmlybWF0aW9uU2VydmljZTogQ29uZmlybWF0aW9uU2VydmljZSwgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIG9uU2VhcmNoKHZhbHVlKSB7XG4gICAgdGhpcy5wYWdlUXVlcnkuZmlsdGVyID0gdmFsdWU7XG4gICAgdGhpcy5nZXQoKTtcbiAgfVxuXG4gIGJ1aWxkRm9ybSgpIHtcbiAgICB0aGlzLnJvbGVzID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChJZGVudGl0eVN0YXRlLmdldFJvbGVzKTtcbiAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIHVzZXJOYW1lOiBbdGhpcy5zZWxlY3RlZC51c2VyTmFtZSB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICAgIGVtYWlsOiBbdGhpcy5zZWxlY3RlZC5lbWFpbCB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMuZW1haWwsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICAgIG5hbWU6IFt0aGlzLnNlbGVjdGVkLm5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLm1heExlbmd0aCg2NCldXSxcbiAgICAgIHN1cm5hbWU6IFt0aGlzLnNlbGVjdGVkLnN1cm5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLm1heExlbmd0aCg2NCldXSxcbiAgICAgIHBob25lTnVtYmVyOiBbdGhpcy5zZWxlY3RlZC5waG9uZU51bWJlciB8fCAnJywgW1ZhbGlkYXRvcnMubWF4TGVuZ3RoKDE2KV1dLFxuICAgICAgbG9ja291dEVuYWJsZWQ6IFt0aGlzLnNlbGVjdGVkLnR3b0ZhY3RvckVuYWJsZWQgfHwgKHRoaXMuc2VsZWN0ZWQuaWQgPyBmYWxzZSA6IHRydWUpXSxcbiAgICAgIHR3b0ZhY3RvckVuYWJsZWQ6IFt0aGlzLnNlbGVjdGVkLnR3b0ZhY3RvckVuYWJsZWQgfHwgKHRoaXMuc2VsZWN0ZWQuaWQgPyBmYWxzZSA6IHRydWUpXSxcbiAgICAgIHJvbGVOYW1lczogdGhpcy5mYi5hcnJheShcbiAgICAgICAgdGhpcy5yb2xlcy5tYXAocm9sZSA9PlxuICAgICAgICAgIHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgICAgICAgW3JvbGUubmFtZV06IFshIXNucSgoKSA9PiB0aGlzLnNlbGVjdGVkVXNlclJvbGVzLmZpbmQodXNlclJvbGUgPT4gdXNlclJvbGUuaWQgPT09IHJvbGUuaWQpKV0sXG4gICAgICAgICAgfSksXG4gICAgICAgICksXG4gICAgICApLFxuICAgIH0pO1xuICAgIGlmICghdGhpcy5zZWxlY3RlZC51c2VyTmFtZSkge1xuICAgICAgdGhpcy5mb3JtLmFkZENvbnRyb2woJ3Bhc3N3b3JkJywgbmV3IEZvcm1Db250cm9sKCcnLCBbVmFsaWRhdG9ycy5yZXF1aXJlZCwgVmFsaWRhdG9ycy5tYXhMZW5ndGgoMzIpXSkpO1xuICAgIH1cbiAgfVxuXG4gIG9wZW5Nb2RhbCgpIHtcbiAgICB0aGlzLmJ1aWxkRm9ybSgpO1xuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgb25BZGQoKSB7XG4gICAgdGhpcy5zZWxlY3RlZCA9IHt9IGFzIElkZW50aXR5LlVzZXJJdGVtO1xuICAgIHRoaXMuc2VsZWN0ZWRVc2VyUm9sZXMgPSBbXSBhcyBJZGVudGl0eS5Sb2xlSXRlbVtdO1xuICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gIH1cblxuICBvbkVkaXQoaWQ6IHN0cmluZykge1xuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChuZXcgR2V0VXNlckJ5SWQoaWQpKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHN3aXRjaE1hcCgoKSA9PiB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBHZXRVc2VyUm9sZXMoaWQpKSksXG4gICAgICAgIHBsdWNrKCdJZGVudGl0eVN0YXRlJyksXG4gICAgICAgIHRha2UoMSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKChzdGF0ZTogSWRlbnRpdHkuU3RhdGUpID0+IHtcbiAgICAgICAgdGhpcy5zZWxlY3RlZCA9IHN0YXRlLnNlbGVjdGVkVXNlcjtcbiAgICAgICAgdGhpcy5zZWxlY3RlZFVzZXJSb2xlcyA9IHN0YXRlLnNlbGVjdGVkVXNlclJvbGVzO1xuICAgICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGlmICghdGhpcy5mb3JtLnZhbGlkKSByZXR1cm47XG4gICAgdGhpcy5tb2RhbEJ1c3kgPSB0cnVlO1xuXG4gICAgY29uc3QgeyByb2xlTmFtZXMgfSA9IHRoaXMuZm9ybS52YWx1ZTtcbiAgICBjb25zdCBtYXBwZWRSb2xlTmFtZXMgPSBzbnEoXG4gICAgICAoKSA9PiByb2xlTmFtZXMuZmlsdGVyKHJvbGUgPT4gISFyb2xlW09iamVjdC5rZXlzKHJvbGUpWzBdXSkubWFwKHJvbGUgPT4gT2JqZWN0LmtleXMocm9sZSlbMF0pLFxuICAgICAgW10sXG4gICAgKTtcblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgdGhpcy5zZWxlY3RlZC5pZFxuICAgICAgICAgID8gbmV3IFVwZGF0ZVVzZXIoe1xuICAgICAgICAgICAgICAuLi50aGlzLmZvcm0udmFsdWUsXG4gICAgICAgICAgICAgIGlkOiB0aGlzLnNlbGVjdGVkLmlkLFxuICAgICAgICAgICAgICByb2xlTmFtZXM6IG1hcHBlZFJvbGVOYW1lcyxcbiAgICAgICAgICAgIH0pXG4gICAgICAgICAgOiBuZXcgQ3JlYXRlVXNlcih7XG4gICAgICAgICAgICAgIC4uLnRoaXMuZm9ybS52YWx1ZSxcbiAgICAgICAgICAgICAgcm9sZU5hbWVzOiBtYXBwZWRSb2xlTmFtZXMsXG4gICAgICAgICAgICB9KSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICB0aGlzLm1vZGFsQnVzeSA9IGZhbHNlO1xuICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICB9KTtcbiAgfVxuXG4gIGRlbGV0ZShpZDogc3RyaW5nLCB1c2VyTmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybignQWJwSWRlbnRpdHk6OlVzZXJEZWxldGlvbkNvbmZpcm1hdGlvbk1lc3NhZ2UnLCAnQWJwSWRlbnRpdHk6OkFyZVlvdVN1cmUnLCB7XG4gICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFt1c2VyTmFtZV0sXG4gICAgICB9KVxuICAgICAgLnN1YnNjcmliZSgoc3RhdHVzOiBUb2FzdGVyLlN0YXR1cykgPT4ge1xuICAgICAgICBpZiAoc3RhdHVzID09PSBUb2FzdGVyLlN0YXR1cy5jb25maXJtKSB7XG4gICAgICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgRGVsZXRlVXNlcihpZCkpO1xuICAgICAgICB9XG4gICAgICB9KTtcbiAgfVxuXG4gIG9uUGFnZUNoYW5nZShkYXRhKSB7XG4gICAgdGhpcy5wYWdlUXVlcnkuc2tpcENvdW50ID0gZGF0YS5maXJzdDtcbiAgICB0aGlzLnBhZ2VRdWVyeS5tYXhSZXN1bHRDb3VudCA9IGRhdGEucm93cztcblxuICAgIHRoaXMuZ2V0KCk7XG4gIH1cblxuICBnZXQoKSB7XG4gICAgdGhpcy5sb2FkaW5nID0gdHJ1ZTtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFVzZXJzKHRoaXMucGFnZVF1ZXJ5KSlcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLmxvYWRpbmcgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgpO1xuICB9XG59XG4iXX0=
