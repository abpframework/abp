import { EventEmitter, ElementRef, Renderer2, OnInit } from '@angular/core';
import { ABP } from '@abp/ng.core';
export declare class ButtonComponent implements OnInit {
    private renderer;
    buttonId: string;
    buttonClass: string;
    buttonType: string;
    iconClass: string;
    loading: boolean;
    disabled: boolean;
    attributes: ABP.Dictionary<string>;
    readonly click: EventEmitter<MouseEvent>;
    readonly focus: EventEmitter<FocusEvent>;
    readonly blur: EventEmitter<FocusEvent>;
    buttonRef: ElementRef<HTMLButtonElement>;
    readonly icon: string;
    constructor(renderer: Renderer2);
    ngOnInit(): void;
    onClick(event: MouseEvent): void;
    onFocus(event: FocusEvent): void;
    onBlur(event: FocusEvent): void;
}
