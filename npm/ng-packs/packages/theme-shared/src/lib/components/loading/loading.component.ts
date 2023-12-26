import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'abp-loading',
  template: `
    <div class="abp-loading">
        <div class="spinner-border" role="status">
            <span class="visually-hidden">{{"AbpUi::LoadingWithThreeDot" | abpLocalization}}</span>
        </div>
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
        background: rgba(0,0,0,0.5);
        display: flex;
        justify-content: center;
        align-items: center;
      }
    `,
  ],
})
export class LoadingComponent {}
