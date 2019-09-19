import { IterableDiffers, OnChanges, TemplateRef, TrackByFunction, ViewContainerRef } from '@angular/core';
export declare type CompareFn<T = any> = (value: T, comparison: T) => boolean;
declare class AbpForContext {
    $implicit: any;
    index: number;
    count: number;
    list: any[];
    constructor($implicit: any, index: number, count: number, list: any[]);
}
export declare class ForDirective implements OnChanges {
    private tempRef;
    private vcRef;
    private differs;
    items: any[];
    orderBy: string;
    orderDir: 'ASC' | 'DESC';
    filterBy: string;
    filterVal: any;
    trackBy: any;
    compareBy: CompareFn;
    emptyRef: TemplateRef<any>;
    private differ;
    private isShowEmptyRef;
    readonly compareFn: CompareFn;
    readonly trackByFn: TrackByFunction<any>;
    constructor(tempRef: TemplateRef<AbpForContext>, vcRef: ViewContainerRef, differs: IterableDiffers);
    private iterateOverAppliedOperations;
    private iterateOverAttachedViews;
    private projectItems;
    private sortItems;
    ngOnChanges(): void;
}
export {};
