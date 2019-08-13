import { Directive, ElementRef, EventEmitter, OnInit, Output, Renderer2 } from '@angular/core';
import { fromEvent } from 'rxjs';
import { takeUntilDestroy } from '@ngx-validate/core';

@Directive({
  selector: '[click.stop]',
})
export class ClickEventStopPropagationDirective implements OnInit {
  @Output('click.stop') stopPropEvent = new EventEmitter<MouseEvent>();

  constructor(private renderer: Renderer2, private el: ElementRef) {}

  ngOnInit(): void {
    fromEvent(this.el.nativeElement, 'click')
      .pipe(takeUntilDestroy(this))
      .subscribe((event: MouseEvent) => {
        event.stopPropagation();
        this.stopPropEvent.emit(event);
      });
  }
}
