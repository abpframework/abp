import { Directive, OnDestroy, ViewContainerRef, TemplateRef, inject, input, effect } from '@angular/core';
import { EMPTY, from, Observable, of, Subscription } from 'rxjs';

type VisibleInput = boolean | Promise<boolean> | Observable<boolean> | undefined | null;

@Directive({
  selector: '[abpVisible]',
})
export class AbpVisibleDirective implements OnDestroy {
  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject<TemplateRef<unknown>>(TemplateRef);

  private conditionSubscription: Subscription | undefined;
  private isVisible: boolean | undefined;
  private condition$: Observable<boolean> = of(false);

  readonly abpVisible = input<VisibleInput>();

  private lastInput: VisibleInput;

  constructor() {
    effect(() => {
      const value = this.abpVisible();
      if (value === this.lastInput) return;
      this.lastInput = value;
      this.condition$ = checkType(value);
      this.subscribeToCondition();
    });
  }

  ngOnDestroy(): void {
    this.conditionSubscription?.unsubscribe();
  }

  private subscribeToCondition() {
    this.conditionSubscription?.unsubscribe();
    this.conditionSubscription = this.condition$.subscribe(value => {
      this.isVisible = value;
      this.updateVisibility();
    });
  }

  private updateVisibility() {
    this.viewContainerRef.clear();
    // it should be false not falsy
    if (this.isVisible === false) {
      return;
    }
    this.viewContainerRef.createEmbeddedView(this.templateRef);
  }
}

function checkType(value: VisibleInput) {
  if (value instanceof Promise) {
    return from(value);
  } else if (value instanceof Observable) {
    return value;
  } else if (typeof value === 'boolean') {
    return of(value);
  } else if (value === undefined || value === null) {
    return of(true);
  } else {
    return EMPTY;
  }
}
