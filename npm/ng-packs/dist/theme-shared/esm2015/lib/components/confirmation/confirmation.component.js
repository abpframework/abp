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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7Ozs7QUFBQSxPQUFPLEVBQUUsU0FBUyxFQUFFLE1BQU0sZUFBZSxDQUFDO0FBQzFDLE9BQU8sRUFBRSxtQkFBbUIsRUFBRSxNQUFNLHFDQUFxQyxDQUFDO0FBaUQxRSxNQUFNLE9BQU8scUJBQXFCOzs7O0lBS2hDLFlBQW9CLG1CQUF3QztRQUF4Qyx3QkFBbUIsR0FBbkIsbUJBQW1CLENBQXFCO1FBSjVELFlBQU8sMkJBQTBCO1FBQ2pDLFdBQU0seUJBQXlCO1FBQy9CLFlBQU8sMkJBQTBCO0lBRThCLENBQUM7Ozs7O0lBRWhFLEtBQUssQ0FBQyxNQUFzQjtRQUMxQixJQUFJLENBQUMsbUJBQW1CLENBQUMsS0FBSyxDQUFDLE1BQU0sQ0FBQyxDQUFDO0lBQ3pDLENBQUM7OztZQXZERixTQUFTLFNBQUM7Z0JBQ1QsUUFBUSxFQUFFLGtCQUFrQjs7Z0JBRTVCLFFBQVEsRUFBRTs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7R0F5Q1Q7YUFDRjs7OztZQWhEUSxtQkFBbUI7Ozs7SUFrRDFCLHdDQUFpQzs7SUFDakMsdUNBQStCOztJQUMvQix3Q0FBaUM7Ozs7O0lBRXJCLG9EQUFnRCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2NvbmZpcm1hdGlvbi5zZXJ2aWNlJztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi8uLi9tb2RlbHMvdG9hc3Rlcic7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1jb25maXJtYXRpb24nLFxuICAvLyB0c2xpbnQ6ZGlzYWJsZS1uZXh0LWxpbmU6IGNvbXBvbmVudC1tYXgtaW5saW5lLWRlY2xhcmF0aW9uc1xuICB0ZW1wbGF0ZTogYFxuICAgIDxwLXRvYXN0XG4gICAgICBwb3NpdGlvbj1cImNlbnRlclwiXG4gICAgICBrZXk9XCJhYnBDb25maXJtYXRpb25cIlxuICAgICAgKG9uQ2xvc2UpPVwiY2xvc2UoZGlzbWlzcylcIlxuICAgICAgW21vZGFsXT1cInRydWVcIlxuICAgICAgW2Jhc2VaSW5kZXhdPVwiMTAwMFwiXG4gICAgICBzdHlsZUNsYXNzPVwiYWJwLWNvbmZpcm1cIlxuICAgID5cbiAgICAgIDxuZy10ZW1wbGF0ZSBsZXQtbWVzc2FnZSBwVGVtcGxhdGU9XCJtZXNzYWdlXCI+XG4gICAgICAgIDxpIGNsYXNzPVwiZmEgZmEtZXhjbGFtYXRpb24tY2lyY2xlIGFicC1jb25maXJtLWljb25cIj48L2k+XG4gICAgICAgIDxkaXYgKm5nSWY9XCJtZXNzYWdlLnN1bW1hcnlcIiBjbGFzcz1cImFicC1jb25maXJtLXN1bW1hcnlcIj5cbiAgICAgICAgICB7eyBtZXNzYWdlLnN1bW1hcnkgfCBhYnBMb2NhbGl6YXRpb246IG1lc3NhZ2UudGl0bGVMb2NhbGl6YXRpb25QYXJhbXMgfX1cbiAgICAgICAgPC9kaXY+XG4gICAgICAgIDxkaXYgY2xhc3M9XCJhYnAtY29uZmlybS1ib2R5XCI+XG4gICAgICAgICAge3sgbWVzc2FnZS5kZXRhaWwgfCBhYnBMb2NhbGl6YXRpb246IG1lc3NhZ2UubWVzc2FnZUxvY2FsaXphdGlvblBhcmFtcyB9fVxuICAgICAgICA8L2Rpdj5cblxuICAgICAgICA8ZGl2IGNsYXNzPVwiYWJwLWNvbmZpcm0tZm9vdGVyIGp1c3RpZnktY29udGVudC1jZW50ZXJcIj5cbiAgICAgICAgICA8YnV0dG9uXG4gICAgICAgICAgICAqbmdJZj1cIiFtZXNzYWdlLmhpZGVDYW5jZWxCdG5cIlxuICAgICAgICAgICAgaWQ9XCJjYW5jZWxcIlxuICAgICAgICAgICAgdHlwZT1cImJ1dHRvblwiXG4gICAgICAgICAgICBjbGFzcz1cImJ0biBidG4tc20gYnRuLXByaW1hcnlcIlxuICAgICAgICAgICAgKGNsaWNrKT1cImNsb3NlKHJlamVjdClcIlxuICAgICAgICAgID5cbiAgICAgICAgICAgIHt7IG1lc3NhZ2UuY2FuY2VsVGV4dCB8fCBtZXNzYWdlLmNhbmNlbENvcHkgfHwgJ0FicElkZW50aXR5OjpDYW5jZWwnIHwgYWJwTG9jYWxpemF0aW9uIH19XG4gICAgICAgICAgPC9idXR0b24+XG4gICAgICAgICAgPGJ1dHRvblxuICAgICAgICAgICAgKm5nSWY9XCIhbWVzc2FnZS5oaWRlWWVzQnRuXCJcbiAgICAgICAgICAgIGlkPVwiY29uZmlybVwiXG4gICAgICAgICAgICB0eXBlPVwiYnV0dG9uXCJcbiAgICAgICAgICAgIGNsYXNzPVwiYnRuIGJ0bi1zbSBidG4tcHJpbWFyeVwiXG4gICAgICAgICAgICAoY2xpY2spPVwiY2xvc2UoY29uZmlybSlcIlxuICAgICAgICAgICAgYXV0b2ZvY3VzXG4gICAgICAgICAgPlxuICAgICAgICAgICAgPHNwYW4+e3sgbWVzc2FnZS55ZXNUZXh0IHx8IG1lc3NhZ2UueWVzQ29weSB8fCAnQWJwSWRlbnRpdHk6OlllcycgfCBhYnBMb2NhbGl6YXRpb24gfX08L3NwYW4+XG4gICAgICAgICAgPC9idXR0b24+XG4gICAgICAgIDwvZGl2PlxuICAgICAgPC9uZy10ZW1wbGF0ZT5cbiAgICA8L3AtdG9hc3Q+XG4gIGAsXG59KVxuZXhwb3J0IGNsYXNzIENvbmZpcm1hdGlvbkNvbXBvbmVudCB7XG4gIGNvbmZpcm0gPSBUb2FzdGVyLlN0YXR1cy5jb25maXJtO1xuICByZWplY3QgPSBUb2FzdGVyLlN0YXR1cy5yZWplY3Q7XG4gIGRpc21pc3MgPSBUb2FzdGVyLlN0YXR1cy5kaXNtaXNzO1xuXG4gIGNvbnN0cnVjdG9yKHByaXZhdGUgY29uZmlybWF0aW9uU2VydmljZTogQ29uZmlybWF0aW9uU2VydmljZSkge31cblxuICBjbG9zZShzdGF0dXM6IFRvYXN0ZXIuU3RhdHVzKSB7XG4gICAgdGhpcy5jb25maXJtYXRpb25TZXJ2aWNlLmNsZWFyKHN0YXR1cyk7XG4gIH1cbn1cbiJdfQ==