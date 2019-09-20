import { Component, Input } from '@angular/core';

@Component({
  selector: 'abp-button',
  template: `
    <button [attr.type]="type" [ngClass]="buttonClass" [disabled]="loading || disabled">
      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>
    </button>
  `,
})
export class ButtonComponent {
  @Input()
  buttonClass: string = 'btn btn-primary';

  @Input()
  type: string = 'button';

  @Input()
  iconClass: string;

  @Input()
  loading: boolean = false;

  @Input()
  disabled: boolean = false;

  get icon(): string {
    return `${this.loading ? 'fa fa-pulse fa-spinner' : this.iconClass || 'd-none'}`;
  }
}
