/**
 * @fileoverview added by tsickle
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
            {{ message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}
          </button>
          <button
            *ngIf="!message.hideYesBtn"
            id="confirm"
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
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiY29uZmlybWF0aW9uLmNvbXBvbmVudC5qcyIsInNvdXJjZVJvb3QiOiJuZzovL0BhYnAvbmcudGhlbWUuc2hhcmVkLyIsInNvdXJjZXMiOlsibGliL2NvbXBvbmVudHMvY29uZmlybWF0aW9uL2NvbmZpcm1hdGlvbi5jb21wb25lbnQudHMiXSwibmFtZXMiOltdLCJtYXBwaW5ncyI6Ijs7OztBQUFBLE9BQU8sRUFBRSxTQUFTLEVBQUUsTUFBTSxlQUFlLENBQUM7QUFDMUMsT0FBTyxFQUFFLG1CQUFtQixFQUFFLE1BQU0scUNBQXFDLENBQUM7QUFpRDFFLE1BQU0sT0FBTyxxQkFBcUI7Ozs7SUFLaEMsWUFBb0IsbUJBQXdDO1FBQXhDLHdCQUFtQixHQUFuQixtQkFBbUIsQ0FBcUI7UUFKNUQsWUFBTywyQkFBMEI7UUFDakMsV0FBTSx5QkFBeUI7UUFDL0IsWUFBTywyQkFBMEI7SUFFOEIsQ0FBQzs7Ozs7SUFFaEUsS0FBSyxDQUFDLE1BQXNCO1FBQzFCLElBQUksQ0FBQyxtQkFBbUIsQ0FBQyxLQUFLLENBQUMsTUFBTSxDQUFDLENBQUM7SUFDekMsQ0FBQzs7O1lBdkRGLFNBQVMsU0FBQztnQkFDVCxRQUFRLEVBQUUsa0JBQWtCOztnQkFFNUIsUUFBUSxFQUFFOzs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7Ozs7OztHQXlDVDthQUNGOzs7O1lBaERRLG1CQUFtQjs7OztJQWtEMUIsd0NBQWlDOztJQUNqQyx1Q0FBK0I7O0lBQy9CLHdDQUFpQzs7Ozs7SUFFckIsb0RBQWdEIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHsgQ29tcG9uZW50IH0gZnJvbSAnQGFuZ3VsYXIvY29yZSc7XHJcbmltcG9ydCB7IENvbmZpcm1hdGlvblNlcnZpY2UgfSBmcm9tICcuLi8uLi9zZXJ2aWNlcy9jb25maXJtYXRpb24uc2VydmljZSc7XHJcbmltcG9ydCB7IFRvYXN0ZXIgfSBmcm9tICcuLi8uLi9tb2RlbHMvdG9hc3Rlcic7XHJcblxyXG5AQ29tcG9uZW50KHtcclxuICBzZWxlY3RvcjogJ2FicC1jb25maXJtYXRpb24nLFxyXG4gIC8vIHRzbGludDpkaXNhYmxlLW5leHQtbGluZTogY29tcG9uZW50LW1heC1pbmxpbmUtZGVjbGFyYXRpb25zXHJcbiAgdGVtcGxhdGU6IGBcclxuICAgIDxwLXRvYXN0XHJcbiAgICAgIHBvc2l0aW9uPVwiY2VudGVyXCJcclxuICAgICAga2V5PVwiYWJwQ29uZmlybWF0aW9uXCJcclxuICAgICAgKG9uQ2xvc2UpPVwiY2xvc2UoZGlzbWlzcylcIlxyXG4gICAgICBbbW9kYWxdPVwidHJ1ZVwiXHJcbiAgICAgIFtiYXNlWkluZGV4XT1cIjEwMDBcIlxyXG4gICAgICBzdHlsZUNsYXNzPVwiYWJwLWNvbmZpcm1cIlxyXG4gICAgPlxyXG4gICAgICA8bmctdGVtcGxhdGUgbGV0LW1lc3NhZ2UgcFRlbXBsYXRlPVwibWVzc2FnZVwiPlxyXG4gICAgICAgIDxpIGNsYXNzPVwiZmEgZmEtZXhjbGFtYXRpb24tY2lyY2xlIGFicC1jb25maXJtLWljb25cIj48L2k+XHJcbiAgICAgICAgPGRpdiAqbmdJZj1cIm1lc3NhZ2Uuc3VtbWFyeVwiIGNsYXNzPVwiYWJwLWNvbmZpcm0tc3VtbWFyeVwiPlxyXG4gICAgICAgICAge3sgbWVzc2FnZS5zdW1tYXJ5IHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLnRpdGxlTG9jYWxpemF0aW9uUGFyYW1zIH19XHJcbiAgICAgICAgPC9kaXY+XHJcbiAgICAgICAgPGRpdiBjbGFzcz1cImFicC1jb25maXJtLWJvZHlcIj5cclxuICAgICAgICAgIHt7IG1lc3NhZ2UuZGV0YWlsIHwgYWJwTG9jYWxpemF0aW9uOiBtZXNzYWdlLm1lc3NhZ2VMb2NhbGl6YXRpb25QYXJhbXMgfX1cclxuICAgICAgICA8L2Rpdj5cclxuXHJcbiAgICAgICAgPGRpdiBjbGFzcz1cImFicC1jb25maXJtLWZvb3RlciBqdXN0aWZ5LWNvbnRlbnQtY2VudGVyXCI+XHJcbiAgICAgICAgICA8YnV0dG9uXHJcbiAgICAgICAgICAgICpuZ0lmPVwiIW1lc3NhZ2UuaGlkZUNhbmNlbEJ0blwiXHJcbiAgICAgICAgICAgIGlkPVwiY2FuY2VsXCJcclxuICAgICAgICAgICAgdHlwZT1cImJ1dHRvblwiXHJcbiAgICAgICAgICAgIGNsYXNzPVwiYnRuIGJ0bi1zbSBidG4tcHJpbWFyeVwiXHJcbiAgICAgICAgICAgIChjbGljayk9XCJjbG9zZShyZWplY3QpXCJcclxuICAgICAgICAgID5cclxuICAgICAgICAgICAge3sgbWVzc2FnZS5jYW5jZWxDb3B5IHx8ICdBYnBJZGVudGl0eTo6Q2FuY2VsJyB8IGFicExvY2FsaXphdGlvbiB9fVxyXG4gICAgICAgICAgPC9idXR0b24+XHJcbiAgICAgICAgICA8YnV0dG9uXHJcbiAgICAgICAgICAgICpuZ0lmPVwiIW1lc3NhZ2UuaGlkZVllc0J0blwiXHJcbiAgICAgICAgICAgIGlkPVwiY29uZmlybVwiXHJcbiAgICAgICAgICAgIHR5cGU9XCJidXR0b25cIlxyXG4gICAgICAgICAgICBjbGFzcz1cImJ0biBidG4tc20gYnRuLXByaW1hcnlcIlxyXG4gICAgICAgICAgICAoY2xpY2spPVwiY2xvc2UoY29uZmlybSlcIlxyXG4gICAgICAgICAgICBhdXRvZm9jdXNcclxuICAgICAgICAgID5cclxuICAgICAgICAgICAgPHNwYW4+e3sgbWVzc2FnZS55ZXNDb3B5IHx8ICdBYnBJZGVudGl0eTo6WWVzJyB8IGFicExvY2FsaXphdGlvbiB9fTwvc3Bhbj5cclxuICAgICAgICAgIDwvYnV0dG9uPlxyXG4gICAgICAgIDwvZGl2PlxyXG4gICAgICA8L25nLXRlbXBsYXRlPlxyXG4gICAgPC9wLXRvYXN0PlxyXG4gIGBcclxufSlcclxuZXhwb3J0IGNsYXNzIENvbmZpcm1hdGlvbkNvbXBvbmVudCB7XHJcbiAgY29uZmlybSA9IFRvYXN0ZXIuU3RhdHVzLmNvbmZpcm07XHJcbiAgcmVqZWN0ID0gVG9hc3Rlci5TdGF0dXMucmVqZWN0O1xyXG4gIGRpc21pc3MgPSBUb2FzdGVyLlN0YXR1cy5kaXNtaXNzO1xyXG5cclxuICBjb25zdHJ1Y3Rvcihwcml2YXRlIGNvbmZpcm1hdGlvblNlcnZpY2U6IENvbmZpcm1hdGlvblNlcnZpY2UpIHt9XHJcblxyXG4gIGNsb3NlKHN0YXR1czogVG9hc3Rlci5TdGF0dXMpIHtcclxuICAgIHRoaXMuY29uZmlybWF0aW9uU2VydmljZS5jbGVhcihzdGF0dXMpO1xyXG4gIH1cclxufVxyXG4iXX0=