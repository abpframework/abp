import {Component, OnInit, input, output, ChangeDetectionStrategy,} from '@angular/core';
import { Toaster } from '../../models/toaster';
import { LocalizationPipe } from '@abp/ng.core';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss'],
  host: {
    '[class.abp-toast-leaving]': 'isLeaving',
  },
  imports: [LocalizationPipe],
})
export class ToastComponent implements OnInit {
  readonly toast = input.required<Toaster.Toast>();

  readonly remove = output<number>();

  get severityClass(): string {
    const toast = this.toast();
    if (!toast || !toast.severity) return '';
    return `abp-toast-${toast.severity}`;
  }

  get iconClass(): string {
    const { iconClass } = this.toast().options || {};

    if (iconClass) {
      return iconClass;
    }

    switch (this.toast().severity) {
      case 'success':
        return 'bi-check';
      case 'info':
        return 'bi-info-circle';
      case 'warning':
        return 'bi-exclamation-triangle';
      case 'error':
        return 'bi-shield-exclamation';
      default:
        return 'bi-exclamation-triangle';
    }
  }

  ngOnInit() {
    const { sticky, life } = this.toast().options || {};

    if (sticky) return;
    const timeout = life || 5000;
    setTimeout(() => {
      this.close();
    }, timeout);
  }

  isLeaving = false;

  close() {
    if (this.isLeaving) {
      return;
    }

    this.isLeaving = true;
    setTimeout(() => {
      this.remove.emit(this.toast().options?.id);
    }, 450);
  }

  tap() {
    if (this.toast().options?.tapToDismiss) this.close();
  }
}
