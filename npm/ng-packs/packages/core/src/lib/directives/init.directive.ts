import { Directive, ElementRef, AfterViewInit, inject, output } from '@angular/core';

@Directive({
  selector: '[abpInit]',
})
export class InitDirective implements AfterViewInit {
  private elRef = inject(ElementRef);

  readonly init = output<ElementRef<any>>({ alias: 'abpInit' });

  ngAfterViewInit() {
    this.init.emit(this.elRef);
  }
}
