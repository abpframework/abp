import {
  Directive,
  ElementRef,
  EventEmitter,
  OnInit,
  Output,
} from '@angular/core';
import { fromEvent } from 'rxjs';
import { SubscriptionService } from '../services/subscription.service';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[click.stop]',
  providers: [SubscriptionService],
})
export class StopPropagationDirective implements OnInit {
  @Output('click.stop') readonly stopPropEvent = new EventEmitter<MouseEvent>();

  constructor(
    private el: ElementRef,
    private subscription: SubscriptionService
  ) {}

  ngOnInit(): void {
    this.subscription.addOne(
      fromEvent(this.el.nativeElement, 'click'),
      (event: MouseEvent) => {
        event.stopPropagation();
        this.stopPropEvent.emit(event);
      }
    );
  }
}
