import { Component } from '@angular/core';
import { ConfirmationService } from '../../services/confirmation.service';
import { Confirmation } from '../../models/confirmation';
import { LocalizationService } from '@abp/ng.core';

@Component({
  selector: 'abp-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss'],
})
export class ConfirmationComponent {
  confirm = Confirmation.Status.confirm;
  reject = Confirmation.Status.reject;
  dismiss = Confirmation.Status.dismiss;

  visible = false;

  data: Confirmation.DialogData;

  get iconClass(): string {
    switch (this.data.severity) {
      case 'info':
        return 'fa-info-circle';
      case 'success':
        return 'fa-check-circle';
      case 'warning':
        return 'fa-exclamation-triangle';
      case 'error':
        return 'fa-times-circle';
      default:
        return 'fa-question-circle';
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

  close(status: Confirmation.Status) {
    this.confirmationService.clear(status);
  }
}
