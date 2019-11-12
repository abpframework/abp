/**
 * @fileoverview added by tsickle
 * Generated from: lib/components/confirmation/confirmation.component.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { Component } from '@angular/core';
import { ConfirmationService } from '../../services/confirmation.service';
export class ConfirmationComponent {
    /**
     * @param {?} confirmationService
     */
    constructor(confirmationService) {
        this.confirmationService = confirmationService;
        this.confirm = "confirm" /* confirm */;
        this.reject = "reject" /* reject */;
        this.dismiss = "dismiss" /* dismiss */;
    }
    /**
     * @param {?} status
     * @return {?}
     */
    close(status) {
        this.confirmationService.clear(status);
    }
}
ConfirmationComponent.decorators = [
    { type: Component, args: [{
                selector: 'abp-confirmation',
                // tslint:disable-next-line: component-max-inline-declarations
                template: `
    <p-toast
      position="center"
      key="abpConfirmation"
      (onClose)="close(dismiss)"
      [modal]="true"
      [baseZIndex]="1000"
      styleClass="abp-confirm"
    >
      <ng-template let-message pTemplate="message">
        <i class="fa fa-exclamation-circle abp-confirm-icon"></i>
        <div *ngIf="message.summary" class="abp-confirm-summary">
          {{ message.summary | abpLocalization: message.titleLocalizationParams }}
        </div>
        <div class="abp-confirm-body">
          {{ message.detail | abpLocalization: message.messageLocalizationParams }}
        </div>

        <div class="abp-confirm-footer justify-content-center">
          <button
            *ngIf="!message.hideCancelBtn"
            id="cancel"
            type="button"
            class="btn btn-sm btn-primary"
            (click)="close(reject)"
          >
            {{ message.cancelText || message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}
          </button>
          <button
            *ngIf="!message.hideYesBtn"
            id="confirm"
            type="button"
            class="btn btn-sm btn-primary"
            (click)="close(confirm)"
            autofocus
          >
            <span>{{ message.yesText || message.yesCopy || 'AbpIdentity::Yes' | abpLocalization }}</span>
          </button>
        </div>
      </ng-template>
    </p-toast>
  `
            }] }
];
/** @nocollapse */
ConfirmationComponent.ctorParameters = () => [
    { type: ConfirmationService }
];
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzFDLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBaUQxRSxNQUFNLE9BQU8scUJBQXFCOzs7O0lBS2hDLFlBQW9CLG1CQUF3QztRQUF4Qyx3QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCO1FBSjVELFlBQU8sMkJBQTBCO1FBQ2pDLFdBQU0seUJBQXlCO1FBQy9CLFlBQU8sMkJBQTBCO0lBRThCLENBQUM7Ozs7O0lBRWhFLEtBQUssQ0FBQyxNQUFzQjtRQUMxQixJQUFJLENBQUMsbUJBQW1CLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0lBQ3pDLENBQUM7OztZQXZERixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGtCQUFrQjs7Z0JBRTVCLFFBQVEsRUFBRTs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7R0F5Q1Q7YUFDRjs7OztZQWhEUSxtQkFBbUI7Ozs7SUFrRDFCLHdDQUFpQzs7SUFDakMsdUNBQStCOztJQUMvQix3Q0FBaUM7Ozs7O0lBRXJCLG9EQUFnRCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5pbXBvcnQgeyBDb25maXJtYXRpb25TZXJ2aWNlIH0gZnJvbSAnLi4vLi4vc2VydmljZXMvY29uZmlybWF0aW9uLnNlcnZpY2UnO1xyXG5pbXBvcnQgeyBUb2FzdGVyIH0gZnJvbSAnLi4vLi4vbW9kZWxzL3RvYXN0ZXInO1xyXG5cclxuQENvbXBvbmVudCh7XHJcbiAgc2VsZWN0b3I6ICdhYnAtY29uZmlybWF0aW9uJyxcclxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGNvbXBvbmVudC1tYXgtaW5saW5lLWRlY2xhcmF0aW9uc1xyXG4gIHRlbXBsYXRlOiBgXHJcbiAgICA8cC10b2FzdFxyXG4gICAgICBwb3NpdGlvbj1cImNlbnRlclwiXHJcbiAgICAgIGtleT1cImFicENvbmZpcm1hdGlvblwiXHJcbiAgICAgIChvbkNsb3NlKT1cImNsb3NlKGRpc21pc3MpXCJcclxuICAgICAgW21vZGFsXT1cInRydWVcIlxyXG4gICAgICBbYmFzZVpJbmRleF09XCIxMDAwXCJcclxuICAgICAgc3R5bGVDbGFzcz1cImFicC1jb25maXJtXCJcclxuICAgID5cclxuICAgICAgPG5nLXRlbXBsYXRlIGxldC1tZXNzYWdlIHBUZW1wbGF0ZT1cIm1lc3NhZ2VcIj5cclxuICAgICAgICA8aSBjbGFzcz1cImZhIGZhLWV4Y2xhbWF0aW9uLWNpcmNsZSBhYnAtY29uZmlybS1pY29uXCI+PC9pPlxyXG4gICAgICAgIDxkaXYgKm5nSWY9XCJtZXNzYWdlLnN1bW1hcnlcIiBjbGFzcz1cImFicC1jb25maXJtLXN1bW1hcnlcIj5cclxuICAgICAgICAgIHt7IG1lc3NhZ2Uuc3VtbWFyeSB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS50aXRsZUxvY2FsaXphdGlvblBhcmFtcyB9fVxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgICAgIDxkaXYgY2xhc3M9XCJhYnAtY29uZmlybS1ib2R5XCI+XHJcbiAgICAgICAgICB7eyBtZXNzYWdlLmRldGFpbCB8IGFicExvY2FsaXphdGlvbjogbWVzc2FnZS5tZXNzYWdlTG9jYWxpemF0aW9uUGFyYW1zIH19XHJcbiAgICAgICAgPC9kaXY+XHJcblxyXG4gICAgICAgIDxkaXYgY2xhc3M9XCJhYnAtY29uZmlybS1mb290ZXIganVzdGlmeS1jb250ZW50LWNlbnRlclwiPlxyXG4gICAgICAgICAgPGJ1dHRvblxyXG4gICAgICAgICAgICAqbmdJZj1cIiFtZXNzYWdlLmhpZGVDYW5jZWxCdG5cIlxyXG4gICAgICAgICAgICBpZD1cImNhbmNlbFwiXHJcbiAgICAgICAgICAgIHR5cGU9XCJidXR0b25cIlxyXG4gICAgICAgICAgICBjbGFzcz1cImJ0biBidG4tc20gYnRuLXByaW1hcnlcIlxyXG4gICAgICAgICAgICAoY2xpY2spPVwiY2xvc2UocmVqZWN0KVwiXHJcbiAgICAgICAgICA+XHJcbiAgICAgICAgICAgIHt7IG1lc3NhZ2UuY2FuY2VsVGV4dCB8fCBtZXNzYWdlLmNhbmNlbENvcHkgfHwgJ0FicElkZW50aXR5OjpDYW5jZWwnIHwgYWJwTG9jYWxpemF0aW9uIH19XHJcbiAgICAgICAgICA8L2J1dHRvbj5cclxuICAgICAgICAgIDxidXR0b25cclxuICAgICAgICAgICAgKm5nSWY9XCIhbWVzc2FnZS5oaWRlWWVzQnRuXCJcclxuICAgICAgICAgICAgaWQ9XCJjb25maXJtXCJcclxuICAgICAgICAgICAgdHlwZT1cImJ1dHRvblwiXHJcbiAgICAgICAgICAgIGNsYXNzPVwiYnRuIGJ0bi1zbSBidG4tcHJpbWFyeVwiXHJcbiAgICAgICAgICAgIChjbGljayk9XCJjbG9zZShjb25maXJtKVwiXHJcbiAgICAgICAgICAgIGF1dG9mb2N1c1xyXG4gICAgICAgICAgPlxyXG4gICAgICAgICAgICA8c3Bhbj57eyBtZXNzYWdlLnllc1RleHQgfHwgbWVzc2FnZS55ZXNDb3B5IHx8ICdBYnBJZGVudGl0eTo6WWVzJyB8IGFicExvY2FsaXphdGlvbiB9fTwvc3Bhbj5cclxuICAgICAgICAgIDwvYnV0dG9uPlxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgICA8L25nLXRlbXBsYXRlPlxyXG4gICAgPC9wLXRvYXN0PlxyXG4gIGAsXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBDb25maXJtYXRpb25Db21wb25lbnQge1xyXG4gIGNvbmZpcm0gPSBUb2FzdGVyLlN0YXR1cy5jb25maXJtO1xyXG4gIHJlamVjdCA9IFRvYXN0ZXIuU3RhdHVzLnJlamVjdDtcclxuICBkaXNtaXNzID0gVG9hc3Rlci5TdGF0dXMuZGlzbWlzcztcclxuXHJcbiAgY29uc3RydWN0b3IocHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlKSB7fVxyXG5cclxuICBjbG9zZShzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSB7XHJcbiAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2UuY2xlYXIoc3RhdHVzKTtcclxuICB9XHJcbn1cclxuIl19