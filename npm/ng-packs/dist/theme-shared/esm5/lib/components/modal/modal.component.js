/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import * as tslib_1 from 'tslib';
import {
  Component,
  ContentChild,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  Renderer2,
  TemplateRef,
  ViewChild,
  ViewChildren,
} from '@angular/core';
import { fromEvent, Subject } from 'rxjs';
import { debounceTime, filter, takeUntil } from 'rxjs/operators';
import { ConfirmationService } from '../../services/confirmation.service';
import { ButtonComponent } from '../button/button.component';
import { fadeAnimation, dialogAnimation } from '../../animations/modal.animations';
var ModalComponent = /** @class */ (function() {
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
  Object.defineProperty(ModalComponent.prototype, 'visible', {
    /**
     * @return {?}
     */
    get: function() {
      return this._visible;
    },
    /**
     * @param {?} value
     * @return {?}
     */
    set: function(value) {
      var _this = this;
      if (typeof value !== 'boolean') return;
      this.isModalOpen = value;
      this._visible = value;
      this.visibleChange.emit(value);
      if (value) {
        setTimeout(
          /**
           * @return {?}
           */
          function() {
            return _this.listen();
          },
          0,
        );
        this.renderer.addClass(document.body, 'modal-open');
        this.appear.emit();
      } else {
        this.renderer.removeClass(document.body, 'modal-open');
        this.disappear.emit();
      }
    },
    enumerable: true,
    configurable: true,
  });
  Object.defineProperty(ModalComponent.prototype, 'busy', {
    /**
     * @return {?}
     */
    get: function() {
      return this._busy;
    },
    /**
     * @param {?} value
     * @return {?}
     */
    set: function(value) {
      if (this.abpSubmit && this.abpSubmit instanceof ButtonComponent) {
        this.abpSubmit.loading = value;
      }
      this._busy = value;
    },
    enumerable: true,
    configurable: true,
  });
  /**
   * @return {?}
   */
  ModalComponent.prototype.ngOnDestroy
  /**
   * @return {?}
   */ = function() {
    this.destroy$.next();
  };
  /**
   * @return {?}
   */
  ModalComponent.prototype.close
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    if (this.busy) return;
    /** @type {?} */
    var nodes = getFlatNodes(
      /** @type {?} */ (this.modalContent.nativeElement.querySelector('#abp-modal-body')).childNodes,
    );
    if (hasNgDirty(nodes)) {
      if (this.isConfirmationOpen) return;
      this.isConfirmationOpen = true;
      this.confirmationService
        .warn('AbpAccount::AreYouSureYouWantToCancelEditingWarningMessage', 'AbpAccount::AreYouSure')
        .subscribe(
          /**
           * @param {?} status
           * @return {?}
           */
          function(status) {
            _this.isConfirmationOpen = false;
            if (status === 'confirm' /* confirm */) {
              _this.visible = false;
            }
          },
        );
    } else {
      this.visible = false;
    }
  };
  /**
   * @return {?}
   */
  ModalComponent.prototype.listen
  /**
   * @return {?}
   */ = function() {
    var _this = this;
    fromEvent(document, 'keyup')
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(150),
        filter(
          /**
           * @param {?} key
           * @return {?}
           */
          function(key) {
            return key && key.code === 'Escape';
          },
        ),
      )
      .subscribe(
        /**
         * @param {?} _
         * @return {?}
         */
        function(_) {
          _this.close();
        },
      );
    setTimeout(
      /**
       * @return {?}
       */
      function() {
        if (!_this.abpClose) return;
        fromEvent(_this.abpClose.nativeElement, 'click')
          .pipe(
            takeUntil(_this.destroy$),
            filter(
              /**
               * @return {?}
               */
              function() {
                return !!_this.modalContent;
              },
            ),
          )
          .subscribe(
            /**
             * @return {?}
             */
            function() {
              return _this.close();
            },
          );
      },
      0,
    );
    this.init.emit();
  };
  ModalComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-modal',
          template:
            '<ng-container *ngIf="visible">\n  <div class="modal show {{ modalClass }}" tabindex="-1" role="dialog">\n    <div class="modal-backdrop" [@fade]="isModalOpen" (click)="close()"></div>\n    <div\n      id="abp-modal-dialog"\n      class="modal-dialog modal-{{ size }}"\n      role="document"\n      [@dialog]="isModalOpen"\n      #abpModalContent\n    >\n      <div id="abp-modal-content" class="modal-content">\n        <div id="abp-modal-header" class="modal-header">\n          <ng-container *ngTemplateOutlet="abpHeader"></ng-container>\n          \u200B\n          <button id="abp-modal-close-button" type="button" class="close" aria-label="Close" (click)="close()">\n            <span aria-hidden="true">&times;</span>\n          </button>\n        </div>\n        <div id="abp-modal-body" class="modal-body">\n          <ng-container *ngTemplateOutlet="abpBody"></ng-container>\n        </div>\n        <div id="abp-modal-footer" class="modal-footer">\n          <ng-container *ngTemplateOutlet="abpFooter"></ng-container>\n        </div>\n      </div>\n    </div>\n    <ng-content></ng-content>\n  </div>\n</ng-container>\n',
          animations: [fadeAnimation, dialogAnimation],
        },
      ],
    },
  ];
  /** @nocollapse */
  ModalComponent.ctorParameters = function() {
    return [{ type: Renderer2 }, { type: ConfirmationService }];
  };
  ModalComponent.propDecorators = {
    visible: [{ type: Input }],
    busy: [{ type: Input }],
    centered: [{ type: Input }],
    modalClass: [{ type: Input }],
    size: [{ type: Input }],
    abpSubmit: [{ type: ContentChild, args: [ButtonComponent, { static: false, read: ButtonComponent }] }],
    abpHeader: [{ type: ContentChild, args: ['abpHeader', { static: false }] }],
    abpBody: [{ type: ContentChild, args: ['abpBody', { static: false }] }],
    abpFooter: [{ type: ContentChild, args: ['abpFooter', { static: false }] }],
    abpClose: [{ type: ContentChild, args: ['abpClose', { static: false, read: ElementRef }] }],
    modalContent: [{ type: ViewChild, args: ['abpModalContent', { static: false }] }],
    abpButtons: [{ type: ViewChildren, args: ['abp-button'] }],
    visibleChange: [{ type: Output }],
    init: [{ type: Output }],
    appear: [{ type: Output }],
    disappear: [{ type: Output }],
  };
  return ModalComponent;
})();
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
  return Array.from(nodes).reduce(
    /**
     * @param {?} acc
     * @param {?} val
     * @return {?}
     */
    function(acc, val) {
      return tslib_1.__spread(acc, val.childNodes && val.childNodes.length ? getFlatNodes(val.childNodes) : [val]);
    },
    [],
  );
}
/**
 * @param {?} nodes
 * @return {?}
 */
function hasNgDirty(nodes) {
  return (
    nodes.findIndex(
      /**
       * @param {?} node
       * @return {?}
       */
      function(node) {
        return (node.className || '').indexOf('ng-dirty') > -1;
      },
    ) > -1
  );
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibW9kYWwuY29tcG9uZW50LmpzIiwic291cmNlUm9vdCI6Im5nOi8vQGFicC9uZy50aGVtZS5zaGFyZWQvIiwic291cmNlcyI6WyJsaWIvY29tcG9uZW50cy9tb2RhbC9tb2RhbC5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQ0wsU0FBUyxFQUNULFlBQVksRUFDWixVQUFVLEVBQ1YsWUFBWSxFQUNaLEtBQUssRUFFTCxNQUFNLEVBQ04sU0FBUyxFQUNULFdBQVcsRUFDWCxTQUFTLEVBQ1QsWUFBWSxHQUNiLE1BQU0sZUFBZSxDQUFDO0FBQ3ZCLE9BQU8sRUFBRSxTQUFTLEVBQUUsT0FBTyxFQUFFLE1BQU0sTUFBTSxDQUFDO0FBQzFDLE9BQU8sRUFBRSxZQUFZLEVBQUUsTUFBTSxFQUFFLFNBQVMsRUFBRSxNQUFNLGdCQUFnQixDQUFDO0FBRWpFLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBQzFFLE9BQU8sRUFBRSxlQUFlLEVBQUUsTUFBTSw0QkFBNEIsQ0FBQztBQUM3RCxPQUFPLEVBQUUsYUFBYSxFQUFFLGVBQWUsRUFBRSxNQUFNLG1DQUFtQyxDQUFDO0FBSW5GO0lBK0VFLHdCQUFvQixRQUFtQixFQUFVLG1CQUF3QztRQUFyRSxhQUFRLEdBQVIsUUFBUSxDQUFXO1FBQVUsd0JBQW1CLEdBQW5CLG1CQUFtQixDQUFxQjtRQXhDaEYsYUFBUSxHQUFHLEtBQUssQ0FBQztRQUVqQixlQUFVLEdBQUcsRUFBRSxDQUFDO1FBRWhCLFNBQUksR0FBYyxJQUFJLENBQUM7UUFrQmIsa0JBQWEsR0FBRyxJQUFJLFlBQVksRUFBVyxDQUFDO1FBRTVDLFNBQUksR0FBRyxJQUFJLFlBQVksRUFBUSxDQUFDO1FBRWhDLFdBQU0sR0FBRyxJQUFJLFlBQVksRUFBRSxDQUFDO1FBRTVCLGNBQVMsR0FBRyxJQUFJLFlBQVksRUFBRSxDQUFDO1FBRWxELGFBQVEsR0FBRyxLQUFLLENBQUM7UUFFakIs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Q0FDSCxDQUFDO0FBQ0osQ0FBQzs7Ozs7QUFFRCxTQUFTLFVBQVUsQ0FBQyxLQUFvQjtJQUN0QyxPQUFPLEtBQUssQ0FBQyxTQUFTOzs7O0lBQUMsVUFBQSxJQUFJLElBQUksT0FBQSxDQUFDLElBQUksQ0FBQyxTQUFTLElBQUksRUFBRSxDQUFDLENBQUMsT0FBTyxDQUFDLFVBQVUsQ0FBQyxHQUFHLENBQUMsQ0FBQyxFQUEvQyxDQUErQyxFQUFDLEdBQUcsQ0FBQyxDQUFDLENBQUM7QUFDdkYsQ0FBQyIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7XG4gIENvbXBvbmVudCxcbiAgQ29udGVudENoaWxkLFxuICBFbGVtZW50UmVmLFxuICBFdmVudEVtaXR0ZXIsXG4gIElucHV0LFxuICBPbkRlc3Ryb3ksXG4gIE91dHB1dCxcbiAgUmVuZGVyZXIyLFxuICBUZW1wbGF0ZVJlZixcbiAgVmlld0NoaWxkLFxuICBWaWV3Q2hpbGRyZW4sXG59IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgZnJvbUV2ZW50LCBTdWJqZWN0IH0gZnJvbSAncnhqcyc7XG5pbXBvcnQgeyBkZWJvdW5jZVRpbWUsIGZpbHRlciwgdGFrZVVudGlsIH0gZnJvbSAncnhqcy9vcGVyYXRvcnMnO1xuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uLy4uL21vZGVscy90b2FzdGVyJztcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZSc7XG5pbXBvcnQgeyBCdXR0b25Db21wb25lbnQgfSBmcm9tICcuLi9idXR0b24vYnV0dG9uLmNvbXBvbmVudCc7XG5pbXBvcnQgeyBmYWRlQW5pbWF0aW9uLCBkaWFsb2dBbmltYXRpb24gfSBmcm9tICcuLi8uLi9hbmltYXRpb25zL21vZGFsLmFuaW1hdGlvbnMnO1xuXG5leHBvcnQgdHlwZSBNb2RhbFNpemUgPSAnc20nIHwgJ21kJyB8ICdsZycgfCAneGwnO1xuXG5AQ29tcG9uZW50KHtcbiAgc2VsZWN0b3I6ICdhYnAtbW9kYWwnLFxuICB0ZW1wbGF0ZVVybDogJy4vbW9kYWwuY29tcG9uZW50Lmh0bWwnLFxuICBhbmltYXRpb25zOiBbZmFkZUFuaW1hdGlvbiwgZGlhbG9nQW5pbWF0aW9uXSxcbn0pXG5leHBvcnQgY2xhc3MgTW9kYWxDb21wb25lbnQgaW1wbGVtZW50cyBPbkRlc3Ryb3kge1xuICBASW5wdXQoKVxuICBnZXQgdmlzaWJsZSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fdmlzaWJsZTtcbiAgfVxuICBzZXQgdmlzaWJsZSh2YWx1ZTogYm9vbGVhbikge1xuICAgIGlmICh0eXBlb2YgdmFsdWUgIT09ICdib29sZWFuJykgcmV0dXJuO1xuXG4gICAgdGhpcy5pc01vZGFsT3BlbiA9IHZhbHVlO1xuICAgIHRoaXMuX3Zpc2libGUgPSB2YWx1ZTtcbiAgICB0aGlzLnZpc2libGVDaGFuZ2UuZW1pdCh2YWx1ZSk7XG5cbiAgICBpZiAodmFsdWUpIHtcbiAgICAgIHNldFRpbWVvdXQoKCkgPT4gdGhpcy5saXN0ZW4oKSwgMCk7XG4gICAgICB0aGlzLnJlbmRlcmVyLmFkZENsYXNzKGRvY3VtZW50LmJvZHksICdtb2RhbC1vcGVuJyk7XG4gICAgICB0aGlzLmFwcGVhci5lbWl0KCk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMucmVuZGVyZXIucmVtb3ZlQ2xhc3MoZG9jdW1lbnQuYm9keSwgJ21vZGFsLW9wZW4nKTtcbiAgICAgIHRoaXMuZGlzYXBwZWFyLmVtaXQoKTtcbiAgICB9XG4gIH1cblxuICBASW5wdXQoKVxuICBnZXQgYnVzeSgpOiBib29sZWFuIHtcbiAgICByZXR1cm4gdGhpcy5fYnVzeTtcbiAgfVxuICBzZXQgYnVzeSh2YWx1ZTogYm9vbGVhbikge1xuICAgIGlmICh0aGlzLmFicFN1Ym1pdCAmJiB0aGlzLmFicFN1Ym1pdCBpbnN0YW5jZW9mIEJ1dHRvbkNvbXBvbmVudCkge1xuICAgICAgdGhpcy5hYnBTdWJtaXQubG9hZGluZyA9IHZhbHVlO1xuICAgIH1cblxuICAgIHRoaXMuX2J1c3kgPSB2YWx1ZTtcbiAgfVxuXG4gIEBJbnB1dCgpIGNlbnRlcmVkID0gZmFsc2U7XG5cbiAgQElucHV0KCkgbW9kYWxDbGFzcyA9ICcnO1xuXG4gIEBJbnB1dCgpIHNpemU6IE1vZGFsU2l6ZSA9ICdsZyc7XG5cbiAgQENvbnRlbnRDaGlsZChCdXR0b25Db21wb25lbnQsIHsgc3RhdGljOiBmYWxzZSwgcmVhZDogQnV0dG9uQ29tcG9uZW50IH0pXG4gIGFicFN1Ym1pdDogQnV0dG9uQ29tcG9uZW50O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEhlYWRlcicsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBIZWFkZXI6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZCgnYWJwQm9keScsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBCb2R5OiBUZW1wbGF0ZVJlZjxhbnk+O1xuXG4gIEBDb250ZW50Q2hpbGQoJ2FicEZvb3RlcicsIHsgc3RhdGljOiBmYWxzZSB9KSBhYnBGb290ZXI6IFRlbXBsYXRlUmVmPGFueT47XG5cbiAgQENvbnRlbnRDaGlsZCgnYWJwQ2xvc2UnLCB7IHN0YXRpYzogZmFsc2UsIHJlYWQ6IEVsZW1lbnRSZWYgfSlcbiAgYWJwQ2xvc2U6IEVsZW1lbnRSZWY8YW55PjtcblxuICBAVmlld0NoaWxkKCdhYnBNb2RhbENvbnRlbnQnLCB7IHN0YXRpYzogZmFsc2UgfSkgbW9kYWxDb250ZW50OiBFbGVtZW50UmVmO1xuXG4gIEBWaWV3Q2hpbGRyZW4oJ2FicC1idXR0b24nKSBhYnBCdXR0b25zO1xuXG4gIEBPdXRwdXQoKSByZWFkb25seSB2aXNpYmxlQ2hhbmdlID0gbmV3IEV2ZW50RW1pdHRlcjxib29sZWFuPigpO1xuXG4gIEBPdXRwdXQoKSByZWFkb25seSBpbml0ID0gbmV3IEV2ZW50RW1pdHRlcjx2b2lkPigpO1xuXG4gIEBPdXRwdXQoKSByZWFkb25seSBhcHBlYXIgPSBuZXcgRXZlbnRFbWl0dGVyKCk7XG5cbiAgQE91dHB1dCgpIHJlYWRvbmx5IGRpc2FwcGVhciA9IG5ldyBFdmVudEVtaXR0ZXIoKTtcblxuICBfdmlzaWJsZSA9IGZhbHNlO1xuXG4gIF9idXN5ID0gZmFsc2U7XG5cbiAgaXNNb2RhbE9wZW4gPSBmYWxzZTtcblxuICBpc0NvbmZpcm1hdGlvbk9wZW4gPSBmYWxzZTtcblxuICBkZXN0cm95JCA9IG5ldyBTdWJqZWN0PHZvaWQ+KCk7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSByZW5kZXJlcjogUmVuZGVyZXIyLCBwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UpIHt9XG5cbiAgbmdPbkRlc3Ryb3koKTogdm9pZCB7XG4gICAgdGhpcy5kZXN0cm95JC5uZXh0KCk7XG4gIH1cblxuICBjbG9zZSgpIHtcbiAgICBpZiAodGhpcy5idXN5KSByZXR1cm47XG5cbiAgICBjb25zdCBub2RlcyA9IGdldEZsYXROb2RlcyhcbiAgICAgICh0aGlzLm1vZGFsQ29udGVudC5uYXRpdmVFbGVtZW50LnF1ZXJ5U2VsZWN0b3IoJyNhYnAtbW9kYWwtYm9keScpIGFzIEhUTUxFbGVtZW50KS5jaGlsZE5vZGVzLFxuICAgICk7XG5cbiAgICBpZiAoaGFzTmdEaXJ0eShub2RlcykpIHtcbiAgICAgIGlmICh0aGlzLmlzQ29uZmlybWF0aW9uT3BlbikgcmV0dXJuO1xuXG4gICAgICB0aGlzLmlzQ29uZmlybWF0aW9uT3BlbiA9IHRydWU7XG4gICAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2VcbiAgICAgICAgLndhcm4oJ0FicEFjY291bnQ6OkFyZVlvdVN1cmVZb3VXYW50VG9DYW5jZWxFZGl0aW5nV2FybmluZ01lc3NhZ2UnLCAnQWJwQWNjb3VudDo6QXJlWW91U3VyZScpXG4gICAgICAgIC5zdWJzY3JpYmUoKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpID0+IHtcbiAgICAgICAgICB0aGlzLmlzQ29uZmlybWF0aW9uT3BlbiA9IGZhbHNlO1xuICAgICAgICAgIGlmIChzdGF0dXMgPT09IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm0pIHtcbiAgICAgICAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgICAgICAgIH1cbiAgICAgICAgfSk7XG4gICAgfSBlbHNlIHtcbiAgICAgIHRoaXMudmlzaWJsZSA9IGZhbHNlO1xuICAgIH1cbiAgfVxuXG4gIGxpc3RlbigpIHtcbiAgICBmcm9tRXZlbnQoZG9jdW1lbnQsICdrZXl1cCcpXG4gICAgICAucGlwZShcbiAgICAgICAgdGFrZVVudGlsKHRoaXMuZGVzdHJveSQpLFxuICAgICAgICBkZWJvdW5jZVRpbWUoMTUwKSxcbiAgICAgICAgZmlsdGVyKChrZXk6IEtleWJvYXJkRXZlbnQpID0+IGtleSAmJiBrZXkuY29kZSA9PT0gJ0VzY2FwZScpLFxuICAgICAgKVxuICAgICAgLnN1YnNjcmliZShfID0+IHtcbiAgICAgICAgdGhpcy5jbG9zZSgpO1xuICAgICAgfSk7XG5cbiAgICBzZXRUaW1lb3V0KCgpID0+IHtcbiAgICAgIGlmICghdGhpcy5hYnBDbG9zZSkgcmV0dXJuO1xuICAgICAgZnJvbUV2ZW50KHRoaXMuYWJwQ2xvc2UubmF0aXZlRWxlbWVudCwgJ2NsaWNrJylcbiAgICAgICAgLnBpcGUoXG4gICAgICAgICAgdGFrZVVudGlsKHRoaXMuZGVzdHJveSQpLFxuICAgICAgICAgIGZpbHRlcigoKSA9PiAhIXRoaXMubW9kYWxDb250ZW50KSxcbiAgICAgICAgKVxuICAgICAgICAuc3Vic2NyaWJlKCgpID0+IHRoaXMuY2xvc2UoKSk7XG4gICAgfSwgMCk7XG5cbiAgICB0aGlzLmluaXQuZW1pdCgpO1xuICB9XG59XG5cbmZ1bmN0aW9uIGdldEZsYXROb2Rlcyhub2RlczogTm9kZUxpc3QpOiBIVE1MRWxlbWVudFtdIHtcbiAgcmV0dXJuIEFycmF5LmZyb20obm9kZXMpLnJlZHVjZShcbiAgICAoYWNjLCB2YWwpID0+IFsuLi5hY2MsIC4uLih2YWwuY2hpbGROb2RlcyAmJiB2YWwuY2hpbGROb2Rlcy5sZW5ndGggPyBnZXRGbGF0Tm9kZXModmFsLmNoaWxkTm9kZXMpIDogW3ZhbF0pXSxcbiAgICBbXSxcbiAgKTtcbn1cblxuZnVuY3Rpb24gaGFzTmdEaXJ0eShub2RlczogSFRNTEVsZW1lbnRbXSkge1xuICByZXR1cm4gbm9kZXMuZmluZEluZGV4KG5vZGUgPT4gKG5vZGUuY2xhc3NOYW1lIHx8ICcnKS5pbmRleE9mKCduZy1kaXJ0eScpID4gLTEpID4gLTE7XG59XG4iXX0=
