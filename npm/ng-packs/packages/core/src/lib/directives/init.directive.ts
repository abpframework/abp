import { Directive, Output, EventEmitter, ElementRef, AfterViewInit } from '@angular/core';

@Directive({
  standalone: true,
  selector: '[abpInit]',
})
export class InitDirective implements AfterViewInit {
  @Output('abpInit') readonly init = new EventEmitter<ElementRef<any>>();

  constructor(private elRef: ElementRef) {}

  ngAfterViewInit() {
    this.init.emit(this.elRef);
  }
}
