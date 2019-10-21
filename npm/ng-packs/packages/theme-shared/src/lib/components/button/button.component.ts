import { Component, EventEmitter, Input, Output, ViewChild, ElementRef, Renderer2, OnInit } from '@angular/core';
import { ABP } from '@abp/ng.core';

@Component({
  selector: 'abp-button',
  // tslint:disable-next-line: component-max-inline-declarations
  template: `
    <button
      #button
      [attr.type]="buttonType || type"
      [ngClass]="buttonClass"
      [disabled]="loading || disabled"
      (click)="click.emit($event)"
      (focus)="focus.emit($event)"
      (blur)="blur.emit($event)"
    >
      <i [ngClass]="icon" class="mr-1"></i><ng-content></ng-content>
    </button>
  `,
})
export class ButtonComponent implements OnInit {
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

  /**
   * @deprecated Use buttonType instead. To be deleted in v1
   */
  @Input() type = 'button';

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
