import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Toaster } from '../../models/toaster';
@Component({
  selector: 'abp-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss'],
})
export class ToastComponent implements OnInit {
  @Input()
  toast!: Toaster.Toast;

  @Output() remove = new EventEmitter<number>();

  get severityClass(): string {
    if (!this.toast || !this.toast.severity) return '';
    return `abp-toast-${this.toast.severity}`;
  }

  get iconClass(): string {
    switch (this.toast.severity) {
      case 'success':
        return 'fa-check-circle';
      case 'info':
        return 'fa-info-circle';
      case 'warning':
        return 'fa-exclamation-triangle';
      case 'error':
        return 'fa-times-circle';
      default:
        return 'fa-exclamation-circle';
    }
  }

  ngOnInit() {
    const { sticky, life } = this.toast.options || {};

    if (sticky) return;
    const timeout = life || 5000;
    setTimeout(() => {
      this.close();
    }, timeout);
  }

  close() {
    this.remove.emit(this.toast.options?.id);
  }

  tap() {
    if (this.toast.options?.tapToDismiss) this.close();
  }
}
