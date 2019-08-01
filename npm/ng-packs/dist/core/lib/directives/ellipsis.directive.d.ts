import { AfterContentInit, ChangeDetectorRef, ElementRef } from '@angular/core';
export declare class EllipsisDirective implements AfterContentInit {
    private cdRef;
    private elRef;
    witdh: string;
    title: string;
    enabled: boolean;
    readonly class: boolean;
    readonly maxWidth: string;
    constructor(cdRef: ChangeDetectorRef, elRef: ElementRef);
    ngAfterContentInit(): void;
}
