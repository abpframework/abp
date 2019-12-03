import { ElementRef, OnDestroy, OnInit, Renderer2 } from '@angular/core';
import { Store } from '@ngxs/store';
export declare class PermissionDirective implements OnInit, OnDestroy {
    private elRef;
    private renderer;
    private store;
    condition: string;
    constructor(elRef: ElementRef, renderer: Renderer2, store: Store);
    ngOnInit(): void;
    ngOnDestroy(): void;
}
