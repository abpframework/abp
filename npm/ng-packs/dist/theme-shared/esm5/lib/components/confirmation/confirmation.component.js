/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { ConfirmationService } from '../../services/confirmation.service';
var ConfirmationComponent = /** @class */ (function() {
  function ConfirmationComponent(confirmationService) {
    this.confirmationService = confirmationService;
    this.confirm = 'confirm' /* confirm */;
    this.reject = 'reject' /* reject */;
    this.dismiss = 'dismiss' /* dismiss */;
  }
  /**
   * @param {?} status
   * @return {?}
   */
  ConfirmationComponent.prototype.close
  /**
   * @param {?} status
   * @return {?}
   */ = function(status) {
    this.confirmationService.clear(status);
  };
  ConfirmationComponent.decorators = [
    {
      type: Component,
      args: [
        {
          selector: 'abp-confirmation',
          // tslint:disable-next-line: component-max-inline-declarations
          template:
            '\n    <p-toast\n      position="center"\n      key="abpConfirmation"\n      (onClose)="close(dismiss)"\n      [modal]="true"\n      [baseZIndex]="1000"\n      styleClass="abp-confirm"\n    >\n      <ng-template let-message pTemplate="message">\n        <i class="fa fa-exclamation-circle abp-confirm-icon"></i>\n        <div *ngIf="message.summary" class="abp-confirm-summary">\n          {{ message.summary | abpLocalization: message.titleLocalizationParams }}\n        </div>\n        <div class="abp-confirm-body">\n          {{ message.detail | abpLocalization: message.messageLocalizationParams }}\n        </div>\n\n        <div class="abp-confirm-footer justify-content-center">\n          <button\n            *ngIf="!message.hideCancelBtn"\n            id="cancel"\n            type="button"\n            class="btn btn-sm btn-primary"\n            (click)="close(reject)"\n          >\n            {{ message.cancelCopy || \'AbpIdentity::Cancel\' | abpLocalization }}\n          </button>\n          <button\n            *ngIf="!message.hideYesBtn"\n            id="confirm"\n            type="button"\n            class="btn btn-sm btn-primary"\n            (click)="close(confirm)"\n            autofocus\n          >\n            <span>{{ message.yesCopy || \'AbpIdentity::Yes\' | abpLocalization }}</span>\n          </button>\n        </div>\n      </ng-template>\n    </p-toast>\n  ',
        },
      ],
    },
  ];
  /** @nocollapse */
  ConfirmationComponent.ctorParameters = function() {
    return [{ type: ConfirmationService }];
  };
  return ConfirmationComponent;
})();
export { ConfirmationComponent };
if (false) {
  /** @type {?} */
  ConfirmationComponent.prototype.confirm;
  /** @type {?} */
  ConfirmationComponent.prototype.reject;
  /** @type {?} */
  ConfirmationComponent.prototype.dismiss;
  /**
   * @type {?}
   * @private
   */
  ConfirmationComponent.prototype.confirmationService;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDMUMsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFHMUU7SUFtREUsK0JBQW9CLG1CQUF3QztRQUF4Qyx3QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCO1FBSjVELFlBQU8sMkJBQTBCO1FBQ2pDLFdBQU0seUJBQXlCO1FBQy9CLFlBQU8sMkJBQTBCO0lBRThCLENBQUM7Ozs7O0lBRWhFLHFDQUFLOzs7O0lBQUwsVUFBTSxNQUFzQjtRQUMxQixJQUFJLENBQUMsbUJBQW1CLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0lBQ3pDLENBQUM7O2dCQXZERixTQUFTLFNBQUM7b0JBQ1QsUUFBUSxFQUFFLGtCQUFrQjs7b0JBRTVCLFFBQVEsRUFBRSxnNkNBeUNUO2lCQUNGOzs7O2dCQWhEUSxtQkFBbUI7O0lBMkQ1Qiw0QkFBQztDQUFBLEFBeERELElBd0RDO1NBVlkscUJBQXFCOzs7SUFDaEMsd0NBQWlDOztJQUNqQyx1Q0FBK0I7O0lBQy9CLHdDQUFpQzs7Ozs7SUFFckIsb0RBQWdEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvY29uZmlybWF0aW9uLnNlcnZpY2UnO1xuaW1wb3J0IHsgVG9hc3RlciB9IGZyb20gJy4uLy4uL21vZGVscy90b2FzdGVyJztcblxuQENvbXBvbmVudCh7XG4gIHNlbGVjdG9yOiAnYWJwLWNvbmZpcm1hdGlvbicsXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogY29tcG9uZW50LW1heC1pbmxpbmUtZGVjbGFyYXRpb25zXG4gIHRlbXBsYXRlOiBgXG4gICAgPHAtdG9hc3RcbiAgICAgIHBvc2l0aW9uPVwiY2VudGVyXCJcbiAgICAgIGtleT1cImFicENvbmZpcm1hdGlvblwiXG4gICAgICAob25DbG9zZSk9XCJjbG9zZShkaXNtaXNzKVwiXG4gICAgICBbbW9kYWxdPVwidHJ1ZVwiXG4gICAgICBbYmFzZVpJbmRleF09XCIxMDAwXCJcbiAgICAgIHN0eWxlQ2xhc3M9XCJhYnAtY29uZmlybVwiXG4gICAgPlxuICAgICAgPG5nLXRlbXBsYXRlIGxldC1tZXNzYWdlIHBUZW1wbGF0ZT1cIm1lc3NhZ2VcIj5cbiAgICAgICAgPGkgY2xhc3M9XCJmYSBmYS1leGNsYW1hdGlvbi1jaXJjbGUgYWJwLWNvbmZpcm0taWNvblwiPjwvaT5cbiAgICAgICAgPGRpdiAqbmdJZj1cIm1lc3NhZ2Uuc3VtbWFyeVwiIGNsYXNzPVwiYWJwLWNvbmZpcm0tc3VtbWFyeVwiPlxuICAgICAgICAgIHt7IG1lc3NhZ2Uuc3VtbWFyeSB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS50aXRsZUxvY2FsaXphdGlvblBhcmFtcyB9fVxuICAgICAgICA8L2Rpdj5cbiAgICAgICAgPGRpdiBjbGFzcz1cImFicC1jb25maXJtLWJvZHlcIj5cbiAgICAgICAgICB7eyBtZXNzYWdlLmRldGFpbCB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS5tZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zIH19XG4gICAgICAgIDwvZGl2PlxuXG4gICAgICAgIDxkaXYgY2xhc3M9XCJhYnAtY29uZmlybS1mb290ZXIganVzdGlmeS1jb250ZW50LWNlbnRlclwiPlxuICAgICAgICAgIDxidXR0b25cbiAgICAgICAgICAgICpuZ0lmPVwiIW1lc3NhZ2UuaGlkZUNhbmNlbEJ0blwiXG4gICAgICAgICAgICBpZD1cImNhbmNlbFwiXG4gICAgICAgICAgICB0eXBlPVwiYnV0dG9uXCJcbiAgICAgICAgICAgIGNsYXNzPVwiYnRuIGJ0bi1zbSBidG4tcHJpbWFyeVwiXG4gICAgICAgICAgICAoY2xpY2spPVwiY2xvc2UocmVqZWN0KVwiXG4gICAgICAgICAgPlxuICAgICAgICAgICAge3sgbWVzc2FnZS5jYW5jZWxDb3B5IHx8ICdBYnBJZGVudGl0eTo6Q2FuY2VsJyB8IGFicExvY2FsaXphdGlvbiB9fVxuICAgICAgICAgIDwvYnV0dG9uPlxuICAgICAgICAgIDxidXR0b25cbiAgICAgICAgICAgICpuZ0lmPVwiIW1lc3NhZ2UuaGlkZVllc0J0blwiXG4gICAgICAgICAgICBpZD1cImNvbmZpcm1cIlxuICAgICAgICAgICAgdHlwZT1cImJ1dHRvblwiXG4gICAgICAgICAgICBjbGFzcz1cImJ0biBidG4tc20gYnRuLXByaW1hcnlcIlxuICAgICAgICAgICAgKGNsaWNrKT1cImNsb3NlKGNvbmZpcm0pXCJcbiAgICAgICAgICAgIGF1dG9mb2N1c1xuICAgICAgICAgID5cbiAgICAgICAgICAgIDxzcGFuPnt7IG1lc3NhZ2UueWVzQ29weSB8fCAnQWJwSWRlbnRpdHk6OlllcycgfCBhYnBMb2NhbGl6YXRpb24gfX08L3NwYW4+XG4gICAgICAgICAgPC9idXR0b24+XG4gICAgICAgIDwvZGl2PlxuICAgICAgPC9uZy10ZW1wbGF0ZT5cbiAgICA8L3AtdG9hc3Q+XG4gIGBcbn0pXG5leHBvcnQgY2xhc3MgQ29uZmlybWF0aW9uQ29tcG9uZW50IHtcbiAgY29uZmlybSA9IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm07XG4gIHJlamVjdCA9IFRvYXN0ZXIuU3RhdHVzLnJlamVjdDtcbiAgZGlzbWlzcyA9IFRvYXN0ZXIuU3RhdHVzLmRpc21pc3M7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlKSB7fVxuXG4gIGNsb3NlKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpIHtcbiAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2UuY2xlYXIoc3RhdHVzKTtcbiAgfVxufVxuIl19
