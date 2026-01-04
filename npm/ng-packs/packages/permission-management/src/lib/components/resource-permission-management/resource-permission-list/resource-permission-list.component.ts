import { Component, inject, output, ChangeDetectionStrategy } from '@angular/core';
import { ListService, LocalizationPipe } from '@abp/ng.core';
import { ExtensibleTableComponent, EXTENSIONS_IDENTIFIER } from '@abp/ng.components/extensible';
import { ResourcePermissionGrantInfoDto } from '@abp/ng.permission-management/proxy';
import { ResourcePermissionStateService } from '../../../services/resource-permission-state.service';
import { ePermissionManagementComponents } from '../../../enums/components';
import { configureResourcePermissionExtensions } from '../../../services/extensions.service';

@Component({
    selector: 'abp-resource-permission-list',
    templateUrl: './resource-permission-list.component.html',
    providers: [
        ListService,
        {
            provide: EXTENSIONS_IDENTIFIER,
            useValue: ePermissionManagementComponents.ResourcePermissions,
        },
    ],
    imports: [LocalizationPipe, ExtensibleTableComponent],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResourcePermissionListComponent {
    readonly state = inject(ResourcePermissionStateService);
    readonly list = inject(ListService);

    readonly addClicked = output<void>();
    readonly editClicked = output<ResourcePermissionGrantInfoDto>();
    readonly deleteClicked = output<ResourcePermissionGrantInfoDto>();

    constructor() {
        configureResourcePermissionExtensions();
    }
}
