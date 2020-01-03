import { Component } from '@angular/core';
import { ConfirmationService } from '../../services/confirmation.service';
import { Toaster } from '../../models/toaster';

@Component({
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
  `,
})
export class ConfirmationComponent {
  confirm = Toaster.Status.confirm;
  reject = Toaster.Status.reject;
  dismiss = Toaster.Status.dismiss;

  constructor(private confirmationService: ConfirmationService) {}

  close(status: Toaster.Status) {
    this.confirmationService.clear(status);
  }
}
