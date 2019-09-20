import { Directive, Input, Optional, ElementRef, Renderer2, AfterViewInit } from '@angular/core';
import { Subject } from 'rxjs';
import snq from 'snq';

@Directive({
  selector: '[abpVisibility]',
})
export class VisibilityDirective implements AfterViewInit {
  @Input('abpVisibility')
  focusedElement: HTMLElement;

  @Input()
  mutationObserverEnabled: boolean = true;

  completed$ = new Subject<boolean>();

  constructor(@Optional() private elRef: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    let observer: MutationObserver;
    if (this.mutationObserverEnabled) {
      observer = new MutationObserver(mutations => {
        mutations.forEach(mutation => {
          if (!mutation.target) return;

          const htmlNodes = snq(
            () => Array.from(mutation.target.childNodes).filter(node => node instanceof HTMLElement),
            [],
          );

          if (!htmlNodes.length) {
            this.removeFromDOM();
            this.disconnect();
          } else {
            setTimeout(() => {
              this.disconnect();
            }, 0);
          }
        });
      });

      observer.observe(this.focusedElement, {
        childList: true,
      });
    } else {
      setTimeout(() => {
        const htmlNodes = snq(
          () => Array.from(this.focusedElement.childNodes).filter(node => node instanceof HTMLElement),
          [],
        );

        if (!htmlNodes.length) this.removeFromDOM();
      }, 0);
    }

    this.completed$.subscribe(() => observer.disconnect());
  }

  disconnect() {
    this.completed$.next();
    this.completed$.complete();
  }

  removeFromDOM() {
    this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
  }
}
