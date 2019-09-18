import { AfterViewInit, ElementRef, EventEmitter, OnDestroy, ChangeDetectorRef } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
export declare class ChartComponent implements AfterViewInit, OnDestroy {
    el: ElementRef;
    private cdRef;
    type: string;
    options: any;
    plugins: any[];
    width: string;
    height: string;
    responsive: boolean;
    onDataSelect: EventEmitter<any>;
    initialized: BehaviorSubject<this>;
    private _initialized;
    _data: any;
    chart: any;
    constructor(el: ElementRef, cdRef: ChangeDetectorRef);
    data: any;
    readonly canvas: any;
    readonly base64Image: any;
    ngAfterViewInit(): void;
    onCanvasClick: (event: any) => void;
    initChart: () => void;
    generateLegend: () => any;
    refresh: () => void;
    reinit: () => void;
    ngOnDestroy(): void;
}
