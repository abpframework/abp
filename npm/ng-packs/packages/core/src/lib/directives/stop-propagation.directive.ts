import { Directive, ElementRef, EventEmitter, OnInit, Output, OnDestroy } from '@angular/core';
import { fromEvent } from 'rxjs';
import { takeUntilDestroy } from '../utils/rxjs-utils';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[click.stop]',
})
export class StopPropagationDirective implements OnInit, OnDestroy {
  @Output('click.stop') readonly stopPropEvent = new EventEmitter<MouseEvent>();

  constructor(private el: ElementRef) {}

  ngOnInit(): void {
    fromEvent(this.el.nativeElement, 'click')
      .pipe(takeUntilDestroy(this))
      .subscribe((event: MouseEvent) => {
        event.stopPropagation();
        this.stopPropEvent.emit(event);
      });
  }

  ngOnDestroy(): void {}
}
