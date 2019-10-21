import { ElementRef, AfterViewInit } from '@angular/core';
export declare class AutofocusDirective implements AfterViewInit {
    private elRef;
    delay: number;
    constructor(elRef: ElementRef);
    ngAfterViewInit(): void;
}
