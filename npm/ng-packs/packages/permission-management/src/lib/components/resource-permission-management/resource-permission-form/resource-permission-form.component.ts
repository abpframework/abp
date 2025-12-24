import { Component, input, inject, output, ChangeDetectionStrategy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LocalizationPipe } from '@abp/ng.core';
import { ResourcePermissionStateService } from '../../../services/resource-permission-state.service';
import { ProviderKeySearchComponent } from '../provider-key-search/provider-key-search.component';
import { PermissionCheckboxListComponent } from '../permission-checkbox-list/permission-checkbox-list.component';

import { eResourcePermissionViewModes } from '../../../enums/view-modes';

@Component({
    selector: 'abp-resource-permission-form',
    templateUrl: './resource-permission-form.component.html',
    imports: [
        FormsModule,
        LocalizationPipe,
        ProviderKeySearchComponent,
        PermissionCheckboxListComponent,
    ],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ResourcePermissionFormComponent {
    readonly state = inject(ResourcePermissionStateService);
    readonly eResourcePermissionViewModes = eResourcePermissionViewModes;

    readonly mode = input.required<eResourcePermissionViewModes>();
    readonly resourceName = input.required<string>();

    readonly save = output<void>();
    readonly cancel = output<void>();
}

