import {
  Directive,
  ElementRef,
  Input,
  OnDestroy,
  OnInit,
  Renderer2,
  ViewContainerRef,
  TemplateRef,
  Optional,
  SimpleChanges,
  OnChanges,
} from '@angular/core';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';
import { takeUntilDestroy } from '../utils';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[abpPermission]',
})
export class PermissionDirective implements OnInit, OnDestroy, OnChanges {
  @Input('abpPermission') condition: string;

  subscription: Subscription;

  constructor(
    private elRef: ElementRef,
    private renderer: Renderer2,
    private store: Store,
    @Optional() private templateRef: TemplateRef<any>,
    private vcRef: ViewContainerRef,
  ) {}

  private check() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    this.subscription = this.store
      .select(ConfigState.getGrantedPolicy(this.condition))
      .pipe(takeUntilDestroy(this))
      .subscribe(isGranted => {
        if (this.templateRef && isGranted) {
          this.vcRef.clear();
          this.vcRef.createEmbeddedView(this.templateRef);
        } else if (this.templateRef && !isGranted) {
          this.vcRef.clear();
        } else if (!isGranted && !this.templateRef) {
          this.renderer.removeChild((this.elRef.nativeElement as HTMLElement).parentElement, this.elRef.nativeElement);
        }
      });
  }

  ngOnInit() {
    if (this.templateRef && !this.condition) {
      this.vcRef.createEmbeddedView(this.templateRef);
    }
  }

  ngOnDestroy(): void {}

  ngOnChanges({ condition }: SimpleChanges) {
    if ((condition || { currentValue: null }).currentValue) {
      this.check();
    }
  }
}
