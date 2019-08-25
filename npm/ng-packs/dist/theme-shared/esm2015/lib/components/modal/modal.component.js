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
        value
            ? timer(ANIMATION_TIMEOUT + 100)
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
                template: "<div\n  *ngIf=\"showModal\"\n  (click)=\"close()\"\n  id=\"abp-modal\"\n  class=\"modal fade {{ modalClass }} d-block show\"\n  [style.padding-right.px]=\"'15'\"\n>\n  <div\n    id=\"abp-modal-container\"\n    class=\"modal-dialog modal-{{ size }} fade-in-top\"\n    tabindex=\"-1\"\n    [class.modal-dialog-centered]=\"centered\"\n    #abpModalContent\n  >\n    <div #content id=\"abp-modal-content\" class=\"modal-content\" (click)=\"$event.stopPropagation()\">\n      <div id=\"abp-modal-header\" class=\"modal-header\">\n        <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\n\n        <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" (click)=\"close()\">\n          <span aria-hidden=\"true\">&times;</span>\n        </button>\n      </div>\n      <div\n        id=\"abp-modal-body\"\n        class=\"modal-body\"\n        [style.height]=\"height || undefined\"\n        [style.minHeight]=\"minHeight || undefined\"\n      >\n        <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\n\n        <div id=\"abp-modal-footer\" class=\"modal-footer\">\n          <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\n        </div>\n      </div>\n    </div>\n  </div>\n\n  <ng-content></ng-content>\n</div>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFDTCxTQUFTLEVBQ1QsWUFBWSxFQUNaLFVBQVUsRUFDVixZQUFZLEVBQ1osS0FBSyxFQUVMLE1BQU0sRUFDTixTQUFTLEVBQ1QsV0FBVyxFQUNYLFNBQVMsRUFDVCxZQUFZLEdBQ2IsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLFNBQVMsRUFBRSxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2pELE9BQU8sRUFBRSxNQUFNLEVBQUUsSUFBSSxFQUFFLFNBQVMsRUFBRSxZQUFZLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUV2RSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxxQ0FBcUMsQ0FBQztBQUMxRSxPQUFPLEVBQUUsZUFBZSxFQUFFLE1BQU0sNEJBQTRCLENBQUM7O01BSXZELGlCQUFpQixHQUFHLEdBQUc7QUFNN0IsTUFBTSxPQUFPLGNBQWM7Ozs7O0lBbUZ6QixZQUFvQixRQUFtQixFQUFVLG1CQUF3QztRQUFyRSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBQVUsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQXhDaEYsYUFBUSxHQUFZLEtBQUssQ0FBQztRQUUxQixlQUFVLEdBQVcsRUFBRSxDQUFDO1FBRXhCLFNBQUksR0FBYyxJQUFJLENBQUM7UUFNdEIsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBRTVDLFNBQUksR0FBRyxJQUFJLFlBQVksRUFBUSxDQUFDO1FBZ0IxQyxhQUFRLEdBQVksS0FBSyxDQUFDO1FBRTFCLFVBQUssR0FBWSxLQUFLLENBQUM7UUFFdkIsY0FBUyxHQUFZLEtBQUssQ0FBQztRQUUzQix1QkFBa0IsR0FB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RSxpQ0FBc0U7O0lBRXRFLG1DQUEwRTs7SUFFMUUsa0NBQXlGOztJQUV6RixtQ0FBb0c7O0lBRXBHLHNDQUEwRTs7SUFFMUUsb0NBQXVDOztJQUV2QyxrQ0FBMEI7O0lBRTFCLCtCQUF1Qjs7SUFFdkIsbUNBQTJCOztJQUUzQiw0Q0FBb0M7O0lBRXBDLGtDQUEwQjs7SUFFMUIsa0NBQStCOzs7OztJQUVuQixrQ0FBMkI7Ozs7O0lBQUUsNkNBQWdEOzs7Ozs7QUFzRTNGLFNBQVMsWUFBWSxDQUFDLEtBQWU7SUFDbkMsT0FBTyxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDLE1BQU07Ozs7O0lBQzdCLENBQUMsR0FBRyxFQUFFLEdBQUcsRUFBRSxFQUFFLENBQUMsQ0FBQyxHQUFHLEdBQUcsRUFBRSxHQUFHLENBQUMsR0FBRyxDQUFDLFVBQVUsSUFBSSxHQUFHLENBQUMsVUFBVSxDQUFDLE1BQU0sQ0FBQyxDQUFDLENBQUMsWUFBWSxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxDQUFDLEdBQzNHLEVBQUUsQ0FDSCxDQUFDO0FBQ0osQ0FBQzs7Ozs7QUFFRCxTQUFTLFVBQVUsQ0FBQyxLQUFvQjtJQUN0QyxPQUFPLEtBQUssQ0FBQyxTQUFTOzs7O0lBQUMsSUFBSSxDQUFDLEVBQUUsQ0FBQyxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksRUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxFQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7QUFDdkYsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIENvbXBvbmVudCxcbiAgQ29udGVudENoaWxkLFxuICBFbGVtZW50UmVmLFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkRlc3Ryb3ksXG4gIE91dHB1dCxcbiAgUmVuZGVyZXIyLFxuICBUZW1wbGF0ZVJlZixcbiAgVmlld0NoaWxkLFxuICBWaWV3Q2hpbGRyZW4sXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50LCBTdWJqZWN0LCB0aW1lciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZmlsdGVyLCB0YWtlLCB0YWtlVW50aWwsIGRlYm91bmNlVGltZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi8uLi9tb2RlbHMvdG9hc3Rlcic7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvY29uZmlybWF0aW9uLnNlcnZpY2UnO1xuaW1wb3J0IHsgQnV0dG9uQ29tcG9uZW50IH0gZnJvbSAnLi4vYnV0dG9uL2J1dHRvbi5jb21wb25lbnQnO1xuXG5leHBvcnQgdHlwZSBNb2RhbFNpemUgPSAnc20nIHwgJ21kJyB8ICdsZycgfCAneGwnO1xuXG5jb25zdCBBTklNQVRJT05fVElNRU9VVCA9IDIwMDtcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLW1vZGFsJyxcbiAgdGVtcGxhdGVVcmw6ICcuL21vZGFsLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgTW9kYWxDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xuICBASW5wdXQoKVxuICBnZXQgdmlzaWJsZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcbiAgfVxuICBzZXQgdmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIGlmICh0eXBlb2YgdmFsdWUgIT09ICdib29sZWFuJykgcmV0dXJuO1xuXG4gICAgaWYgKCF0aGlzLm1vZGFsQ29udGVudCkge1xuICAgICAgaWYgKHZhbHVlKSB7XG4gICAgICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgICAgIHRoaXMuc2hvd01vZGFsID0gdmFsdWU7XG4gICAgICAgICAgdGhpcy52aXNpYmxlID0gdmFsdWU7XG4gICAgICAgIH0sIDApO1xuICAgICAgfVxuICAgICAgcmV0dXJuO1xuICAgIH1cblxuICAgIGlmICh2YWx1ZSkge1xuICAgICAgdGhpcy5zZXRWaXNpYmxlKHZhbHVlKTtcbiAgICAgIHRoaXMubGlzdGVuKCk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMuY2xvc2FibGUgPSBmYWxzZTtcbiAgICAgIHRoaXMucmVuZGVyZXIuYWRkQ2xhc3ModGhpcy5tb2RhbENvbnRlbnQubmF0aXZlRWxlbWVudCwgJ2ZhZGUtb3V0LXRvcCcpO1xuICAgICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICAgIHRoaXMuc2V0VmlzaWJsZSh2YWx1ZSk7XG4gICAgICAgIHRoaXMubmdPbkRlc3Ryb3koKTtcbiAgICAgIH0sIEFOSU1BVElPTl9USU1FT1VUIC0gMTApO1xuICAgIH1cbiAgfVxuXG4gIEBJbnB1dCgpXG4gIGdldCBidXN5KCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiB0aGlzLl9idXN5O1xuICB9XG4gIHNldCBidXN5KHZhbHVlOiBib29sZWFuKSB7XG4gICAgaWYgKHRoaXMuYWJwU3VibWl0ICYmIHRoaXMuYWJwU3VibWl0IGluc3RhbmNlb2YgQnV0dG9uQ29tcG9uZW50KSB7XG4gICAgICB0aGlzLmFicFN1Ym1pdC5sb2FkaW5nID0gdmFsdWU7XG4gICAgfVxuXG4gICAgdGhpcy5fYnVzeSA9IHZhbHVlO1xuICB9XG5cbiAgQElucHV0KCkgY2VudGVyZWQ6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBASW5wdXQoKSBtb2RhbENsYXNzOiBzdHJpbmcgPSAnJztcblxuICBASW5wdXQoKSBzaXplOiBNb2RhbFNpemUgPSAnbGcnO1xuXG4gIEBJbnB1dCgpIGhlaWdodDogbnVtYmVyO1xuXG4gIEBJbnB1dCgpIG1pbkhlaWdodDogbnVtYmVyO1xuXG4gIEBPdXRwdXQoKSB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBPdXRwdXQoKSBpbml0ID0gbmV3IEV2ZW50RW1pdHRlcjx2b2lkPigpO1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEhlYWRlcicsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBIZWFkZXI6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZCgnYWJwQm9keScsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBCb2R5OiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEZvb3RlcicsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBGb290ZXI6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZCgnYWJwQ2xvc2UnLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IEVsZW1lbnRSZWYgfSkgYWJwQ2xvc2U6IEVsZW1lbnRSZWY8YW55PjtcblxuICBAQ29udGVudENoaWxkKEJ1dHRvbkNvbXBvbmVudCwgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBCdXR0b25Db21wb25lbnQgfSkgYWJwU3VibWl0OiBCdXR0b25Db21wb25lbnQ7XG5cbiAgQFZpZXdDaGlsZCgnYWJwTW9kYWxDb250ZW50JywgeyBzdGF0aWM6IGZhbHNlIH0pIG1vZGFsQ29udGVudDogRWxlbWVudFJlZjtcblxuICBAVmlld0NoaWxkcmVuKCdhYnAtYnV0dG9uJykgYWJwQnV0dG9ucztcblxuICBfdmlzaWJsZTogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIF9idXN5OiBib29sZWFuID0gZmFsc2U7XG5cbiAgc2hvd01vZGFsOiBib29sZWFuID0gZmFsc2U7XG5cbiAgaXNPcGVuQ29uZmlybWF0aW9uOiBib29sZWFuID0gZmFsc2U7XG5cbiAgY2xvc2FibGU6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBkZXN0cm95JCA9IG5ldyBTdWJqZWN0PHZvaWQ+KCk7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyLCBwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UpIHt9XG5cbiAgbmdPbkRlc3Ryb3koKTogdm9pZCB7XG4gICAgdGhpcy5kZXN0cm95JC5uZXh0KCk7XG4gIH1cblxuICBzZXRWaXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XG4gICAgdGhpcy5fdmlzaWJsZSA9IHZhbHVlO1xuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcbiAgICB0aGlzLnNob3dNb2RhbCA9IHZhbHVlO1xuXG4gICAgdmFsdWVcbiAgICAgID8gdGltZXIoQU5JTUFUSU9OX1RJTUVPVVQgKyAxMDApXG4gICAgICAgICAgLnBpcGUodGFrZSgxKSlcbiAgICAgICAgICAuc3Vic2NyaWJlKF8gPT4gKHRoaXMuY2xvc2FibGUgPSB0cnVlKSlcbiAgICAgIDogKHRoaXMuY2xvc2FibGUgPSBmYWxzZSk7XG4gIH1cblxuICBsaXN0ZW4oKSB7XG4gICAgZnJvbUV2ZW50KGRvY3VtZW50LCAna2V5dXAnKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHRha2VVbnRpbCh0aGlzLmRlc3Ryb3kkKSxcbiAgICAgICAgZGVib3VuY2VUaW1lKDE1MCksXG4gICAgICAgIGZpbHRlcigoa2V5OiBLZXlib2FyZEV2ZW50KSA9PiBrZXkgJiYga2V5LmNvZGUgPT09ICdFc2NhcGUnICYmIHRoaXMuY2xvc2FibGUpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShfID0+IHtcbiAgICAgICAgdGhpcy5jbG9zZSgpO1xuICAgICAgfSk7XG5cbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIGlmICghdGhpcy5hYnBDbG9zZSkgcmV0dXJuO1xuICAgICAgZnJvbUV2ZW50KHRoaXMuYWJwQ2xvc2UubmF0aXZlRWxlbWVudCwgJ2NsaWNrJylcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgdGFrZVVudGlsKHRoaXMuZGVzdHJveSQpLFxuICAgICAgICAgIGZpbHRlcigoKSA9PiAhISh0aGlzLmNsb3NhYmxlICYmIHRoaXMubW9kYWxDb250ZW50KSksXG4gICAgICAgIClcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB0aGlzLmNsb3NlKCkpO1xuICAgIH0sIDApO1xuXG4gICAgdGhpcy5pbml0LmVtaXQoKTtcbiAgfVxuXG4gIGNsb3NlKCkge1xuICAgIGlmICghdGhpcy5jbG9zYWJsZSB8fCB0aGlzLmJ1c3kpIHJldHVybjtcblxuICAgIGNvbnN0IG5vZGVzID0gZ2V0RmxhdE5vZGVzKFxuICAgICAgKHRoaXMubW9kYWxDb250ZW50Lm5hdGl2ZUVsZW1lbnQucXVlcnlTZWxlY3RvcignI2FicC1tb2RhbC1ib2R5JykgYXMgSFRNTEVsZW1lbnQpLmNoaWxkTm9kZXMsXG4gICAgKTtcblxuICAgIGlmIChoYXNOZ0RpcnR5KG5vZGVzKSkge1xuICAgICAgaWYgKHRoaXMuaXNPcGVuQ29uZmlybWF0aW9uKSByZXR1cm47XG5cbiAgICAgIHRoaXMuaXNPcGVuQ29uZmlybWF0aW9uID0gdHJ1ZTtcbiAgICAgIHRoaXMuY29uZmlybWF0aW9uU2VydmljZVxuICAgICAgICAud2FybignQWJwQWNjb3VudDo6QXJlWW91U3VyZVlvdVdhbnRUb0NhbmNlbEVkaXRpbmdXYXJuaW5nTWVzc2FnZScsICdBYnBBY2NvdW50OjpBcmVZb3VTdXJlJylcbiAgICAgICAgLnN1YnNjcmliZSgoc3RhdHVzOiBUb2FzdGVyLlN0YXR1cykgPT4ge1xuICAgICAgICAgIHRpbWVyKEFOSU1BVElPTl9USU1FT1VUKS5zdWJzY3JpYmUoKCkgPT4ge1xuICAgICAgICAgICAgdGhpcy5pc09wZW5Db25maXJtYXRpb24gPSBmYWxzZTtcbiAgICAgICAgICB9KTtcblxuICAgICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICAgIH1cbiAgICAgICAgfSk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgIH1cbiAgfVxufVxuXG5mdW5jdGlvbiBnZXRGbGF0Tm9kZXMobm9kZXM6IE5vZGVMaXN0KTogSFRNTEVsZW1lbnRbXSB7XG4gIHJldHVybiBBcnJheS5mcm9tKG5vZGVzKS5yZWR1Y2UoXG4gICAgKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi4odmFsLmNoaWxkTm9kZXMgJiYgdmFsLmNoaWxkTm9kZXMubGVuZ3RoID8gZ2V0RmxhdE5vZGVzKHZhbC5jaGlsZE5vZGVzKSA6IFt2YWxdKV0sXG4gICAgW10sXG4gICk7XG59XG5cbmZ1bmN0aW9uIGhhc05nRGlydHkobm9kZXM6IEhUTUxFbGVtZW50W10pIHtcbiAgcmV0dXJuIG5vZGVzLmZpbmRJbmRleChub2RlID0+IChub2RlLmNsYXNzTmFtZSB8fCAnJykuaW5kZXhPZignbmctZGlydHknKSA+IC0xKSA+IC0xO1xufVxuIl19