import { ElementRef, EventEmitter, OnInit, Renderer2 } from '@angular/core';
export declare class ClickEventStopPropagationDirective implements OnInit {
    private renderer;
    private el;
    stopPropEvent: EventEmitter<MouseEvent>;
    constructor(renderer: Renderer2, el: ElementRef);
    ngOnInit(): void;
}
