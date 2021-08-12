import {
  Component,
  EventEmitter,
  Input,
  Output,
  ViewChild,
  ElementRef,
  Renderer2,
  OnInit,
} from '@angular/core';
import { ABP } from '@abp/ng.core';

@Component({
  selector: 'abp-button',
  template: `
    <button
      #button
      [id]="buttonId"
      [attr.type]="buttonType"
      [ngClass]="buttonClass"
      [disabled]="loading || disabled"
      (click.stop)="click.next($event); abpClick.next($event)"
      (focus)="focus.next($event); abpFocus.next($event)"
      (blur)="blur.next($event); abpBlur.next($event)"
    >
      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>
    </button>
  `,
})
export class ButtonComponent implements OnInit {
  @Input()
  buttonId = '';

  @Input()
  buttonClass = 'btn btn-primary';

  @Input()
  buttonType = 'button';

  @Input()
  iconClass: string;

  @Input()
  loading = false;

  @Input()
  disabled = false;

  @Input()
  attributes: ABP.Dictionary<string>;

  // tslint:disable
  @Output() readonly click = new EventEmitter<MouseEvent>();

  @Output() readonly focus = new EventEmitter<FocusEvent>();

  @Output() readonly blur = new EventEmitter<FocusEvent>();
  // tslint:enable

  @Output() readonly abpClick = new EventEmitter<MouseEvent>();

  @Output() readonly abpFocus = new EventEmitter<FocusEvent>();

  @Output() readonly abpBlur = new EventEmitter<FocusEvent>();

  @ViewChild('button', { static: true })
  buttonRef: ElementRef<HTMLButtonElement>;

  get icon(): string {
    return `${this.loading ? 'fa fa-spinner fa-spin' : this.iconClass || 'd-none'}`;
  }

  constructor(private renderer: Renderer2) {}

  ngOnInit() {
    if (this.attributes) {
      Object.keys(this.attributes).forEach(key => {
        this.renderer.setAttribute(this.buttonRef.nativeElement, key, this.attributes[key]);
      });
    }
  }
}
