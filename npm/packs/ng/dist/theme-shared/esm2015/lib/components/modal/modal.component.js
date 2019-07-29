/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component, ContentChild, ElementRef, EventEmitter, Input, Output, Renderer2, TemplateRef, ViewChild, } from '@angular/core';
import { fromEvent, Subject, timer } from 'rxjs';
import { debounceTime, filter, take, takeUntil } from 'rxjs/operators';
import { ConfirmationService } from '../../services/confirmation.service';
export class ModalComponent {
    /**
     * @param {?} renderer
     * @param {?} confirmationService
     */
    constructor(renderer, confirmationService) {
        this.renderer = renderer;
        this.confirmationService = confirmationService;
        this.centered = true;
        this.modalClass = '';
        this.size = 'lg';
        this.visibleChange = new EventEmitter();
        this._visible = false;
        this.closable = false;
        this.isOpenConfirmation = false;
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
        if (!this.modalContent) {
            setTimeout((/**
             * @return {?}
             */
            () => (this.visible = value)), 0);
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
                this.renderer.removeClass(this.modalContent.nativeElement, 'fade-out-top');
                this.ngOnDestroy();
            }), 350);
        }
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
        value
            ? timer(500)
                .pipe(take(1))
                .subscribe((/**
             * @param {?} _
             * @return {?}
             */
            _ => (this.closable = true)))
            : (this.closable = false);
    }
    /**
     * @return {?}
     */
    listen() {
        fromEvent(document, 'click')
            .pipe(debounceTime(350), takeUntil(this.destroy$), filter((/**
         * @param {?} event
         * @return {?}
         */
        (event) => event &&
            this.closable &&
            this.modalContent &&
            !this.isOpenConfirmation &&
            !this.modalContent.nativeElement.contains(event.target))))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        _ => {
            this.close();
        }));
        fromEvent(document, 'keyup')
            .pipe(takeUntil(this.destroy$), filter((/**
         * @param {?} key
         * @return {?}
         */
        (key) => key && key.code === 'Escape' && this.closable)), debounceTime(350))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        _ => {
            this.close();
        }));
        fromEvent(this.abpClose.nativeElement, 'click')
            .pipe(takeUntil(this.destroy$), filter((/**
         * @return {?}
         */
        () => !!(this.closable && this.modalContent))), debounceTime(350))
            .subscribe((/**
         * @return {?}
         */
        () => this.close()));
    }
    /**
     * @return {?}
     */
    close() {
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
                timer(400).subscribe((/**
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
                template: "<div\n  id=\"abp-modal\"\n  tabindex=\"-1\"\n  class=\"modal fade {{ modalClass }}\"\n  [class.show]=\"visible\"\n  [style.display]=\"visible ? 'block' : 'none'\"\n  [style.padding-right.px]=\"'15'\"\n>\n  <div\n    id=\"abp-modal-container\"\n    class=\"modal-dialog modal-{{ size }} fade-in-top\"\n    [class.modal-dialog-centered]=\"centered\"\n    #abpModalContent\n  >\n    <div #content id=\"abp-modal-content\" class=\"modal-content\">\n      <div id=\"abp-modal-header\" class=\"modal-header\">\n        <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\n\n        <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" (click)=\"close()\">\n          <span aria-hidden=\"true\">&times;</span>\n        </button>\n      </div>\n      <div id=\"abp-modal-body\" class=\"modal-body\">\n        <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\n\n        <div id=\"abp-modal-footer\" class=\"modal-footer\">\n          <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\n        </div>\n      </div>\n    </div>\n  </div>\n\n  <ng-content></ng-content>\n</div>\n"
            }] }
];
/** @nocollapse */
ModalComponent.ctorParameters = () => [
    { type: Renderer2 },
    { type: ConfirmationService }
];
ModalComponent.propDecorators = {
    visible: [{ type: Input }],
    centered: [{ type: Input }],
    modalClass: [{ type: Input }],
    size: [{ type: Input }],
    visibleChange: [{ type: Output }],
    abpHeader: [{ type: ContentChild, args: ['abpHeader', { static: false },] }],
    abpBody: [{ type: ContentChild, args: ['abpBody', { static: false },] }],
    abpFooter: [{ type: ContentChild, args: ['abpFooter', { static: false },] }],
    abpClose: [{ type: ContentChild, args: ['abpClose', { static: false, read: ElementRef },] }],
    modalContent: [{ type: ViewChild, args: ['abpModalContent', { static: false },] }]
};
if (false) {
    /** @type {?} */
    ModalComponent.prototype.centered;
    /** @type {?} */
    ModalComponent.prototype.modalClass;
    /** @type {?} */
    ModalComponent.prototype.size;
    /** @type {?} */
    ModalComponent.prototype.visibleChange;
    /** @type {?} */
    ModalComponent.prototype.abpHeader;
    /** @type {?} */
    ModalComponent.prototype.abpBody;
    /** @type {?} */
    ModalComponent.prototype.abpFooter;
    /** @type {?} */
    ModalComponent.prototype.abpClose;
    /** @type {?} */
    ModalComponent.prototype.modalContent;
    /** @type {?} */
    ModalComponent.prototype._visible;
    /** @type {?} */
    ModalComponent.prototype.closable;
    /** @type {?} */
    ModalComponent.prototype.isOpenConfirmation;
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
    (acc, val) => [...acc, ...(val.childNodes && val.childNodes.length ? Array.from(val.childNodes) : [val])]), []);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLFVBQVUsRUFDVixZQUFZLEVBQ1osS0FBSyxFQUVMLE1BQU0sRUFDTixTQUFTLEVBQ1QsV0FBVyxFQUNYLFNBQVMsR0FDVixNQUFNLGVBQWUsQ0FBQztBQUN2QixPQUFPLEVBQUUsU0FBUyxFQUFFLE9BQU8sRUFBRSxLQUFLLEVBQUUsTUFBTSxNQUFNLENBQUM7QUFDakQsT0FBTyxFQUFFLFlBQVksRUFBRSxNQUFNLEVBQUUsSUFBSSxFQUFFLFNBQVMsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBQ3ZFLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBUzFFLE1BQU0sT0FBTyxjQUFjOzs7OztJQW1EekIsWUFBb0IsUUFBbUIsRUFBVSxtQkFBd0M7UUFBckUsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQUFVLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7UUExQmhGLGFBQVEsR0FBWSxJQUFJLENBQUM7UUFFekIsZUFBVSxHQUFXLEVBQUUsQ0FBQztRQUV4QixTQUFJLEdBQWMsSUFBSSxDQUFDO1FBRXRCLGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQVl0RCxhQUFRLEdBQVksS0FBSyxDQUFDO1FBRTFCLGFBQVEsR0FBWSxLQUFLLENBQUM7UUFFMUIsdUJBQWtCLEdBQVksS0FBSyxDQUFDO1FBRXBDLGFBQVEsR0FBRyxJQUFJLE9BQU8sRUFBUSxDQUFDO0lBRTZELENBQUM7Ozs7SUFsRDdGLElBQ0ksT0FBTztRQUNULE9BQU8sSUFBSSxDQUFDLFFBQVEsQ0FBQztJQUN2QixDQUFDOzs7OztJQUNELElBQUksT0FBTyxD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QUVELFNBQVMsVUFBVSxDQUFDLEtBQW9CO0lBQ3RDLE9BQU8sS0FBSyxDQUFDLFNBQVM7Ozs7SUFBQyxJQUFJLENBQUMsRUFBRSxDQUFDLENBQUMsSUFBSSxDQUFDLFNBQVMsSUFBSSxFQUFFLENBQUMsQ0FBQyxPQUFPLENBQUMsVUFBVSxDQUFDLEdBQUcsQ0FBQyxDQUFDLEVBQUMsR0FBRyxDQUFDLENBQUMsQ0FBQztBQUN2RixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtcbiAgQ29tcG9uZW50LFxuICBDb250ZW50Q2hpbGQsXG4gIEVsZW1lbnRSZWYsXG4gIEV2ZW50RW1pdHRlcixcbiAgSW5wdXQsXG4gIE9uRGVzdHJveSxcbiAgT3V0cHV0LFxuICBSZW5kZXJlcjIsXG4gIFRlbXBsYXRlUmVmLFxuICBWaWV3Q2hpbGQsXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50LCBTdWJqZWN0LCB0aW1lciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZGVib3VuY2VUaW1lLCBmaWx0ZXIsIHRha2UsIHRha2VVbnRpbCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZSc7XG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RvYXN0ZXInO1xuXG5leHBvcnQgdHlwZSBNb2RhbFNpemUgPSAnc20nIHwgJ21kJyB8ICdsZycgfCAneGwnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbW9kYWwnLFxuICB0ZW1wbGF0ZVVybDogJy4vbW9kYWwuY29tcG9uZW50Lmh0bWwnLFxufSlcbmV4cG9ydCBjbGFzcyBNb2RhbENvbXBvbmVudCBpbXBsZW1lbnRzIE9uRGVzdHJveSB7XG4gIEBJbnB1dCgpXG4gIGdldCB2aXNpYmxlKCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiB0aGlzLl92aXNpYmxlO1xuICB9XG4gIHNldCB2aXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XG4gICAgaWYgKCF0aGlzLm1vZGFsQ29udGVudCkge1xuICAgICAgc2V0VGltZW91dCgoKSA9PiAodGhpcy52aXNpYmxlID0gdmFsdWUpLCAwKTtcbiAgICAgIHJldHVybjtcbiAgICB9XG5cbiAgICBpZiAodmFsdWUpIHtcbiAgICAgIHRoaXMuc2V0VmlzaWJsZSh2YWx1ZSk7XG4gICAgICB0aGlzLmxpc3RlbigpO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLmNsb3NhYmxlID0gZmFsc2U7XG4gICAgICB0aGlzLnJlbmRlcmVyLmFkZENsYXNzKHRoaXMubW9kYWxDb250ZW50Lm5hdGl2ZUVsZW1lbnQsICdmYWRlLW91dC10b3AnKTtcbiAgICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgICB0aGlzLnNldFZpc2libGUodmFsdWUpO1xuICAgICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNsYXNzKHRoaXMubW9kYWxDb250ZW50Lm5hdGl2ZUVsZW1lbnQsICdmYWRlLW91dC10b3AnKTtcbiAgICAgICAgdGhpcy5uZ09uRGVzdHJveSgpO1xuICAgICAgfSwgMzUwKTtcbiAgICB9XG4gIH1cblxuICBASW5wdXQoKSBjZW50ZXJlZDogYm9vbGVhbiA9IHRydWU7XG5cbiAgQElucHV0KCkgbW9kYWxDbGFzczogc3RyaW5nID0gJyc7XG5cbiAgQElucHV0KCkgc2l6ZTogTW9kYWxTaXplID0gJ2xnJztcblxuICBAT3V0cHV0KCkgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcblxuICBAQ29udGVudENoaWxkKCdhYnBIZWFkZXInLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwSGVhZGVyOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEJvZHknLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwQm9keTogVGVtcGxhdGVSZWY8YW55PjtcblxuICBAQ29udGVudENoaWxkKCdhYnBGb290ZXInLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwRm9vdGVyOiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicENsb3NlJywgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBFbGVtZW50UmVmIH0pIGFicENsb3NlOiBFbGVtZW50UmVmPGFueT47XG5cbiAgQFZpZXdDaGlsZCgnYWJwTW9kYWxDb250ZW50JywgeyBzdGF0aWM6IGZhbHNlIH0pIG1vZGFsQ29udGVudDogRWxlbWVudFJlZjtcblxuICBfdmlzaWJsZTogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIGNsb3NhYmxlOiBib29sZWFuID0gZmFsc2U7XG5cbiAgaXNPcGVuQ29uZmlybWF0aW9uOiBib29sZWFuID0gZmFsc2U7XG5cbiAgZGVzdHJveSQgPSBuZXcgU3ViamVjdDx2b2lkPigpO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMiwgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlKSB7fVxuXG4gIG5nT25EZXN0cm95KCk6IHZvaWQge1xuICAgIHRoaXMuZGVzdHJveSQubmV4dCgpO1xuICB9XG5cbiAgc2V0VmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG4gICAgdmFsdWVcbiAgICAgID8gdGltZXIoNTAwKVxuICAgICAgICAgIC5waXBlKHRha2UoMSkpXG4gICAgICAgICAgLnN1YnNjcmliZShfID0+ICh0aGlzLmNsb3NhYmxlID0gdHJ1ZSkpXG4gICAgICA6ICh0aGlzLmNsb3NhYmxlID0gZmFsc2UpO1xuICB9XG5cbiAgbGlzdGVuKCkge1xuICAgIGZyb21FdmVudChkb2N1bWVudCwgJ2NsaWNrJylcbiAgICAgIC5waXBlKFxuICAgICAgICBkZWJvdW5jZVRpbWUoMzUwKSxcbiAgICAgICAgdGFrZVVudGlsKHRoaXMuZGVzdHJveSQpLFxuICAgICAgICBmaWx0ZXIoXG4gICAgICAgICAgKGV2ZW50OiBNb3VzZUV2ZW50KSA9PlxuICAgICAgICAgICAgZXZlbnQgJiZcbiAgICAgICAgICAgIHRoaXMuY2xvc2FibGUgJiZcbiAgICAgICAgICAgIHRoaXMubW9kYWxDb250ZW50ICYmXG4gICAgICAgICAgICAhdGhpcy5pc09wZW5Db25maXJtYXRpb24gJiZcbiAgICAgICAgICAgICF0aGlzLm1vZGFsQ29udGVudC5uYXRpdmVFbGVtZW50LmNvbnRhaW5zKGV2ZW50LnRhcmdldCksXG4gICAgICAgICksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKF8gPT4ge1xuICAgICAgICB0aGlzLmNsb3NlKCk7XG4gICAgICB9KTtcblxuICAgIGZyb21FdmVudChkb2N1bWVudCwgJ2tleXVwJylcbiAgICAgIC5waXBlKFxuICAgICAgICB0YWtlVW50aWwodGhpcy5kZXN0cm95JCksXG4gICAgICAgIGZpbHRlcigoa2V5OiBLZXlib2FyZEV2ZW50KSA9PiBrZXkgJiYga2V5LmNvZGUgPT09ICdFc2NhcGUnICYmIHRoaXMuY2xvc2FibGUpLFxuICAgICAgICBkZWJvdW5jZVRpbWUoMzUwKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoXyA9PiB7XG4gICAgICAgIHRoaXMuY2xvc2UoKTtcbiAgICAgIH0pO1xuXG4gICAgZnJvbUV2ZW50KHRoaXMuYWJwQ2xvc2UubmF0aXZlRWxlbWVudCwgJ2NsaWNrJylcbiAgICAgIC5waXBlKFxuICAgICAgICB0YWtlVW50aWwodGhpcy5kZXN0cm95JCksXG4gICAgICAgIGZpbHRlcigoKSA9PiAhISh0aGlzLmNsb3NhYmxlICYmIHRoaXMubW9kYWxDb250ZW50KSksXG4gICAgICAgIGRlYm91bmNlVGltZSgzNTApLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZSgoKSA9PiB0aGlzLmNsb3NlKCkpO1xuICB9XG5cbiAgY2xvc2UoKSB7XG4gICAgY29uc3Qgbm9kZXMgPSBnZXRGbGF0Tm9kZXMoXG4gICAgICAodGhpcy5tb2RhbENvbnRlbnQubmF0aXZlRWxlbWVudC5xdWVyeVNlbGVjdG9yKCcjYWJwLW1vZGFsLWJvZHknKSBhcyBIVE1MRWxlbWVudCkuY2hpbGROb2RlcyxcbiAgICApO1xuXG4gICAgaWYgKGhhc05nRGlydHkobm9kZXMpKSB7XG4gICAgICBpZiAodGhpcy5pc09wZW5Db25maXJtYXRpb24pIHJldHVybjtcblxuICAgICAgdGhpcy5pc09wZW5Db25maXJtYXRpb24gPSB0cnVlO1xuICAgICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAgIC53YXJuKCdBYnBBY2NvdW50OjpBcmVZb3VTdXJlWW91V2FudFRvQ2FuY2VsRWRpdGluZ1dhcm5pbmdNZXNzYWdlJywgJ0FicEFjY291bnQ6OkFyZVlvdVN1cmUnKVxuICAgICAgICAuc3Vic2NyaWJlKChzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSA9PiB7XG4gICAgICAgICAgdGltZXIoNDAwKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgICAgdGhpcy5pc09wZW5Db25maXJtYXRpb24gPSBmYWxzZTtcbiAgICAgICAgICB9KTtcblxuICAgICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICAgIH1cbiAgICAgICAgfSk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgIH1cbiAgfVxufVxuXG5mdW5jdGlvbiBnZXRGbGF0Tm9kZXMobm9kZXM6IE5vZGVMaXN0KTogSFRNTEVsZW1lbnRbXSB7XG4gIHJldHVybiBBcnJheS5mcm9tKG5vZGVzKS5yZWR1Y2UoXG4gICAgKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi4odmFsLmNoaWxkTm9kZXMgJiYgdmFsLmNoaWxkTm9kZXMubGVuZ3RoID8gQXJyYXkuZnJvbSh2YWwuY2hpbGROb2RlcykgOiBbdmFsXSldLFxuICAgIFtdLFxuICApO1xufVxuXG5mdW5jdGlvbiBoYXNOZ0RpcnR5KG5vZGVzOiBIVE1MRWxlbWVudFtdKSB7XG4gIHJldHVybiBub2Rlcy5maW5kSW5kZXgobm9kZSA9PiAobm9kZS5jbGFzc05hbWUgfHwgJycpLmluZGV4T2YoJ25nLWRpcnR5JykgPiAtMSkgPiAtMTtcbn1cbiJdfQ==