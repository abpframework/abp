import { AfterViewInit, Directive, ElementRef, Input, inject } from '@angular/core';

@Directive({
  selector: '[autofocus]',
})
export class AutofocusDirective implements AfterViewInit {
  private elRef = inject(ElementRef);

  private _delay = 0;

  @Input('autofocus')
  set delay(val: number | string | undefined) {
    this._delay = Number(val) || 0;
  }

  get delay() {
    return this._delay;
  }

  ngAfterViewInit(): void {
    setTimeout(() => this.elRef.nativeElement.focus(), this.delay as number);
  }
}
