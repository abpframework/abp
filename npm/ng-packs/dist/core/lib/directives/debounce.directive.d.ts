import { ElementRef, EventEmitter, OnDestroy, OnInit } from '@angular/core';
export declare class InputEventDebounceDirective implements OnInit, OnDestroy {
    private el;
    debounce: number;
    readonly debounceEvent: EventEmitter<Event>;
    constructor(el: ElementRef);
    ngOnInit(): void;
    ngOnDestroy(): void;
}
