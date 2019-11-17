import { EventEmitter } from '@angular/core';
export declare class SortOrderIconComponent {
    private _order;
    private _selectedKey;
    selectedKey: string;
    readonly selectedKeyChange: EventEmitter<string>;
    key: string;
    order: string;
    readonly orderChange: EventEmitter<string>;
    iconClass: string;
    readonly icon: string;
    sort(key: string): void;
}
