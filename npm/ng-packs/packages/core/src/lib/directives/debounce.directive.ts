import { Directive, ElementRef, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';
import { SubscriptionService } from '../services/subscription.service';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[input.debounce]',
  providers: [SubscriptionService],
})
export class InputEventDebounceDirective implements OnInit {
  @Input() debounce = 300;

  @Output('input.debounce') readonly debounceEvent = new EventEmitter<Event>();

  constructor(private el: ElementRef, private subscription: SubscriptionService) {}

  ngOnInit(): void {
    const input$ = fromEvent(this.el.nativeElement, 'input').pipe(debounceTime(this.debounce));

    this.subscription.addOne(input$, (event: Event) => {
      this.debounceEvent.emit(event);
    });
  }
}
