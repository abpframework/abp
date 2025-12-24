import { Component, input, inject, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { PermissionsService } from '@abp/ng.permission-management/proxy';
import { LookupSearchComponent, LookupItem } from '@abp/ng.components/lookup';
import { Observable, map, Subject, takeUntil } from 'rxjs';
import { ResourcePermissionStateService } from '../../../services/resource-permission-state.service';

interface ProviderKeyLookupItem extends LookupItem {
    providerKey: string;
    providerDisplayName?: string;
}

@Component({
    selector: 'abp-provider-key-search',
    templateUrl: './provider-key-search.component.html',
    imports: [LookupSearchComponent],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProviderKeySearchComponent implements OnInit, OnDestroy {
    readonly state = inject(ResourcePermissionStateService);
    private readonly service = inject(PermissionsService);

    readonly resourceName = input.required<string>();

    private readonly destroy$ = new Subject<void>();

    searchFn: (filter: string) => Observable<ProviderKeyLookupItem[]> = () => new Observable();

    ngOnInit() {
        this.searchFn = (filter: string) => this.loadProviderKeys(filter);
    }

    ngOnDestroy() {
        this.destroy$.next();
        this.destroy$.complete();
    }

    onItemSelected(item: ProviderKeyLookupItem) {
        // State is already updated via displayValue and selectedValue bindings
        // This handler can be used for additional side effects if needed
    }

    private loadProviderKeys(filter: string): Observable<ProviderKeyLookupItem[]> {
        const providerName = this.state.selectedProviderName();
        if (!providerName) {
            return new Observable(subscriber => {
                subscriber.next([]);
                subscriber.complete();
            });
        }

        return this.service.searchResourceProviderKey(
            this.resourceName(),
            providerName,
            filter,
            1
        ).pipe(
            map(res => (res.keys || []).map(k => ({
                key: k.providerKey || '',
                displayName: k.providerDisplayName || k.providerKey || '',
                providerKey: k.providerKey || '',
                providerDisplayName: k.providerDisplayName || undefined,
            }))),
            takeUntil(this.destroy$)
        );
    }
}
