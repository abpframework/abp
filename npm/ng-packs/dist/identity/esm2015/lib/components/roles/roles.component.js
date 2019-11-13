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
import { finalize, pluck } from 'rxjs/operators';
import { CreateRole, DeleteRole, GetRoleById, GetRoles, UpdateRole } from '../../actions/identity.actions';
import { IdentityState } from '../../states/identity.state';
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
    this.pageQuery = {};
    this.loading = false;
    this.modalBusy = false;
    this.sortOrder = '';
    this.sortKey = '';
  }
  /**
   * @return {?}
   */
  ngOnInit() {
    this.get();
  }
  /**
   * @return {?}
   */
  createForm() {
    this.form = this.fb.group({
      name: new FormControl({ value: this.selected.name || '', disabled: this.selected.isStatic }, [
        Validators.required,
        Validators.maxLength(256),
      ]),
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
    this.selected = /** @type {?} */ ({});
    this.openModal();
  }
  /**
   * @param {?} id
   * @return {?}
   */
  onEdit(id) {
    this.store
      .dispatch(new GetRoleById(id))
      .pipe(pluck('IdentityState', 'selectedRole'))
      .subscribe(
        /**
         * @param {?} selectedRole
         * @return {?}
         */
        selectedRole => {
          this.selected = selectedRole;
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
    this.store
      .dispatch(
        this.selected.id
          ? new UpdateRole(Object.assign({}, this.form.value, { id: this.selected.id }))
          : new CreateRole(this.form.value),
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
   * @param {?} name
   * @return {?}
   */
  delete(id, name) {
    this.confirmationService
      .warn('AbpIdentity::RoleDeletionConfirmationMessage', 'AbpIdentity::AreYouSure', {
        messageLocalizationParams: [name],
      })
      .subscribe(
        /**
         * @param {?} status
         * @return {?}
         */
        status => {
          if (status === 'confirm' /* confirm */) {
            this.store.dispatch(new DeleteRole(id));
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
      .dispatch(new GetRoles(this.pageQuery))
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
RolesComponent.decorators = [
  {
    type: Component,
    args: [
      {
        selector: 'abp-roles',
        template:
          '<div class="row entry-row">\n  <div class="col-auto">\n    <h1 class="content-header-title">{{ \'AbpIdentity::Roles\' | abpLocalization }}</h1>\n  </div>\n  <div class="col">\n    <div class="text-lg-right pt-2" id="AbpContentToolbar">\n      <button\n        [abpPermission]="\'AbpIdentity.Roles.Create\'"\n        id="create-role"\n        class="btn btn-primary"\n        type="button"\n        (click)="onAdd()"\n      >\n        <i class="fa fa-plus mr-1"></i> <span>{{ \'AbpIdentity::NewRole\' | abpLocalization }}</span>\n      </button>\n    </div>\n  </div>\n</div>\n\n<div id="identity-roles-wrapper" class="card">\n  <div class="card-body">\n    <p-table\n      *ngIf="[150, 0] as columnWidths"\n      [value]="data$ | async"\n      [abpTableSort]="{ key: sortKey, order: sortOrder }"\n      [lazy]="true"\n      [lazyLoadOnInit]="false"\n      [paginator]="true"\n      [rows]="10"\n      [totalRecords]="totalCount$ | async"\n      [loading]="loading"\n      [resizableColumns]="true"\n      [scrollable]="true"\n      (onLazyLoad)="onPageChange($event)"\n    >\n      <ng-template pTemplate="colgroup">\n        <colgroup>\n          <col *ngFor="let width of columnWidths" [ngStyle]="{ \'width.px\': width || undefined }" />\n        </colgroup>\n      </ng-template>\n      <ng-template pTemplate="emptymessage" let-columns>\n        <tr\n          abp-table-empty-message\n          [attr.colspan]="columnWidths.length"\n          localizationResource="AbpIdentity"\n          localizationProp="NoDataAvailableInDatatable"\n        ></tr>\n      </ng-template>\n      <ng-template pTemplate="header" let-columns>\n        <tr>\n          <th>{{ \'AbpIdentity::Actions\' | abpLocalization }}</th>\n          <th pResizableColumn (click)="sortOrderIcon.sort(\'name\')">\n            {{ \'AbpIdentity::RoleName\' | abpLocalization }}\n            <abp-sort-order-icon\n              #sortOrderIcon\n              key="name"\n              [(selectedKey)]="sortKey"\n              [(order)]="sortOrder"\n            ></abp-sort-order-icon>\n          </th>\n        </tr>\n      </ng-template>\n      <ng-template pTemplate="body" let-data>\n        <tr>\n          <td class="text-center">\n            <div ngbDropdown container="body" class="d-inline-block">\n              <button\n                class="btn btn-primary btn-sm dropdown-toggle"\n                data-toggle="dropdown"\n                aria-haspopup="true"\n                ngbDropdownToggle\n              >\n                <i class="fa fa-cog mr-1"></i>{{ \'AbpIdentity::Actions\' | abpLocalization }}\n              </button>\n              <div ngbDropdownMenu>\n                <button [abpPermission]="\'AbpIdentity.Roles.Update\'" ngbDropdownItem (click)="onEdit(data.id)">\n                  {{ \'AbpIdentity::Edit\' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]="\'AbpIdentity.Roles.ManagePermissions\'"\n                  ngbDropdownItem\n                  (click)="providerKey = data.name; visiblePermissions = true"\n                >\n                  {{ \'AbpIdentity::Permissions\' | abpLocalization }}\n                </button>\n                <button\n                  [abpPermission]="\'AbpIdentity.Roles.Delete\'"\n                  ngbDropdownItem\n                  (click)="delete(data.id, data.name)"\n                >\n                  {{ \'AbpIdentity::Delete\' | abpLocalization }}\n                </button>\n              </div>\n            </div>\n          </td>\n          <td>{{ data.name }}</td>\n        </tr>\n      </ng-template>\n    </p-table>\n  </div>\n</div>\n\n<abp-modal size="md" [(visible)]="isModalVisible" [busy]="modalBusy">\n  <ng-template #abpHeader>\n    <h3>{{ (selected?.id ? \'AbpIdentity::Edit\' : \'AbpIdentity::NewRole\') | abpLocalization }}</h3>\n  </ng-template>\n\n  <ng-template #abpBody>\n    <form [formGroup]="form" (ngSubmit)="save()">\n      <div class="form-group">\n        <label for="role-name">{{ \'AbpIdentity::RoleName\' | abpLocalization }}</label\n        ><span> * </span>\n        <input autofocus type="text" id="role-name" class="form-control" formControlName="name" />\n      </div>\n\n      <div class="custom-checkbox custom-control mb-2">\n        <input type="checkbox" id="role-is-default" class="custom-control-input" formControlName="isDefault" />\n        <label class="custom-control-label" for="role-is-default">{{\n          \'AbpIdentity::DisplayName:IsDefault\' | abpLocalization\n        }}</label>\n      </div>\n\n      <div class="custom-checkbox custom-control mb-2">\n        <input type="checkbox" id="role-is-public" class="custom-control-input" formControlName="isPublic" />\n        <label class="custom-control-label" for="role-is-public">{{\n          \'AbpIdentity::DisplayName:IsPublic\' | abpLocalization\n        }}</label>\n      </div>\n    </form>\n  </ng-template>\n\n  <ng-template #abpFooter>\n    <button type="button" class="btn btn-secondary" #abpClose>\n      {{ \'AbpIdentity::Cancel\' | abpLocalization }}\n    </button>\n    <abp-button iconClass="fa fa-check" (click)="save()">{{ \'AbpIdentity::Save\' | abpLocalization }}</abp-button>\n  </ng-template>\n</abp-modal>\n\n<abp-permission-management [(visible)]="visiblePermissions" providerName="R" [providerKey]="providerKey">\n</abp-permission-management>\n',
      },
    ],
  },
];
/** @nocollapse */
RolesComponent.ctorParameters = () => [{ type: ConfirmationService }, { type: FormBuilder }, { type: Store }];
RolesComponent.propDecorators = {
  modalContent: [{ type: ViewChild, args: ['modalContent', { static: false }] }],
};
tslib_1.__decorate(
  [Select(IdentityState.getRoles), tslib_1.__metadata('design:type', Observable)],
  RolesComponent.prototype,
  'data$',
  void 0,
);
tslib_1.__decorate(
  [Select(IdentityState.getRolesTotalCount), tslib_1.__metadata('design:type', Observable)],
  RolesComponent.prototype,
  'totalCount$',
  void 0,
);
if (false) {
  /** @type {?} */
  RolesComponent.prototype.data$;
  /** @type {?} */
  RolesComponent.prototype.totalCount$;
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
  RolesComponent.prototype.pageQuery;
  /** @type {?} */
  RolesComponent.prototype.loading;
  /** @type {?} */
  RolesComponent.prototype.modalBusy;
  /** @type {?} */
  RolesComponent.prototype.sortOrder;
  /** @type {?} */
  RolesComponent.prototype.sortKey;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoicm9sZXMuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy5pZGVudGl0eS8iLCJzb3VyY2VzIjpbImxpYi9jb21wb25lbnRzL3JvbGVzL3JvbGVzLmNvbXBvbmVudC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUNBLE9BQU8sRUFBRSxtQkFBbUIsRUFBVyxNQUFNLHNCQUFzQixDQUFDO0FBQ3BFLE9BQU8sRUFBRSxTQUFTLEVBQUUsV0FBVyxFQUFFLFNBQVMsRUFBVSxNQUFNLGVBQWUsQ0FBQztBQUMxRSxPQUFPLEVBQUUsV0FBVyxFQUFhLFVBQVUsRUFBRSxXQUFXLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUNqRixPQUFPLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxNQUFNLGFBQWEsQ0FBQztBQUM1QyxPQUFPLEVBQUUsVUFBVSxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2xDLE9BQU8sRUFBRSxRQUFRLEVBQUUsS0FBSyxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFDakQsT0FBTyxFQUFFLFVBQVUsRUFBRSxVQUFVLEVBQUUsV0FBVyxFQUFFLFFBQVEsRUFBRSxVQUFVLEVBQUUsTUFBTSxnQ0FBZ0MsQ0FBQztBQUUzRyxPQUFPLEVBQUUsYUFBYSxFQUFFLE1BQU0sNkJBQTZCLENBQUM7QUFNNUQsTUFBTSxPQUFPLGNBQWM7Ozs7OztJQThCekIsWUFBb0IsbUJBQXdDLEVBQVUsRUFBZSxFQUFVLEtBQVk7UUFBdkYsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQUFVLE9BQUUsR0FBRixFQUFFLENBQWE7UUFBVSxVQUFLLEdBQUwsS0FBSyxDQUFPO1FBakIzRyx1QkFBa0IsR0FBRyxLQUFLLENBQUM7UUFJM0IsY0FBUyxHQUF3QixFQUFFLENBQUM7UUFFcEMsWUFBTyxHQUFHLEtBQUssQ0FBQztRQUVoQixjQUFTLEdBQUcsS0FBSyxDQUFDO1FBRWxCLGNBQVMsR0FBRyxFQUFFLENBQUM7UUFFZixZQUFPLEdBQUcsRUFB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ZSc7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlLCBUb2FzdGVyIH0gZnJvbSAnQGFicC9uZy50aGVtZS5zaGFyZWQnO1xuaW1wb3J0IHsgQ29tcG9uZW50LCBUZW1wbGF0ZVJlZiwgVmlld0NoaWxkLCBPbkluaXQgfSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IEZvcm1CdWlsZGVyLCBGb3JtR3JvdXAsIFZhbGlkYXRvcnMsIEZvcm1Db250cm9sIH0gZnJvbSAnQGFuZ3VsYXIvZm9ybXMnO1xuaW1wb3J0IHsgU2VsZWN0LCBTdG9yZSB9IGZyb20gJ0BuZ3hzL3N0b3JlJztcbmltcG9ydCB7IE9ic2VydmFibGUgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGZpbmFsaXplLCBwbHVjayB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IENyZWF0ZVJvbGUsIERlbGV0ZVJvbGUsIEdldFJvbGVCeUlkLCBHZXRSb2xlcywgVXBkYXRlUm9sZSB9IGZyb20gJy4uLy4uL2FjdGlvbnMvaWRlbnRpdHkuYWN0aW9ucyc7XG5pbXBvcnQgeyBJZGVudGl0eSB9IGZyb20gJy4uLy4uL21vZGVscy9pZGVudGl0eSc7XG5pbXBvcnQgeyBJZGVudGl0eVN0YXRlIH0gZnJvbSAnLi4vLi4vc3RhdGVzL2lkZW50aXR5LnN0YXRlJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLXJvbGVzJyxcbiAgdGVtcGxhdGVVcmw6ICcuL3JvbGVzLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgUm9sZXNDb21wb25lbnQgaW1wbGVtZW50cyBPbkluaXQge1xuICBAU2VsZWN0KElkZW50aXR5U3RhdGUuZ2V0Um9sZXMpXG4gIGRhdGEkOiBPYnNlcnZhYmxlPElkZW50aXR5LlJvbGVJdGVtW10+O1xuXG4gIEBTZWxlY3QoSWRlbnRpdHlTdGF0ZS5nZXRSb2xlc1RvdGFsQ291bnQpXG4gIHRvdGFsQ291bnQkOiBPYnNlcnZhYmxlPG51bWJlcj47XG5cbiAgZm9ybTogRm9ybUdyb3VwO1xuXG4gIHNlbGVjdGVkOiBJZGVudGl0eS5Sb2xlSXRlbTtcblxuICBpc01vZGFsVmlzaWJsZTogYm9vbGVhbjtcblxuICB2aXNpYmxlUGVybWlzc2lvbnMgPSBmYWxzZTtcblxuICBwcm92aWRlcktleTogc3RyaW5nO1xuXG4gIHBhZ2VRdWVyeTogQUJQLlBhZ2VRdWVyeVBhcmFtcyA9IHt9O1xuXG4gIGxvYWRpbmcgPSBmYWxzZTtcblxuICBtb2RhbEJ1c3kgPSBmYWxzZTtcblxuICBzb3J0T3JkZXIgPSAnJztcblxuICBzb3J0S2V5ID0gJyc7XG5cbiAgQFZpZXdDaGlsZCgnbW9kYWxDb250ZW50JywgeyBzdGF0aWM6IGZhbHNlIH0pXG4gIG1vZGFsQ29udGVudDogVGVtcGxhdGVSZWY8YW55PjtcblxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UsIHByaXZhdGUgZmI6IEZvcm1CdWlsZGVyLCBwcml2YXRlIHN0b3JlOiBTdG9yZSkge31cblxuICBuZ09uSW5pdCgpIHtcbiAgICB0aGlzLmdldCgpO1xuICB9XG5cbiAgY3JlYXRlRm9ybSgpIHtcbiAgICB0aGlzLmZvcm0gPSB0aGlzLmZiLmdyb3VwKHtcbiAgICAgIG5hbWU6IG5ldyBGb3JtQ29udHJvbCh7IHZhbHVlOiB0aGlzLnNlbGVjdGVkLm5hbWUgfHwgJycsIGRpc2FibGVkOiB0aGlzLnNlbGVjdGVkLmlzU3RhdGljIH0sIFtcbiAgICAgICAgVmFsaWRhdG9ycy5yZXF1aXJlZCxcbiAgICAgICAgVmFsaWRhdG9ycy5tYXhMZW5ndGgoMjU2KSxcbiAgICAgIF0pLFxuICAgICAgaXNEZWZhdWx0OiBbdGhpcy5zZWxlY3RlZC5pc0RlZmF1bHQgfHwgZmFsc2VdLFxuICAgICAgaXNQdWJsaWM6IFt0aGlzLnNlbGVjdGVkLmlzUHVibGljIHx8IGZhbHNlXSxcbiAgICB9KTtcbiAgfVxuXG4gIG9wZW5Nb2RhbCgpIHtcbiAgICB0aGlzLmNyZWF0ZUZvcm0oKTtcbiAgICB0aGlzLmlzTW9kYWxWaXNpYmxlID0gdHJ1ZTtcbiAgfVxuXG4gIG9uQWRkKCkge1xuICAgIHRoaXMuc2VsZWN0ZWQgPSB7fSBhcyBJZGVudGl0eS5Sb2xlSXRlbTtcbiAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICB9XG5cbiAgb25FZGl0KGlkOiBzdHJpbmcpIHtcbiAgICB0aGlzLnN0b3JlXG4gICAgICAuZGlzcGF0Y2gobmV3IEdldFJvbGVCeUlkKGlkKSlcbiAgICAgIC5waXBlKHBsdWNrKCdJZGVudGl0eVN0YXRlJywgJ3NlbGVjdGVkUm9sZScpKVxuICAgICAgLnN1YnNjcmliZShzZWxlY3RlZFJvbGUgPT4ge1xuICAgICAgICB0aGlzLnNlbGVjdGVkID0gc2VsZWN0ZWRSb2xlO1xuICAgICAgICB0aGlzLm9wZW5Nb2RhbCgpO1xuICAgICAgfSk7XG4gIH1cblxuICBzYXZlKCkge1xuICAgIGlmICghdGhpcy5mb3JtLnZhbGlkKSByZXR1cm47XG4gICAgdGhpcy5tb2RhbEJ1c3kgPSB0cnVlO1xuXG4gICAgdGhpcy5zdG9yZVxuICAgICAgLmRpc3BhdGNoKFxuICAgICAgICB0aGlzLnNlbGVjdGVkLmlkXG4gICAgICAgICAgPyBuZXcgVXBkYXRlUm9sZSh7IC4uLnRoaXMuZm9ybS52YWx1ZSwgaWQ6IHRoaXMuc2VsZWN0ZWQuaWQgfSlcbiAgICAgICAgICA6IG5ldyBDcmVhdGVSb2xlKHRoaXMuZm9ybS52YWx1ZSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgdGhpcy5tb2RhbEJ1c3kgPSBmYWxzZTtcbiAgICAgICAgdGhpcy5pc01vZGFsVmlzaWJsZSA9IGZhbHNlO1xuICAgICAgfSk7XG4gIH1cblxuICBkZWxldGUoaWQ6IHN0cmluZywgbmFtZTogc3RyaW5nKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAud2FybignQWJwSWRlbnRpdHk6OlJvbGVEZWxldGlvbkNvbmZpcm1hdGlvbk1lc3NhZ2UnLCAnQWJwSWRlbnRpdHk6OkFyZVlvdVN1cmUnLCB7XG4gICAgICAgIG1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXM6IFtuYW1lXSxcbiAgICAgIH0pXG4gICAgICAuc3Vic2NyaWJlKChzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSA9PiB7XG4gICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICB0aGlzLnN0b3JlLmRpc3BhdGNoKG5ldyBEZWxldGVSb2xlKGlkKSk7XG4gICAgICAgIH1cbiAgICAgIH0pO1xuICB9XG5cbiAgb25QYWdlQ2hhbmdlKGRhdGEpIHtcbiAgICB0aGlzLnBhZ2VRdWVyeS5za2lwQ291bnQgPSBkYXRhLmZpcnN0O1xuICAgIHRoaXMucGFnZVF1ZXJ5Lm1heFJlc3VsdENvdW50ID0gZGF0YS5yb3dzO1xuXG4gICAgdGhpcy5nZXQoKTtcbiAgfVxuXG4gIGdldCgpIHtcbiAgICB0aGlzLmxvYWRpbmcgPSB0cnVlO1xuICAgIHRoaXMuc3RvcmVcbiAgICAgIC5kaXNwYXRjaChuZXcgR2V0Um9sZXModGhpcy5wYWdlUXVlcnkpKVxuICAgICAgLnBpcGUoZmluYWxpemUoKCkgPT4gKHRoaXMubG9hZGluZyA9IGZhbHNlKSkpXG4gICAgICAuc3Vic2NyaWJlKCk7XG4gIH1cbn1cbiJdfQ==
