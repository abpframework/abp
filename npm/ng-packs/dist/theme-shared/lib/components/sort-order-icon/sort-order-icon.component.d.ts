import { EventEmitter } from '@angular/core';
export declare class SortOrderIconComponent {
    private _order;
    private _selectedSortKey;
    /**
     * @deprecated use selectedSortKey instead.
     */
    selectedKey: string;
    selectedSortKey: string;
    readonly selectedKeyChange: EventEmitter<string>;
    readonly selectedSortKeyChange: EventEmitter<string>;
    /**
     * @deprecated use sortKey instead.
     */
    key: string;
    sortKey: string;
    order: 'asc' | 'desc' | '';
    readonly orderChange: EventEmitter<string>;
    iconClass: string;
    readonly icon: string;
    sort(key: string): void;
}
