/**
 * @fileoverview added by tsickle
 * @suppress {checkTypes,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
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
          <button *ngIf="!message.hideCancelBtn" type="button" class="btn btn-sm btn-primary" (click)="close(reject)">
            {{ message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}
          </button>
          <button
            *ngIf="!message.hideYesBtn"
            type="button"
            class="btn btn-sm btn-primary"
            (click)="close(confirm)"
            autofocus
          >
            <span>{{ message.yesCopy || 'AbpIdentity::Yes' | abpLocalization }}</span>
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDMUMsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUF5QzFFLE1BQU0sT0FBTyxxQkFBcUI7Ozs7SUFLaEMsWUFBb0IsbUJBQXdDO1FBQXhDLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7UUFKNUQsWUFBTywyQkFBMEI7UUFDakMsV0FBTSx5QkFBeUI7UUFDL0IsWUFBTywyQkFBMEI7SUFFOEIsQ0FBQzs7Ozs7SUFFaEUsS0FBSyxDQUFDLE1BQXNCO1FBQzFCLElBQUksQ0FBQyxtQkFBbUIsQ0FBQyxLQUFLLENBQUMsTUFBTSxDQUFDLENBQUM7SUFDekMsQ0FBQzs7O1lBL0NGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsa0JBQWtCO2dCQUM1QixRQUFRLEVBQUU7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7R0FrQ1Q7YUFDRjs7OztZQXhDUSxtQkFBbUI7Ozs7SUEwQzFCLHdDQUFpQzs7SUFDakMsdUNBQStCOztJQUMvQix3Q0FBaUM7Ozs7O0lBRXJCLG9EQUFnRCIsInNvdXJjZXNDb250ZW50IjpbImltcG9ydCB7IENvbXBvbmVudCB9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xuaW1wb3J0IHsgQ29uZmlybWF0aW9uU2VydmljZSB9IGZyb20gJy4uLy4uL3NlcnZpY2VzL2NvbmZpcm1hdGlvbi5zZXJ2aWNlJztcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi8uLi9tb2RlbHMvdG9hc3Rlcic7XG5cbkBDb21wb25lbnQoe1xuICBzZWxlY3RvcjogJ2FicC1jb25maXJtYXRpb24nLFxuICB0ZW1wbGF0ZTogYFxuICAgIDxwLXRvYXN0XG4gICAgICBwb3NpdGlvbj1cImNlbnRlclwiXG4gICAgICBrZXk9XCJhYnBDb25maXJtYXRpb25cIlxuICAgICAgKG9uQ2xvc2UpPVwiY2xvc2UoZGlzbWlzcylcIlxuICAgICAgW21vZGFsXT1cInRydWVcIlxuICAgICAgW2Jhc2VaSW5kZXhdPVwiMTAwMFwiXG4gICAgICBzdHlsZUNsYXNzPVwiYWJwLWNvbmZpcm1cIlxuICAgID5cbiAgICAgIDxuZy10ZW1wbGF0ZSBsZXQtbWVzc2FnZSBwVGVtcGxhdGU9XCJtZXNzYWdlXCI+XG4gICAgICAgIDxpIGNsYXNzPVwiZmEgZmEtZXhjbGFtYXRpb24tY2lyY2xlIGFicC1jb25maXJtLWljb25cIj48L2k+XG4gICAgICAgIDxkaXYgKm5nSWY9XCJtZXNzYWdlLnN1bW1hcnlcIiBjbGFzcz1cImFicC1jb25maXJtLXN1bW1hcnlcIj5cbiAgICAgICAgICB7eyBtZXNzYWdlLnN1bW1hcnkgfCBhYnBMb2NhbGl6YXRpb246IG1lc3NhZ2UudGl0bGVMb2NhbGl6YXRpb25QYXJhbXMgfX1cbiAgICAgICAgPC9kaXY+XG4gICAgICAgIDxkaXYgY2xhc3M9XCJhYnAtY29uZmlybS1ib2R5XCI+XG4gICAgICAgICAge3sgbWVzc2FnZS5kZXRhaWwgfCBhYnBMb2NhbGl6YXRpb246IG1lc3NhZ2UubWVzc2FnZUxvY2FsaXphdGlvblBhcmFtcyB9fVxuICAgICAgICA8L2Rpdj5cblxuICAgICAgICA8ZGl2IGNsYXNzPVwiYWJwLWNvbmZpcm0tZm9vdGVyIGp1c3RpZnktY29udGVudC1jZW50ZXJcIj5cbiAgICAgICAgICA8YnV0dG9uICpuZ0lmPVwiIW1lc3NhZ2UuaGlkZUNhbmNlbEJ0blwiIHR5cGU9XCJidXR0b25cIiBjbGFzcz1cImJ0biBidG4tc20gYnRuLXByaW1hcnlcIiAoY2xpY2spPVwiY2xvc2UocmVqZWN0KVwiPlxuICAgICAgICAgICAge3sgbWVzc2FnZS5jYW5jZWxDb3B5IHx8ICdBYnBJZGVudGl0eTo6Q2FuY2VsJyB8IGFicExvY2FsaXphdGlvbiB9fVxuICAgICAgICAgIDwvYnV0dG9uPlxuICAgICAgICAgIDxidXR0b25cbiAgICAgICAgICAgICpuZ0lmPVwiIW1lc3NhZ2UuaGlkZVllc0J0blwiXG4gICAgICAgICAgICB0eXBlPVwiYnV0dG9uXCJcbiAgICAgICAgICAgIGNsYXNzPVwiYnRuIGJ0bi1zbSBidG4tcHJpbWFyeVwiXG4gICAgICAgICAgICAoY2xpY2spPVwiY2xvc2UoY29uZmlybSlcIlxuICAgICAgICAgICAgYXV0b2ZvY3VzXG4gICAgICAgICAgPlxuICAgICAgICAgICAgPHNwYW4+e3sgbWVzc2FnZS55ZXNDb3B5IHx8ICdBYnBJZGVudGl0eTo6WWVzJyB8IGFicExvY2FsaXphdGlvbiB9fTwvc3Bhbj5cbiAgICAgICAgICA8L2J1dHRvbj5cbiAgICAgICAgPC9kaXY+XG4gICAgICA8L25nLXRlbXBsYXRlPlxuICAgIDwvcC10b2FzdD5cbiAgYCxcbn0pXG5leHBvcnQgY2xhc3MgQ29uZmlybWF0aW9uQ29tcG9uZW50IHtcbiAgY29uZmlybSA9IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm07XG4gIHJlamVjdCA9IFRvYXN0ZXIuU3RhdHVzLnJlamVjdDtcbiAgZGlzbWlzcyA9IFRvYXN0ZXIuU3RhdHVzLmRpc21pc3M7XG5cbiAgY29uc3RydWN0b3IocHJpdmF0ZSBjb25maXJtYXRpb25TZXJ2aWNlOiBDb25maXJtYXRpb25TZXJ2aWNlKSB7fVxuXG4gIGNsb3NlKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpIHtcbiAgICB0aGlzLmNvbmZpcm1hdGlvblNlcnZpY2UuY2xlYXIoc3RhdHVzKTtcbiAgfVxufVxuIl19