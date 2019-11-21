import { AfterContentInit, ChangeDetectorRef, ElementRef } from '@angular/core';
export declare class EllipsisDirective implements AfterContentInit {
    private cdRef;
    private elRef;
    width: string;
    title: string;
    enabled: boolean;
    readonly inlineClass: string;
    readonly class: boolean;
    readonly maxWidth: string;
    constructor(cdRef: ChangeDetectorRef, elRef: ElementRef);
    ngAfterContentInit(): void;
}
