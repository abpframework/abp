import { Directive, Input, Optional, ElementRef, Renderer2, AfterViewInit } from '@angular/core';
import { Subject } from 'rxjs';
import snq from 'snq';

@Directive({
  selector: '[abpVisibility]',
})
export class VisibilityDirective implements AfterViewInit {
  @Input('abpVisibility')
  focusedElement: HTMLElement;

  completed$ = new Subject<boolean>();

  constructor(@Optional() private elRef: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    const observer = new MutationObserver(mutations => {
      mutations.forEach(mutation => {
        if (!mutation.target) return;

        const htmlNodes = snq(
          () => Array.from(mutation.target.childNodes).filter(node => node instanceof HTMLElement),
          [],
        );

        if (!htmlNodes.length) {
          this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
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

    this.completed$.subscribe(() => observer.disconnect());
  }

  disconnect() {
    this.completed$.next();
    this.completed$.complete();
  }
}
