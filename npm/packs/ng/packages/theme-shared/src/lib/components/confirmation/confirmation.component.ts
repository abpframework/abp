import { Component } from '@angular/core';
import { ConfirmationService } from '../../services/confirmation.service';
import { Toaster } from '../../models/toaster';

@Component({
  selector: 'abp-confirmation',
  template: `
    <p-toast
      position="center"
      key="abpConfirmation"
      (onClose)="close(dismiss)"
      [modal]="true"
      [baseZIndex]="1000"
      styleClass=""
    >
      <ng-template let-message pTemplate="message">
        <div *ngIf="message.summary" class="modal-header">
          <h4 class="modal-title">
            {{ message.summary | abpLocalization: message.titleLocalizationParams }}
          </h4>
        </div>
        <div class="modal-body">
          {{ message.detail | abpLocalization: message.messageLocalizationParams }}
        </div>

        <div class="modal-footer justify-content-center">
          <button *ngIf="!message.hideCancelBtn" type="button" class="btn btn-secondary" (click)="close(reject)">
            {{ message.cancelCopy || 'AbpIdentity::Cancel' | abpLocalization }}
          </button>
          <button *ngIf="!message.hideYesBtn" type="button" class="btn btn-secondary" (click)="close(confirm)">
            <span>{{ message.yesCopy || 'AbpIdentity::Yes' | abpLocalization }}</span>
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
