import { Component, OnInit } from '@angular/core';
import { ReplaySubject } from 'rxjs';
import { Confirmation } from '../../models/confirmation';

@Component({
  selector: 'abp-confirmation',
  templateUrl: './confirmation.component.html',
  styleUrls: ['./confirmation.component.scss'],
})
export class ConfirmationComponent implements OnInit {
  confirm = Confirmation.Status.confirm;
  reject = Confirmation.Status.reject;
  dismiss = Confirmation.Status.dismiss;

  visible = false;

  data: Confirmation.DialogData;

  confirmation$: ReplaySubject<Confirmation.DialogData>;

  clear: (status: Confirmation.Status) => void;

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

  ngOnInit() {
    this.confirmation$.subscribe(confirmation => {
      this.data = confirmation;
      this.visible = !!confirmation;
    });
  }

  close(status: Confirmation.Status) {
    this.clear(status);
  }
}
