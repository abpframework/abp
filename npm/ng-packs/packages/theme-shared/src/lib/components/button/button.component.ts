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
  buttonClass = 'btn btn-primary';

  @Input()
  buttonType; // TODO: Add initial value.

  @Input()
  iconClass: string;

  @Input()
  loading = false;

  @Input()
  disabled = false;

  /**
   * @deprecated Use buttonType instead. To be deleted in v1
   */
  @Input() type = 'button';

  get icon(): string {
    return `${this.loading ? 'fa fa-pulse fa-spinner' : this.iconClass || 'd-none'}`;
  }
}
