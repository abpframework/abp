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

export interface PageRenderStrategy {
  shouldRender(type: string);
  onInit?(type: string, injector: Injector, context?: any);
  onDestroy?(type: string, injector?: Injector, context?: any);
  onContextUpdate?(change: SimpleChange);
}

export const PAGE_RENDER_STRATEGY = new InjectionToken<PageRenderStrategy>('PAGE_RENDER_STRATEGY');

@Directive({ selector: '[abpPagePart]' })
export class PagePartDirective implements OnInit, OnDestroy, OnChanges {
  hasRendered = false;
  type: string;

  @Input() set abpPagePart(type: string) {
    this.type = type;
    const shouldRender = this.shouldRender(type);

    if (shouldRender && !this.hasRendered) {
      this.viewContainer.createEmbeddedView(this.templateRef);
      this.hasRendered = true;
    } else if (!shouldRender && this.hasRendered) {
      this.viewContainer.clear();
      this.hasRendered = false;
    }
  }

  @Input('abpPagePartContext') context: any;

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
    if (this.renderLogic?.onDestroy) {
      this.renderLogic.onDestroy(this.type, this.injector, this.context);
    }
  }

  shouldRender(type: string) {
    if (this.renderLogic) {
      return this.renderLogic.shouldRender(type);
    }
    return true;
  }
}
