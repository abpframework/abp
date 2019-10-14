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
var UsersComponent = /** @class */ (function() {
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
    this.trackByFn
    /**
     * @param {?} index
     * @param {?} item
     * @return {?}
     */ = function(index, item) {
      return Object.keys(item)[0] || index;
    };
  }
  Object.defineProperty(UsersComponent.prototype, 'roleGroups', {
    /**
     * @return {?}
     */
    get: function() {
      var _this = this;
      return snq(
        /**
         * @return {?}
         */
        function() {
          return /** @type {?} */ (/** @type {?} */ (_this.form.get('roleNames')).controls);
        },
        [],
      );
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @param {?} value
   * @return {?}
   */
  UsersComponent.prototype.onSearch
  /**
   * @param {?} value
   * @return {?}
   */ = function(value) {
    this.pageQuery.filter = value;
    this.get();
  };
  /**
   * @return {?}
   */
  UsersComponent.prototype.buildForm
  /**
   * @return {?}
   */ = function() {
    var _this = this;
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
          function(role) {
            var _a;
            return _this.fb.group(
              ((_a = {}),
              (_a[role.name] = [
                !!snq(
                  /**
                   * @return {?}
                   */
                  function() {
                    return _this.selectedUserRoles.find(
                      /**
                       * @param {?} userRole
                       * @return {?}
                       */
                      function(userRole) {
                        return userRole.id === role.id;
                      },
                    );
                  },
                ),
              ]),
              _a),
            );
          },
        ),
      ),
    });
    if (!this.selected.userName) {
      this.form.addControl('password', new FormControl('', [Validators.required, Validators.maxLength(32)]));
    }
  };
  /**
   * @return {?}
   */
  UsersComponent.prototype.openModal
  /**
   * @return {?}
   */ = function() {
    this.buildForm();
    this.isModalVisible = true;
  };
  /**
   * @return {?}
   */
  UsersComponent.prototype.onAdd
  /**
   * @return {?}
   */ = function() {
    this.selected = /** @type {?} */ ({});
    this.selectedUserRoles = /** @type {?} */ ([]);
    this.openModal();
  };
  /**
   * @param {?} id
   * @return {?}
   */
  UsersComponent.prototype.onEdit
  /**
   * @param {?} id
   * @return {?}
   */ = function(id) {
    var _this = this;
    this.store
      .dispatch(new GetUserById(id))
      .pipe(
        switchMap(
          /**
           * @return {?}
           */
          function() {
            return _this.store.dispatch(new GetUserRoles(id));
          },
        ),
        pluck('IdentityState'),
        take(1),
      )
      .subscribe(
        /**
         * @param {?} state
         * @return {?}
         */
        function(state) {
          _this.selected = state.selectedUser;
          _this.selectedUserRoles = state.selectedUserRoles;
          _this.openModal();
        },
      );
  };
  /**
   * @return {?}
   */
  UsersComponent.prototype.save
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    if (!this.form.valid) return;
    this.modalBusy = true;
    var roleNames = this.form.value.roleNames;
    /** @type {?} */
    var mappedRoleNames = snq(
      /**
       * @return {?}
       */
      (function() {
        return roleNames
          .filter(
            /**
             * @param {?} role
             * @return {?}
             */
            function(role) {
              return !!role[Object.keys(role)[0]];
            },
          )
          .map(
            /**
             * @param {?} role
             * @return {?}
             */
            function(role) {
              return Object.keys(role)[0];
            },
          );
      }),
      [],
    );
    this.store
      .dispatch(
        this.selected.id
          ? new UpdateUser(tslib_1.__assign({}, this.form.value, { id: this.selected.id, roleNames: mappedRoleNames }))
          : new CreateUser(tslib_1.__assign({}, this.form.value, { roleNames: mappedRoleNames })),
      )
      .subscribe(
        /**
         * @return {?}
         */
        function() {
          _this.modalBusy = false;
          _this.isModalVisible = false;
        },
      );
  };
  /**
   * @param {?} id
   * @param {?} userName
   * @return {?}
   */
  UsersComponent.prototype.delete
  /**
   * @param {?} id
   * @param {?} userName
   * @return {?}
   */ = function(id, userName) {
    var _this = this;
    this.confirmationService
      .warn('AbpIdentity::UserDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
        messageLocalizationParams: [userName],
      })
      .subscribe(
        /**
         * @param {?} status
         * @return {?}
         */
        function(status) {
          if (status === 'confirm' /* confirm */) {
            _this.store.dispatch(new DeleteUser(id));
          }
        },
      );
  };
  /**
   * @param {?} data
   * @return {?}
   */
  UsersComponent.prototype.onPageChange
  /**
   * @param {?} data
   * @return {?}
   */ = function(data) {
    this.pageQuery.skipCount = data.first;
    this.pageQuery.maxResultCount = data.rows;
    this.get();
  };
  /**
   * @return {?}
   */
  UsersComponent.prototype.get
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    this.loading = true;
    this.store
      .dispatch(new GetUsers(this.pageQuery))
      .pipe(
        finalize(
          /**
           * @return {?}
           */
          function() {
            return (_this.loading = false);
          },
        ),
      )
      .subscribe();
  };
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
  UsersComponent.ctorParameters = function() {
    return [{ type: ConfirmationService }, { type: FormBuilder }, { type: Store }];
  };
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
  return UsersComponent;
})();
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoidXNlcnMuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3VzZXJzL3VzZXJzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFtQixTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDbkYsT0FBTyxFQUE4QixXQUFXLEVBQWEsVUFBVSxFQUFFLFdBQVcsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQzdHLE9BQU8sRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFLE1BQU0sYUFBYSxDQUFDO0FBQzVDLE9BQU8sRUFBRSxVQUFVLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDbEMsT0FBTyxFQUFFLFFBQVEsRUFBRSxLQUFLLEVBQUUsU0FBUyxFQUFFLElBQUksRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ2xFLE9BQU8sR0FBRyxNQUFNLEtBQUssQ0FBQztBQUN0QixPQUFPLEVBQ0wsVUFBVSxFQUNWLFVBQVUsRUFDVixXQUFXLEVBQ1gsWUFBWSxFQUNaLFFBQVEsRUFDUixVQUFVLEdBQ1gsTUFBTSxnQ0FBZ0MsQ0FBQztBQUV4QyxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0sNkJBQTZCLENBQUM7QUFDNUQ7SUE0Q0Usd0JBQW9CLG1CQUF3QyxFQUFVLEVBQWUsRUFBVSxLQUFZO1FBQXZGLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7UUFBVSxPQUFFLEdBQUYsRUFBRSxDQUFhO1FBQVUsVUFBSyxHQUFMLEtBQUssQ0FBTztRQXRCM0csdUJBQWtCLEdBQUcsS0FBSyxDQUFDO1FBSTNCLGNBQVMsR0FBd0IsRUFBRSxDQUFDO1FBSXBDLFlBQU8sR0FBRyxLQUFLLENBQUM7UUFFaEIsY0FBUyxHQUFHLEtBQUssQ0FBQztR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USxtQkFBbUI7Z0JBRVMsV0FBVztnQkFDL0IsS0FBSzs7OytCQXlCbkIsU0FBUyxTQUFDLGNBQWMsRUFBRSxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUU7O0lBTDVDO1FBREMsTUFBTSxDQUFDLGFBQWEsQ0FBQyxRQUFRLENBQUM7MENBQ3hCLFVBQVU7aURBQXNCO0lBR3ZDO1FBREMsTUFBTSxDQUFDLGFBQWEsQ0FBQyxrQkFBa0IsQ0FBQzswQ0FDNUIsVUFBVTt1REFBUztJQWtKbEMscUJBQUM7Q0FBQSxBQTNKRCxJQTJKQztTQXZKWSxjQUFjOzs7SUFDekIsK0JBQ3VDOztJQUV2QyxxQ0FDZ0M7O0lBRWhDLHNDQUMrQjs7SUFFL0IsOEJBQWdCOztJQUVoQixrQ0FBNEI7O0lBRTVCLDJDQUF1Qzs7SUFFdkMsK0JBQTJCOztJQUUzQiw0Q0FBMkI7O0lBRTNCLHFDQUFvQjs7SUFFcEIsbUNBQW9DOztJQUVwQyx3Q0FBd0I7O0lBRXhCLGlDQUFnQjs7SUFFaEIsbUNBQWtCOztJQUVsQixtQ0FBZTs7SUFFZixpQ0FBYTs7SUFFYixtQ0FBNkY7Ozs7O0lBTWpGLDZDQUFnRDs7Ozs7SUFBRSw0QkFBdUI7Ozs7O0lBQUUsK0JBQW9CIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQUJQIH0gZnJvbSAnQGFicC9uZy5jb3JlJztcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UsIFRvYXN0ZXIgfSBmcm9tICdAYWJwL25nLnRoZW1lLnNoYXJlZCc7XG5pbXBvcnQgeyBDb21wb25lbnQsIFRlbXBsYXRlUmVmLCBUcmFja0J5RnVuY3Rpb24sIFZpZXdDaGlsZCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQWJzdHJhY3RDb250cm9sLCBGb3JtQXJyYXksIEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMsIEZvcm1Db250cm9sIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGZpbmFsaXplLCBwbHVjaywgc3dpdGNoTWFwLCB0YWtlIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHNucSBmcm9tICdzbnEnO1xuaW1wb3J0IHtcbiAgQ3JlYXRlVXNlcixcbiAgRGVsZXRlVXNlcixcbiAgR2V0VXNlckJ5SWQsXG4gIEdldFVzZXJSb2xlcyxcbiAgR2V0VXNlcnMsXG4gIFVwZGF0ZVVzZXIsXG59IGZyb20gJy4uLy4uL2FjdGlvbnMvaWRlbnRpdHkuYWN0aW9ucyc7XG5pbXBvcnQgeyBJZGVudGl0eSB9IGZyb20gJy4uLy4uL21vZGVscy9pZGVudGl0eSc7XG5pbXBvcnQgeyBJZGVudGl0eVN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzL2lkZW50aXR5LnN0YXRlJztcbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC11c2VycycsXG4gIHRlbXBsYXRlVXJsOiAnLi91c2Vycy5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIFVzZXJzQ29tcG9uZW50IHtcbiAgQFNlbGVjdChJZGVudGl0eVN0YXRlLmdldFVzZXJzKVxuICBkYXRhJDogT2JzZXJ2YWJsZTxJZGVudGl0eS5Vc2VySXRlbVtdPjtcblxuICBAU2VsZWN0KElkZW50aXR5U3RhdGUuZ2V0VXNlcnNUb3RhbENvdW50KVxuICB0b3RhbENvdW50JDogT2JzZXJ2YWJsZTxudW1iZXI+O1xuXG4gIEBWaWV3Q2hpbGQoJ21vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KVxuICBtb2RhbENvbnRlbnQ6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIHNlbGVjdGVkOiBJZGVudGl0eS5Vc2VySXRlbTtcblxuICBzZWxlY3RlZFVzZXJSb2xlczogSWRlbnRpdHkuUm9sZUl0ZW1bXTtcblxuICByb2xlczogSWRlbnRpdHkuUm9sZUl0ZW1bXTtcblxuICB2aXNpYmxlUGVybWlzc2lvbnMgPSBmYWxzZTtcblxuICBwcm92aWRlcktleTogc3RyaW5nO1xuXG4gIHBhZ2VRdWVyeTogQUJQLlBhZ2VRdWVyeVBhcmFtcyA9IHt9O1xuXG4gIGlzTW9kYWxWaXNpYmxlOiBib29sZWFuO1xuXG4gIGxvYWRpbmcgPSBmYWxzZTtcblxuICBtb2RhbEJ1c3kgPSBmYWxzZTtcblxuICBzb3J0T3JkZXIgPSAnJztcblxuICBzb3J0S2V5ID0gJyc7XG5cbiAgdHJhY2tCeUZuOiBUcmFja0J5RnVuY3Rpb248QWJzdHJhY3RDb250cm9sPiA9IChpbmRleCwgaXRlbSkgPT4gT2JqZWN0LmtleXMoaXRlbSlbMF0gfHwgaW5kZXg7XG5cbiAgZ2V0IHJvbGVHcm91cHMoKTogRm9ybUdyb3VwW10ge1xuICAgIHJldHVybiBzbnEoKCkgPT4gKHRoaXMuZm9ybS5nZXQoJ3JvbGVOYW1lcycpIGFzIEZvcm1BcnJheSkuY29udHJvbHMgYXMgRm9ybUdyb3VwW10sIFtdKTtcbiAgfVxuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgY29uZmlybWF0aW9uU2VydmljZTogQ29uZmlybWF0aW9uU2VydmljZSwgcHJpdmF0ZSBmYjogRm9ybUJ1aWxkZXIsIHByaXZhdGUgc3RvcmU6IFN0b3JlKSB7fVxuXG4gIG9uU2VhcmNoKHZhbHVlKSB7XG4gICAgdGhpcy5wYWdlUXVlcnkuZmlsdGVyID0gdmFsdWU7XG4gICAgdGhpcy5nZXQoKTtcbiAgfVxuXG4gIGJ1aWxkRm9ybSgpIHtcbiAgICB0aGlzLnJvbGVzID0gdGhpcy5zdG9yZS5zZWxlY3RTbmFwc2hvdChJZGVudGl0eVN0YXRlLmdldFJvbGVzKTtcbiAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIHVzZXJOYW1lOiBbdGhpcy5zZWxlY3RlZC51c2VyTmFtZSB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICAgIGVtYWlsOiBbdGhpcy5zZWxlY3RlZC5lbWFpbCB8fCAnJywgW1ZhbGlkYXRvcnMucmVxdWlyZWQsIFZhbGlkYXRvcnMuZW1haWwsIFZhbGlkYXRvcnMubWF4TGVuZ3RoKDI1NildXSxcbiAgICAgIG5hbWU6IFt0aGlzLnNlbGVjdGVkLm5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLm1heExlbmd0aCg2NCldXSxcbiAgICAgIHN1cm5hbWU6IFt0aGlzLnNlbGVjdGVkLnN1cm5hbWUgfHwgJycsIFtWYWxpZGF0b3JzLm1heExlbmd0aCg2NCldXSxcbiAgICAgIHBob25lTnVtYmVyOiBbdGhpcy5zZWxlY3RlZC5waG9uZU51bWJlciB8fCAnJywgW1ZhbGlkYXRvcnMubWF4TGVuZ3RoKDE2KV1dLFxuICAgICAgbG9ja291dEVuYWJsZWQ6IFt0aGlzLnNlbGVjdGVkLnR3b0ZhY3RvckVuYWJsZWQgfHwgKHRoaXMuc2VsZWN0ZWQuaWQgPyBmYWxzZSA6IHRydWUpXSxcbiAgICAgIHR3b0ZhY3RvckVuYWJsZWQ6IFt0aGlzLnNlbGVjdGVkLnR3b0ZhY3RvckVuYWJsZWQgfHwgKHRoaXMuc2VsZWN0ZWQuaWQgPyBmYWxzZSA6IHRydWUpXSxcbiAgICAgIHJvbGVOYW1lczogdGhpcy5mYi5hcnJheShcbiAgICAgICAgdGhpcy5yb2xlcy5tYXAocm9sZSA9PlxuICAgICAgICAgIHRoaXMuZmIuZ3JvdXAoe1xuICAgICAgICAgICAgW3JvbGUubmFtZV06IFshIXNucSgoKSA9PiB0aGlzLnNlbGVjdGVkVXNlclJvbGVzLmZpbmQodXNlclJvbGUgPT4gdXNlclJvbGUuaWQgPT09IHJvbGUuaWQpKV0sXG4gICAgICAgICAgfSksXG4gICAgICAgICksXG4gICAgICApLFxuICAgIH0pO1xuICAgIGlmICghdGhpcy5zZWxlY3RlZC51c2VyTmFtZSkge1xuICAgICAgdGhpcy5mb3JtLmFkZENvbnRyb2woJ3Bhc3N3b3JkJywgbmV3IEZvcm1Db250cm9sKCcnLCBbVmFsaWRhdG9ycy5yZXF1aXJlZCwgVmFsaWRhdG9ycy5tYXhMZW5ndGgoMzIpXSkpO1xuICAgIH1cbiAgfVxuXG4gIG9wZW5Nb2RhbCgpIHtcbiAgICB0aGlzLmJ1aWxkRm9ybSgpO1xuICAgIHRoaXMuaXNNb2RhbFZpc2libGUgPSB0cnVlO1xuICB9XG5cbiAgb25BZGQoKSB7XG4gICAgdGhpcy5zZWxlY3RlZCA9IHt9IGFzIElkZW50aXR5LlVzZXJJdGVtO1xuICAgIHRoaXMuc2VsZWN0ZWRVc2VyUm9sZXMgPSBbXSBhcyBJZGVudGl0eS5Sb2xlSXRlbVtdO1xuICAgIHRoaXMub3Blbk1vZGFsKCk7XG4gIH1cblxuICBvbkVkaXQoaWQ6IHN0cmluZykge1xuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChuZXcgR2V0VXNlckJ5SWQoaWQpKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHN3aXRjaE1hcCgoKSA9PiB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBHZXRVc2VyUm9sZXMoaWQpKSksXG4gICAgICAgIHBsdWNrKCdJZGVudGl0eVN0YXRlJyksXG4gICAgICAgIHRha2UoMSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKChzdGF0ZTogSWRlbnRpdHkuU3RhdGUpID0+IHtcbiAgICAgICAgdGhpcy5zZWxlY3RlZCA9IHN0YXRlLnNlbGVjdGVkVXNlcjtcbiAgICAgICAgdGhpcy5zZWxlY3RlZFVzZXJSb2xlcyA9IHN0YXRlLnNlbGVjdGVkVXNlclJvbGVzO1xuICAgICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGlmICghdGhpcy5mb3JtLnZhbGlkKSByZXR1cm47XG4gICAgdGhpcy5tb2RhbEJ1c3kgPSB0cnVlO1xuXG4gICAgY29uc3QgeyByb2xlTmFtZXMgfSA9IHRoaXMuZm9ybS52YWx1ZTtcbiAgICBjb25zdCBtYXBwZWRSb2xlTmFtZXMgPSBzbnEoXG4gICAgICAoKSA9PiByb2xlTmFtZXMuZmlsdGVyKHJvbGUgPT4gISFyb2xlW09iamVjdC5rZXlzKHJvbGUpWzBdXSkubWFwKHJvbGUgPT4gT2JqZWN0LmtleXMocm9sZSlbMF0pLFxuICAgICAgW10sXG4gICAgKTtcblxuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChcbiAgICAgICAgdGhpcy5zZWxlY3RlZC5pZFxuICAgICAgICAgID8gbmV3IFVwZGF0ZVVzZXIoe1xuICAgICAgICAgICAgICAuLi50aGlzLmZvcm0udmFsdWUsXG4gICAgICAgICAgICAgIGlkOiB0aGlzLnNlbGVjdGVkLmlkLFxuICAgICAgICAgICAgICByb2xlTmFtZXM6IG1hcHBlZFJvbGVOYW1lcyxcbiAgICAgICAgICAgIH0pXG4gICAgICAgICAgOiBuZXcgQ3JlYXRlVXNlcih7XG4gICAgICAgICAgICAgIC4uLnRoaXMuZm9ybS52YWx1ZSxcbiAgICAgICAgICAgICAgcm9sZU5hbWVzOiBtYXBwZWRSb2xlTmFtZXMsXG4gICAgICAgICAgICB9KSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICB0aGlzLm1vZGFsQnVzeSA9IGZhbHNlO1xuICAgICAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gZmFsc2U7XG4gICAgICB9KTtcbiAgfVxuXG4gIGRlbGV0ZShpZDogc3RyaW5nLCB1c2VyTmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybignQWJwSWRlbnRpdHk6OlVzZXJEZWxldGlvbkNvbmZpcm1hdGlvbk1lc3NhZ2UnLCAnQWJwSWRlbnRpdHk6OkFyZVlvdVN1cmUnLCB7XG4gICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFt1c2VyTmFtZV0sXG4gICAgICB9KVxuICAgICAgLnN1YnNjcmliZSgoc3RhdHVzOiBUb2FzdGVyLlN0YXR1cykgPT4ge1xuICAgICAgICBpZiAoc3RhdHVzID09PSBUb2FzdGVyLlN0YXR1cy5jb25maXJtKSB7XG4gICAgICAgICAgdGhpcy5zdG9yZS5kaXNwYXRjaChuZXcgRGVsZXRlVXNlcihpZCkpO1xuICAgICAgICB9XG4gICAgICB9KTtcbiAgfVxuXG4gIG9uUGFnZUNoYW5nZShkYXRhKSB7XG4gICAgdGhpcy5wYWdlUXVlcnkuc2tpcENvdW50ID0gZGF0YS5maXJzdDtcbiAgICB0aGlzLnBhZ2VRdWVyeS5tYXhSZXN1bHRDb3VudCA9IGRhdGEucm93cztcblxuICAgIHRoaXMuZ2V0KCk7XG4gIH1cblxuICBnZXQoKSB7XG4gICAgdGhpcy5sb2FkaW5nID0gdHJ1ZTtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFVzZXJzKHRoaXMucGFnZVF1ZXJ5KSlcbiAgICAgIC5waXBlKGZpbmFsaXplKCgpID0+ICh0aGlzLmxvYWRpbmcgPSBmYWxzZSkpKVxuICAgICAgLnN1YnNjcmliZSgpO1xuICB9XG59XG4iXX0=
