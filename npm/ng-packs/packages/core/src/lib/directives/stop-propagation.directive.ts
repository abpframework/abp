import { Directive, ElementRef, EventEmitter, OnInit, Output, Renderer2 } from '@angular/core';
import { fromEvent } from 'rxjs';

@Directive({
  selector: '[click.stop]',
})
export class StopPropagationDirective implements OnInit {
  @Output('click.stop') stopPropEvent = new EventEmitter<MouseEvent>();

  constructor(private renderer: Renderer2, private el: ElementRef) {}

  ngOnInit(): void {
    fromEvent(this.el.nativeElement, 'click').subscribe((event: MouseEvent) => {
      event.stopPropagation();
      this.stopPropEvent.emit(event);
    });
  }
}
