import { AfterViewInit, Directive, ElementRef, Input } from '@angular/core';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[autofocus]',
})
export class AutofocusDirective implements AfterViewInit {
  @Input('autofocus')
  delay = 0;

  constructor(private elRef: ElementRef) {}

  ngAfterViewInit(): void {
    setTimeout(() => this.elRef.nativeElement.focus(), this.delay);
  }
}
