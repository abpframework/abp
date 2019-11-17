import { ElementRef, Renderer2, AfterViewInit } from '@angular/core';
import { Subject } from 'rxjs';
export declare class VisibilityDirective implements AfterViewInit {
    private elRef;
    private renderer;
    focusedElement: HTMLElement;
    completed$: Subject<boolean>;
    constructor(elRef: ElementRef, renderer: Renderer2);
    ngAfterViewInit(): void;
    disconnect(): void;
    removeFromDOM(): void;
}
