import { AfterViewInit, Directive, ElementRef, inject, input } from '@angular/core';

@Directive({
  selector: '[autofocus]',
})
export class AutofocusDirective implements AfterViewInit {
  private elRef = inject(ElementRef);

  readonly delay = input(0, {
    alias: 'autofocus',
    transform: (v: unknown) => Number(v) || 0,
  });

  ngAfterViewInit(): void {
    setTimeout(() => this.elRef.nativeElement.focus(), this.delay());
  }
}
