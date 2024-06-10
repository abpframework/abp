import { Component, inject } from '@angular/core';
import { InternetConnectionService, LocalizationModule } from '@abp/ng.core';
import { NgbTooltip } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'abp-internet-status',
  standalone: true,
  imports: [LocalizationModule, NgbTooltip],
  template: `
    @if (!isOnline()) {
      <div class="status-icon">
        <i
          ngbTooltip="{{ 'AbpUi::InternetConnectionInfo' | abpLocalization }}"
          container="body"
          placement="left-top" 
          class="fa fa-wifi text-blinking blink"
        >
        </i>
      </div>
    }
  `,
  styles: [
    `
      .blink {
        animation: blinker 0.9s cubic-bezier(0.5, 0, 1, 1) infinite alternate;
      }
      @keyframes blinker {
        0% {
          color: #c1c1c1;
        }
        70% {
          color: #fa2379;
        }
        100% {
          color: #fa2379;
        }
      }

      .text-blinking {
        font-size: 30px;
      }

      .status-icon {
        position: fixed;
        z-index: 999999;
        top: 50%;
        left: 50%;
        width: 30px;
        text-align: center;
        margin-left: -15px;
        margin-top: -15px;
        translate: transform(-50%, -50%);
      }
    `,
  ],
})
export class InternetConnectionStatusComponent {
  internetConnectionService = inject(InternetConnectionService);
  isOnline = this.internetConnectionService.networkStatus;
}
