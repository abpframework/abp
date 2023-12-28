import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
  standalone: true,
  selector: '[autofocus]',
})
export class AutofocusDirective implements AfterViewInit {
  private _delay = 0;

  @Input('autofocus')
  set delay(val: number | string | undefined) {
    this._delay = Number(val) || 0;
  }

  get delay() {
    return this._delay;
  }

  constructor(private elRef: ElementRef) {}

  ngAfterViewInit(): void {
    setTimeout(() => this.elRef.nativeElement.focus(), this.delay as number);
  }
}
