import { Component, input, inject, ChangeDetectionStrategy } from '@angular/core';
import { LocalizationPipe } from '@abp/ng.core';
import { ResourcePermissionStateService } from '../../../services/resource-permission-state.service';

interface PermissionItem {
    name?: string | null;
    displayName?: string | null;
}

@Component({
    selector: 'abp-permission-checkbox-list',
    templateUrl: './permission-checkbox-list.component.html',
    imports: [LocalizationPipe],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PermissionCheckboxListComponent {
    readonly state = inject(ResourcePermissionStateService);

    readonly permissions = input.required<PermissionItem[]>();
    readonly idPrefix = input<string>('default');
    readonly title = input<string>('AbpPermissionManagement::ResourcePermissionPermissions');
    readonly showTitle = input<boolean>(true);
}
