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
import { InferedInstanceOf } from '../models/utility';
import { ContainerStrategy, CONTAINER_STRATEGY } from './container.strategy';
import {
  ComponentContextStrategy,
  ContextStrategy,
  CONTEXT_STRATEGY,
  TemplateContextStrategy,
} from './context.strategy';
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
    const factory = resolver.resolveComponentFactory<InferedInstanceOf<T>>(this.content);

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
      .resolveComponentFactory<InferedInstanceOf<T>>(this.content)
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
    template: T,
    private containerStrategy: ContainerStrategy,
    private contextStrategy = CONTEXT_STRATEGY.None(),
  ) {
    super(template);
  }

  injectContent(injector: Injector) {
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
    contextStrategy?: ComponentContextStrategy<T>,
  ) {
    return new RootComponentProjectionStrategy<T>(component, contextStrategy);
  },
  AppendComponentToContainer<T extends Type<unknown>>(
    component: T,
    containerRef: ViewContainerRef,
    contextStrategy?: ComponentContextStrategy<T>,
  ) {
    return new ComponentProjectionStrategy<T>(
      component,
      CONTAINER_STRATEGY.Append(containerRef),
      contextStrategy,
    );
  },
  AppendTemplateToContainer<T extends TemplateRef<unknown>>(
    template: T,
    containerRef: ViewContainerRef,
    contextStrategy?: TemplateContextStrategy<T>,
  ) {
    return new TemplateProjectionStrategy<T>(
      template,
      CONTAINER_STRATEGY.Append(containerRef),
      contextStrategy,
    );
  },
  PrependComponentToContainer<T extends Type<unknown>>(
    component: T,
    containerRef: ViewContainerRef,
    contextStrategy?: ComponentContextStrategy<T>,
  ) {
    return new ComponentProjectionStrategy<T>(
      component,
      CONTAINER_STRATEGY.Prepend(containerRef),
      contextStrategy,
    );
  },
  PrependTemplateToContainer<T extends TemplateRef<unknown>>(
    template: T,
    containerRef: ViewContainerRef,
    contextStrategy?: TemplateContextStrategy<T>,
  ) {
    return new TemplateProjectionStrategy<T>(
      template,
      CONTAINER_STRATEGY.Prepend(containerRef),
      contextStrategy,
    );
  },
  ProjectComponentToContainer<T extends Type<unknown>>(
    component: T,
    containerRef: ViewContainerRef,
    contextStrategy?: ComponentContextStrategy<T>,
  ) {
    return new ComponentProjectionStrategy<T>(
      component,
      CONTAINER_STRATEGY.Clear(containerRef),
      contextStrategy,
    );
  },
  ProjectTemplateToContainer<T extends TemplateRef<unknown>>(
    template: T,
    containerRef: ViewContainerRef,
    contextStrategy?: TemplateContextStrategy<T>,
  ) {
    return new TemplateProjectionStrategy<T>(
      template,
      CONTAINER_STRATEGY.Clear(containerRef),
      contextStrategy,
    );
  },
};

type ComponentRefOrEmbeddedViewRef<T> = T extends Type<infer U>
  ? ComponentRef<U>
  : T extends TemplateRef<infer C>
  ? EmbeddedViewRef<C>
  : never;
