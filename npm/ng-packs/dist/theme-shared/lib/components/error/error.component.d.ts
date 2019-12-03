import { Config } from '@abp/ng.core';
import { AfterViewInit, ComponentFactoryResolver, ElementRef, OnDestroy, Type } from '@angular/core';
import { Subject } from 'rxjs';
export declare class ErrorComponent implements AfterViewInit, OnDestroy {
    cfRes: ComponentFactoryResolver;
    status: number;
    title: Config.LocalizationParam;
    details: Config.LocalizationParam;
    customComponent: Type<any>;
    destroy$: Subject<void>;
    containerRef: ElementRef<HTMLDivElement>;
    readonly statusText: string;
    ngAfterViewInit(): void;
    ngOnDestroy(): void;
    destroy(): void;
}
