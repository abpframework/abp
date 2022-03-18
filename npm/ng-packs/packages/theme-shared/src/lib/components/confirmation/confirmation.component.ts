import { Component, Inject } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { Confirmation } from '../../models/confirmation';
import { CONFIRMATION_ICONS, ConfirmationIcons } from '../../tokens/confirmation-icons.token';

@Component({
  selector: 'abp-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss'],
})
export class ConfirmationComponent {
  constructor(@Inject(CONFIRMATION_ICONS) private icons: ConfirmationIcons) {}

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
    if (!this.icons) {
      return '';
    }
    return this.icons[severity] || this.icons.default;
  }

  isCustomIconExists({ options }: Confirmation.DialogData): boolean {
    return !!(options && (options.iconTemplate || options.icon));
  }

  isIconTemplateExits({ options }: Confirmation.DialogData): boolean {
    return !!(options && options.iconTemplate);
  }
}
