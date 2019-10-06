/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from "tslib";
import { Component, ContentChild, ElementRef, EventEmitter, Input, Output, Renderer2, TemplateRef, ViewChild, ViewChildren, } from '@angular/core';
import { fromEvent, Subject, timer } from 'rxjs';
import { filter, take, takeUntil, debounceTime } from 'rxjs/operators';
import { ConfirmationService } from '../../services/confirmation.service';
import { ButtonComponent } from '../button/button.component';
/** @type {?} */
var ANIMATION_TIMEOUT = 200;
var ModalComponent = /** @class */ (function () {
    function ModalComponent(renderer, confirmationService) {
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
            if (!this.modalContent) {
                if (value) {
                    setTimeout((/**
                     * @return {?}
                     */
                    function () {
                        _this.showModal = value;
                        _this.visible = value;
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
                function () {
                    _this.setVisible(value);
                    _this.ngOnDestroy();
                }), ANIMATION_TIMEOUT - 10);
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
        this.showModal = value;
        if (value) {
            timer(ANIMATION_TIMEOUT + 100)
                .pipe(take(1))
                .subscribe((/**
             * @param {?} _
             * @return {?}
             */
            function (_) { return (_this.closable = true); }));
            this.renderer.addClass(document.body, 'modal-open');
            this.show.emit();
        }
        else {
            this.closable = false;
            this.renderer.removeClass(document.body, 'modal-open');
            this.hide.emit();
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
        function (key) { return key && key.code === 'Escape' && _this.closable; })))
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
            function () { return !!(_this.closable && _this.modalContent); })))
                .subscribe((/**
             * @return {?}
             */
            function () { return _this.close(); }));
        }), 0);
        this.init.emit();
    };
    /**
     * @return {?}
     */
    ModalComponent.prototype.close = /**
     * @return {?}
     */
    function () {
        var _this = this;
        if (!this.closable || this.busy)
            return;
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
                timer(ANIMATION_TIMEOUT).subscribe((/**
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
                    template: "<div\n  *ngIf=\"showModal\"\n  (click)=\"close()\"\n  id=\"abp-modal\"\n  class=\"modal fade {{ modalClass }} d-block show\"\n  [style.padding-right.px]=\"'15'\"\n>\n  <div\n    id=\"abp-modal-container\"\n    class=\"modal-dialog modal-{{ size }} fade-in-top\"\n    tabindex=\"-1\"\n    [class.modal-dialog-centered]=\"centered\"\n    #abpModalContent\n  >\n    <div #content id=\"abp-modal-content\" class=\"modal-content\" (click)=\"$event.stopPropagation()\">\n      <div id=\"abp-modal-header\" class=\"modal-header\">\n        <ng-container *ngTemplateOutlet=\"abpHeader\"></ng-container>\n\n        <button id=\"abp-modal-close-button\" type=\"button\" class=\"close\" (click)=\"close()\">\n          <span aria-hidden=\"true\">&times;</span>\n        </button>\n      </div>\n      <div\n        id=\"abp-modal-body\"\n        class=\"modal-body\"\n        [style.height]=\"height || undefined\"\n        [style.minHeight]=\"minHeight || undefined\"\n      >\n        <ng-container *ngTemplateOutlet=\"abpBody\"></ng-container>\n      </div>\n      <div id=\"abp-modal-footer\" class=\"modal-footer\">\n        <ng-container *ngTemplateOutlet=\"abpFooter\"></ng-container>\n      </div>\n    </div>\n  </div>\n\n  <ng-content></ng-content>\n</div>\n"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQ0wsU0FBUyxFQUNULFlBQVksRUFDWixVQUFVLEVBQ1YsWUFBWSxFQUNaLEtBQUssRUFFTCxNQUFNLEVBQ04sU0FBUyxFQUNULFdBQVcsRUFDWCxTQUFTLEVBQ1QsWUFBWSxHQUNiLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxTQUFTLEVBQUUsT0FBTyxFQUFFLEtBQUssRUFBRSxNQUFNLE1BQU0sQ0FBQztBQUNqRCxPQUFPLEVBQUUsTUFBTSxFQUFFLElBQUksRUFBRSxTQUFTLEVBQUUsWUFBWSxFQUFFLE1BQU0sZ0JBQWdCLENBQUM7QUFFdkUsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFDMUUsT0FBTyxFQUFFLGVBQWUsRUFBRSxNQUFNLDRCQUE0QixDQUFDOztJQUl2RCxpQkFBaUIsR0FBRyxHQUFHO0FBRTdCO0lBNkZFLHdCQUFvQixRQUFtQixFQUFVLG1CQUF3QztRQUFyRSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBQVUsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQTlDaEYsYUFBUSxHQUFZLEtBQUssQ0FBQztRQUUxQixlQUFVLEdBQVcsRUFBRSxDQUFDO1FBRXhCLFNBQUksR0FBYyxJQUFJLENBQUM7UUFNdEIsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBRTVDLFNBQUksR0FBRyxJQUFJLFlBQVksRUFBUSxDQUFDO1FBaUIxQyxTQUFJLEdBQUcsSUFBSSxZQUFZLEVBQUUsQ0FBQztRQUcxQixTQUFJLEdBQUcsSUFBSSxZQUFZLEVBQUUsQ0FBQztRQUUxQixhQUFRLEdBQVksS0FBSyxDQUFDO1FBRTFCLFVBQUssR0FBWSxLQUFLLENBQUM7UUFFdkIsY0FBUyxHQUFZLEtBQUssQ0FBQztRQUUzQix1QkFBa0IsR0FBWSxLQUFLLENBQUM7UUFFcEMsYUFBUSxHQUFZLEtBQUssQ0FBQztRQUUxQixhQUFRLEdBQUcsSUFBSSxPQUFPLEVBQVEsQ0FBQztJQUU2RCxDQUFDO0lBeEY3RixzQkFDSSxtQ0FBTzs7OztRQURYO1lBRUUsT0FBTyxJQUFJLENBQUMsUUFBUSxDQUFDO1FBQ3ZCLENBQUM7Ozs7O1FBQ0QsVUFBWSxLQUFjO1lBQTFCLGlCQXdCQztZQXZCQyxJQUFJLE9BQU8sS0FBSyxLQUFLLFNBQVM7Z0JBQUUsT0FBTztZQUV2QyxJQUFJLENBQUMsSUFBSSxDQUFDLFlBQVksRUFBRTtnQkFDdEIsSUFBSSxLQUFLLEVBQUU7b0JBQ1QsVUFBVTs7O29CQUFDO3dCQUNULEtBQUksQ0FBQyxTQUFTLEdBQUcsS0FBSyxDQUFDO3dCQUN2QixLQUFJLENBQUMsT0FBTyxHQUFHLEtBQUssQ0FBQztvQkFDdkIsQ0FBQyxHQUFFLENBQUMsQ0FBQyxDQUFDO2lCQUNQO2dCQUNELE9BQU87YUFDUjtZQUVELElBQUksS0FBSyxFQUFFO2dCQUNULElBQUksQ0FBQyxVQUFVLENBQUMsS0FBSyxDQUFDLENBQUM7Z0JBQ3ZCLElBQUksQ0FBQyxNQUFNLEVBQUUsQ0FBQzthQUNmO2lCQUFNO2dCQUNMLElBQUksQ0FBQyxRQUFRLEdBQUcsS0FBSyxDQUFDO2dCQUN0QixJQUFJLENBQUMsUUFBUSxDQUFDLFFBQVEsQ0FBQyxJQUFJLENBQUMsWUFBWSxDQUFDLGFBQWEsRUFBRSxjQUFjLENBQUMsQ0FBQztnQkFDeEUsVUFBVTs7O2dCQUFDO29CQUNULEtBQUksQ0FBQyxVQUFVLENBQUMsS0FBSyxDQUFDLENBQUM7b0JBQ3ZCLEtBQUksQ0FBQyxXQUFXLEVBQUUsQ0FBQztnQkFDckIsQ0FBQyxHQUFFLGlCQUFpQixHQUFHLEVBQUUsQ0FBQyxDQUFDO2FBQzVCO1FBQ0gsQ0FBQzs7O09BekJBO0lBMkJELHNCQUNJLGdDQUFJOzs7O1FBRFI7WUFFRSxPQUFPLElBQUksQ0FBQyxLQUFLLENBQUM7UUFDcEIsQ0FBQzs7Ozs7UUFDRCxVQUFTLEtBQWM7WUFDckIsSUFBSSxJQUFJLENBQUMsU0FBUyxJQUFJLElBQUksQ0FBQyxTQUFTLFlBQVksZUFBZSxFQUFFO2dCQUMvRCxJQUFJLENBQUMsU0FBUyxDQUFDLE9BQU8sR0FBRyxLQUFLLENBQUM7YUFDaEM7WUFFRCxJQUFJLENBQUMsS0FBSyxHQUFHLEtBQUssQ0FBQztRQUNyQixDQUFDOzs7T0FQQTs7OztJQXlERCxvQ0FBVzs7O0lBQVg7UUFDRSxJQUFJLENBQUMsUUFBUSxDQUFDLElBQUksRUFBRSxDQUFDO0lBQ3ZCLENBQUM7Ozs7O0lBRUQsbUNBQVU7Ozs7SUFBVixVQUFXLEtBQWM7UUFBekIsaUJBaUJDO1FBaEJDLElBQUksQ0FBQyxRQUFRLEdBQUcsS0FBSyxDQUFDO1FBQ3RCLElBQUksQ0FBQyxhQUFhLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDO1FBQy9CLElBQUksQ0FBQyxTQUFTLEdBQUcsS0FBSyxDQUFDO1FBRXZCLElBQUksS0FBSyxFQUFFO1lBQ1QsS0FBSyxDQUFDLGlCQUFpQixHQUFHLEdBQUcsQ0FBQztpQkFDM0IsSUFBSSxDQUFDLElBQUksQ0FBQyxDQUFDLENBQUMsQ0FBQztpQkFDYixTQUFTOzs7O1lBQUMsVUFBQSxDQUFDLElBQUksT0FBQSxDQUFDLEtBQUksQ0FBQyxRQUFRLEdBQUcsSUFBSSxDQUFDLEVBQXRCLENBQXNCLEVBQUMsQ0FBQztZQUUxQyxJQUFJLENBQUMsUUFBUSxDQUFDLFFBQVEsQ0FBQyxRQUFRLENBQUMsSUFBSSxFQUFFLFlBQVksQ0FBQyxDQUFDO1lBQ3BELElBQUksQ0FBQyxJQUFJLENBQUMsSUFBSSxFQUFFLENBQUM7U0FDbEI7YUFBTTtZQUNMLElBQUksQ0FBQyxRQUFRLEdBQUcsS0FBSyxDQUFDO1lBQ3RCLElBQUksQ0FBQyxRQUFRLENBQUMsV0FBVyxDQUFDLFFBQVEsQ0FBQyxJQUFJLEVBQUUsWUFBWSxDQUFDLENBQUM7WUFDdkQsSUFBSSxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUUsQ0FBQztTQUNsQjtJQUNILENBQUM7Ozs7SUFFRCwrQkFBTTs7O0lBQU47UUFBQSxpQkFzQkM7UUFyQkMsU0FBUyxDQUFDLFFBQVEsRUFBRSxPQUFPLENBQUM7YUFDekIsSUFBSSxDQUNILFNBQVMsQ0FBQyxJQUFJLENBQUMsUUFBUSxDQUFDLEVBQ3hCLFlBQVksQ0FBQyxHQUFHLENBQUMsRUFDakIsTUFBTTs7OztRQUFDLFVBQUMsR0FBa0IsSUFBSyxPQUFBLEdBQUcsSUFBSSxHQUFHLENBQUMsSUFBSSxLQUFLLFFBQVEsSUFBSSxLQUFJLENBQUMsUUFBUSxFQUE3QyxDQUE2QyxFQUFDLENBQzlFO2FBQ0EsU0FBUzs7OztRQUFDLFVBQUEsQ0FBQztZQUNWLEtBQUksQ0FBQyxLQUFLLEVBQUUsQ0FBQztRQUNmLENBQUMsRUFBQyxDQUFDO1FBRUwsVUFBVTs7O1FBQUM7WUFDVCxJQUFJLENBQUMsS0FBSSxDQUFDLFFBQVE7Z0JBQUUsT0FBTztZQUMzQixTQUFTLENBQUMsS0FBSSxDQUFDLFFBQVEsQ0FBQyxhQUFhLEVBQUUsT0FBTyxDQUFDO2lCQUM1QyxJQUFJLENBQ0gsU0FBUyxDQUFDLEtBQUksQ0FBQyxRQUFRLENBQUMsRUFDeEIsTUFBTTs7O1lBQUMsY0FBTSxPQUFBLENBQUMsQ0FBQyxDQUFDLEtBQUksQ0FBQyxRQUFRLElBQUksS0FBSSxDQUFDLFlBQVksQ0FBQyxFQUF0QyxDQUFzQyxFQUFDLENBQ3JEO2lCQUNBLFNBQVM7OztZQUFDLGNBQU0sT0FBQSxLQUFJLENBQUMsS0FBSyxFQUFFLEVBQVosQ0FBWSxFQUFDLENBQUM7UUFDbkMsQ0FBQyxHQUFFLENBQUMsQ0FBQyxDQUFDO1FBRU4sSUFBSSxDQUFDLElBQUksQ0FBQyxJQUFJLEVBQUUsQ0FBQztJQUNuQixDQUFDOzs7O0lBRUQsOEJBQUs7OztJQUFMO1FBQUEsaUJBeUJDO1FBeEJDLElBQUksQ0FBQyxJQUFJLENBQUMsUUFBUSxJQUFJLElBQUksQ0FBQyxJQUFJO1lBQUUsT0FBTzs7WUFFbEMsS0FBSyxHQUFHLFlBQVksQ0FDeEIsQ0FBQyxtQkFBQSxJQUFJLENBQUMsWUFBWSxDQUFDLGFBQWEsQ0FBQyxhQUFhLENBQUMsaUJBQWlCLENBQUMsRUFBZSxDQUFDLENBQUMsVUFBVSxDQUM3RjtRQUVELElBQUksVUFBVSxDQUFDLEtBQUssQ0FBQyxFQUFFO1lBQ3JCLElBQUksSUFBSSxDQUFDLGtCQUFrQjtnQkFBRSxPQUFPO1lBRXBDLElBQUksQ0FBQyxrQkFBa0IsR0FBRyxJQUFJLENBQUM7WUFDL0IsSUFBSSxDQUFDLG1CQUFtQjtpQkFDckIsSUFBSSxDQUFDLDREQUE0RCxFQUFFLHdCQUF3QixDQUFDO2lCQUM1RixTQUFTOzs7O1lBQUMsVUFBQyxNQUFzQjtnQkFDaEMsS0FBSyxDQUFDLGlCQUFpQixDQUFDLENBQUMsU0FBUzs7O2dCQUFDO29CQUNqQyxLQUFJLENBQUMsa0JBQWtCLEdBQUcsS0FBSyxDQUFDO2dCQUNsQyxDQUFDLEVBQUMsQ0FBQztnQkFFSCxJQUFJLE1BQU0sNEJBQTJCLEVBQUU7b0JBQ3JDLEtBQUksQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDO2lCQUN0QjtZQUNILENBQUMsRUFBQyxDQUFDO1NBQ047YUFBTTtZQUNMLElBQUksQ0FBQyxPQUFPLEdBQUcsS0FBSyxDQUFDO1NBQ3RCO0lBQ0gsQ0FBQzs7Z0JBdktGLFNBQVMsU0FBQztvQkFDVCxRQUFRLEVBQUUsV0FBVztvQkFDckIscXZDQUFxQztpQkFDdEM7Ozs7Z0JBbEJDLFNBQVM7Z0JBUUYsbUJBQW1COzs7MEJBWXpCLEtBQUs7dUJBOEJMLEtBQUs7MkJBWUwsS0FBSzs2QkFFTCxLQUFLO3VCQUVMLEtBQUs7eUJBRUwsS0FBSzs0QkFFTCxLQUFLO2dDQUVMLE1BQU07dUJBRU4sTUFBTTs0QkFFTixZQUFZLFNBQUMsV0FBVyxFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRTswQkFFM0MsWUFBWSxTQUFDLFNBQVMsRUFBRSxFQUFFLE1BQU0sRUFBRSxLQUFLLEVBQUU7NEJBRXpDLFlBQVksU0FBQyxXQUFXLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFOzJCQUUzQyxZQUFZLFNBQUMsVUFBVSxFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxJQUFJLEVBQUUsVUFBVSxFQUFFOzRCQUU1RCxZQUFZLFNBQUMsZUFBZSxFQUFFLEVBQUUsTUFBTSxFQUFFLEtBQUssRUFBRSxJQUFJLEVBQUUsZUFBZSxFQUFFOytCQUV0RSxTQUFTLFNBQUMsaUJBQWlCLEVBQUUsRUFBRSxNQUFNLEVBQUUsS0FBSyxFQUFFOzZCQUU5QyxZQUFZLFNBQUMsWUFBWTt1QkFFekIsTUFBTTt1QkFHTixNQUFNOztJQTBGVCxxQkFBQztDQUFBLEFBeEtELElBd0tDO1NBcEtZLGNBQWM7OztJQTJDekIsa0NBQW1DOztJQUVuQyxvQ0FBaUM7O0lBRWpDLDhCQUFnQzs7SUFFaEMsZ0NBQXdCOztJQUV4QixtQ0FBMkI7O0lBRTNCLHVDQUFzRDs7SUFFdEQsOEJBQTBDOztJQUUxQyxtQ0FBMEU7O0lBRTFFLGlDQUFzRTs7SUFFdEUsbUNBQTBFOztJQUUxRSxrQ0FBeUY7O0lBRXpGLG1DQUFvRzs7SUFFcEcsc0NBQTBFOztJQUUxRSxvQ0FBdUM7O0lBRXZDLDhCQUMwQjs7SUFFMUIsOEJBQzBCOztJQUUxQixrQ0FBMEI7O0lBRTFCLCtCQUF1Qjs7SUFFdkIsbUNBQTJCOztJQUUzQiw0Q0FBb0M7O0lBRXBDLGtDQUEwQjs7SUFFMUIsa0NBQStCOzs7OztJQUVuQixrQ0FBMkI7Ozs7O0lBQUUsNkNBQWdEOzs7Ozs7QUE2RTNGLFNBQVMsWUFBWSxDQUFDLEtBQWU7SUFDbkMsT0FBTyxLQUFLLENBQUMsSUFBSSxDQUFDLEtBQUssQ0FBQyxDQUFDLE1BQU07Ozs7O0lBQzdCLFVBQUMsR0FBRyxFQUFFLEdBQUcsSUFBSyx3QkFBSSxHQUFHLEVBQUssQ0FBQyxHQUFHLENBQUMsVUFBVSxJQUFJLEdBQUcsQ0FBQyxVQUFVLENBQUMsTUFBTSxDQUFDLENBQUMsQ0FBQyxZQUFZLENBQUMsR0FBRyxDQUFDLFVBQVUsQ0FBQyxDQUFDLENBQUMsQ0FBQyxDQUFDLEdBQUcsQ0FBQyxDQUFDLEdBQTVGLENBQTZGLEdBQzNHLEVBQUUsQ0FDSCxDQUFDO0FBQ0osQ0FBQzs7Ozs7QUFFRCxTQUFTLFVBQVUsQ0FBQyxLQUFvQjtJQUN0QyxPQUFPLEtBQUssQ0FBQyxTQUFTOzs7O0lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksRUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxFQUEvQyxDQUErQyxFQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7QUFDdkYsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIENvbXBvbmVudCxcbiAgQ29udGVudENoaWxkLFxuICBFbGVtZW50UmVmLFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkRlc3Ryb3ksXG4gIE91dHB1dCxcbiAgUmVuZGVyZXIyLFxuICBUZW1wbGF0ZVJlZixcbiAgVmlld0NoaWxkLFxuICBWaWV3Q2hpbGRyZW4sXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50LCBTdWJqZWN0LCB0aW1lciB9IGZyb20gJ3J4anMnO1xuaW1wb3J0IHsgZmlsdGVyLCB0YWtlLCB0YWtlVW50aWwsIGRlYm91bmNlVGltZSB9IGZyb20gJ3J4anMvb3BlcmF0b3JzJztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi8uLi9tb2RlbHMvdG9hc3Rlcic7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvY29uZmlybWF0aW9uLnNlcnZpY2UnO1xuaW1wb3J0IHsgQnV0dG9uQ29tcG9uZW50IH0gZnJvbSAnLi4vYnV0dG9uL2J1dHRvbi5jb21wb25lbnQnO1xuXG5leHBvcnQgdHlwZSBNb2RhbFNpemUgPSAnc20nIHwgJ21kJyB8ICdsZycgfCAneGwnO1xuXG5jb25zdCBBTklNQVRJT05fVElNRU9VVCA9IDIwMDtcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLW1vZGFsJyxcbiAgdGVtcGxhdGVVcmw6ICcuL21vZGFsLmNvbXBvbmVudC5odG1sJyxcbn0pXG5leHBvcnQgY2xhc3MgTW9kYWxDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xuICBASW5wdXQoKVxuICBnZXQgdmlzaWJsZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcbiAgfVxuICBzZXQgdmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIGlmICh0eXBlb2YgdmFsdWUgIT09ICdib29sZWFuJykgcmV0dXJuO1xuXG4gICAgaWYgKCF0aGlzLm1vZGFsQ29udGVudCkge1xuICAgICAgaWYgKHZhbHVlKSB7XG4gICAgICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgICAgIHRoaXMuc2hvd01vZGFsID0gdmFsdWU7XG4gICAgICAgICAgdGhpcy52aXNpYmxlID0gdmFsdWU7XG4gICAgICAgIH0sIDApO1xuICAgICAgfVxuICAgICAgcmV0dXJuO1xuICAgIH1cblxuICAgIGlmICh2YWx1ZSkge1xuICAgICAgdGhpcy5zZXRWaXNpYmxlKHZhbHVlKTtcbiAgICAgIHRoaXMubGlzdGVuKCk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMuY2xvc2FibGUgPSBmYWxzZTtcbiAgICAgIHRoaXMucmVuZGVyZXIuYWRkQ2xhc3ModGhpcy5tb2RhbENvbnRlbnQubmF0aXZlRWxlbWVudCwgJ2ZhZGUtb3V0LXRvcCcpO1xuICAgICAgc2V0VGltZW91dCgoKSA9PiB7XG4gICAgICAgIHRoaXMuc2V0VmlzaWJsZSh2YWx1ZSk7XG4gICAgICAgIHRoaXMubmdPbkRlc3Ryb3koKTtcbiAgICAgIH0sIEFOSU1BVElPTl9USU1FT1VUIC0gMTApO1xuICAgIH1cbiAgfVxuXG4gIEBJbnB1dCgpXG4gIGdldCBidXN5KCk6IGJvb2xlYW4ge1xuICAgIHJldHVybiB0aGlzLl9idXN5O1xuICB9XG4gIHNldCBidXN5KHZhbHVlOiBib29sZWFuKSB7XG4gICAgaWYgKHRoaXMuYWJwU3VibWl0ICYmIHRoaXMuYWJwU3VibWl0IGluc3RhbmNlb2YgQnV0dG9uQ29tcG9uZW50KSB7XG4gICAgICB0aGlzLmFicFN1Ym1pdC5sb2FkaW5nID0gdmFsdWU7XG4gICAgfVxuXG4gICAgdGhpcy5fYnVzeSA9IHZhbHVlO1xuICB9XG5cbiAgQElucHV0KCkgY2VudGVyZWQ6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBASW5wdXQoKSBtb2RhbENsYXNzOiBzdHJpbmcgPSAnJztcblxuICBASW5wdXQoKSBzaXplOiBNb2RhbFNpemUgPSAnbGcnO1xuXG4gIEBJbnB1dCgpIGhlaWdodDogbnVtYmVyO1xuXG4gIEBJbnB1dCgpIG1pbkhlaWdodDogbnVtYmVyO1xuXG4gIEBPdXRwdXQoKSB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBPdXRwdXQoKSBpbml0ID0gbmV3IEV2ZW50RW1pdHRlcjx2b2lkPigpO1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEhlYWRlcicsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBIZWFkZXI6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZCgnYWJwQm9keScsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBCb2R5OiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEZvb3RlcicsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBGb290ZXI6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZCgnYWJwQ2xvc2UnLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IEVsZW1lbnRSZWYgfSkgYWJwQ2xvc2U6IEVsZW1lbnRSZWY8YW55PjtcblxuICBAQ29udGVudENoaWxkKEJ1dHRvbkNvbXBvbmVudCwgeyBzdGF0aWM6IGZhbHNlLCByZWFkOiBCdXR0b25Db21wb25lbnQgfSkgYWJwU3VibWl0OiBCdXR0b25Db21wb25lbnQ7XG5cbiAgQFZpZXdDaGlsZCgnYWJwTW9kYWxDb250ZW50JywgeyBzdGF0aWM6IGZhbHNlIH0pIG1vZGFsQ29udGVudDogRWxlbWVudFJlZjtcblxuICBAVmlld0NoaWxkcmVuKCdhYnAtYnV0dG9uJykgYWJwQnV0dG9ucztcblxuICBAT3V0cHV0KClcbiAgc2hvdyA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcblxuICBAT3V0cHV0KClcbiAgaGlkZSA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcblxuICBfdmlzaWJsZTogYm9vbGVhbiA9IGZhbHNlO1xuXG4gIF9idXN5OiBib29sZWFuID0gZmFsc2U7XG5cbiAgc2hvd01vZGFsOiBib29sZWFuID0gZmFsc2U7XG5cbiAgaXNPcGVuQ29uZmlybWF0aW9uOiBib29sZWFuID0gZmFsc2U7XG5cbiAgY2xvc2FibGU6IGJvb2xlYW4gPSBmYWxzZTtcblxuICBkZXN0cm95JCA9IG5ldyBTdWJqZWN0PHZvaWQ+KCk7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyLCBwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UpIHt9XG5cbiAgbmdPbkRlc3Ryb3koKTogdm9pZCB7XG4gICAgdGhpcy5kZXN0cm95JC5uZXh0KCk7XG4gIH1cblxuICBzZXRWaXNpYmxlKHZhbHVlOiBib29sZWFuKSB7XG4gICAgdGhpcy5fdmlzaWJsZSA9IHZhbHVlO1xuICAgIHRoaXMudmlzaWJsZUNoYW5nZS5lbWl0KHZhbHVlKTtcbiAgICB0aGlzLnNob3dNb2RhbCA9IHZhbHVlO1xuXG4gICAgaWYgKHZhbHVlKSB7XG4gICAgICB0aW1lcihBTklNQVRJT05fVElNRU9VVCArIDEwMClcbiAgICAgICAgLnBpcGUodGFrZSgxKSlcbiAgICAgICAgLnN1YnNjcmliZShfID0+ICh0aGlzLmNsb3NhYmxlID0gdHJ1ZSkpO1xuXG4gICAgICB0aGlzLnJlbmRlcmVyLmFkZENsYXNzKGRvY3VtZW50LmJvZHksICdtb2RhbC1vcGVuJyk7XG4gICAgICB0aGlzLnNob3cuZW1pdCgpO1xuICAgIH0gZWxzZSB7XG4gICAgICB0aGlzLmNsb3NhYmxlID0gZmFsc2U7XG4gICAgICB0aGlzLnJlbmRlcmVyLnJlbW92ZUNsYXNzKGRvY3VtZW50LmJvZHksICdtb2RhbC1vcGVuJyk7XG4gICAgICB0aGlzLmhpZGUuZW1pdCgpO1xuICAgIH1cbiAgfVxuXG4gIGxpc3RlbigpIHtcbiAgICBmcm9tRXZlbnQoZG9jdW1lbnQsICdrZXl1cCcpXG4gICAgICAucGlwZShcbiAgICAgICAgdGFrZVVudGlsKHRoaXMuZGVzdHJveSQpLFxuICAgICAgICBkZWJvdW5jZVRpbWUoMTUwKSxcbiAgICAgICAgZmlsdGVyKChrZXk6IEtleWJvYXJkRXZlbnQpID0+IGtleSAmJiBrZXkuY29kZSA9PT0gJ0VzY2FwZScgJiYgdGhpcy5jbG9zYWJsZSksXG4gICAgICApXG4gICAgICAuc3Vic2NyaWJlKF8gPT4ge1xuICAgICAgICB0aGlzLmNsb3NlKCk7XG4gICAgICB9KTtcblxuICAgIHNldFRpbWVvdXQoKCkgPT4ge1xuICAgICAgaWYgKCF0aGlzLmFicENsb3NlKSByZXR1cm47XG4gICAgICBmcm9tRXZlbnQodGhpcy5hYnBDbG9zZS5uYXRpdmVFbGVtZW50LCAnY2xpY2snKVxuICAgICAgICAucGlwZShcbiAgICAgICAgICB0YWtlVW50aWwodGhpcy5kZXN0cm95JCksXG4gICAgICAgICAgZmlsdGVyKCgpID0+ICEhKHRoaXMuY2xvc2FibGUgJiYgdGhpcy5tb2RhbENvbnRlbnQpKSxcbiAgICAgICAgKVxuICAgICAgICAuc3Vic2NyaWJlKCgpID0+IHRoaXMuY2xvc2UoKSk7XG4gICAgfSwgMCk7XG5cbiAgICB0aGlzLmluaXQuZW1pdCgpO1xuICB9XG5cbiAgY2xvc2UoKSB7XG4gICAgaWYgKCF0aGlzLmNsb3NhYmxlIHx8IHRoaXMuYnVzeSkgcmV0dXJuO1xuXG4gICAgY29uc3Qgbm9kZXMgPSBnZXRGbGF0Tm9kZXMoXG4gICAgICAodGhpcy5tb2RhbENvbnRlbnQubmF0aXZlRWxlbWVudC5xdWVyeVNlbGVjdG9yKCcjYWJwLW1vZGFsLWJvZHknKSBhcyBIVE1MRWxlbWVudCkuY2hpbGROb2RlcyxcbiAgICApO1xuXG4gICAgaWYgKGhhc05nRGlydHkobm9kZXMpKSB7XG4gICAgICBpZiAodGhpcy5pc09wZW5Db25maXJtYXRpb24pIHJldHVybjtcblxuICAgICAgdGhpcy5pc09wZW5Db25maXJtYXRpb24gPSB0cnVlO1xuICAgICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlXG4gICAgICAgIC53YXJuKCdBYnBBY2NvdW50OjpBcmVZb3VTdXJlWW91V2FudFRvQ2FuY2VsRWRpdGluZ1dhcm5pbmdNZXNzYWdlJywgJ0FicEFjY291bnQ6OkFyZVlvdVN1cmUnKVxuICAgICAgICAuc3Vic2NyaWJlKChzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSA9PiB7XG4gICAgICAgICAgdGltZXIoQU5JTUFUSU9OX1RJTUVPVVQpLnN1YnNjcmliZSgoKSA9PiB7XG4gICAgICAgICAgICB0aGlzLmlzT3BlbkNvbmZpcm1hdGlvbiA9IGZhbHNlO1xuICAgICAgICAgIH0pO1xuXG4gICAgICAgICAgaWYgKHN0YXR1cyA9PT0gVG9hc3Rlci5TdGF0dXMuY29uZmlybSkge1xuICAgICAgICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgICAgICAgfVxuICAgICAgICB9KTtcbiAgICB9IGVsc2Uge1xuICAgICAgdGhpcy52aXNpYmxlID0gZmFsc2U7XG4gICAgfVxuICB9XG59XG5cbmZ1bmN0aW9uIGdldEZsYXROb2Rlcyhub2RlczogTm9kZUxpc3QpOiBIVE1MRWxlbWVudFtdIHtcbiAgcmV0dXJuIEFycmF5LmZyb20obm9kZXMpLnJlZHVjZShcbiAgICAoYWNjLCB2YWwpID0+IFsuLi5hY2MsIC4uLih2YWwuY2hpbGROb2RlcyAmJiB2YWwuY2hpbGROb2Rlcy5sZW5ndGggPyBnZXRGbGF0Tm9kZXModmFsLmNoaWxkTm9kZXMpIDogW3ZhbF0pXSxcbiAgICBbXSxcbiAgKTtcbn1cblxuZnVuY3Rpb24gaGFzTmdEaXJ0eShub2RlczogSFRNTEVsZW1lbnRbXSkge1xuICByZXR1cm4gbm9kZXMuZmluZEluZGV4KG5vZGUgPT4gKG5vZGUuY2xhc3NOYW1lIHx8ICcnKS5pbmRleE9mKCduZy1kaXJ0eScpID4gLTEpID4gLTE7XG59XG4iXX0=