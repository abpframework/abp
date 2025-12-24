import {
    Component,
    input,
    output,
    model,
    signal,
    OnInit,
    OnDestroy,
    ChangeDetectionStrategy,
    TemplateRef,
    contentChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { LocalizationPipe } from '@abp/ng.core';
import { Subject, Observable, debounceTime, distinctUntilChanged, takeUntil, of } from 'rxjs';

export interface LookupItem {
    key: string;
    displayName: string;
    [key: string]: unknown;
}

export type LookupSearchFn<T = LookupItem> = (filter: string) => Observable<T[]>;

@Component({
    selector: 'abp-lookup-search',
    templateUrl: './lookup-search.component.html',
    imports: [CommonModule, FormsModule, LocalizationPipe],
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LookupSearchComponent<T extends LookupItem = LookupItem> implements OnInit, OnDestroy {
    // Inputs
    readonly label = input<string>();
    readonly placeholder = input<string>('');
    readonly debounceTime = input<number>(300);
    readonly minSearchLength = input<number>(0);
    readonly displayKey = input<keyof T>('displayName' as keyof T);
    readonly valueKey = input<keyof T>('key' as keyof T);
    readonly disabled = input<boolean>(false);

    // Search function - should be provided by parent
    readonly searchFn = input<LookupSearchFn<T>>(() => of([]));

    // Two-way binding for selected value
    readonly selectedValue = model<string>('');
    readonly displayValue = model<string>('');

    // Outputs
    readonly itemSelected = output<T>();
    readonly searchChanged = output<string>();

    // Custom templates
    readonly itemTemplate = contentChild<TemplateRef<{ $implicit: T }>>('itemTemplate');
    readonly noResultsTemplate = contentChild<TemplateRef<void>>('noResultsTemplate');

    // Internal state
    readonly searchResults = signal<T[]>([]);
    readonly showDropdown = signal(false);
    readonly isLoading = signal(false);

    private readonly searchSubject = new Subject<string>();
    private readonly destroy$ = new Subject<void>();

    ngOnInit() {
        this.searchSubject.pipe(
            debounceTime(this.debounceTime()),
            distinctUntilChanged(),
            takeUntil(this.destroy$)
        ).subscribe(filter => {
            this.performSearch(filter);
        });
    }

    ngOnDestroy() {
        this.destroy$.next();
        this.destroy$.complete();
    }

    onSearchInput(filter: string) {
        this.displayValue.set(filter);
        this.showDropdown.set(true);
        this.searchChanged.emit(filter);

        if (filter.length >= this.minSearchLength()) {
            this.searchSubject.next(filter);
        } else {
            this.searchResults.set([]);
        }
    }

    onSearchFocus() {
        this.showDropdown.set(true);
        const currentFilter = this.displayValue() || '';
        if (currentFilter.length >= this.minSearchLength()) {
            this.performSearch(currentFilter);
        }
    }

    onSearchBlur(event: FocusEvent) {
        const relatedTarget = event.relatedTarget as HTMLElement;
        if (!relatedTarget?.closest('.abp-lookup-dropdown')) {
            this.showDropdown.set(false);
        }
    }

    selectItem(item: T) {
        const displayKeyValue = String(item[this.displayKey()] ?? '');
        const valueKeyValue = String(item[this.valueKey()] ?? '');

        this.displayValue.set(displayKeyValue);
        this.selectedValue.set(valueKeyValue);
        this.searchResults.set([]);
        this.showDropdown.set(false);
        this.itemSelected.emit(item);
    }

    clearSelection() {
        this.displayValue.set('');
        this.selectedValue.set('');
        this.searchResults.set([]);
    }

    private performSearch(filter: string) {
        this.isLoading.set(true);

        this.searchFn()(filter).pipe(
            takeUntil(this.destroy$)
        ).subscribe({
            next: results => {
                this.searchResults.set(results);
                this.isLoading.set(false);
            },
            error: () => {
                this.searchResults.set([]);
                this.isLoading.set(false);
            }
        });
    }

    getDisplayValue(item: T): string {
        return String(item[this.displayKey()] ?? item[this.valueKey()] ?? '');
    }
}
