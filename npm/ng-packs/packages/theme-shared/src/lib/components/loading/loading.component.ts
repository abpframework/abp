import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'abp-loading',
  template: `
    <div class="abp-loading">
      <i class="fa fa-spinner fa-pulse abp-spinner"></i>
    </div>
  `,
  encapsulation: ViewEncapsulation.None,
  styles: [
    `
      .abp-loading {
        position: absolute;
        width: 100%;
        height: 100%;
        top: 0;
        left: 0;
        z-index: 1040;
      }

      .abp-loading .abp-spinner {
        position: absolute;
        top: 50%;
        left: 50%;
        font-size: 14px;
        -moz-transform: translateX(-50%) translateY(-50%);
        -o-transform: translateX(-50%) translateY(-50%);
        -ms-transform: translateX(-50%) translateY(-50%);
        -webkit-transform: translateX(-50%) translateY(-50%);
        transform: translateX(-50%) translateY(-50%);
      }
    `,
  ],
})
export class LoadingComponent {}
