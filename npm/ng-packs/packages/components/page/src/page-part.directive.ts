import {
  Directive,
  TemplateRef,
  ViewContainerRef,
  Input,
  InjectionToken,
  Optional,
  Inject,
  OnInit,
  OnDestroy,
  Injector,
  OnChanges,
  SimpleChanges,
  SimpleChange,
} from '@angular/core';
import { Observable, Subscription, of } from 'rxjs';

export interface PageRenderStrategy {
  shouldRender(type?: string): boolean | Observable<boolean>;
  onInit?(type?: string, injector?: Injector, context?: any): void;
  onDestroy?(type?: string, injector?: Injector, context?: any): void;
  onContextUpdate?(change?: SimpleChange): void;
}

export const PAGE_RENDER_STRATEGY = new InjectionToken<PageRenderStrategy>('PAGE_RENDER_STRATEGY');

@Directive({ selector: '[abpPagePart]' })
export class PagePartDirective implements OnInit, OnDestroy, OnChanges {
  hasRendered = false;
  type: string;
  subscription: Subscription;

  @Input('abpPagePartContext') context: any;
  @Input() set abpPagePart(type: string) {
    this.type = type;
    this.createRenderStream(type);
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

  constructor(
    private templateRef: TemplateRef<any>,
    private viewContainer: ViewContainerRef,
    @Optional() @Inject(PAGE_RENDER_STRATEGY) private renderLogic: PageRenderStrategy,
    private injector: Injector,
  ) {}

  ngOnChanges({ context }: SimpleChanges): void {
    if (this.renderLogic?.onContextUpdate) {
      this.renderLogic.onContextUpdate(context);
    }
  }

  ngOnInit() {
    if (this.renderLogic?.onInit) {
      this.renderLogic.onInit(this.type, this.injector, this.context);
    }
  }

  ngOnDestroy() {
    this.clearSubscription();

    if (this.renderLogic?.onDestroy) {
      this.renderLogic.onDestroy(this.type, this.injector, this.context);
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
