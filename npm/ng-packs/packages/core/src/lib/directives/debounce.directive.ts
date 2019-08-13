import { Directive, Output, Renderer2, ElementRef, OnInit, EventEmitter, Input } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { takeUntilDestroy } from '@ngx-validate/core';

@Directive({
  selector: '[input.debounce]',
})
export class InputEventDebounceDirective implements OnInit {
  @Input() debounce: number = 300;

  @Output('input.debounce') debounceEvent = new EventEmitter<Event>();

  constructor(private renderer: Renderer2, private el: ElementRef) {}

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
}
