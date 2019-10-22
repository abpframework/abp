/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, ContentChild, ElementRef, EventEmitter, Input, Output, Renderer2, TemplateRef, ViewChild, ViewChildren, } from '@angular/core';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, filter, takeUntil } from 'rxjs/operators';
import { ConfirmationService } from '../../services/confirmation.service';
import { ButtonComponent } from '../button/button.component';
import { fadeAnimation, dialogAnimation } from '../../animations/modal.animations';
var ModalComponent = /** @class */ (function () {
    function ModalComponent(renderer, confirmationService) {
        this.renderer = renderer;
        this.confirmationService = confirmationService;
        this.centered = false;
        this.modalClass = '';
        this.size = 'lg';
        this.visibleChange = new EventEmitter();
        this.init = new EventEmitter();
        this.appear = new EventEmitter();
        this.disappear = new EventEmitter();
        this._visible = false;
        this._busy = false;
        this.isModalOpen = false;
        this.isConfirmationOpen = false;
        this.destroy$ = new Subject();
    }
    Object.defineProperty(ModalComponent.prototype, "visible", {
        get: /**
         * @return {?}
         */
        function () {
            return this._visible;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            var _this = this;
            if (typeof value !== 'boolean')
                return;
            this.isModalOpen = value;
            this._visible = value;
            this.visibleChange.emit(value);
            if (value) {
                setTimeout((/**
                 * @return {?}
                 */
                function () { return _this.listen(); }), 0);
                this.renderer.addClass(document.body, 'modal-open');
                this.appear.emit();
            }
            else {
                this.renderer.removeClass(document.body, 'modal-open');
                this.disappear.emit();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ModalComponent.prototype, "busy", {
        get: /**
         * @return {?}
         */
        function () {
            return this._busy;
        },
        set: /**
         * @param {?} value
         * @return {?}
         */
        function (value) {
            if (this.abpSubmit && this.abpSubmit instanceof ButtonComponent) {
                this.abpSubmit.loading = value;
            }
            this._busy = value;
        },
        enumerable: true,
        configurable: true
    });
    /**
     * @return {?}
     */
    ModalComponent.prototype.ngOnDestroy = /**
     * @return {?}
     */
    function () {
        this.destroy$.next();
    };
    /**
     * @return {?}
     */
    ModalComponent.prototype.close = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (this.busy)
            return;
        /** @type {?} */
        var nodes = getFlatNodes(((/** @type {?} */ (this.modalContent.nativeElement.querySelector('#abp-modal-body')))).childNodes);
        if (hasNgDirty(nodes)) {
            if (this.isConfirmationOpen)
                return;
            this.isConfirmationOpen = true;
            this.confirmationService
                .warn('AbpAccount::AreYouSureYouWantToCancelEditingWarningMessage', 'AbpAccount::AreYouSure')
                .subscribe((/**
             * @param {?} status
             * @return {?}
             */
            function (status) {
                _this.isConfirmationOpen = false;
                if (status === "confirm" /* confirm */) {
                    _this.visible = false;
                }
            }));
        }
        else {
            this.visible = false;
        }
    };
    /**
     * @return {?}
     */
    ModalComponent.prototype.listen = /**
     * @return {?}
     */
    function () {
        var _this = this;
        fromEvent(document, 'keyup')
            .pipe(takeUntil(this.destroy$), debounceTime(150), filter((/**
         * @param {?} key
         * @return {?}
         */
        function (key) { return key && key.code === 'Escape'; })))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        function (_) {
            _this.close();
        }));
        setTimeout((/**
         * @return {?}
         */
        function () {
            if (!_this.abpClose)
                return;
            fromEvent(_this.abpClose.nativeElement, 'click')
                .pipe(takeUntil(_this.destroy$), filter((/**
             * @return {?}
             */
            function () { return !!_this.modalContent; })))
                .subscribe((/**
             * @return {?}
             */
            function () { return _this.close(); }));
        }), 0);
        this.init.emit();
    };
    ModalComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-modal',
                    template: "<ng-container *ngIf=\"visible\">\r\n  <div class=\"modal show {{ modalClass }}\" tabindex=\"-1\" role=\"dialog\">\r\n    <div class=\"modal-backdrop\" [@fade]=\"isModalOpen\" (click)=\"close()\"></div>\r\n    <div\r\n      id=\"abp-modal-dialog\"\r\n      class=\"modal-dialog modal-{{ size }}\"\r\n      role=\"document\"\r\n      [@dialog]=\"isModalOpen\"\r\n      #abpModalContent\r\n    >\r\n      <div id=\"abp-modal-content\" class=\"modal-content\">\r\n        <div id=\"abp-modal-header\" class=\"modal-header\">\r\n          <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\r\n          \u200B\r\n          <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" aria-label=\"Close\" (click)=\"close()\">\r\n            <span aria-hidden=\"true\">&times;</span>\r\n          </button>\r\n        </div>\r\n        <div id=\"abp-modal-body\" class=\"modal-body\">\r\n          <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\r\n        </div>\r\n        <div id=\"abp-modal-footer\" class=\"modal-footer\">\r\n          <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\r\n        </div>\r\n      </div>\r\n    </div>\r\n    <ng-content></ng-content>\r\n  </div>\r\n</ng-container>\r\n",
                    animations: [fadeAnimation, dialogAnimation]
                }] }
    ];
    /** @nocollapse */
    ModalComponent.ctorParameters = function () { return [
        { type: Renderer2 },
        { type: ConfirmationService }
    ]; };
    ModalComponent.propDecorators = {
        visible: [{ type: Input }],
        busy: [{ type: Input }],
        centered: [{ type: Input }],
        modalClass: [{ type: Input }],
        size: [{ type: Input }],
        abpSubmit: [{ type: ContentChild, args: [ButtonComponent, { static: false, read: ButtonComponent },] }],
        abpHeader: [{ type: ContentChild, args: ['abpHeader', { static: false },] }],
        abpBody: [{ type: ContentChild, args: ['abpBody', { static: false },] }],
        abpFooter: [{ type: ContentChild, args: ['abpFooter', { static: false },] }],
        abpClose: [{ type: ContentChild, args: ['abpClose', { static: false, read: ElementRef },] }],
        modalContent: [{ type: ViewChild, args: ['abpModalContent', { static: false },] }],
        abpButtons: [{ type: ViewChildren, args: ['abp-button',] }],
        visibleChange: [{ type: Output }],
        init: [{ type: Output }],
        appear: [{ type: Output }],
        disappear: [{ type: Output }]
    };
    return ModalComponent;
}());
export { ModalComponent };
if (false) {
    /** @type {?} */
    ModalComponent.prototype.centered;
    /** @type {?} */
    ModalComponent.prototype.modalClass;
    /** @type {?} */
    ModalComponent.prototype.size;
    /** @type {?} */
    ModalComponent.prototype.abpSubmit;
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
    ModalComponent.prototype.abpButtons;
    /** @type {?} */
    ModalComponent.prototype.visibleChange;
    /** @type {?} */
    ModalComponent.prototype.init;
    /** @type {?} */
    ModalComponent.prototype.appear;
    /** @type {?} */
    ModalComponent.prototype.disappear;
    /** @type {?} */
    ModalComponent.prototype._visible;
    /** @type {?} */
    ModalComponent.prototype._busy;
    /** @type {?} */
    ModalComponent.prototype.isModalOpen;
    /** @type {?} */
    ModalComponent.prototype.isConfirmationOpen;
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
    function (acc, val) { return tslib_1.__spread(acc, (val.childNodes && val.childNodes.length ? getFlatNodes(val.childNodes) : [val])); }), []);
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
    function (node) { return (node.className || '').indexOf('ng-dirty') > -1; })) > -1;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQ0wsU0FBUyxFQUNULFlBQVksRUFDWixVQUFVLEVBQ1YsWUFBWSxFQUNaLEtBQUssRUFFTCxNQUFNLEVBQ04sU0FBUyxFQUNULFdBQVcsRUFDWCxTQUFTLEVBQ1QsWUFBWSxHQUNiLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxTQUFTLEVBQUUsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzFDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLFNBQVMsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRWpFLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBQzFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUM3RCxPQUFPLEVBQUUsYUFBYSxFQUFFLGVBQWUsRUFBRSxNQUFNLG1DQUFtQyxDQUFDO0FBSW5GO0lBK0VFLHdCQUFvQixRQUFtQixFQUFVLG1CQUF3QztRQUFyRSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBQVUsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQXhDaEYsYUFBUSxHQUFHLEtBQUssQ0FBQztRQUVqQixlQUFVLEdBQUcsRUFBRSxDQUFDO1FBRWhCLFNBQUksR0FBYyxJQUFJLENBQUM7UUFrQmIsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBRTVDLFNBQUksR0FBRyxJQUFJLFlBQVksRUFBUSxDQUFDO1FBRWhDLFdBQU0sR0FBRyxJQUFJLFlBQVksRUFBRSxDQUFDO1FBRTVCLGNBQVMsR0FBRyxJQUFJLFlBQVksRUFBRSxDQUFDO1FBRWxELGFBQVEsR0FBRyxLQUFLLENBQUM7UUFFakIs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Q0FDSCxDQUFDO0FBQ0osQ0FBQzs7Ozs7QUFFRCxTQUFTLFVBQVUsQ0FBQyxLQUFvQjtJQUN0QyxPQUFPLEtBQUssQ0FBQyxTQUFTOzs7O0lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksRUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxFQUEvQyxDQUErQyxFQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7QUFDdkYsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XHJcbiAgQ29tcG9uZW50LFxyXG4gIENvbnRlbnRDaGlsZCxcclxuICBFbGVtZW50UmVmLFxyXG4gIEV2ZW50RW1pdHRlcixcclxuICBJbnB1dCxcclxuICBPbkRlc3Ryb3ksXHJcbiAgT3V0cHV0LFxyXG4gIFJlbmRlcmVyMixcclxuICBUZW1wbGF0ZVJlZixcclxuICBWaWV3Q2hpbGQsXHJcbiAgVmlld0NoaWxkcmVuLFxyXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBmcm9tRXZlbnQsIFN1YmplY3QgfSBmcm9tICdyeGpzJztcclxuaW1wb3J0IHsgZGVib3VuY2VUaW1lLCBmaWx0ZXIsIHRha2VVbnRpbCB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcclxuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uLy4uL21vZGVscy90b2FzdGVyJztcclxuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2NvbmZpcm1hdGlvbi5zZXJ2aWNlJztcclxuaW1wb3J0IHsgQnV0dG9uQ29tcG9uZW50IH0gZnJvbSAnLi4vYnV0dG9uL2J1dHRvbi5jb21wb25lbnQnO1xyXG5pbXBvcnQgeyBmYWRlQW5pbWF0aW9uLCBkaWFsb2dBbmltYXRpb24gfSBmcm9tICcuLi8uLi9hbmltYXRpb25zL21vZGFsLmFuaW1hdGlvbnMnO1xyXG5cclxuZXhwb3J0IHR5cGUgTW9kYWxTaXplID0gJ3NtJyB8ICdtZCcgfCAnbGcnIHwgJ3hsJztcclxuXHJcbkBDb21wb25lbnQoe1xyXG4gIHNlbGVjdG9yOiAnYWJwLW1vZGFsJyxcclxuICB0ZW1wbGF0ZVVybDogJy4vbW9kYWwuY29tcG9uZW50Lmh0bWwnLFxyXG4gIGFuaW1hdGlvbnM6IFtmYWRlQW5pbWF0aW9uLCBkaWFsb2dBbmltYXRpb25dLFxyXG59KVxyXG5leHBvcnQgY2xhc3MgTW9kYWxDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xyXG4gIEBJbnB1dCgpXHJcbiAgZ2V0IHZpc2libGUoKTogYm9vbGVhbiB7XHJcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcclxuICB9XHJcbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcclxuICAgIGlmICh0eXBlb2YgdmFsdWUgIT09ICdib29sZWFuJykgcmV0dXJuO1xyXG5cclxuICAgIHRoaXMuaXNNb2RhbE9wZW4gPSB2YWx1ZTtcclxuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcclxuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcclxuXHJcbiAgICBpZiAodmFsdWUpIHtcclxuICAgICAgc2V0VGltZW91dCgoKSA9PiB0aGlzLmxpc3RlbigpLCAwKTtcclxuICAgICAgdGhpcy5yZW5kZXJlci5hZGRDbGFzcyhkb2N1bWVudC5ib2R5LCAnbW9kYWwtb3BlbicpO1xyXG4gICAgICB0aGlzLmFwcGVhci5lbWl0KCk7XHJcbiAgICB9IGVsc2Uge1xyXG4gICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNsYXNzKGRvY3VtZW50LmJvZHksICdtb2RhbC1vcGVuJyk7XHJcbiAgICAgIHRoaXMuZGlzYXBwZWFyLmVtaXQoKTtcclxuICAgIH1cclxuICB9XHJcblxyXG4gIEBJbnB1dCgpXHJcbiAgZ2V0IGJ1c3koKTogYm9vbGVhbiB7XHJcbiAgICByZXR1cm4gdGhpcy5fYnVzeTtcclxuICB9XHJcbiAgc2V0IGJ1c3kodmFsdWU6IGJvb2xlYW4pIHtcclxuICAgIGlmICh0aGlzLmFicFN1Ym1pdCAmJiB0aGlzLmFicFN1Ym1pdCBpbnN0YW5jZW9mIEJ1dHRvbkNvbXBvbmVudCkge1xyXG4gICAgICB0aGlzLmFicFN1Ym1pdC5sb2FkaW5nID0gdmFsdWU7XHJcbiAgICB9XHJcblxyXG4gICAgdGhpcy5fYnVzeSA9IHZhbHVlO1xyXG4gIH1cclxuXHJcbiAgQElucHV0KCkgY2VudGVyZWQgPSBmYWxzZTtcclxuXHJcbiAgQElucHV0KCkgbW9kYWxDbGFzcyA9ICcnO1xyXG5cclxuICBASW5wdXQoKSBzaXplOiBNb2RhbFNpemUgPSAnbGcnO1xyXG5cclxuICBAQ29udGVudENoaWxkKEJ1dHRvbkNvbXBvbmVudCwgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBCdXR0b25Db21wb25lbnQgfSlcclxuICBhYnBTdWJtaXQ6IEJ1dHRvbkNvbXBvbmVudDtcclxuXHJcbiAgQENvbnRlbnRDaGlsZCgnYWJwSGVhZGVyJywgeyBzdGF0aWM6IGZhbHNlIH0pIGFicEhlYWRlcjogVGVtcGxhdGVSZWY8YW55PjtcclxuXHJcbiAgQENvbnRlbnRDaGlsZCgnYWJwQm9keScsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBCb2R5OiBUZW1wbGF0ZVJlZjxhbnk+O1xyXG5cclxuICBAQ29udGVudENoaWxkKCdhYnBGb290ZXInLCB7IHN0YXRpYzogZmFsc2UgfSkgYWJwRm9vdGVyOiBUZW1wbGF0ZVJlZjxhbnk+O1xyXG5cclxuICBAQ29udGVudENoaWxkKCdhYnBDbG9zZScsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogRWxlbWVudFJlZiB9KVxyXG4gIGFicENsb3NlOiBFbGVtZW50UmVmPGFueT47XHJcblxyXG4gIEBWaWV3Q2hpbGQoJ2FicE1vZGFsQ29udGVudCcsIHsgc3RhdGljOiBmYWxzZSB9KSBtb2RhbENvbnRlbnQ6IEVsZW1lbnRSZWY7XHJcblxyXG4gIEBWaWV3Q2hpbGRyZW4oJ2FicC1idXR0b24nKSBhYnBCdXR0b25zO1xyXG5cclxuICBAT3V0cHV0KCkgcmVhZG9ubHkgdmlzaWJsZUNoYW5nZSA9IG5ldyBFdmVudEVtaXR0ZXI8Ym9vbGVhbj4oKTtcclxuXHJcbiAgQE91dHB1dCgpIHJlYWRvbmx5IGluaXQgPSBuZXcgRXZlbnRFbWl0dGVyPHZvaWQ+KCk7XHJcblxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBhcHBlYXIgPSBuZXcgRXZlbnRFbWl0dGVyKCk7XHJcblxyXG4gIEBPdXRwdXQoKSByZWFkb25seSBkaXNhcHBlYXIgPSBuZXcgRXZlbnRFbWl0dGVyKCk7XHJcblxyXG4gIF92aXNpYmxlID0gZmFsc2U7XHJcblxyXG4gIF9idXN5ID0gZmFsc2U7XHJcblxyXG4gIGlzTW9kYWxPcGVuID0gZmFsc2U7XHJcblxyXG4gIGlzQ29uZmlybWF0aW9uT3BlbiA9IGZhbHNlO1xyXG5cclxuICBkZXN0cm95JCA9IG5ldyBTdWJqZWN0PHZvaWQ+KCk7XHJcblxyXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgcmVuZGVyZXI6IFJlbmRlcmVyMiwgcHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlKSB7fVxyXG5cclxuICBuZ09uRGVzdHJveSgpOiB2b2lkIHtcclxuICAgIHRoaXMuZGVzdHJveSQubmV4dCgpO1xyXG4gIH1cclxuXHJcbiAgY2xvc2UoKSB7XHJcbiAgICBpZiAodGhpcy5idXN5KSByZXR1cm47XHJcblxyXG4gICAgY29uc3Qgbm9kZXMgPSBnZXRGbGF0Tm9kZXMoXHJcbiAgICAgICh0aGlzLm1vZGFsQ29udGVudC5uYXRpdmVFbGVtZW50LnF1ZXJ5U2VsZWN0b3IoJyNhYnAtbW9kYWwtYm9keScpIGFzIEhUTUxFbGVtZW50KS5jaGlsZE5vZGVzLFxyXG4gICAgKTtcclxuXHJcbiAgICBpZiAoaGFzTmdEaXJ0eShub2RlcykpIHtcclxuICAgICAgaWYgKHRoaXMuaXNDb25maXJtYXRpb25PcGVuKSByZXR1cm47XHJcblxyXG4gICAgICB0aGlzLmlzQ29uZmlybWF0aW9uT3BlbiA9IHRydWU7XHJcbiAgICAgIHRoaXMuY29uZmlybWF0aW9uU2VydmljZVxyXG4gICAgICAgIC53YXJuKCdBYnBBY2NvdW50OjpBcmVZb3VTdXJlWW91V2FudFRvQ2FuY2VsRWRpdGluZ1dhcm5pbmdNZXNzYWdlJywgJ0FicEFjY291bnQ6OkFyZVlvdVN1cmUnKVxyXG4gICAgICAgIC5zdWJzY3JpYmUoKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpID0+IHtcclxuICAgICAgICAgIHRoaXMuaXNDb25maXJtYXRpb25PcGVuID0gZmFsc2U7XHJcbiAgICAgICAgICBpZiAoc3RhdHVzID09PSBUb2FzdGVyLlN0YXR1cy5jb25maXJtKSB7XHJcbiAgICAgICAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xyXG4gICAgICAgICAgfVxyXG4gICAgICAgIH0pO1xyXG4gICAgfSBlbHNlIHtcclxuICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XHJcbiAgICB9XHJcbiAgfVxyXG5cclxuICBsaXN0ZW4oKSB7XHJcbiAgICBmcm9tRXZlbnQoZG9jdW1lbnQsICdrZXl1cCcpXHJcbiAgICAgIC5waXBlKFxyXG4gICAgICAgIHRha2VVbnRpbCh0aGlzLmRlc3Ryb3kkKSxcclxuICAgICAgICBkZWJvdW5jZVRpbWUoMTUwKSxcclxuICAgICAgICBmaWx0ZXIoKGtleTogS2V5Ym9hcmRFdmVudCkgPT4ga2V5ICYmIGtleS5jb2RlID09PSAnRXNjYXBlJyksXHJcbiAgICAgIClcclxuICAgICAgLnN1YnNjcmliZShfID0+IHtcclxuICAgICAgICB0aGlzLmNsb3NlKCk7XHJcbiAgICAgIH0pO1xyXG5cclxuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xyXG4gICAgICBpZiAoIXRoaXMuYWJwQ2xvc2UpIHJldHVybjtcclxuICAgICAgZnJvbUV2ZW50KHRoaXMuYWJwQ2xvc2UubmF0aXZlRWxlbWVudCwgJ2NsaWNrJylcclxuICAgICAgICAucGlwZShcclxuICAgICAgICAgIHRha2VVbnRpbCh0aGlzLmRlc3Ryb3kkKSxcclxuICAgICAgICAgIGZpbHRlcigoKSA9PiAhIXRoaXMubW9kYWxDb250ZW50KSxcclxuICAgICAgICApXHJcbiAgICAgICAgLnN1YnNjcmliZSgoKSA9PiB0aGlzLmNsb3NlKCkpO1xyXG4gICAgfSwgMCk7XHJcblxyXG4gICAgdGhpcy5pbml0LmVtaXQoKTtcclxuICB9XHJcbn1cclxuXHJcbmZ1bmN0aW9uIGdldEZsYXROb2Rlcyhub2RlczogTm9kZUxpc3QpOiBIVE1MRWxlbWVudFtdIHtcclxuICByZXR1cm4gQXJyYXkuZnJvbShub2RlcykucmVkdWNlKFxyXG4gICAgKGFjYywgdmFsKSA9PiBbLi4uYWNjLCAuLi4odmFsLmNoaWxkTm9kZXMgJiYgdmFsLmNoaWxkTm9kZXMubGVuZ3RoID8gZ2V0RmxhdE5vZGVzKHZhbC5jaGlsZE5vZGVzKSA6IFt2YWxdKV0sXHJcbiAgICBbXSxcclxuICApO1xyXG59XHJcblxyXG5mdW5jdGlvbiBoYXNOZ0RpcnR5KG5vZGVzOiBIVE1MRWxlbWVudFtdKSB7XHJcbiAgcmV0dXJuIG5vZGVzLmZpbmRJbmRleChub2RlID0+IChub2RlLmNsYXNzTmFtZSB8fCAnJykuaW5kZXhPZignbmctZGlydHknKSA+IC0xKSA+IC0xO1xyXG59XHJcbiJdfQ==