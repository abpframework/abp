import { ListService, LocalizationPipe } from '@abp/ng.core';
import {
    ButtonComponent,
    Confirmation,
    ConfirmationService,
    ModalCloseDirective,
    ModalComponent,
    ToasterService,
} from '@abp/ng.theme.shared';
import {
    PermissionsService,
    ResourcePermissionGrantInfoDto,
} from '@abp/ng.permission-management/proxy';
import {
    Component,
    inject,
    input,
    model,
    OnInit,
    effect,
} from '@angular/core';
import { finalize, switchMap, of } from 'rxjs';
import { ResourcePermissionStateService } from '../../services/resource-permission-state.service';
import { ResourcePermissionListComponent } from './resource-permission-list/resource-permission-list.component';
import { ResourcePermissionFormComponent } from './resource-permission-form/resource-permission-form.component';

import { eResourcePermissionViewModes } from '../../enums/view-modes';

@Component({
    selector: 'abp-resource-permission-management',
    templateUrl: './resource-permission-management.component.html',
    exportAs: 'abpResourcePermissionManagement',
    providers: [ResourcePermissionStateService, ListService],
    imports: [
        ModalComponent,
        LocalizationPipe,
        ButtonComponent,
        ModalCloseDirective,
        ResourcePermissionListComponent,
        ResourcePermissionFormComponent,
    ],
})
export class ResourcePermissionManagementComponent implements OnInit {
    readonly eResourcePermissionViewModes = eResourcePermissionViewModes;

    protected readonly service = inject(PermissionsService);
    protected readonly toasterService = inject(ToasterService);
    protected readonly confirmationService = inject(ConfirmationService);
    protected readonly state = inject(ResourcePermissionStateService);
    private readonly list = inject(ListService);

    readonly resourceName = input.required<string>();
    readonly resourceKey = input.required<string>();
    readonly resourceDisplayName = input<string>();

    readonly visible = model<boolean>(false);

    private previousVisible = false;

    constructor() {
        effect(() => {
            this.state.resourceName.set(this.resourceName());
            this.state.resourceKey.set(this.resourceKey());
            this.state.resourceDisplayName.set(this.resourceDisplayName());
        });

        effect(() => {
            const isVisible = this.visible();
            if (isVisible && !this.previousVisible) {
                this.openModal();
            } else if (!isVisible && this.previousVisible) {
                this.state.reset();
            }
            this.previousVisible = isVisible;
        });
    }

    ngOnInit() {
        this.list.maxResultCount = 10;

        this.list.hookToQuery(query => {
            const allData = this.state.allResourcePermissions();
            const skipCount = query.skipCount || 0;
            const maxResultCount = query.maxResultCount || 10;

            const paginatedData = allData.slice(skipCount, skipCount + maxResultCount);

            return of({
                items: paginatedData,
                totalCount: allData.length
            });
        }).subscribe(result => {
            this.state.resourcePermissions.set(result.items);
            this.state.totalCount.set(result.totalCount);
        });
    }

    openModal() {
        this.state.modalBusy.set(true);

        this.service.getResource(this.resourceName(), this.resourceKey()).pipe(
            switchMap(permRes => {
                this.state.setResourceData(permRes.permissions || []);
                this.list.get();
                return this.service.getResourceProviderKeyLookupServices(this.resourceName());
            }),
            switchMap(providerRes => {
                this.state.setProviders(providerRes.providers || []);
                return this.service.getResourceDefinitions(this.resourceName());
            }),
            finalize(() => this.state.modalBusy.set(false))
        ).subscribe({
            next: defRes => {
                this.state.setDefinitions(defRes.permissions || []);
            },
            error: () => {
                this.toasterService.error('AbpPermissionManagement::ErrorLoadingPermissions');
            }
        });
    }

    onAddClicked() {
        this.state.goToAddMode();
    }

    onEditClicked(grant: ResourcePermissionGrantInfoDto) {
        this.state.prepareEditMode(grant);
        this.state.modalBusy.set(true);

        this.service.getResourceByProvider(
            this.resourceName(),
            this.resourceKey(),
            grant.providerName || '',
            grant.providerKey || ''
        ).pipe(
            finalize(() => this.state.modalBusy.set(false))
        ).subscribe({
            next: res => {
                this.state.setEditModePermissions(res.permissions || []);
            }
        });
    }

    onDeleteClicked(grant: ResourcePermissionGrantInfoDto) {
        this.confirmationService
            .warn(
                'AbpPermissionManagement::PermissionDeletionConfirmationMessage',
                'AbpPermissionManagement::AreYouSure',
                {
                    messageLocalizationParams: [grant.providerKey || ''],
                }
            )
            .subscribe((status: Confirmation.Status) => {
                if (status === Confirmation.Status.confirm) {
                    this.state.modalBusy.set(true);
                    this.service.deleteResource(
                        this.resourceName(),
                        this.resourceKey(),
                        grant.providerName || '',
                        grant.providerKey || ''
                    ).pipe(
                        switchMap(() => this.service.getResource(this.resourceName(), this.resourceKey())),
                        finalize(() => this.state.modalBusy.set(false))
                    ).subscribe({
                        next: res => {
                            this.state.setResourceData(res.permissions || []);
                            this.list.get();
                            this.toasterService.success('AbpUi::SuccessfullyDeleted');
                        }
                    });
                }
            });
    }

    savePermission() {
        const isEdit = this.state.isEditMode();
        const providerName = isEdit ? this.state.editProviderName() : this.state.selectedProviderName();
        const providerKey = isEdit ? this.state.editProviderKey() : this.state.selectedProviderKey();

        if (!isEdit && !this.state.canSave()) {
            this.toasterService.warn('AbpPermissionManagement::PleaseSelectProviderAndPermissions');
            return;
        }

        this.state.modalBusy.set(true);
        this.service.updateResource(
            this.resourceName(),
            this.resourceKey(),
            {
                providerName,
                providerKey,
                permissions: this.state.selectedPermissions()
            }
        ).pipe(
            switchMap(() => this.service.getResource(this.resourceName(), this.resourceKey())),
            finalize(() => this.state.modalBusy.set(false))
        ).subscribe({
            next: res => {
                this.state.setResourceData(res.permissions || []);
                this.list.get();
                this.toasterService.success('AbpUi::SavedSuccessfully');
                this.state.goToListMode();
            }
        });
    }
}
