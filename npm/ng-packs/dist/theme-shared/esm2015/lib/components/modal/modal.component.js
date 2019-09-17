/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, ContentChild, ElementRef, EventEmitter, Input, Output, Renderer2, TemplateRef, ViewChild, ViewChildren, } from '@angular/core';
import { fromEvent, Subject, timer } from 'rxjs';
import { filter, take, takeUntil, debounceTime } from 'rxjs/operators';
import { ConfirmationService } from '../../services/confirmation.service';
import { ButtonComponent } from '../button/button.component';
/** @type {?} */
const ANIMATION_TIMEOUT = 200;
export class ModalComponent {
    /**
     * @param {?} renderer
     * @param {?} confirmationService
     */
    constructor(renderer, confirmationService) {
        this.renderer = renderer;
        this.confirmationService = confirmationService;
        this.centered = false;
        this.modalClass = '';
        this.size = 'lg';
        this.visibleChange = new EventEmitter();
        this.init = new EventEmitter();
        this._visible = false;
        this._busy = false;
        this.showModal = false;
        this.isOpenConfirmation = false;
        this.closable = false;
        this.destroy$ = new Subject();
    }
    /**
     * @return {?}
     */
    get visible() {
        return this._visible;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set visible(value) {
        if (typeof value !== 'boolean')
            return;
        if (!this.modalContent) {
            if (value) {
                setTimeout((/**
                 * @return {?}
                 */
                () => {
                    this.showModal = value;
                    this.visible = value;
                }), 0);
            }
            return;
        }
        if (value) {
            this.setVisible(value);
            this.listen();
        }
        else {
            this.closable = false;
            this.renderer.addClass(this.modalContent.nativeElement, 'fade-out-top');
            setTimeout((/**
             * @return {?}
             */
            () => {
                this.setVisible(value);
                this.ngOnDestroy();
            }), ANIMATION_TIMEOUT - 10);
        }
    }
    /**
     * @return {?}
     */
    get busy() {
        return this._busy;
    }
    /**
     * @param {?} value
     * @return {?}
     */
    set busy(value) {
        if (this.abpSubmit && this.abpSubmit instanceof ButtonComponent) {
            this.abpSubmit.loading = value;
        }
        this._busy = value;
    }
    /**
     * @return {?}
     */
    ngOnDestroy() {
        this.destroy$.next();
    }
    /**
     * @param {?} value
     * @return {?}
     */
    setVisible(value) {
        this._visible = value;
        this.visibleChange.emit(value);
        this.showModal = value;
        if (value) {
            timer(ANIMATION_TIMEOUT + 100)
                .pipe(take(1))
                .subscribe((/**
             * @param {?} _
             * @return {?}
             */
            _ => (this.closable = true)));
            this.renderer.addClass(document.body, 'modal-open');
        }
        else {
            this.closable = false;
            this.renderer.removeClass(document.body, 'modal-open');
        }
    }
    /**
     * @return {?}
     */
    listen() {
        fromEvent(document, 'keyup')
            .pipe(takeUntil(this.destroy$), debounceTime(150), filter((/**
         * @param {?} key
         * @return {?}
         */
        (key) => key && key.code === 'Escape' && this.closable)))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        _ => {
            this.close();
        }));
        setTimeout((/**
         * @return {?}
         */
        () => {
            if (!this.abpClose)
                return;
            fromEvent(this.abpClose.nativeElement, 'click')
                .pipe(takeUntil(this.destroy$), filter((/**
             * @return {?}
             */
            () => !!(this.closable && this.modalContent))))
                .subscribe((/**
             * @return {?}
             */
            () => this.close()));
        }), 0);
        this.init.emit();
    }
    /**
     * @return {?}
     */
    close() {
        if (!this.closable || this.busy)
            return;
        /** @type {?} */
        const nodes = getFlatNodes(((/** @type {?} */ (this.modalContent.nativeElement.querySelector('#abp-modal-body')))).childNodes);
        if (hasNgDirty(nodes)) {
            if (this.isOpenConfirmation)
                return;
            this.isOpenConfirmation = true;
            this.confirmationService
                .warn('AbpAccount::AreYouSureYouWantToCancelEditingWarningMessage', 'AbpAccount::AreYouSure')
                .subscribe((/**
             * @param {?} status
             * @return {?}
             */
            (status) => {
                timer(ANIMATION_TIMEOUT).subscribe((/**
                 * @return {?}
                 */
                () => {
                    this.isOpenConfirmation = false;
                }));
                if (status === "confirm" /* confirm */) {
                    this.visible = false;
                }
            }));
        }
        else {
            this.visible = false;
        }
    }
}
ModalComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-modal',
                template: "<div\n  *ngIf=\"showModal\"\n  (click)=\"close()\"\n  id=\"abp-modal\"\n  class=\"modal fade {{ modalClass }} d-block show\"\n  [style.padding-right.px]=\"'15'\"\n>\n  <div\n    id=\"abp-modal-container\"\n    class=\"modal-dialog modal-{{ size }} fade-in-top\"\n    tabindex=\"-1\"\n    [class.modal-dialog-centered]=\"centered\"\n    #abpModalContent\n  >\n    <div #content id=\"abp-modal-content\" class=\"modal-content\" (click)=\"$event.stopPropagation()\">\n      <div id=\"abp-modal-header\" class=\"modal-header\">\n        <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\n\n        <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" (click)=\"close()\">\n          <span aria-hidden=\"true\">&times;</span>\n        </button>\n      </div>\n      <div\n        id=\"abp-modal-body\"\n        class=\"modal-body\"\n        [style.height]=\"height || undefined\"\n        [style.minHeight]=\"minHeight || undefined\"\n      >\n        <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\n      </div>\n      <div id=\"abp-modal-footer\" class=\"modal-footer\">\n        <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\n      </div>\n    </div>\n  </div>\n\n  <ng-content></ng-content>\n</div>\n"
            }] }
];
/** @nocollapse */
ModalComponent.ctorParameters = () => [
    { type: Renderer2 },
    { type: ConfirmationService }
];
ModalComponent.propDecorators = {
    visible: [{ type: Input }],
    busy: [{ type: Input }],
    centered: [{ type: Input }],
    modalClass: [{ type: Input }],
    size: [{ type: Input }],
    height: [{ type: Input }],
    minHeight: [{ type: Input }],
    visibleChange: [{ type: Output }],
    init: [{ type: Output }],
    abpHeader: [{ type: ContentChild, args: ['abpHeader', { static: false },] }],
    abpBody: [{ type: ContentChild, args: ['abpBody', { static: false },] }],
    abpFooter: [{ type: ContentChild, args: ['abpFooter', { static: false },] }],
    abpClose: [{ type: ContentChild, args: ['abpClose', { static: false, read: ElementRef },] }],
    abpSubmit: [{ type: ContentChild, args: [ButtonComponent, { static: false, read: ButtonComponent },] }],
    modalContent: [{ type: ViewChild, args: ['abpModalContent', { static: false },] }],
    abpButtons: [{ type: ViewChildren, args: ['abp-button',] }]
};
if (false) {
    /** @type {?} */
    ModalComponent.prototype.centered;
    /** @type {?} */
    ModalComponent.prototype.modalClass;
    /** @type {?} */
    ModalComponent.prototype.size;
    /** @type {?} */
    ModalComponent.prototype.height;
    /** @type {?} */
    ModalComponent.prototype.minHeight;
    /** @type {?} */
    ModalComponent.prototype.visibleChange;
    /** @type {?} */
    ModalComponent.prototype.init;
    /** @type {?} */
    ModalComponent.prototype.abpHeader;
    /** @type {?} */
    ModalComponent.prototype.abpBody;
    /** @type {?} */
    ModalComponent.prototype.abpFooter;
    /** @type {?} */
    ModalComponent.prototype.abpClose;
    /** @type {?} */
    ModalComponent.prototype.abpSubmit;
    /** @type {?} */
    ModalComponent.prototype.modalContent;
    /** @type {?} */
    ModalComponent.prototype.abpButtons;
    /** @type {?} */
    ModalComponent.prototype._visible;
    /** @type {?} */
    ModalComponent.prototype._busy;
    /** @type {?} */
    ModalComponent.prototype.showModal;
    /** @type {?} */
    ModalComponent.prototype.isOpenConfirmation;
    /** @type {?} */
    ModalComponent.prototype.closable;
    /** @type {?} */
    ModalComponent.prototype.destroy$;
    /**
     * @type {?}
     * @private
     */
    ModalComponent.prototype.renderer;
    /**
     * @type {?}
     * @private
     */
    ModalComponent.prototype.confirmationService;
}
/**
 * @param {?} nodes
 * @return {?}
 */
function getFlatNodes(nodes) {
    return Array.from(nodes).reduce((/**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    (acc, val) => [...acc, ...(val.childNodes && val.childNodes.length ? getFlatNodes(val.childNodes) : [val])]), []);
}
/**
 * @param {?} nodes
 * @return {?}
 */
function hasNgDirty(nodes) {
    return nodes.findIndex((/**
     * @param {?} node
     * @return {?}
     */
    node => (node.className || '').indexOf('ng-dirty') > -1)) > -1;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLFVBQVUsRUFDVixZQUFZLEVBQ1osS0FBSyxFQUVMLE1BQU0sRUFDTixTQUFTLEVBQ1QsV0FBVyxFQUNYLFNBQVMsRUFDVCxZQUFZLEdBQ2IsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLFNBQVMsRUFBRSxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2pELE9BQU8sRUFBRSxNQUFNLEVBQUUsSUFBSSxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUV2RSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxxQ0FBcUMsQ0FBQztBQUMxRSxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7O01BSXZELGlCQUFpQixHQUFHLEdBQUc7QUFNN0IsTUFBTSxPQUFPLGNBQWM7Ozs7O0lBbUZ6QixZQUFvQixRQUFtQixFQUFVLG1CQUF3QztRQUFyRSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBQVUsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQXhDaEYsYUFBUSxHQUFZLEtBQUssQ0FBQztRQUUxQixlQUFVLEdBQVcsRUFBRSxDQUFDO1FBRXhCLFNBQUksR0FBYyxJQUFJLENBQUM7UUFNdEIsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBRTVDLFNBQUksR0FBRyxJQUFJLFlBQVksRUFBUSxDQUFDO1FBZ0IxQyxhQUFRLEdBQVksS0FBSyxDQUFDO1FBRTFCLFVBQUssR0FBWSxLQUFLLENBQUM7UUFFdkIsY0FBUyxHQUFZLEtBQUssQ0FBQztRQUUzQix1QkFBa0IsR0FB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bnRlbnRDaGlsZCxcbiAgRWxlbWVudFJlZixcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25EZXN0cm95LFxuICBPdXRwdXQsXG4gIFJlbmRlcmVyMixcbiAgVGVtcGxhdGVSZWYsXG4gIFZpZXdDaGlsZCxcbiAgVmlld0NoaWxkcmVuLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IGZyb21FdmVudCwgU3ViamVjdCwgdGltZXIgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGZpbHRlciwgdGFrZSwgdGFrZVVudGlsLCBkZWJvdW5jZVRpbWUgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RvYXN0ZXInO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2NvbmZpcm1hdGlvbi5zZXJ2aWNlJztcbmltcG9ydCB7IEJ1dHRvbkNvbXBvbmVudCB9IGZyb20gJy4uL2J1dHRvbi9idXR0b24uY29tcG9uZW50JztcblxuZXhwb3J0IHR5cGUgTW9kYWxTaXplID0gJ3NtJyB8ICdtZCcgfCAnbGcnIHwgJ3hsJztcblxuY29uc3QgQU5JTUFUSU9OX1RJTUVPVVQgPSAyMDA7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1tb2RhbCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9tb2RhbC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIE1vZGFsQ29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcbiAgQElucHV0KClcbiAgZ2V0IHZpc2libGUoKTogYm9vbGVhbiB7XG4gICAgcmV0dXJuIHRoaXMuX3Zpc2libGU7XG4gIH1cbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcbiAgICBpZiAodHlwZW9mIHZhbHVlICE9PSAnYm9vbGVhbicpIHJldHVybjtcblxuICAgIGlmICghdGhpcy5tb2RhbENvbnRlbnQpIHtcbiAgICAgIGlmICh2YWx1ZSkge1xuICAgICAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgICAgICB0aGlzLnNob3dNb2RhbCA9IHZhbHVlO1xuICAgICAgICAgIHRoaXMudmlzaWJsZSA9IHZhbHVlO1xuICAgICAgICB9LCAwKTtcbiAgICAgIH1cbiAgICAgIHJldHVybjtcbiAgICB9XG5cbiAgICBpZiAodmFsdWUpIHtcbiAgICAgIHRoaXMuc2V0VmlzaWJsZSh2YWx1ZSk7XG4gICAgICB0aGlzLmxpc3RlbigpO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLmNsb3NhYmxlID0gZmFsc2U7XG4gICAgICB0aGlzLnJlbmRlcmVyLmFkZENsYXNzKHRoaXMubW9kYWxDb250ZW50Lm5hdGl2ZUVsZW1lbnQsICdmYWRlLW91dC10b3AnKTtcbiAgICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgICB0aGlzLnNldFZpc2libGUodmFsdWUpO1xuICAgICAgICB0aGlzLm5nT25EZXN0cm95KCk7XG4gICAgICB9LCBBTklNQVRJT05fVElNRU9VVCAtIDEwKTtcbiAgICB9XG4gIH1cblxuICBASW5wdXQoKVxuICBnZXQgYnVzeSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fYnVzeTtcbiAgfVxuICBzZXQgYnVzeSh2YWx1ZTogYm9vbGVhbikge1xuICAgIGlmICh0aGlzLmFicFN1Ym1pdCAmJiB0aGlzLmFicFN1Ym1pdCBpbnN0YW5jZW9mIEJ1dHRvbkNvbXBvbmVudCkge1xuICAgICAgdGhpcy5hYnBTdWJtaXQubG9hZGluZyA9IHZhbHVlO1xuICAgIH1cblxuICAgIHRoaXMuX2J1c3kgPSB2YWx1ZTtcbiAgfVxuXG4gIEBJbnB1dCgpIGNlbnRlcmVkOiBib29sZWFuID0gZmFsc2U7XG5cbiAgQElucHV0KCkgbW9kYWxDbGFzczogc3RyaW5nID0gJyc7XG5cbiAgQElucHV0KCkgc2l6ZTogTW9kYWxTaXplID0gJ2xnJztcblxuICBASW5wdXQoKSBoZWlnaHQ6IG51bWJlcjtcblxuICBASW5wdXQoKSBtaW5IZWlnaHQ6IG51bWJlcjtcblxuICBAT3V0cHV0KCkgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcblxuICBAT3V0cHV0KCkgaW5pdCA9IG5ldyBFdmVudEVtaXR0ZXI8dm9pZD4oKTtcblxuICBAQ29udGVudENoaWxkKCdhYnBIZWFkZXInLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwSGVhZGVyOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEJvZHknLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwQm9keTogVGVtcGxhdGVSZWY8YW55PjtcblxuICBAQ29udGVudENoaWxkKCdhYnBGb290ZXInLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwRm9vdGVyOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicENsb3NlJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBFbGVtZW50UmVmIH0pIGFicENsb3NlOiBFbGVtZW50UmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZChCdXR0b25Db21wb25lbnQsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogQnV0dG9uQ29tcG9uZW50IH0pIGFicFN1Ym1pdDogQnV0dG9uQ29tcG9uZW50O1xuXG4gIEBWaWV3Q2hpbGQoJ2FicE1vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KSBtb2RhbENvbnRlbnQ6IEVsZW1lbnRSZWY7XG5cbiAgQFZpZXdDaGlsZHJlbignYWJwLWJ1dHRvbicpIGFicEJ1dHRvbnM7XG5cbiAgX3Zpc2libGU6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBfYnVzeTogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIHNob3dNb2RhbDogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGlzT3BlbkNvbmZpcm1hdGlvbjogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGNsb3NhYmxlOiBib29sZWFuID0gZmFsc2U7XG5cbiAgZGVzdHJveSQgPSBuZXcgU3ViamVjdDx2b2lkPigpO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMiwgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlKSB7fVxuXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge1xuICAgIHRoaXMuZGVzdHJveSQubmV4dCgpO1xuICB9XG5cbiAgc2V0VmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gICAgdGhpcy5zaG93TW9kYWwgPSB2YWx1ZTtcblxuICAgIGlmICh2YWx1ZSkge1xuICAgICAgdGltZXIoQU5JTUFUSU9OX1RJTUVPVVQgKyAxMDApXG4gICAgICAgIC5waXBlKHRha2UoMSkpXG4gICAgICAgIC5zdWJzY3JpYmUoXyA9PiAodGhpcy5jbG9zYWJsZSA9IHRydWUpKTtcblxuICAgICAgdGhpcy5yZW5kZXJlci5hZGRDbGFzcyhkb2N1bWVudC5ib2R5LCAnbW9kYWwtb3BlbicpO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLmNsb3NhYmxlID0gZmFsc2U7XG4gICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNsYXNzKGRvY3VtZW50LmJvZHksICdtb2RhbC1vcGVuJyk7XG4gICAgfVxuICB9XG5cbiAgbGlzdGVuKCkge1xuICAgIGZyb21FdmVudChkb2N1bWVudCwgJ2tleXVwJylcbiAgICAgIC5waXBlKFxuICAgICAgICB0YWtlVW50aWwodGhpcy5kZXN0cm95JCksXG4gICAgICAgIGRlYm91bmNlVGltZSgxNTApLFxuICAgICAgICBmaWx0ZXIoKGtleTogS2V5Ym9hcmRFdmVudCkgPT4ga2V5ICYmIGtleS5jb2RlID09PSAnRXNjYXBlJyAmJiB0aGlzLmNsb3NhYmxlKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoXyA9PiB7XG4gICAgICAgIHRoaXMuY2xvc2UoKTtcbiAgICAgIH0pO1xuXG4gICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICBpZiAoIXRoaXMuYWJwQ2xvc2UpIHJldHVybjtcbiAgICAgIGZyb21FdmVudCh0aGlzLmFicENsb3NlLm5hdGl2ZUVsZW1lbnQsICdjbGljaycpXG4gICAgICAgIC5waXBlKFxuICAgICAgICAgIHRha2VVbnRpbCh0aGlzLmRlc3Ryb3kkKSxcbiAgICAgICAgICBmaWx0ZXIoKCkgPT4gISEodGhpcy5jbG9zYWJsZSAmJiB0aGlzLm1vZGFsQ29udGVudCkpLFxuICAgICAgICApXG4gICAgICAgIC5zdWJzY3JpYmUoKCkgPT4gdGhpcy5jbG9zZSgpKTtcbiAgICB9LCAwKTtcblxuICAgIHRoaXMuaW5pdC5lbWl0KCk7XG4gIH1cblxuICBjbG9zZSgpIHtcbiAgICBpZiAoIXRoaXMuY2xvc2FibGUgfHwgdGhpcy5idXN5KSByZXR1cm47XG5cbiAgICBjb25zdCBub2RlcyA9IGdldEZsYXROb2RlcyhcbiAgICAgICh0aGlzLm1vZGFsQ29udGVudC5uYXRpdmVFbGVtZW50LnF1ZXJ5U2VsZWN0b3IoJyNhYnAtbW9kYWwtYm9keScpIGFzIEhUTUxFbGVtZW50KS5jaGlsZE5vZGVzLFxuICAgICk7XG5cbiAgICBpZiAoaGFzTmdEaXJ0eShub2RlcykpIHtcbiAgICAgIGlmICh0aGlzLmlzT3BlbkNvbmZpcm1hdGlvbikgcmV0dXJuO1xuXG4gICAgICB0aGlzLmlzT3BlbkNvbmZpcm1hdGlvbiA9IHRydWU7XG4gICAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2VcbiAgICAgICAgLndhcm4oJ0FicEFjY291bnQ6OkFyZVlvdVN1cmVZb3VXYW50VG9DYW5jZWxFZGl0aW5nV2FybmluZ01lc3NhZ2UnLCAnQWJwQWNjb3VudDo6QXJlWW91U3VyZScpXG4gICAgICAgIC5zdWJzY3JpYmUoKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpID0+IHtcbiAgICAgICAgICB0aW1lcihBTklNQVRJT05fVElNRU9VVCkuc3Vic2NyaWJlKCgpID0+IHtcbiAgICAgICAgICAgIHRoaXMuaXNPcGVuQ29uZmlybWF0aW9uID0gZmFsc2U7XG4gICAgICAgICAgfSk7XG5cbiAgICAgICAgICBpZiAoc3RhdHVzID09PSBUb2FzdGVyLlN0YXR1cy5jb25maXJtKSB7XG4gICAgICAgICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICAgICAgICB9XG4gICAgICAgIH0pO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLnZpc2libGUgPSBmYWxzZTtcbiAgICB9XG4gIH1cbn1cblxuZnVuY3Rpb24gZ2V0RmxhdE5vZGVzKG5vZGVzOiBOb2RlTGlzdCk6IEhUTUxFbGVtZW50W10ge1xuICByZXR1cm4gQXJyYXkuZnJvbShub2RlcykucmVkdWNlKFxuICAgIChhY2MsIHZhbCkgPT4gWy4uLmFjYywgLi4uKHZhbC5jaGlsZE5vZGVzICYmIHZhbC5jaGlsZE5vZGVzLmxlbmd0aCA/IGdldEZsYXROb2Rlcyh2YWwuY2hpbGROb2RlcykgOiBbdmFsXSldLFxuICAgIFtdLFxuICApO1xufVxuXG5mdW5jdGlvbiBoYXNOZ0RpcnR5KG5vZGVzOiBIVE1MRWxlbWVudFtdKSB7XG4gIHJldHVybiBub2Rlcy5maW5kSW5kZXgobm9kZSA9PiAobm9kZS5jbGFzc05hbWUgfHwgJycpLmluZGV4T2YoJ25nLWRpcnR5JykgPiAtMSkgPiAtMTtcbn1cbiJdfQ==