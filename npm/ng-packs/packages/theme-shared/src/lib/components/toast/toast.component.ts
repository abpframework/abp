import { Component } from '@angular/core';

@Component({
  selector: 'abp-toast',
  // tslint:disable-next-line: component-max-inline-declarations
  template: `
    <p-toast position="bottom-right" key="abpToast" styleClass="abp-toast" [baseZIndex]="1000">
      <ng-template let-message pTemplate="message">
        <span
          class="ui-toast-icon pi"
          [ngClass]="{
            'pi-info-circle': message.severity === 'info',
            'pi-exclamation-triangle': message.severity === 'warn',
            'pi-times': message.severity === 'error',
            'pi-check': message.severity === 'success'
          }"
        ></span>
        <div class="ui-toast-message-text-content">
          <div class="ui-toast-summary">{{ message.summary | abpLocalization: message.titleLocalizationParams }}</div>
          <div class="ui-toast-detail">{{ message.detail | abpLocalization: message.messageLocalizationParams }}</div>
        </div>
      </ng-template>
    </p-toast>
  `,
})
export class ToastComponent {}
