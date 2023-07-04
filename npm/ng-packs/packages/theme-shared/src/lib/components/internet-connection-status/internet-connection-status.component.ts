import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common'
import { of } from 'rxjs';
import { InternetConnectionService, LocalizationModule } from '@abp/ng.core';

@Component({
  selector: 'abp-internet-status',
  standalone: true,
  imports:[CommonModule,LocalizationModule],
  template: `
    <div class="status-icon" *ngIf="!(internetConnection$ | async)">
      <i data-toggle="tooltip" title="{{ 'AbpUi::InternetConnectionInfo' | abpLocalization }}" data-placement="left" class="fa fa-circle text-blinking blink">
      </i>
    </div>
  `,
  styles: [`
    .blink {
      animation: blinker 0.9s cubic-bezier(.5, 0, 1, 1) infinite alternate;  
    }
    @keyframes blinker {  
      0% { color:#c1c1c1 }
      70% { color: #DC3545 }
      100% { color: #DC3545 }
    }

    .text-blinking{
      font-size:1.2rem;
    }

    .status-icon{
      position: fixed;
      z-index: 999999;
      top: 10px;
      right: 10px;
    }

    @media (width < 768px){
      .status-icon{
        top: 80px;
      }
    }
  `]
})
export class InternetConnectionStatusComponent implements OnInit{
  internetConnection$ = of(true);
  internetConnectionService = inject(InternetConnectionService)

  ngOnInit(): void {
    this.internetConnection$ = this.internetConnectionService.getStatus$()
  }
  
}
