import { Config } from '@abp/ng.core';
import { AfterViewInit, ApplicationRef, ComponentFactoryResolver, ElementRef, Injector, OnDestroy, OnInit, Type } from '@angular/core';
import { Subject } from 'rxjs';
export declare class HttpErrorWrapperComponent implements AfterViewInit, OnDestroy, OnInit {
    appRef: ApplicationRef;
    cfRes: ComponentFactoryResolver;
    injector: Injector;
    status: number;
    title: Config.LocalizationParam;
    details: Config.LocalizationParam;
    customComponent: Type<any>;
    destroy$: Subject<void>;
    hideCloseIcon: boolean;
    backgroundColor: string;
    containerRef: ElementRef<HTMLDivElement>;
    readonly statusText: string;
    ngOnInit(): void;
    ngAfterViewInit(): void;
    ngOnDestroy(): void;
    destroy(): void;
}
