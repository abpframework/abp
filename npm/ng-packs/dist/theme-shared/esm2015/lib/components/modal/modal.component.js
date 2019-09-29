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
        this.show = new EventEmitter();
        this.hide = new EventEmitter();
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
            this.show.emit();
        }
        else {
            this.closable = false;
            this.renderer.removeClass(document.body, 'modal-open');
            this.hide.emit();
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
    abpButtons: [{ type: ViewChildren, args: ['abp-button',] }],
    show: [{ type: Output }],
    hide: [{ type: Output }]
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
    ModalComponent.prototype.show;
    /** @type {?} */
    ModalComponent.prototype.hide;
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLFVBQVUsRUFDVixZQUFZLEVBQ1osS0FBSyxFQUVMLE1BQU0sRUFDTixTQUFTLEVBQ1QsV0FBVyxFQUNYLFNBQVMsRUFDVCxZQUFZLEdBQ2IsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLFNBQVMsRUFBRSxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2pELE9BQU8sRUFBRSxNQUFNLEVBQUUsSUFBSSxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUV2RSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxxQ0FBcUMsQ0FBQztBQUMxRSxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7O01BSXZELGlCQUFpQixHQUFHLEdBQUc7QUFNN0IsTUFBTSxPQUFPLGNBQWM7Ozs7O0lBeUZ6QixZQUFvQixRQUFtQixFQUFVLG1CQUF3QztRQUFyRSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBQVUsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQTlDaEYsYUFBUSxHQUFZLEtBQUssQ0FBQztRQUUxQixlQUFVLEdBQVcsRUFBRSxDQUFDO1FBRXhCLFNBQUksR0FBYyxJQUFJLENBQUM7UUFNdEIsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBRTVDLFNBQUksR0FBRyxJQUFJLFlBQVksRUFBUSxDQUFDO1FBaUIxQyxTQUFJLEdBQUcsSUFBSSxZQUFZLEVBQUUsQ0FBQztRQUcxQixTQUFJLEdBQUcsSUFBSSxZQUFZLEVBQUUsQ0FBQztRQUUxQixhQUFRLEdBQVksS0FBSyxD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TyxLQUFLLENBQUMsU0FBUzs7OztJQUFDLElBQUksQ0FBQyxFQUFFLENBQUMsQ0FBQyxJQUFJLENBQUMsU0FBUyxJQUFJLEVBQUUsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUMsRUFBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO0FBQ3ZGLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBDb21wb25lbnQsXG4gIENvbnRlbnRDaGlsZCxcbiAgRWxlbWVudFJlZixcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25EZXN0cm95LFxuICBPdXRwdXQsXG4gIFJlbmRlcmVyMixcbiAgVGVtcGxhdGVSZWYsXG4gIFZpZXdDaGlsZCxcbiAgVmlld0NoaWxkcmVuLFxufSBmcm9tICdAYW5ndWxhci9jb3JlJztcbmltcG9ydCB7IGZyb21FdmVudCwgU3ViamVjdCwgdGltZXIgfSBmcm9tICdyeGpzJztcbmltcG9ydCB7IGZpbHRlciwgdGFrZSwgdGFrZVVudGlsLCBkZWJvdW5jZVRpbWUgfSBmcm9tICdyeGpzL29wZXJhdG9ycyc7XG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RvYXN0ZXInO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2NvbmZpcm1hdGlvbi5zZXJ2aWNlJztcbmltcG9ydCB7IEJ1dHRvbkNvbXBvbmVudCB9IGZyb20gJy4uL2J1dHRvbi9idXR0b24uY29tcG9uZW50JztcblxuZXhwb3J0IHR5cGUgTW9kYWxTaXplID0gJ3NtJyB8ICdtZCcgfCAnbGcnIHwgJ3hsJztcblxuY29uc3QgQU5JTUFUSU9OX1RJTUVPVVQgPSAyMDA7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1tb2RhbCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9tb2RhbC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIE1vZGFsQ29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcbiAgQElucHV0KClcbiAgZ2V0IHZpc2libGUoKTogYm9vbGVhbiB7XG4gICAgcmV0dXJuIHRoaXMuX3Zpc2libGU7XG4gIH1cbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcbiAgICBpZiAodHlwZW9mIHZhbHVlICE9PSAnYm9vbGVhbicpIHJldHVybjtcblxuICAgIGlmICghdGhpcy5tb2RhbENvbnRlbnQpIHtcbiAgICAgIGlmICh2YWx1ZSkge1xuICAgICAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgICAgICB0aGlzLnNob3dNb2RhbCA9IHZhbHVlO1xuICAgICAgICAgIHRoaXMudmlzaWJsZSA9IHZhbHVlO1xuICAgICAgICB9LCAwKTtcbiAgICAgIH1cbiAgICAgIHJldHVybjtcbiAgICB9XG5cbiAgICBpZiAodmFsdWUpIHtcbiAgICAgIHRoaXMuc2V0VmlzaWJsZSh2YWx1ZSk7XG4gICAgICB0aGlzLmxpc3RlbigpO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLmNsb3NhYmxlID0gZmFsc2U7XG4gICAgICB0aGlzLnJlbmRlcmVyLmFkZENsYXNzKHRoaXMubW9kYWxDb250ZW50Lm5hdGl2ZUVsZW1lbnQsICdmYWRlLW91dC10b3AnKTtcbiAgICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgICB0aGlzLnNldFZpc2libGUodmFsdWUpO1xuICAgICAgICB0aGlzLm5nT25EZXN0cm95KCk7XG4gICAgICB9LCBBTklNQVRJT05fVElNRU9VVCAtIDEwKTtcbiAgICB9XG4gIH1cblxuICBASW5wdXQoKVxuICBnZXQgYnVzeSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fYnVzeTtcbiAgfVxuICBzZXQgYnVzeSh2YWx1ZTogYm9vbGVhbikge1xuICAgIGlmICh0aGlzLmFicFN1Ym1pdCAmJiB0aGlzLmFicFN1Ym1pdCBpbnN0YW5jZW9mIEJ1dHRvbkNvbXBvbmVudCkge1xuICAgICAgdGhpcy5hYnBTdWJtaXQubG9hZGluZyA9IHZhbHVlO1xuICAgIH1cblxuICAgIHRoaXMuX2J1c3kgPSB2YWx1ZTtcbiAgfVxuXG4gIEBJbnB1dCgpIGNlbnRlcmVkOiBib29sZWFuID0gZmFsc2U7XG5cbiAgQElucHV0KCkgbW9kYWxDbGFzczogc3RyaW5nID0gJyc7XG5cbiAgQElucHV0KCkgc2l6ZTogTW9kYWxTaXplID0gJ2xnJztcblxuICBASW5wdXQoKSBoZWlnaHQ6IG51bWJlcjtcblxuICBASW5wdXQoKSBtaW5IZWlnaHQ6IG51bWJlcjtcblxuICBAT3V0cHV0KCkgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcblxuICBAT3V0cHV0KCkgaW5pdCA9IG5ldyBFdmVudEVtaXR0ZXI8dm9pZD4oKTtcblxuICBAQ29udGVudENoaWxkKCdhYnBIZWFkZXInLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwSGVhZGVyOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEJvZHknLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwQm9keTogVGVtcGxhdGVSZWY8YW55PjtcblxuICBAQ29udGVudENoaWxkKCdhYnBGb290ZXInLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwRm9vdGVyOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicENsb3NlJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBFbGVtZW50UmVmIH0pIGFicENsb3NlOiBFbGVtZW50UmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZChCdXR0b25Db21wb25lbnQsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogQnV0dG9uQ29tcG9uZW50IH0pIGFicFN1Ym1pdDogQnV0dG9uQ29tcG9uZW50O1xuXG4gIEBWaWV3Q2hpbGQoJ2FicE1vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KSBtb2RhbENvbnRlbnQ6IEVsZW1lbnRSZWY7XG5cbiAgQFZpZXdDaGlsZHJlbignYWJwLWJ1dHRvbicpIGFicEJ1dHRvbnM7XG5cbiAgQE91dHB1dCgpXG4gIHNob3cgPSBuZXcgRXZlbnRFbWl0dGVyKCk7XG5cbiAgQE91dHB1dCgpXG4gIGhpZGUgPSBuZXcgRXZlbnRFbWl0dGVyKCk7XG5cbiAgX3Zpc2libGU6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBfYnVzeTogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIHNob3dNb2RhbDogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGlzT3BlbkNvbmZpcm1hdGlvbjogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGNsb3NhYmxlOiBib29sZWFuID0gZmFsc2U7XG5cbiAgZGVzdHJveSQgPSBuZXcgU3ViamVjdDx2b2lkPigpO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMiwgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlKSB7fVxuXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge1xuICAgIHRoaXMuZGVzdHJveSQubmV4dCgpO1xuICB9XG5cbiAgc2V0VmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gICAgdGhpcy5zaG93TW9kYWwgPSB2YWx1ZTtcblxuICAgIGlmICh2YWx1ZSkge1xuICAgICAgdGltZXIoQU5JTUFUSU9OX1RJTUVPVVQgKyAxMDApXG4gICAgICAgIC5waXBlKHRha2UoMSkpXG4gICAgICAgIC5zdWJzY3JpYmUoXyA9PiAodGhpcy5jbG9zYWJsZSA9IHRydWUpKTtcblxuICAgICAgdGhpcy5yZW5kZXJlci5hZGRDbGFzcyhkb2N1bWVudC5ib2R5LCAnbW9kYWwtb3BlbicpO1xuICAgICAgdGhpcy5zaG93LmVtaXQoKTtcbiAgICB9IGVsc2Uge1xuICAgICAgdGhpcy5jbG9zYWJsZSA9IGZhbHNlO1xuICAgICAgdGhpcy5yZW5kZXJlci5yZW1vdmVDbGFzcyhkb2N1bWVudC5ib2R5LCAnbW9kYWwtb3BlbicpO1xuICAgICAgdGhpcy5oaWRlLmVtaXQoKTtcbiAgICB9XG4gIH1cblxuICBsaXN0ZW4oKSB7XG4gICAgZnJvbUV2ZW50KGRvY3VtZW50LCAna2V5dXAnKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHRha2VVbnRpbCh0aGlzLmRlc3Ryb3kkKSxcbiAgICAgICAgZGVib3VuY2VUaW1lKDE1MCksXG4gICAgICAgIGZpbHRlcigoa2V5OiBLZXlib2FyZEV2ZW50KSA9PiBrZXkgJiYga2V5LmNvZGUgPT09ICdFc2NhcGUnICYmIHRoaXMuY2xvc2FibGUpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShfID0+IHtcbiAgICAgICAgdGhpcy5jbG9zZSgpO1xuICAgICAgfSk7XG5cbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIGlmICghdGhpcy5hYnBDbG9zZSkgcmV0dXJuO1xuICAgICAgZnJvbUV2ZW50KHRoaXMuYWJwQ2xvc2UubmF0aXZlRWxlbWVudCwgJ2NsaWNrJylcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgdGFrZVVudGlsKHRoaXMuZGVzdHJveSQpLFxuICAgICAgICAgIGZpbHRlcigoKSA9PiAhISh0aGlzLmNsb3NhYmxlICYmIHRoaXMubW9kYWxDb250ZW50KSksXG4gICAgICAgIClcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB0aGlzLmNsb3NlKCkpO1xuICAgIH0sIDApO1xuXG4gICAgdGhpcy5pbml0LmVtaXQoKTtcbiAgfVxuXG4gIGNsb3NlKCkge1xuICAgIGlmICghdGhpcy5jbG9zYWJsZSB8fCB0aGlzLmJ1c3kpIHJldHVybjtcblxuICAgIGNvbnN0IG5vZGVzID0gZ2V0RmxhdE5vZGVzKFxuICAgICAgKHRoaXMubW9kYWxDb250ZW50Lm5hdGl2ZUVsZW1lbnQucXVlcnlTZWxlY3RvcignI2FicC1tb2RhbC1ib2R5JykgYXMgSFRNTEVsZW1lbnQpLmNoaWxkTm9kZXMsXG4gICAgKTtcblxuICAgIGlmIChoYXNOZ0RpcnR5KG5vZGVzKSkge1xuICAgICAgaWYgKHRoaXMuaXNPcGVuQ29uZmlybWF0aW9uKSByZXR1cm47XG5cbiAgICAgIHRoaXMuaXNPcGVuQ29uZmlybWF0aW9uID0gdHJ1ZTtcbiAgICAgIHRoaXMuY29uZmlybWF0aW9uU2VydmljZVxuICAgICAgICAud2FybignQWJwQWNjb3VudDo6QXJlWW91U3VyZVlvdVdhbnRUb0NhbmNlbEVkaXRpbmdXYXJuaW5nTWVzc2FnZScsICdBYnBBY2NvdW50OjpBcmVZb3VTdXJlJylcbiAgICAgICAgLnN1YnNjcmliZSgoc3RhdHVzOiBUb2FzdGVyLlN0YXR1cykgPT4ge1xuICAgICAgICAgIHRpbWVyKEFOSU1BVElPTl9USU1FT1VUKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgICAgdGhpcy5pc09wZW5Db25maXJtYXRpb24gPSBmYWxzZTtcbiAgICAgICAgICB9KTtcblxuICAgICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICAgIH1cbiAgICAgICAgfSk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgIH1cbiAgfVxufVxuXG5mdW5jdGlvbiBnZXRGbGF0Tm9kZXMobm9kZXM6IE5vZGVMaXN0KTogSFRNTEVsZW1lbnRbXSB7XG4gIHJldHVybiBBcnJheS5mcm9tKG5vZGVzKS5yZWR1Y2UoXG4gICAgKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi4odmFsLmNoaWxkTm9kZXMgJiYgdmFsLmNoaWxkTm9kZXMubGVuZ3RoID8gZ2V0RmxhdE5vZGVzKHZhbC5jaGlsZE5vZGVzKSA6IFt2YWxdKV0sXG4gICAgW10sXG4gICk7XG59XG5cbmZ1bmN0aW9uIGhhc05nRGlydHkobm9kZXM6IEhUTUxFbGVtZW50W10pIHtcbiAgcmV0dXJuIG5vZGVzLmZpbmRJbmRleChub2RlID0+IChub2RlLmNsYXNzTmFtZSB8fCAnJykuaW5kZXhPZignbmctZGlydHknKSA+IC0xKSA+IC0xO1xufVxuIl19