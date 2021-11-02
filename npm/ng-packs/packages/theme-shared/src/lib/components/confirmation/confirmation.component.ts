import { Component } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { Confirmation } from '../../models/confirmation';

@Component({
  selector: 'abp-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss'],
})
export class ConfirmationComponent {
  confirm = Confirmation.Status.confirm;
  reject = Confirmation.Status.reject;
  dismiss = Confirmation.Status.dismiss;

  confirmation$!: ReplaySubject<Confirmation.DialogData>;

  clear!: (status: Confirmation.Status) => void;

  close(status: Confirmation.Status) {
    this.clear(status);
  }

  getIconClass({ severity }: Confirmation.DialogData): string {
    switch (severity) {
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
}
