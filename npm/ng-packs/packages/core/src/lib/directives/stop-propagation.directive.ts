import { Directive, ElementRef, EventEmitter, OnInit, Output } from '@angular/core';
import { fromEvent } from 'rxjs';
import { SubscriptionService } from '../services/subscription.service';

@Directive({
  standalone: true,
  selector: '[click.stop]',
  providers: [SubscriptionService],
})
export class StopPropagationDirective implements OnInit {
  @Output('click.stop') readonly stopPropEvent = new EventEmitter<MouseEvent>();

  constructor(private el: ElementRef, private subscription: SubscriptionService) {}

  ngOnInit(): void {
    this.subscription.addOne(fromEvent<MouseEvent>(this.el.nativeElement, 'click'), event => {
      event.stopPropagation();
      this.stopPropEvent.emit(event);
    });
  }
}
