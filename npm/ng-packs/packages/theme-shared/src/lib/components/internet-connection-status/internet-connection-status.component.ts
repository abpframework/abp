import { Component } from '@angular/core';
import { InternetConnectionService } from '../../../../../core/src/lib/services/internet-connection-service';



@Component({
  selector: 'abp-internet-status',
  styleUrls: ['./internet-connection-status.scss'],
  template: `
    <div class="statusIcon" *ngIf="!internetConnection">
      <i data-toggle="tooltip" title="The operation could not be performed. Your internet connection is not available at the moment." data-placement="top"  class="fa fa-circle text-blinking Blink"></i>
    </div>
  `,
})
export class InternetConnectionStatusComponent{
  internetConnection: boolean;
  constructor(private internetConnectionService:InternetConnectionService){
    this.internetConnectionService.getStatus$().subscribe((val)=>{
      this.internetConnection = val
    })
  }
}
