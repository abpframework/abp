import { Renderer2, ElementRef, OnInit, EventEmitter } from '@angular/core';
export declare class InputEventDebounceDirective implements OnInit {
    private renderer;
    private el;
    debounce: number;
    debounceEvent: EventEmitter<Event>;
    constructor(renderer: Renderer2, el: ElementRef);
    ngOnInit(): void;
}
