import { OnInit, Directive, OnDestroy, Input, ViewContainerRef, TemplateRef, inject } from '@angular/core';
import { EMPTY, from, Observable, of, Subscription } from 'rxjs';

@Directive({
  selector: '[abpVisible]',
})
export class AbpVisibleDirective implements OnDestroy, OnInit {
  private viewContainerRef = inject(ViewContainerRef);
  private templateRef = inject<TemplateRef<unknown>>(TemplateRef);

  conditionSubscription: Subscription | undefined;
  isVisible: boolean | undefined;

  @Input() set abpVisible(
    value: boolean | Promise<boolean> | Observable<boolean> | undefined | null,
  ) {
    this.condition$ = checkType(value);
    this.subscribeToCondition();
  }

  private condition$: Observable<boolean> = of(false);
  ngOnInit(): void {
    this.updateVisibility();
  }
  ngOnDestroy(): void {
    this.conditionSubscription?.unsubscribe();
  }

  private subscribeToCondition() {
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

function checkType(value: boolean | Promise<boolean> | Observable<boolean> | undefined | null) {
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
