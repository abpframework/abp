import {
  Directive,
  TemplateRef,
  ViewContainerRef,
  InjectionToken,
  OnInit,
  OnDestroy,
  Injector,
  inject,
  input,
  effect
} from '@angular/core';
import { Observable, Subscription, of } from 'rxjs';

export interface PageRenderStrategy {
  shouldRender(type?: string): boolean | Observable<boolean>;
  onInit?(type?: string, injector?: Injector, context?: any): void;
  onDestroy?(type?: string, injector?: Injector, context?: any): void;
  onContextUpdate?(context?: any): void;
}

export const PAGE_RENDER_STRATEGY = new InjectionToken<PageRenderStrategy>('PAGE_RENDER_STRATEGY');

@Directive({
  selector: '[abpPagePart]',
})
export class PagePartDirective implements OnInit, OnDestroy {
  private templateRef = inject<TemplateRef<any>>(TemplateRef);
  private viewContainer = inject(ViewContainerRef);
  private renderLogic = inject<PageRenderStrategy>(PAGE_RENDER_STRATEGY, { optional: true })!;
  private injector = inject(Injector);

  hasRendered = false;
  subscription!: Subscription;

  readonly context = input<any>(undefined, { alias: 'abpPagePartContext' });
  readonly abpPagePart = input<string>('');

  constructor() {
    // Watch for type changes
    effect(() => {
      const type = this.abpPagePart();
      if (type) {
        this.createRenderStream(type);
      }
    });

    // Watch for context changes
    effect(() => {
      const ctx = this.context();
      if (this.renderLogic?.onContextUpdate) {
        this.renderLogic.onContextUpdate(ctx);
      }
    });
  }

  render = (shouldRender: boolean) => {
    if (shouldRender && !this.hasRendered) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasRendered = true;
    } else if (!shouldRender && this.hasRendered) {
      this.viewContainer.clear();
      this.hasRendered = false;
    }
  };

  ngOnInit() {
    if (this.renderLogic?.onInit) {
      this.renderLogic.onInit(this.abpPagePart(), this.injector, this.context());
    }
  }

  ngOnDestroy() {
    this.clearSubscription();

    if (this.renderLogic?.onDestroy) {
      this.renderLogic.onDestroy(this.abpPagePart(), this.injector, this.context());
    }
  }

  shouldRender(type: string) {
    if (this.renderLogic) {
      const willRender = this.renderLogic.shouldRender(type);
      return willRender instanceof Observable ? willRender : of(willRender);
    }
    return of(true);
  }

  protected createRenderStream(type: string) {
    this.clearSubscription();

    this.subscription = this.shouldRender(type).subscribe(this.render);
  }

  protected clearSubscription() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
