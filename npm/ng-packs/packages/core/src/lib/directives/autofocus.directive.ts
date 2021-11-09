import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[autofocus]',
})
export class AutofocusDirective implements AfterViewInit {
  private _delay = 0;

  @Input('autofocus')
  set delay(val: number | undefined | string) {
    this._delay = Number(val) || 0;
  }

  get delay(): number {
    return this._delay;
  }

  constructor(private elRef: ElementRef) {}

  ngAfterViewInit(): void {
    setTimeout(() => this.elRef.nativeElement.focus(), this.delay as number);
  }
}
