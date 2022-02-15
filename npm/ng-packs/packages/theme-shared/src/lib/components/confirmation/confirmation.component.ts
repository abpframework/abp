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

  getIconClass({ severity, options }: Confirmation.DialogData): string {
    if (options && options.icon) {
      return options.icon;
    }
    switch (severity) {
      case 'info':
        return 'fa fa-info-circle';
      case 'success':
        return 'fa fa-check-circle';
      case 'warning':
        return 'fa fa-exclamation-triangle';
      case 'error':
        return 'fa fa-times-circle';
      default:
        return 'fa fa-question-circle';
    }
  }

  isCustomIconExists({ options }: Confirmation.DialogData): boolean {
    return !!(options && (options.iconTemplate || options.icon));
  }

  isIconTemplateExits({ options }: Confirmation.DialogData): boolean {
    return !!(options && options.iconTemplate);
  }
}
