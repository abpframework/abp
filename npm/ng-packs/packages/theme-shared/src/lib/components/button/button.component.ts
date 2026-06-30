/* eslint-disable @angular-eslint/no-output-native */
import {
  Component,
  ElementRef,
  OnInit,
  Renderer2,
  computed,
  inject,
  input,
  output,
  signal,
  viewChild,
  ChangeDetectionStrategy,
} from '@angular/core';
import { ABP, StopPropagationDirective } from '@abp/ng.core';

@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  selector: 'abp-button',
  template: `
    <button
      #button
      [id]="buttonId()"
      [attr.type]="buttonType()"
      [attr.form]="formName()"
      [class]="buttonClass()"
      [disabled]="isLoading() || disabled()"
      (click.stop)="click.emit($event); abpClick.emit($event)"
      (focus)="focus.emit($event); abpFocus.emit($event)"
      (blur)="blur.emit($event); abpBlur.emit($event)"
    >
      <i [class]="icon()" class="me-1" aria-hidden="true"></i><ng-content></ng-content>
    </button>
  `,
  imports: [StopPropagationDirective],
})
export class ButtonComponent implements OnInit {
  private renderer = inject(Renderer2);

  readonly buttonId = input('');
  readonly buttonClass = input('btn btn-primary');
  readonly buttonType = input('button');
  readonly formName = input<string | undefined>(undefined);
  readonly iconClass = input<string | undefined>(undefined);
  readonly loading = input(false);
  readonly disabled = input<boolean | undefined>(false);
  readonly attributes = input<ABP.Dictionary<string> | undefined>(undefined);

  private readonly modalLoading = signal<boolean | null>(null);

  readonly isLoading = computed(() => this.modalLoading() ?? this.loading());

  readonly click = output<MouseEvent>();
  readonly focus = output<FocusEvent>();
  readonly blur = output<FocusEvent>();
  readonly abpClick = output<MouseEvent>();
  readonly abpFocus = output<FocusEvent>();
  readonly abpBlur = output<FocusEvent>();

  readonly buttonRef = viewChild.required<ElementRef<HTMLButtonElement>>('button');

  protected readonly icon = computed(() =>
    this.isLoading() ? 'fa fa-spinner fa-spin' : this.iconClass() || 'd-none',
  );

  ngOnInit() {
    const attributes = this.attributes();
    if (attributes) {
      Object.keys(attributes).forEach(key => {
        if (attributes[key]) {
          this.renderer.setAttribute(this.buttonRef().nativeElement, key, attributes[key]);
        }
      });
    }
  }

  setLoading(value: boolean): void {
    this.modalLoading.set(value);
  }
}
