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
          class="fa fa-circle text-blinking blink"
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
          color: #dc3545;
        }
        100% {
          color: #dc3545;
        }
      }

      .text-blinking {
        font-size: 1.2rem;
      }

      .status-icon {
        position: fixed;
        z-index: 999999;
        top: 10px;
        right: 10px;
      }

      @media (width < 768px) {
        .status-icon {
          top: 26px;
          right: 134px;
        }
      }
    `,
  ],
})
export class InternetConnectionStatusComponent {
  internetConnectionService = inject(InternetConnectionService);
  isOnline = this.internetConnectionService.networkStatus;
}
