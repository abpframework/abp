import { Component } from '@angular/core';
import { ConfirmationService } from '../../services/confirmation.service';
import { Confirmation, Toaster } from '../../models';
import { LocalizationService } from '@abp/ng.core';

@Component({
  selector: 'abp-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss'],
})
export class ConfirmationComponent {
  confirm = Toaster.Status.confirm;
  reject = Toaster.Status.reject;
  dismiss = Toaster.Status.dismiss;

  visible = false;

  data: Confirmation.DialogData;

  get iconClass(): string {
    switch (this.data.severity) {
      case 'info':
        return 'info-circle';
      case 'success':
        return 'check-circle';
      case 'warning':
        return 'exclamation-triangle';
      case 'error':
        return 'times-circle';
      default:
        return 'question-circle-o';
    }
  }

  constructor(
    private confirmationService: ConfirmationService,
    private localizationService: LocalizationService,
  ) {
    this.confirmationService.confirmation$.subscribe(confirmation => {
      this.data = confirmation;
      this.visible = !!confirmation;
    });
  }

  close(status: Toaster.Status) {
    this.confirmationService.clear(status);
  }
}
