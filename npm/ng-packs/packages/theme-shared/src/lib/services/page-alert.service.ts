import { Injectable } from '@angular/core';
import { InternalStore } from '@abp/ng.core';

export interface PageAlert {
  type: 'primary' | 'secondary' | 'success' | 'danger' | 'warning' | 'info' | 'light' | 'dark';
  message: string;
  dismissible?: boolean;
  title?: string;
  messageLocalizationParams?: string[];
  titleLocalizationParams?: string[];
}

@Injectable({ providedIn: 'root' })
export class PageAlertService {
  private alerts = new InternalStore<PageAlert[]>([]);

  alerts$ = this.alerts.sliceState(state => state);

  constructor() {}

  show(alert: PageAlert) {
    const newAlert: PageAlert = {
      ...alert,
      dismissible: alert.dismissible ?? true,
    };
    this.alerts.set([newAlert, ...this.alerts.state]);
  }

  remove(index: number) {
    const alerts = [...this.alerts.state];
    alerts.splice(index, 1);
    this.alerts.set(alerts);
  }
}
