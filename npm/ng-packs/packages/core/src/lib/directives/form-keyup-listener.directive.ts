import { Directive, ElementRef, EventEmitter, Output, OnDestroy } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime, filter, tap } from 'rxjs/operators';
import { takeUntilDestroy } from '../utils/rxjs-utils';

@Directive({
  selector: 'form[ngSubmit]',
})
export class FormKeyupListenerDirective implements OnDestroy {
  @Output() ngSubmit = new EventEmitter();

  constructor(private elRef: ElementRef) {
    fromEvent(elRef.nativeElement as HTMLElement, 'keyup')
      .pipe(
        debounceTime(200),
        filter((key: KeyboardEvent) => key && key.key === 'Enter'),
        takeUntilDestroy(this),
      )
      .subscribe(() => this.ngSubmit.emit());
  }

  ngOnDestroy(): void {}
}
