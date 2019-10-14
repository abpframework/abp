import { Directive, ElementRef, Input, AfterViewInit } from '@angular/core';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[autofocus]'
})
export class AutofocusDirective implements AfterViewInit {
  @Input('autofocus')
  delay = 0;

  constructor(private elRef: ElementRef) {}

  ngAfterViewInit(): void {
    setTimeout(() => this.elRef.nativeElement.focus(), this.delay);
  }
}
