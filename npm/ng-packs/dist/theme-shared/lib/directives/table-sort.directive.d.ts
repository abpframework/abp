import { SimpleChanges, OnChanges } from '@angular/core';
import { Table } from 'primeng/table';
import { SortPipe, SortOrder } from '@abp/ng.core';
export interface TableSortOptions {
    key: string;
    order: SortOrder;
}
export declare class TableSortDirective implements OnChanges {
    private table;
    private sortPipe;
    abpTableSort: TableSortOptions;
    value: any[];
    constructor(table: Table, sortPipe: SortPipe);
    ngOnChanges({ value, abpTableSort }: SimpleChanges): void;
}
