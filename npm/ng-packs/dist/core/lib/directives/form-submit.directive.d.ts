import { ChangeDetectorRef, ElementRef, EventEmitter, OnDestroy, OnInit } from '@angular/core';
import { FormGroupDirective } from '@angular/forms';
export declare class FormSubmitDirective implements OnInit, OnDestroy {
    private formGroupDirective;
    private host;
    private cdRef;
    notValidateOnSubmit: string | boolean;
    ngSubmit: EventEmitter<unknown>;
    executedNgSubmit: boolean;
    constructor(formGroupDirective: FormGroupDirective, host: ElementRef<HTMLFormElement>, cdRef: ChangeDetectorRef);
    ngOnInit(): void;
    ngOnDestroy(): void;
    markAsDirty(): void;
}
