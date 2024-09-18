import { Component } from '@angular/core';

@Component({
  selector: 'abp-spinner',
  standalone: true,
  template: `
    <div class="d-flex justify-content-center align-items-center border-top" style="height: 62px">
      <div class="spinner-border" role="status" id="loading">
        <span class="visually-hidden">Loading...</span>
      </div>
    </div>
  `,
})
export class SpinnerComponent {}
