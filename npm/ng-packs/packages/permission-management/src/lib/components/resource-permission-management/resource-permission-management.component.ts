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
    ResourceProviderDto,
    SearchProviderKeyInfo,
    ResourcePermissionDefinitionDto,
    ResourcePermissionWithProdiverGrantInfoDto,
} from '@abp/ng.permission-management/proxy';
import {
    ExtensibleTableComponent,
    EXTENSIONS_IDENTIFIER,
} from '@abp/ng.components/extensible';
import {
    Component,
    EventEmitter,
    inject,
    Input,
    Output,
    signal,
    computed,
    OnInit,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { finalize, switchMap, debounceTime, Subject, distinctUntilChanged, of } from 'rxjs';
import { ePermissionManagementComponents } from '../../enums/components';
import { configureResourcePermissionExtensions } from '../../services/extensions.service';

type ViewMode = 'list' | 'add' | 'edit';

@Component({
    selector: 'abp-resource-permission-management',
    templateUrl: './resource-permission-management.component.html',
    exportAs: 'abpResourcePermissionManagement',
    providers: [
        ListService,
        {
            provide: EXTENSIONS_IDENTIFIER,
            useValue: ePermissionManagementComponents.ResourcePermissions,
        },
    ],
    imports: [
        FormsModule,
        ModalComponent,
        LocalizationPipe,
        ButtonComponent,
        ModalCloseDirective,
        ExtensibleTableComponent,
    ],
})
export class ResourcePermissionManagementComponent implements OnInit {
    protected readonly service = inject(PermissionsService);
    protected readonly toasterService = inject(ToasterService);
    protected readonly confirmationService = inject(ConfirmationService);
    readonly list = inject(ListService);

    @Input() resourceName!: string;
    @Input() resourceKey!: string;
    @Input() resourceDisplayName?: string;

    protected _visible = false;

    @Input()
    get visible(): boolean {
        return this._visible;
    }

    set visible(value: boolean) {
        if (value === this._visible) return;

        if (value) {
            this.openModal();
        } else {
            this.resetState();
        }
        this._visible = value;
        this.visibleChange.emit(value);
    }

    @Output() readonly visibleChange = new EventEmitter<boolean>();

    viewMode = signal<ViewMode>('list');
    modalBusy = signal(false);
    hasResourcePermission = signal(false);
    hasProviderKeyLookupService = signal(false);

    allResourcePermissions = signal<ResourcePermissionGrantInfoDto[]>([]); // All data for client-side pagination
    resourcePermissions = signal<ResourcePermissionGrantInfoDto[]>([]); // Paginated data for table
    totalCount = signal(0);
    providers = signal<ResourceProviderDto[]>([]);
    permissionDefinitions = signal<ResourcePermissionDefinitionDto[]>([]);
    searchResults = signal<SearchProviderKeyInfo[]>([]);
    permissionsWithProvider = signal<ResourcePermissionWithProdiverGrantInfoDto[]>([]);

    selectedProviderName = signal<string>('');
    selectedProviderKey = signal<string>('');
    searchFilter = signal('');
    selectedPermissions = signal<string[]>([]);

    editProviderName = signal<string>('');
    editProviderKey = signal<string>('');

    showDropdown = signal(false);

    private searchSubject = new Subject<string>();

    constructor() {
        configureResourcePermissionExtensions();

        this.searchSubject.pipe(
            debounceTime(300),
            distinctUntilChanged()
        ).subscribe(filter => {
            this.performSearch(filter);
        });
    }

    ngOnInit() {
        this.list.maxResultCount = 10;

        this.list.hookToQuery(query => {
            const allData = this.allResourcePermissions();
            const skipCount = query.skipCount || 0;
            const maxResultCount = query.maxResultCount || 10;

            const paginatedData = allData.slice(skipCount, skipCount + maxResultCount);

            return of({
                items: paginatedData,
                totalCount: allData.length
            });
        }).subscribe(result => {
            this.resourcePermissions.set(result.items);
            this.totalCount.set(result.totalCount);
        });
    }

    openModal() {
        this.modalBusy.set(true);

        this.service.getResource(this.resourceName, this.resourceKey).pipe(
            switchMap(permRes => {
                this.allResourcePermissions.set(permRes.permissions || []);
                this.totalCount.set(permRes.permissions?.length || 0);
                this.list.get();
                return this.service.getResourceProviderKeyLookupServices(this.resourceName);
            }),
            switchMap(providerRes => {
                this.providers.set(providerRes.providers || []);
                this.hasProviderKeyLookupService.set((providerRes.providers?.length || 0) > 0);
                if (providerRes.providers?.length) {
                    this.selectedProviderName.set(providerRes.providers[0].name || '');
                }
                return this.service.getResourceDefinitions(this.resourceName);
            }),
            finalize(() => this.modalBusy.set(false))
        ).subscribe({
            next: defRes => {
                this.permissionDefinitions.set(defRes.permissions || []);
                this.hasResourcePermission.set((defRes.permissions?.length || 0) > 0);
            },
            error: () => {
                this.toasterService.error('AbpPermissionManagement::ErrorLoadingPermissions');
            }
        });
    }

    resetState() {
        this.viewMode.set('list');
        this.allResourcePermissions.set([]);
        this.resourcePermissions.set([]);
        this.totalCount.set(0);
        this.selectedProviderName.set('');
        this.selectedProviderKey.set('');
        this.searchFilter.set('');
        this.selectedPermissions.set([]);
        this.searchResults.set([]);
    }

    goToAddMode() {
        this.viewMode.set('add');
        this.selectedPermissions.set([]);
        this.selectedProviderKey.set('');
        this.searchResults.set([]);
    }

    goToEditMode(grant: ResourcePermissionGrantInfoDto) {
        this.editProviderName.set(grant.providerName || '');
        this.editProviderKey.set(grant.providerKey || '');
        this.modalBusy.set(true);

        this.service.getResourceByProvider(
            this.resourceName,
            this.resourceKey,
            grant.providerName || '',
            grant.providerKey || ''
        ).pipe(
            finalize(() => this.modalBusy.set(false))
        ).subscribe({
            next: res => {
                this.permissionsWithProvider.set(res.permissions || []);
                this.selectedPermissions.set(
                    (res.permissions || []).filter(p => p.isGranted).map(p => p.name || '')
                );
                this.viewMode.set('edit');
            }
        });
    }

    goToListMode() {
        this.viewMode.set('list');
        this.selectedPermissions.set([]);
    }

    onProviderChange(providerName: string) {
        this.selectedProviderName.set(providerName);
        this.selectedProviderKey.set('');
        this.searchResults.set([]);
        this.searchFilter.set('');
    }

    onSearchInput(filter: string) {
        this.searchFilter.set(filter);
        this.showDropdown.set(true);
        this.searchSubject.next(filter);
    }

    onSearchFocus() {
        this.showDropdown.set(true);
        this.loadProviderKeys(this.searchFilter() || '');
    }

    onSearchBlur() {
        setTimeout(() => {
            this.showDropdown.set(false);
        }, 200);
    }

    private loadProviderKeys(filter: string) {
        if (!this.selectedProviderName()) return;

        this.service.searchResourceProviderKey(
            this.resourceName,
            this.selectedProviderName(),
            filter,
            1
        ).subscribe(res => {
            this.searchResults.set(res.keys || []);
        });
    }

    private performSearch(filter: string) {
        this.loadProviderKeys(filter);
    }

    selectProviderKey(key: SearchProviderKeyInfo) {
        this.selectedProviderKey.set(key.providerKey || '');
        this.searchFilter.set(key.providerDisplayName || key.providerKey || '');
        this.searchResults.set([]);
        this.showDropdown.set(false);
    }

    togglePermission(permissionName: string) {
        const current = this.selectedPermissions();
        if (current.includes(permissionName)) {
            this.selectedPermissions.set(current.filter(p => p !== permissionName));
        } else {
            this.selectedPermissions.set([...current, permissionName]);
        }
    }

    toggleAllPermissions(selectAll: boolean) {
        if (this.viewMode() === 'add') {
            this.selectedPermissions.set(
                selectAll
                    ? this.permissionDefinitions().map(p => p.name || '')
                    : []
            );
        } else {
            this.selectedPermissions.set(
                selectAll
                    ? this.permissionsWithProvider().map(p => p.name || '')
                    : []
            );
        }
    }

    isPermissionSelected(permissionName: string): boolean {
        return this.selectedPermissions().includes(permissionName);
    }

    allPermissionsSelected = computed(() => {
        const definitions = this.viewMode() === 'add'
            ? this.permissionDefinitions()
            : this.permissionsWithProvider();
        return definitions.length > 0 &&
            definitions.every(p => this.selectedPermissions().includes(p.name || ''));
    });

    saveAddPermission() {
        if (!this.selectedProviderKey() || this.selectedPermissions().length === 0) {
            this.toasterService.warn('AbpPermissionManagement::PleaseSelectProviderAndPermissions');
            return;
        }

        this.modalBusy.set(true);
        this.service.updateResource(
            this.resourceName,
            this.resourceKey,
            {
                providerName: this.selectedProviderName(),
                providerKey: this.selectedProviderKey(),
                permissions: this.selectedPermissions()
            }
        ).pipe(
            switchMap(() => this.service.getResource(this.resourceName, this.resourceKey)),
            finalize(() => this.modalBusy.set(false))
        ).subscribe({
            next: res => {
                this.allResourcePermissions.set(res.permissions || []);
                this.list.get();
                this.toasterService.success('AbpUi::SavedSuccessfully');
                this.goToListMode();
            }
        });
    }

    saveEditPermission() {
        this.modalBusy.set(true);
        this.service.updateResource(
            this.resourceName,
            this.resourceKey,
            {
                providerName: this.editProviderName(),
                providerKey: this.editProviderKey(),
                permissions: this.selectedPermissions()
            }
        ).pipe(
            switchMap(() => this.service.getResource(this.resourceName, this.resourceKey)),
            finalize(() => this.modalBusy.set(false))
        ).subscribe({
            next: res => {
                this.allResourcePermissions.set(res.permissions || []);
                this.list.get();
                this.toasterService.success('AbpUi::SavedSuccessfully');
                this.goToListMode();
            }
        });
    }

    deletePermission(grant: ResourcePermissionGrantInfoDto) {
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
                    this.modalBusy.set(true);
                    this.service.deleteResource(
                        this.resourceName,
                        this.resourceKey,
                        grant.providerName || '',
                        grant.providerKey || ''
                    ).pipe(
                        switchMap(() => this.service.getResource(this.resourceName, this.resourceKey)),
                        finalize(() => this.modalBusy.set(false))
                    ).subscribe({
                        next: res => {
                            this.allResourcePermissions.set(res.permissions || []);
                            this.list.get();
                            this.toasterService.success('AbpUi::SuccessfullyDeleted');
                        }
                    });
                }
            });
    }
}
