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
    readonly onDataSelect: EventEmitter<any>;
    readonly initialized: BehaviorSubject<this>;
    private _initialized;
    _data: any;
    chart: any;
    constructor(el: ElementRef, cdRef: ChangeDetectorRef);
    data: any;
    readonly canvas: any;
    readonly base64Image: any;
    ngAfterViewInit(): void;
    testChartJs(): void;
    onCanvasClick: (event: any) => void;
    initChart: () => void;
    generateLegend: () => any;
    refresh: () => void;
    reinit: () => void;
    ngOnDestroy(): void;
}
