import { ElementRef, EventEmitter, OnDestroy, Renderer2, TemplateRef } from '@angular/core';
import { Subject } from 'rxjs';
import { ConfirmationService } from '../../services/confirmation.service';
export declare type ModalSize = 'sm' | 'md' | 'lg' | 'xl';
export declare class ModalComponent implements OnDestroy {
    private renderer;
    private confirmationService;
    visible: boolean;
    centered: boolean;
    modalClass: string;
    size: ModalSize;
    visibleChange: EventEmitter<boolean>;
    abpHeader: TemplateRef<any>;
    abpBody: TemplateRef<any>;
    abpFooter: TemplateRef<any>;
    abpClose: ElementRef<any>;
    modalContent: ElementRef;
    _visible: boolean;
    closable: boolean;
    isOpenConfirmation: boolean;
    destroy$: Subject<void>;
    constructor(renderer: Renderer2, confirmationService: ConfirmationService);
    ngOnDestroy(): void;
    setVisible(value: boolean): void;
    listen(): void;
    close(): void;
}
