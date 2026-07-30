import {
  ComponentRef,
  Directive,
  ElementRef,
  EmbeddedViewRef,
  Injector,
  OnDestroy,
  OnInit,
  Renderer2,
  effect,
  inject,
  input,
  ViewContainerRef
} from '@angular/core';
import { Subscription, timer } from 'rxjs';
import { take } from 'rxjs/operators';
import { LoadingComponent } from '../components';

@Directive({
  selector: '[abpLoading]',
  host: {
    '[style.position]': '"relative"'
  }
})
export class LoadingDirective implements OnInit, OnDestroy {
  private elRef = inject<ElementRef<HTMLElement>>(ElementRef);
  private injector = inject(Injector);
  private renderer = inject(Renderer2);
  private viewContainerRef = inject(ViewContainerRef);

  readonly loading = input(false, { alias: 'abpLoading' });
  readonly targetElementInput = input<HTMLElement | undefined>(undefined, { alias: 'abpLoadingTargetElement' });
  readonly delay = input(0, { alias: 'abpLoadingDelay' });

  targetElement: HTMLElement | undefined;

  componentRef: ComponentRef<LoadingComponent> | null = null;
  rootNode: HTMLDivElement | null = null;
  timerSubscription: Subscription | null = null;

  constructor() {
    effect(() => {
      const newValue = this.loading();
      this.handleLoadingChange(newValue);
    });
  }

  private handleLoadingChange(newValue: boolean) {
    setTimeout(() => {
      if (!newValue) {
        this.clearLoading();
        return;
      }

      if (this.timerSubscription) {
        this.timerSubscription.unsubscribe();
      }

      this.timerSubscription = timer(this.delay())
        .pipe(take(1))
        .subscribe(() => {
          if (!this.loading()) {
            return;
          }

          if (!this.componentRef) {
            this.componentRef = this.viewContainerRef.createComponent(LoadingComponent, {
              injector: this.injector
            });
          }

          if (!this.rootNode) {
            this.rootNode = (this.componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0];
            this.targetElement?.appendChild(this.rootNode as HTMLDivElement);
          }

          this.timerSubscription = null;
        });
    }, 0);
  }

  private clearLoading() {
    if (this.timerSubscription) {
      this.timerSubscription.unsubscribe();
      this.timerSubscription = null;
    }

    if (this.rootNode?.parentElement) {
      this.renderer.removeChild(this.rootNode.parentElement, this.rootNode);
      this.rootNode = null;
    }
  }

  ngOnInit() {
    this.targetElement = this.targetElementInput();
    if (!this.targetElement) {
      const { offsetHeight, offsetWidth } = this.elRef.nativeElement;
      if (!offsetHeight && !offsetWidth && this.elRef.nativeElement.children?.length) {
        this.targetElement = this.elRef.nativeElement.children[0] as HTMLElement;
      } else {
        this.targetElement = this.elRef.nativeElement;
      }
    }
  }

  ngOnDestroy() {
    this.clearLoading();
  }
}
