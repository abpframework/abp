import { AfterViewInit, Directive, ElementRef, Input, Optional, Renderer2 } from '@angular/core';
import { Subject } from 'rxjs';

/**
 *
 * @deprecated To be deleted in v5.0
 */
@Directive({
  selector: '[abpVisibility]',
})
export class VisibilityDirective implements AfterViewInit {
  @Input('abpVisibility')
  focusedElement: HTMLElement;

  completed$ = new Subject<boolean>();

  constructor(@Optional() private elRef: ElementRef, private renderer: Renderer2) {}

  ngAfterViewInit() {
    if (!this.focusedElement && this.elRef) {
      this.focusedElement = this.elRef.nativeElement;
    }

    const observer = new MutationObserver(mutations => {
      mutations.forEach(mutation => {
        if (!mutation.target) return;

        const htmlNodes =
          Array.from(mutation.target.childNodes || []).filter(
            node => node instanceof HTMLElement,
          ) || [];

        if (!htmlNodes.length) {
          this.removeFromDOM();
        }
      });
    });

    observer.observe(this.focusedElement, {
      childList: true,
    });

    setTimeout(() => {
      const htmlNodes =
        Array.from(this.focusedElement.childNodes || []).filter(
          node => node instanceof HTMLElement,
        ) || [];

      if (!htmlNodes.length) this.removeFromDOM();
    }, 0);

    this.completed$.subscribe(() => observer.disconnect());
  }

  disconnect() {
    this.completed$.next();
    this.completed$.complete();
  }

  removeFromDOM() {
    if (!this.elRef.nativeElement) return;

    this.renderer.removeChild(this.elRef.nativeElement.parentElement, this.elRef.nativeElement);
    this.disconnect();
  }
}
