import { ElementRef, OnDestroy, OnInit, Renderer2, ViewContainerRef, TemplateRef, SimpleChanges, OnChanges } from '@angular/core';
import { Store } from '@ngxs/store';
import { Subscription } from 'rxjs';
export declare class PermissionDirective implements OnInit, OnDestroy, OnChanges {
    private elRef;
    private renderer;
    private store;
    private templateRef;
    private vcRef;
    condition: string;
    subscription: Subscription;
    constructor(elRef: ElementRef, renderer: Renderer2, store: Store, templateRef: TemplateRef<any>, vcRef: ViewContainerRef);
    private check;
    ngOnInit(): void;
    ngOnDestroy(): void;
    ngOnChanges({ condition }: SimpleChanges): void;
}
