import { EventEmitter, ElementRef, Renderer2, OnInit } from '@angular/core';
import { ABP } from '@abp/ng.core';
export declare class ButtonComponent implements OnInit {
    private renderer;
    buttonClass: string;
    buttonType: any;
    iconClass: string;
    loading: boolean;
    disabled: boolean;
    attributes: ABP.Dictionary<string>;
    readonly click: EventEmitter<MouseEvent>;
    readonly focus: EventEmitter<FocusEvent>;
    readonly blur: EventEmitter<FocusEvent>;
    buttonRef: ElementRef<HTMLButtonElement>;
    /**
     * @deprecated Use buttonType instead. To be deleted in v1
     */
    type: string;
    readonly icon: string;
    constructor(renderer: Renderer2);
    ngOnInit(): void;
}
