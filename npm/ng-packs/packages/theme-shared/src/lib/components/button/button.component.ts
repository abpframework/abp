/* eslint-disable @angular-eslint/no-output-native */
import {
  Component,
  ElementRef,
  Input,
  OnInit,
  Renderer2,
  inject,
  output,
  viewChild
} from '@angular/core';
import { ABP, StopPropagationDirective } from '@abp/ng.core';

@Component({
  selector: 'abp-button',
  template: `
    <button
      #button
      [id]="buttonId"
      [attr.type]="buttonType"
      [attr.form]="formName"
      [class]="buttonClass"
      [disabled]="loading || disabled"
      (click.stop)="click.emit($event); abpClick.emit($event)"
      (focus)="focus.emit($event); abpFocus.emit($event)"
      (blur)="blur.emit($event); abpBlur.emit($event)"
    >
      <i [class]="icon" class="me-1" aria-hidden="true"></i><ng-content></ng-content>
    </button>
  `,
  imports: [StopPropagationDirective],
})
export class ButtonComponent implements OnInit {
  private renderer = inject(Renderer2);

  @Input()
  buttonId = '';

  @Input()
  buttonClass = 'btn btn-primary';

  @Input()
  buttonType = 'button';

  @Input()
  formName?: string = undefined;

  @Input()
  iconClass?: string;

  @Input()
  loading = false;

  @Input()
  disabled: boolean | undefined = false;

  @Input()
  attributes?: ABP.Dictionary<string>;

  readonly click = output<MouseEvent>();

  readonly focus = output<FocusEvent>();

  readonly blur = output<FocusEvent>();

  readonly abpClick = output<MouseEvent>();

  readonly abpFocus = output<FocusEvent>();

  readonly abpBlur = output<FocusEvent>();

  readonly buttonRef = viewChild.required<ElementRef<HTMLButtonElement>>('button');

  get icon(): string {
    return `${this.loading ? 'fa fa-spinner fa-spin' : this.iconClass || 'd-none'}`;
  }

  ngOnInit() {
    if (this.attributes) {
      Object.keys(this.attributes).forEach(key => {
        if (this.attributes?.[key]) {
          this.renderer.setAttribute(this.buttonRef().nativeElement, key, this.attributes[key]);
        }
      });
    }
  }
}
