import { Component, Input, OnInit } from '@angular/core';
import { Toaster } from '../../models/toaster';
import { ToasterService } from '../../services/toaster.service';
import { LocalizationService } from '@abp/ng.core';
import snq from 'snq';

@Component({
  selector: 'abp-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss'],
})
export class ToastComponent implements OnInit {
  @Input()
  toast: Toaster.Toast;

  get severityClass(): string {
    if (!this.toast || !this.toast.severity) return '';
    return `toast-${this.toast.severity}`;
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

  constructor(
    private toastService: ToasterService,
    private localizationService: LocalizationService,
  ) {}

  ngOnInit() {
    if (snq(() => this.toast.options.sticky)) return;
    const timeout = snq(() => this.toast.options.life) || 5000;
    setTimeout(() => {
      this.close();
    }, timeout);
  }

  close() {
    this.toastService.remove(this.toast.options.id);
  }

  tap() {
    if (this.toast.options && this.toast.options.tapToDismiss) this.close();
  }
}
