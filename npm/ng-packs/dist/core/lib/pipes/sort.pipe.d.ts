import { PipeTransform } from '@angular/core';
export declare type SortOrder = 'asc' | 'desc';
export declare class SortPipe implements PipeTransform {
    transform(value: any[], sortOrder?: SortOrder | string, sortKey?: string): any;
}
