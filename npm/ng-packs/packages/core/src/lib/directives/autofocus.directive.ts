import { Directive, ElementRef, Input, AfterViewInit } from '@angular/core';

@Directive({
  selector: '[autofocus]',
})
export class AutofocusDirective implements AfterViewInit {
  @Input('autofocus')
  delay: number = 0;

  constructor(private elRef: ElementRef) {}

  ngAfterViewInit(): void {
    setTimeout(() => this.elRef.nativeElement.focus(), this.delay);
  }
}
