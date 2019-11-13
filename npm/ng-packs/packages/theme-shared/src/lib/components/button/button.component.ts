import { Component, EventEmitter, Input, Output, ViewChild, ElementRef, Renderer2, OnInit } from '@angular/core';
import { ABP } from '@abp/ng.core';

@Component({
  selector: 'abp-button',
  // tslint:disable-next-line: component-max-inline-declarations
  template: `
    <button
      #button
      [id]="buttonId"
      [attr.type]="buttonType"
      [ngClass]="buttonClass"
      [disabled]="loading || disabled"
      (click)="onClick($event)"
      (focus)="onFocus($event)"
      (blur)="onBlur($event)"
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

  // tslint:disable-next-line: no-output-native
  @Output() readonly click = new EventEmitter<MouseEvent>();

  // tslint:disable-next-line: no-output-native
  @Output() readonly focus = new EventEmitter<FocusEvent>();

  // tslint:disable-next-line: no-output-native
  @Output() readonly blur = new EventEmitter<FocusEvent>();

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

  onClick(event: MouseEvent) {
    event.stopPropagation();
    this.click.next(event);
  }

  onFocus(event: FocusEvent) {
    event.stopPropagation();
    this.focus.next(event);
  }

  onBlur(event: FocusEvent) {
    event.stopPropagation();
    this.blur.next(event);
  }
}
