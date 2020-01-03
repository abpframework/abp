import { Directive, ElementRef, EventEmitter, Input, OnDestroy, OnInit, Output } from '@angular/core';
import { takeUntilDestroy } from '@ngx-validate/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Directive({
  // tslint:disable-next-line: directive-selector
  selector: '[input.debounce]',
})
export class InputEventDebounceDirective implements OnInit, OnDestroy {
  @Input() debounce = 300;

  @Output('input.debounce') readonly debounceEvent = new EventEmitter<Event>();

  constructor(private el: ElementRef) {}

  ngOnInit(): void {
    fromEvent(this.el.nativeElement, 'input')
      .pipe(
        debounceTime(this.debounce),
        takeUntilDestroy(this),
      )
      .subscribe((event: Event) => {
        this.debounceEvent.emit(event);
      });
  }

  ngOnDestroy(): void {}
}
