# How to Replace PermissionManagementComponent

![Permission management modal](./images/permission-management-modal.png)

Run the following command in `angular` folder to create a new component called `PermissionManagementComponent`.

```bash
yarn ng generate component permission-management --entryComponent --inlineStyle

# You don't need the --entryComponent option in Angular 9
```

Open the generated `permission-management.component.ts` in `src/app/permission-management` folder and replace the content with the following:

```js
import {
  Component,
  EventEmitter,
  Input,
  Output,
  Renderer2,
  TrackByFunction,
  Inject,
  Optional,
} from '@angular/core';
import { ReplaceableComponents } from '@abp/ng.core';
import { Select, Store } from '@ngxs/store';
import { Observable } from 'rxjs';
import { finalize, map, pluck, take, tap } from 'rxjs/operators';
import {
  GetPermissions,
  UpdatePermissions,
  PermissionManagement,
  PermissionManagementState,
} from '@abp/ng.permission-management';

type PermissionWithMargin = PermissionManagement.Permission & {
  margin: number;
};

@Component({
  selector: 'app-permission-management',
  templateUrl: './permission-management.component.html',
  styles: [
    `
      .overflow-scroll {
        max-height: 70vh;
        overflow-y: scroll;
      }
    `,
  ],
})
export class PermissionManagementComponent
  implements
    PermissionManagement.PermissionManagementComponentInputs,
    PermissionManagement.PermissionManagementComponentOutputs {
  protected _providerName: string;
  @Input()
  get providerName(): string {
    if (this.replaceableData) return this.replaceableData.inputs.providerName;

    return this._providerName;
  }

  set providerName(value: string) {
    this._providerName = value;
  }

  protected _providerKey: string;
  @Input()
  get providerKey(): string {
    if (this.replaceableData) return this.replaceableData.inputs.providerKey;

    return this._providerKey;
  }

  set providerKey(value: string) {
    this._providerKey = value;
  }

  protected _hideBadges = false;
  @Input()
  get hideBadges(): boolean {
    if (this.replaceableData) return this.replaceableData.inputs.hideBadges;

    return this._hideBadges;
  }

  set hideBadges(value: boolean) {
    this._hideBadges = value;
  }

  protected _visible = false;
  @Input()
  get visible(): boolean {
    return this._visible;
  }

  set visible(value: boolean) {
    if (value === this._visible) return;

    if (value) {
      this.openModal().subscribe(() => {
        this._visible = true;
        this.visibleChange.emit(true);
        if (this.replaceableData) this.replaceableData.outputs.visibleChange(true);
      });
    } else {
      this.selectedGroup = null;
      this._visible = false;
      this.visibleChange.emit(false);
      if (this.replaceableData) this.replaceableData.outputs.visibleChange(false);
    }
  }

  @Output() readonly visibleChange = new EventEmitter<boolean>();

  @Select(PermissionManagementState.getPermissionGroups)
  groups$: Observable<PermissionManagement.Group[]>;

  @Select(PermissionManagementState.getEntityDisplayName)
  entityName$: Observable<string>;

  selectedGroup: PermissionManagement.Group;

  permissions: PermissionManagement.Permission[] = [];

  selectThisTab = false;

  selectAllTab = false;

  modalBusy = false;

  trackByFn: TrackByFunction<PermissionManagement.Group> = (_, item) => item.name;

  get selectedGroupPermissions$(): Observable<PermissionWithMargin[]> {
    return this.groups$.pipe(
      map((groups) =>
        this.selectedGroup
          ? groups.find((group) => group.name === this.selectedGroup.name).permissions
          : []
      ),
      map<PermissionManagement.Permission[], PermissionWithMargin[]>((permissions) =>
        permissions.map(
          (permission) =>
            (({
              ...permission,
              margin: findMargin(permissions, permission),
              isGranted: this.permissions.find((per) => per.name === permission.name).isGranted,
            } as any) as PermissionWithMargin)
        )
      )
    );
  }

  get isVisible(): boolean {
    if (!this.replaceableData) return this.visible;

    return this.replaceableData.inputs.visible;
  }

  constructor(
    @Optional()
    @Inject('REPLACEABLE_DATA')
    public replaceableData: ReplaceableComponents.ReplaceableTemplateData<
      PermissionManagement.PermissionManagementComponentInputs,
      PermissionManagement.PermissionManagementComponentOutputs
    >,
    private store: Store
  ) {}

  getChecked(name: string) {
    return (this.permissions.find((per) => per.name === name) || { isGranted: false }).isGranted;
  }

  isGrantedByOtherProviderName(grantedProviders: PermissionManagement.GrantedProvider[]): boolean {
    if (grantedProviders.length) {
      return grantedProviders.findIndex((p) => p.providerName !== this.providerName) > -1;
    }
    return false;
  }

  onClickCheckbox(clickedPermission: PermissionManagement.Permission, value) {
    if (
      clickedPermission.isGranted &&
      this.isGrantedByOtherProviderName(clickedPermission.grantedProviders)
    )
      return;

    setTimeout(() => {
      this.permissions = this.permissions.map((per) => {
        if (clickedPermission.name === per.name) {
          return { ...per, isGranted: !per.isGranted };
        } else if (clickedPermission.name === per.parentName && clickedPermission.isGranted) {
          return { ...per, isGranted: false };
        } else if (clickedPermission.parentName === per.name && !clickedPermission.isGranted) {
          return { ...per, isGranted: true };
        }

        return per;
      });

      this.setTabCheckboxState();
      this.setGrantCheckboxState();
    }, 0);
  }

  setTabCheckboxState() {
    this.selectedGroupPermissions$.pipe(take(1)).subscribe((permissions) => {
      const selectedPermissions = permissions.filter((per) => per.isGranted);
      const element = document.querySelector('#select-all-in-this-tabs') as any;

      if (selectedPermissions.length === permissions.length) {
        element.indeterminate = false;
        this.selectThisTab = true;
      } else if (selectedPermissions.length === 0) {
        element.indeterminate = false;
        this.selectThisTab = false;
      } else {
        element.indeterminate = true;
      }
    });
  }

  setGrantCheckboxState() {
    const selectedAllPermissions = this.permissions.filter((per) => per.isGranted);
    const checkboxElement = document.querySelector('#select-all-in-all-tabs') as any;

    if (selectedAllPermissions.length === this.permissions.length) {
      checkboxElement.indeterminate = false;
      this.selectAllTab = true;
    } else if (selectedAllPermissions.length === 0) {
      checkboxElement.indeterminate = false;
      this.selectAllTab = false;
    } else {
      checkboxElement.indeterminate = true;
    }
  }

  onClickSelectThisTab() {
    this.selectedGroupPermissions$.pipe(take(1)).subscribe((permissions) => {
      permissions.forEach((permission) => {
        if (permission.isGranted && this.isGrantedByOtherProviderName(permission.grantedProviders))
          return;

        const index = this.permissions.findIndex((per) => per.name === permission.name);

        this.permissions = [
          ...this.permissions.slice(0, index),
          { ...this.permissions[index], isGranted: !this.selectThisTab },
          ...this.permissions.slice(index + 1),
        ];
      });
    });

    this.setGrantCheckboxState();
  }

  onClickSelectAll() {
    this.permissions = this.permissions.map((permission) => ({
      ...permission,
      isGranted:
        this.isGrantedByOtherProviderName(permission.grantedProviders) || !this.selectAllTab,
    }));

    this.selectThisTab = !this.selectAllTab;
  }

  onChangeGroup(group: PermissionManagement.Group) {
    this.selectedGroup = group;
    this.setTabCheckboxState();
  }

  submit() {
    this.modalBusy = true;
    const unchangedPermissions = getPermissions(
      this.store.selectSnapshot(PermissionManagementState.getPermissionGroups)
    );

    const changedPermissions: PermissionManagement.MinimumPermission[] = this.permissions
      .filter((per) =>
        unchangedPermissions.find((unchanged) => unchanged.name === per.name).isGranted ===
        per.isGranted
          ? false
          : true
      )
      .map(({ name, isGranted }) => ({ name, isGranted }));

    if (changedPermissions.length) {
      this.store
        .dispatch(
          new UpdatePermissions({
            providerKey: this.providerKey,
            providerName: this.providerName,
            permissions: changedPermissions,
          })
        )
        .pipe(finalize(() => (this.modalBusy = false)))
        .subscribe(() => {
          this.visible = false;
        });
    } else {
      this.modalBusy = false;
      this.visible = false;
    }
  }

  openModal() {
    if (!this.providerKey || !this.providerName) {
      throw new Error('Provider Key and Provider Name are required.');
    }

    return this.store
      .dispatch(
        new GetPermissions({
          providerKey: this.providerKey,
          providerName: this.providerName,
        })
      )
      .pipe(
        pluck('PermissionManagementState', 'permissionRes'),
        tap((permissionRes: PermissionManagement.Response) => {
          this.selectedGroup = permissionRes.groups[0];
          this.permissions = getPermissions(permissionRes.groups);
        })
      );
  }

  initModal() {
    this.setTabCheckboxState();
    this.setGrantCheckboxState();
  }

  onVisibleChange(visible: boolean) {
    this.visible = visible;

    if (this.replaceableData) {
      this.replaceableData.inputs.visible = visible;
      this.replaceableData.outputs.visibleChange(visible);
    }
  }
}

function findMargin(
  permissions: PermissionManagement.Permission[],
  permission: PermissionManagement.Permission
) {
  const parentPermission = permissions.find((per) => per.name === permission.parentName);

  if (parentPermission && parentPermission.parentName) {
    let margin = 20;
    return (margin += findMargin(permissions, parentPermission));
  }

  return parentPermission ? 20 : 0;
}

function getPermissions(groups: PermissionManagement.Group[]): PermissionManagement.Permission[] {
  return groups.reduce((acc, val) => [...acc, ...val.permissions], []);
}
```

Open the generated `permission-management.component.html` in `src/app/permission-management` folder and replace the content with the below:

```html
<abp-modal
  [visible]="isVisible"
  (visibleChange)="onVisibleChange($event)"
  (init)="initModal()"
  [busy]="modalBusy"
>
  <ng-container *ngIf="{ entityName: entityName$ | async } as data">
    <ng-template #abpHeader>
      <h4>
        {%{{{ 'AbpPermissionManagement::Permissions' | abpLocalization }}}%} - {%{{{ data.entityName }}}%}
      </h4>
    </ng-template>
    <ng-template #abpBody>
      <div class="custom-checkbox custom-control mb-2">
        <input
          type="checkbox"
          id="select-all-in-all-tabs"
          name="select-all-in-all-tabs"
          class="custom-control-input"
          [(ngModel)]="selectAllTab"
          (click)="onClickSelectAll()"
        />
        <label class="custom-control-label" for="select-all-in-all-tabs">{%{{{
          'AbpPermissionManagement::SelectAllInAllTabs' | abpLocalization
        }}}%}</label>
      </div>

      <hr class="mt-2 mb-2" />
      <div class="row">
        <div class="overflow-scroll col-md-4">
          <ul class="nav nav-pills flex-column">
            <li *ngFor="let group of groups$ | async; trackBy: trackByFn" class="nav-item">
              <a
                class="nav-link pointer"
                [class.active]="selectedGroup?.name === group?.name"
                (click)="onChangeGroup(group)"
                >{%{{{ group?.displayName }}}%}</a
              >
            </li>
          </ul>
        </div>
        <div class="col-md-8 overflow-scroll">
          <h4>{%{{{ selectedGroup?.displayName }}}%}</h4>
          <hr class="mt-2 mb-3" />
          <div class="pl-1 pt-1">
            <div class="custom-checkbox custom-control mb-2">
              <input
                type="checkbox"
                id="select-all-in-this-tabs"
                name="select-all-in-this-tabs"
                class="custom-control-input"
                [(ngModel)]="selectThisTab"
                (click)="onClickSelectThisTab()"
              />
              <label class="custom-control-label" for="select-all-in-this-tabs">{%{{{
                'AbpPermissionManagement::SelectAllInThisTab' | abpLocalization
              }}}%}</label>
            </div>
            <hr class="mb-3" />
            <div
              *ngFor="
                let permission of selectedGroupPermissions$ | async;
                let i = index;
                trackBy: trackByFn
              "
              [style.margin-left]="permission.margin + 'px'"
              class="custom-checkbox custom-control mb-2"
            >
              <input
                #permissionCheckbox
                type="checkbox"
                [checked]="getChecked(permission.name)"
                [value]="getChecked(permission.name)"
                [attr.id]="permission.name"
                class="custom-control-input"
                [disabled]="isGrantedByOtherProviderName(permission.grantedProviders)"
              />
              <label
                class="custom-control-label"
                [attr.for]="permission.name"
                (click)="onClickCheckbox(permission, permissionCheckbox.value)"
                >{%{{{ permission.displayName }}}%}
                <ng-container *ngIf="!hideBadges">
                  <span
                    *ngFor="let provider of permission.grantedProviders"
                    class="badge badge-light"
                    >{%{{{ provider.providerName }}}%}: {%{{{ provider.providerKey }}}%}</span
                  >
                </ng-container>
              </label>
            </div>
          </div>
        </div>
      </div>
    </ng-template>
    <ng-template #abpFooter>
      <button type="button" class="btn btn-secondary" abpClose>
        {%{{{ 'AbpIdentity::Cancel' | abpLocalization }}}%}
      </button>
      <abp-button iconClass="fa fa-check" (click)="submit()">{%{{{
        'AbpIdentity::Save' | abpLocalization
      }}}%}</abp-button>
    </ng-template>
  </ng-container>
</abp-modal>
```

Open `app.component.ts` in `src/app` folder and modify it as shown below:

```js
import { ReplaceableComponentsService } from '@abp/ng.core';
import { ePermissionManagementComponents } from '@abp/ng.permission-management';
import { Component, OnInit } from '@angular/core';
import { PermissionManagementComponent } from './permission-management/permission-management.component';

//...
export class AppComponent implements OnInit {
  constructor(private replaceableComponents: ReplaceableComponentsService) {} // injected ReplaceableComponentsService

  ngOnInit() {
    this.replaceableComponents.add({
        component: PermissionManagementComponent,
        key: ePermissionManagementComponents.PermissionManagement,
      });
  }
}
```

## See Also

- [Component Replacement](./Component-Replacement.md)
