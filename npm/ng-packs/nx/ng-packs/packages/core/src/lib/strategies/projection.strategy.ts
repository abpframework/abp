import {
  ApplicationRef,
  ComponentFactoryResolver,
  ComponentRef,
  EmbeddedViewRef,
  Injector,
  TemplateRef,
  Type,
  ViewContainerRef,
} from '@angular/core';
import { InferredContextOf, InferredInstanceOf } from '../models/utility';
import { ContainerStrategy, CONTAINER_STRATEGY } from './container.strategy';
import { ContextStrategy, CONTEXT_STRATEGY } from './context.strategy';
import { DomStrategy, DOM_STRATEGY } from './dom.strategy';

export abstract class ProjectionStrategy<T = any> {
  constructor(public content: T) {}

  abstract injectContent(injector: Injector): ComponentRefOrEmbeddedViewRef<T>;
}

export class ComponentProjectionStrategy<T extends Type<any>> extends ProjectionStrategy<T> {
  constructor(
    component: T,
    private containerStrategy: ContainerStrategy,
    private contextStrategy: ContextStrategy = CONTEXT_STRATEGY.None(),
  ) {
    super(component);
  }

  injectContent(injector: Injector) {
    this.containerStrategy.prepare();

    const resolver = injector.get(ComponentFactoryResolver) as ComponentFactoryResolver;
    const factory = resolver.resolveComponentFactory<InferredInstanceOf<T>>(this.content);

    const componentRef = this.containerStrategy.containerRef.createComponent(
      factory,
      this.containerStrategy.getIndex(),
      injector,
    );
    this.contextStrategy.setContext(componentRef);

    return componentRef as ComponentRefOrEmbeddedViewRef<T>;
  }
}

export class RootComponentProjectionStrategy<T extends Type<any>> extends ProjectionStrategy<T> {
  constructor(
    component: T,
    private contextStrategy: ContextStrategy = CONTEXT_STRATEGY.None(),
    private domStrategy: DomStrategy = DOM_STRATEGY.AppendToBody(),
  ) {
    super(component);
  }

  injectContent(injector: Injector) {
    const appRef = injector.get(ApplicationRef);
    const resolver = injector.get(ComponentFactoryResolver) as ComponentFactoryResolver;
    const componentRef = resolver
      .resolveComponentFactory<InferredInstanceOf<T>>(this.content)
      .create(injector);

    this.contextStrategy.setContext(componentRef);

    appRef.attachView(componentRef.hostView);
    const element: HTMLElement = (componentRef.hostView as EmbeddedViewRef<any>).rootNodes[0];
    this.domStrategy.insertElement(element);

    return componentRef as ComponentRefOrEmbeddedViewRef<T>;
  }
}

export class TemplateProjectionStrategy<T extends TemplateRef<any>> extends ProjectionStrategy<T> {
  constructor(
    templateRef: T,
    private containerStrategy: ContainerStrategy,
    private contextStrategy = CONTEXT_STRATEGY.None(),
  ) {
    super(templateRef);
  }

  injectContent() {
    this.containerStrategy.prepare();

    const embeddedViewRef = this.containerStrategy.containerRef.createEmbeddedView(
      this.content,
      this.contextStrategy.context,
      this.containerStrategy.getIndex(),
    );
    embeddedViewRef.detectChanges();

    return embeddedViewRef as ComponentRefOrEmbeddedViewRef<T>;
  }
}

export const PROJECTION_STRATEGY = {
  AppendComponentToBody<T extends Type<unknown>>(
    component: T,
    context?: Partial<InferredInstanceOf<T>>,
  ) {
    return new RootComponentProjectionStrategy<T>(
      component,
      context && CONTEXT_STRATEGY.Component(context),
    );
  },
  AppendComponentToContainer<T extends Type<unknown>>(
    component: T,
    containerRef: ViewContainerRef,
    context?: Partial<InferredInstanceOf<T>>,
  ) {
    return new ComponentProjectionStrategy<T>(
      component,
      CONTAINER_STRATEGY.Append(containerRef),
      context && CONTEXT_STRATEGY.Component(context),
    );
  },
  AppendTemplateToContainer<T extends TemplateRef<unknown>>(
    templateRef: T,
    containerRef: ViewContainerRef,
    context?: Partial<InferredContextOf<T>>,
  ) {
    return new TemplateProjectionStrategy<T>(
      templateRef,
      CONTAINER_STRATEGY.Append(containerRef),
      context && CONTEXT_STRATEGY.Template(context),
    );
  },
  PrependComponentToContainer<T extends Type<unknown>>(
    component: T,
    containerRef: ViewContainerRef,
    context?: Partial<InferredInstanceOf<T>>,
  ) {
    return new ComponentProjectionStrategy<T>(
      component,
      CONTAINER_STRATEGY.Prepend(containerRef),
      context && CONTEXT_STRATEGY.Component(context),
    );
  },
  PrependTemplateToContainer<T extends TemplateRef<unknown>>(
    templateRef: T,
    containerRef: ViewContainerRef,
    context?: Partial<InferredContextOf<T>>,
  ) {
    return new TemplateProjectionStrategy<T>(
      templateRef,
      CONTAINER_STRATEGY.Prepend(containerRef),
      context && CONTEXT_STRATEGY.Template(context),
    );
  },
  ProjectComponentToContainer<T extends Type<unknown>>(
    component: T,
    containerRef: ViewContainerRef,
    context?: Partial<InferredInstanceOf<T>>,
  ) {
    return new ComponentProjectionStrategy<T>(
      component,
      CONTAINER_STRATEGY.Clear(containerRef),
      context && CONTEXT_STRATEGY.Component(context),
    );
  },
  ProjectTemplateToContainer<T extends TemplateRef<unknown>>(
    templateRef: T,
    containerRef: ViewContainerRef,
    context?: Partial<InferredContextOf<T>>,
  ) {
    return new TemplateProjectionStrategy<T>(
      templateRef,
      CONTAINER_STRATEGY.Clear(containerRef),
      context && CONTEXT_STRATEGY.Template(context),
    );
  },
};

type ComponentRefOrEmbeddedViewRef<T> = T extends Type<infer U>
  ? ComponentRef<U>
  : T extends TemplateRef<infer C>
  ? EmbeddedViewRef<C>
  : never;
