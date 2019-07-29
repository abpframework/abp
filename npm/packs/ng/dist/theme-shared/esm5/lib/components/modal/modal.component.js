/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, ContentChild, ElementRef, EventEmitter, Input, Output, Renderer2, TemplateRef, ViewChild, } from '@angular/core';
import { fromEvent, Subject, timer } from 'rxjs';
import { debounceTime, filter, take, takeUntil } from 'rxjs/operators';
import { ConfirmationService } from '../../services/confirmation.service';
var ModalComponent = /** @class */ (function () {
    function ModalComponent(renderer, confirmationService) {
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
            if (!this.modalContent) {
                setTimeout((/**
                 * @return {?}
                 */
                function () { return (_this.visible = value); }), 0);
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
                function () {
                    _this.setVisible(value);
                    _this.renderer.removeClass(_this.modalContent.nativeElement, 'fade-out-top');
                    _this.ngOnDestroy();
                }), 350);
            }
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
     * @param {?} value
     * @return {?}
     */
    ModalComponent.prototype.setVisible = /**
     * @param {?} value
     * @return {?}
     */
    function (value) {
        var _this = this;
        this._visible = value;
        this.visibleChange.emit(value);
        value
            ? timer(500)
                .pipe(take(1))
                .subscribe((/**
             * @param {?} _
             * @return {?}
             */
            function (_) { return (_this.closable = true); }))
            : (this.closable = false);
    };
    /**
     * @return {?}
     */
    ModalComponent.prototype.listen = /**
     * @return {?}
     */
    function () {
        var _this = this;
        fromEvent(document, 'click')
            .pipe(debounceTime(350), takeUntil(this.destroy$), filter((/**
         * @param {?} event
         * @return {?}
         */
        function (event) {
            return event &&
                _this.closable &&
                _this.modalContent &&
                !_this.isOpenConfirmation &&
                !_this.modalContent.nativeElement.contains(event.target);
        })))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        function (_) {
            _this.close();
        }));
        fromEvent(document, 'keyup')
            .pipe(takeUntil(this.destroy$), filter((/**
         * @param {?} key
         * @return {?}
         */
        function (key) { return key && key.code === 'Escape' && _this.closable; })), debounceTime(350))
            .subscribe((/**
         * @param {?} _
         * @return {?}
         */
        function (_) {
            _this.close();
        }));
        fromEvent(this.abpClose.nativeElement, 'click')
            .pipe(takeUntil(this.destroy$), filter((/**
         * @return {?}
         */
        function () { return !!(_this.closable && _this.modalContent); })), debounceTime(350))
            .subscribe((/**
         * @return {?}
         */
        function () { return _this.close(); }));
    };
    /**
     * @return {?}
     */
    ModalComponent.prototype.close = /**
     * @return {?}
     */
    function () {
        var _this = this;
        /** @type {?} */
        var nodes = getFlatNodes(((/** @type {?} */ (this.modalContent.nativeElement.querySelector('#abp-modal-body')))).childNodes);
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
            function (status) {
                timer(400).subscribe((/**
                 * @return {?}
                 */
                function () {
                    _this.isOpenConfirmation = false;
                }));
                if (status === "confirm" /* confirm */) {
                    _this.visible = false;
                }
            }));
        }
        else {
            this.visible = false;
        }
    };
    ModalComponent.decorators = [
        { type: Component, args: [{
                    selector: 'abp-modal',
                    template: "<div\n  id=\"abp-modal\"\n  tabindex=\"-1\"\n  class=\"modal fade {{ modalClass }}\"\n  [class.show]=\"visible\"\n  [style.display]=\"visible ? 'block' : 'none'\"\n  [style.padding-right.px]=\"'15'\"\n>\n  <div\n    id=\"abp-modal-container\"\n    class=\"modal-dialog modal-{{ size }} fade-in-top\"\n    [class.modal-dialog-centered]=\"centered\"\n    #abpModalContent\n  >\n    <div #content id=\"abp-modal-content\" class=\"modal-content\">\n      <div id=\"abp-modal-header\" class=\"modal-header\">\n        <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\n\n        <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" (click)=\"close()\">\n          <span aria-hidden=\"true\">&times;</span>\n        </button>\n      </div>\n      <div id=\"abp-modal-body\" class=\"modal-body\">\n        <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\n\n        <div id=\"abp-modal-footer\" class=\"modal-footer\">\n          <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\n        </div>\n      </div>\n    </div>\n  </div>\n\n  <ng-content></ng-content>\n</div>\n"
                }] }
    ];
    /** @nocollapse */
    ModalComponent.ctorParameters = function () { return [
        { type: Renderer2 },
        { type: ConfirmationService }
    ]; };
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
    function (acc, val) { return tslib_1.__spread(acc, (val.childNodes && val.childNodes.length ? Array.from(val.childNodes) : [val])); }), []);
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQ0wsU0FBUyxFQUNULFlBQVksRUFDWixVQUFVLEVBQ1YsWUFBWSxFQUNaLEtBQUssRUFFTCxNQUFNLEVBQ04sU0FBUyxFQUNULFdBQVcsRUFDWCxTQUFTLEdBQ1YsTUFBTSxlQUFlLENBQUM7QUFDdkIsT0FBTyxFQUFFLFNBQVMsRUFBRSxPQUFPLEVBQUUsS0FBSyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQ2pELE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLElBQUksRUFBRSxTQUFTLEVBQUUsTUFBTSxnQkFBZ0IsQ0FBQztBQUN2RSxPQUFPLEVBQUUsbUJBQW1CLEVBQUUsTUFBTSxxQ0FBcUMsQ0FBQztBQUsxRTtJQXVERSx3QkFBb0IsUUFBbUIsRUFBVSxtQkFBd0M7UUFBckUsYUFBUSxHQUFSLFFBQVEsQ0FBVztRQUFVLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7UUExQmhGLGFBQVEsR0FBWSxJQUFJLENBQUM7UUFFekIsZUFBVSxHQUFXLEVBQUUsQ0FBQztRQUV4QixTQUFJLEdBQWMsSUFBSSxDQUFDO1FBRXRCLGtCQUFhLEdBQUcsSUFBSSxZQUFZLEVBQVcsQ0FBQztRQVl0RCxhQUFRLEdBQVksS0FBSyxDQUFDO1FBRTFCLGFBQVEsR0FBWSxLQUFLLENBQUM7UUFFMUIsdUJBQWtCLEdBQVksS0FBSyxDQUFDO1FBRXBDLGFBQVEsR0FBRyxJQUFJLE9BQU8sRUFBUSxDQUFDO0lBRTZELENBQUM7SUFsRDdGLHNCQUNJLG1DQUFPOzs7O1FBRFg7WUFFRSxPQUFPLElBQUksQ0FBQyxRQUFRLENBQUM7UUFDdkIsQ0FBQzs7Ozs7UUFDRCxVQUFZLEtBQWM7WUFBMUIsaUJBa0JD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QUFLLENBQUMsSUFBSSxDQUFDLEdBQUcsQ0FBQyxVQUFVLENBQUMsQ0FBQyxDQUFDLENBQUMsQ0FBQyxHQUFHLENBQUMsQ0FBQyxHQUExRixDQUEyRixHQUN6RyxFQUFFLENBQ0gsQ0FBQztBQUNKLENBQUM7Ozs7O0FBRUQsU0FBUyxVQUFVLENBQUMsS0FBb0I7SUFDdEMsT0FBTyxLQUFLLENBQUMsU0FBUzs7OztJQUFDLFVBQUEsSUFBSSxJQUFJLE9BQUEsQ0FBQyxJQUFJLENBQUMsU0FBUyxJQUFJLEVBQUUsQ0FBQyxDQUFDLE9BQU8sQ0FBQyxVQUFVLENBQUMsR0FBRyxDQUFDLENBQUMsRUFBL0MsQ0FBK0MsRUFBQyxHQUFHLENBQUMsQ0FBQyxDQUFDO0FBQ3ZGLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJpbXBvcnQge1xuICBDb21wb25lbnQsXG4gIENvbnRlbnRDaGlsZCxcbiAgRWxlbWVudFJlZixcbiAgRXZlbnRFbWl0dGVyLFxuICBJbnB1dCxcbiAgT25EZXN0cm95LFxuICBPdXRwdXQsXG4gIFJlbmRlcmVyMixcbiAgVGVtcGxhdGVSZWYsXG4gIFZpZXdDaGlsZCxcbn0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBmcm9tRXZlbnQsIFN1YmplY3QsIHRpbWVyIH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUsIGZpbHRlciwgdGFrZSwgdGFrZVVudGlsIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2NvbmZpcm1hdGlvbi5zZXJ2aWNlJztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi8uLi9tb2RlbHMvdG9hc3Rlcic7XG5cbmV4cG9ydCB0eXBlIE1vZGFsU2l6ZSA9ICdzbScgfCAnbWQnIHwgJ2xnJyB8ICd4bCc7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1tb2RhbCcsXG4gIHRlbXBsYXRlVXJsOiAnLi9tb2RhbC5jb21wb25lbnQuaHRtbCcsXG59KVxuZXhwb3J0IGNsYXNzIE1vZGFsQ29tcG9uZW50IGltcGxlbWVudHMgT25EZXN0cm95IHtcbiAgQElucHV0KClcbiAgZ2V0IHZpc2libGUoKTogYm9vbGVhbiB7XG4gICAgcmV0dXJuIHRoaXMuX3Zpc2libGU7XG4gIH1cbiAgc2V0IHZpc2libGUodmFsdWU6IGJvb2xlYW4pIHtcbiAgICBpZiAoIXRoaXMubW9kYWxDb250ZW50KSB7XG4gICAgICBzZXRUaW1lb3V0KCgpID0+ICh0aGlzLnZpc2libGUgPSB2YWx1ZSksIDApO1xuICAgICAgcmV0dXJuO1xuICAgIH1cblxuICAgIGlmICh2YWx1ZSkge1xuICAgICAgdGhpcy5zZXRWaXNpYmxlKHZhbHVlKTtcbiAgICAgIHRoaXMubGlzdGVuKCk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMuY2xvc2FibGUgPSBmYWxzZTtcbiAgICAgIHRoaXMucmVuZGVyZXIuYWRkQ2xhc3ModGhpcy5tb2RhbENvbnRlbnQubmF0aXZlRWxlbWVudCwgJ2ZhZGUtb3V0LXRvcCcpO1xuICAgICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICAgIHRoaXMuc2V0VmlzaWJsZSh2YWx1ZSk7XG4gICAgICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlQ2xhc3ModGhpcy5tb2RhbENvbnRlbnQubmF0aXZlRWxlbWVudCwgJ2ZhZGUtb3V0LXRvcCcpO1xuICAgICAgICB0aGlzLm5nT25EZXN0cm95KCk7XG4gICAgICB9LCAzNTApO1xuICAgIH1cbiAgfVxuXG4gIEBJbnB1dCgpIGNlbnRlcmVkOiBib29sZWFuID0gdHJ1ZTtcblxuICBASW5wdXQoKSBtb2RhbENsYXNzOiBzdHJpbmcgPSAnJztcblxuICBASW5wdXQoKSBzaXplOiBNb2RhbFNpemUgPSAnbGcnO1xuXG4gIEBPdXRwdXQoKSB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEhlYWRlcicsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBIZWFkZXI6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZCgnYWJwQm9keScsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBCb2R5OiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEZvb3RlcicsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBGb290ZXI6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZCgnYWJwQ2xvc2UnLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IEVsZW1lbnRSZWYgfSkgYWJwQ2xvc2U6IEVsZW1lbnRSZWY8YW55PjtcblxuICBAVmlld0NoaWxkKCdhYnBNb2RhbENvbnRlbnQnLCB7IHN0YXRpYzogZmFsc2UgfSkgbW9kYWxDb250ZW50OiBFbGVtZW50UmVmO1xuXG4gIF92aXNpYmxlOiBib29sZWFuID0gZmFsc2U7XG5cbiAgY2xvc2FibGU6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBpc09wZW5Db25maXJtYXRpb246IGJvb2xlYW4gPSBmYWxzZTtcblxuICBkZXN0cm95JCA9IG5ldyBTdWJqZWN0PHZvaWQ+KCk7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyLCBwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UpIHt9XG5cbiAgbmdPbkRlc3Ryb3koKTogdm9pZCB7XG4gICAgdGhpcy5kZXN0cm95JC5uZXh0KCk7XG4gIH1cblxuICBzZXRWaXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XG4gICAgdGhpcy5fdmlzaWJsZSA9IHZhbHVlO1xuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcbiAgICB2YWx1ZVxuICAgICAgPyB0aW1lcig1MDApXG4gICAgICAgICAgLnBpcGUodGFrZSgxKSlcbiAgICAgICAgICAuc3Vic2NyaWJlKF8gPT4gKHRoaXMuY2xvc2FibGUgPSB0cnVlKSlcbiAgICAgIDogKHRoaXMuY2xvc2FibGUgPSBmYWxzZSk7XG4gIH1cblxuICBsaXN0ZW4oKSB7XG4gICAgZnJvbUV2ZW50KGRvY3VtZW50LCAnY2xpY2snKVxuICAgICAgLnBpcGUoXG4gICAgICAgIGRlYm91bmNlVGltZSgzNTApLFxuICAgICAgICB0YWtlVW50aWwodGhpcy5kZXN0cm95JCksXG4gICAgICAgIGZpbHRlcihcbiAgICAgICAgICAoZXZlbnQ6IE1vdXNlRXZlbnQpID0+XG4gICAgICAgICAgICBldmVudCAmJlxuICAgICAgICAgICAgdGhpcy5jbG9zYWJsZSAmJlxuICAgICAgICAgICAgdGhpcy5tb2RhbENvbnRlbnQgJiZcbiAgICAgICAgICAgICF0aGlzLmlzT3BlbkNvbmZpcm1hdGlvbiAmJlxuICAgICAgICAgICAgIXRoaXMubW9kYWxDb250ZW50Lm5hdGl2ZUVsZW1lbnQuY29udGFpbnMoZXZlbnQudGFyZ2V0KSxcbiAgICAgICAgKSxcbiAgICAgIClcbiAgICAgIC5zdWJzY3JpYmUoXyA9PiB7XG4gICAgICAgIHRoaXMuY2xvc2UoKTtcbiAgICAgIH0pO1xuXG4gICAgZnJvbUV2ZW50KGRvY3VtZW50LCAna2V5dXAnKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHRha2VVbnRpbCh0aGlzLmRlc3Ryb3kkKSxcbiAgICAgICAgZmlsdGVyKChrZXk6IEtleWJvYXJkRXZlbnQpID0+IGtleSAmJiBrZXkuY29kZSA9PT0gJ0VzY2FwZScgJiYgdGhpcy5jbG9zYWJsZSksXG4gICAgICAgIGRlYm91bmNlVGltZSgzNTApLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShfID0+IHtcbiAgICAgICAgdGhpcy5jbG9zZSgpO1xuICAgICAgfSk7XG5cbiAgICBmcm9tRXZlbnQodGhpcy5hYnBDbG9zZS5uYXRpdmVFbGVtZW50LCAnY2xpY2snKVxuICAgICAgLnBpcGUoXG4gICAgICAgIHRha2VVbnRpbCh0aGlzLmRlc3Ryb3kkKSxcbiAgICAgICAgZmlsdGVyKCgpID0+ICEhKHRoaXMuY2xvc2FibGUgJiYgdGhpcy5tb2RhbENvbnRlbnQpKSxcbiAgICAgICAgZGVib3VuY2VUaW1lKDM1MCksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKCgpID0+IHRoaXMuY2xvc2UoKSk7XG4gIH1cblxuICBjbG9zZSgpIHtcbiAgICBjb25zdCBub2RlcyA9IGdldEZsYXROb2RlcyhcbiAgICAgICh0aGlzLm1vZGFsQ29udGVudC5uYXRpdmVFbGVtZW50LnF1ZXJ5U2VsZWN0b3IoJyNhYnAtbW9kYWwtYm9keScpIGFzIEhUTUxFbGVtZW50KS5jaGlsZE5vZGVzLFxuICAgICk7XG5cbiAgICBpZiAoaGFzTmdEaXJ0eShub2RlcykpIHtcbiAgICAgIGlmICh0aGlzLmlzT3BlbkNvbmZpcm1hdGlvbikgcmV0dXJuO1xuXG4gICAgICB0aGlzLmlzT3BlbkNvbmZpcm1hdGlvbiA9IHRydWU7XG4gICAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2VcbiAgICAgICAgLndhcm4oJ0FicEFjY291bnQ6OkFyZVlvdVN1cmVZb3VXYW50VG9DYW5jZWxFZGl0aW5nV2FybmluZ01lc3NhZ2UnLCAnQWJwQWNjb3VudDo6QXJlWW91U3VyZScpXG4gICAgICAgIC5zdWJzY3JpYmUoKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpID0+IHtcbiAgICAgICAgICB0aW1lcig0MDApLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgICB0aGlzLmlzT3BlbkNvbmZpcm1hdGlvbiA9IGZhbHNlO1xuICAgICAgICAgIH0pO1xuXG4gICAgICAgICAgaWYgKHN0YXR1cyA9PT0gVG9hc3Rlci5TdGF0dXMuY29uZmlybSkge1xuICAgICAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgICAgICAgfVxuICAgICAgICB9KTtcbiAgICB9IGVsc2Uge1xuICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG59XG5cbmZ1bmN0aW9uIGdldEZsYXROb2Rlcyhub2RlczogTm9kZUxpc3QpOiBIVE1MRWxlbWVudFtdIHtcbiAgcmV0dXJuIEFycmF5LmZyb20obm9kZXMpLnJlZHVjZShcbiAgICAoYWNjLCB2YWwpID0+IFsuLi5hY2MsIC4uLih2YWwuY2hpbGROb2RlcyAmJiB2YWwuY2hpbGROb2Rlcy5sZW5ndGggPyBBcnJheS5mcm9tKHZhbC5jaGlsZE5vZGVzKSA6IFt2YWxdKV0sXG4gICAgW10sXG4gICk7XG59XG5cbmZ1bmN0aW9uIGhhc05nRGlydHkobm9kZXM6IEhUTUxFbGVtZW50W10pIHtcbiAgcmV0dXJuIG5vZGVzLmZpbmRJbmRleChub2RlID0+IChub2RlLmNsYXNzTmFtZSB8fCAnJykuaW5kZXhPZignbmctZGlydHknKSA+IC0xKSA+IC0xO1xufVxuIl19