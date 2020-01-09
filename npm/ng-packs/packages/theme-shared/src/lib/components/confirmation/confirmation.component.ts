import { Component } from '@angular/core';
import { ConfirmationService } from '../../services/confirmation.service';
import { Confirmation } from '../../models';
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

  get titleLocalizationParams(): string {
    return this.localizationService.instant(
      this.data.title,
      ...this.data.options.titleLocalizationParams,
    );
  }

  get messageLocalizationParams(): string {
    return this.localizationService.instant(
      this.data.message,
      ...this.data.options.messageLocalizationParams,
    );
  }

  constructor(
    private confirmationService: ConfirmationService,
    private localizationService: LocalizationService,
  ) {
    this.confirmationService.confirmation$.subscribe(confirmation => {
      this.data = confirmation;
      this.visible = true;
    });
  }

  close(status: Confirmation.Status) {
    this.confirmationService.clear(status);
    this.visible = false;
  }
}
