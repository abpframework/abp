import {
  Component,
  ComponentRef,
  EmbeddedViewRef,
  TemplateRef,
  ViewChild,
  ViewContainerRef,
} from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import {
  ComponentProjectionStrategy,
  ContainerStrategy,
  CONTAINER_STRATEGY,
  CONTEXT_STRATEGY,
  DOM_STRATEGY,
  PROJECTION_STRATEGY,
  RootComponentProjectionStrategy,
  TemplateProjectionStrategy,
} from '../strategies';

describe('ComponentProjectionStrategy', () => {
  @Component({
    template: '<div class="foo">{{ bar || baz }}</div>',
  })
  class TestComponent {
    bar: string;
    baz = 'baz';
  }

  @Component({
    template: '<ng-container #container></ng-container>',
  })
  class HostComponent {
    @ViewChild('container', { static: true, read: ViewContainerRef })
    containerRef: ViewContainerRef;
  }

  let containerStrategy: ContainerStrategy;
  let spectator: Spectator<HostComponent>;
  let componentRef: ComponentRef<TestComponent>;

  const createComponent = createComponentFactory({
    component: HostComponent,
    entryComponents: [TestComponent],
  });

  beforeEach(() => {
    spectator = createComponent({});
    containerStrategy = CONTAINER_STRATEGY.Clear(spectator.component.containerRef);
  });

  afterEach(() => {
    componentRef.destroy();
    spectator.detectChanges();
  });

  describe('#injectContent', () => {
    it('should should insert content into container and return a ComponentRef', () => {
      const strategy = new ComponentProjectionStrategy(TestComponent, containerStrategy);
      componentRef = strategy.injectContent(spectator);
      spectator.detectChanges();

      const div = spectator.query('div.foo');
      expect(div.textContent).toBe('baz');
      expect(componentRef).toBeInstanceOf(ComponentRef);
    });

    it('should be able to map context to projected component', () => {
      const contextStrategy = CONTEXT_STRATEGY.Component({ bar: 'bar' });
      const strategy = new ComponentProjectionStrategy(
        TestComponent,
        containerStrategy,
        contextStrategy,
      );
      componentRef = strategy.injectContent(spectator);
      spectator.detectChanges();

      const div = spectator.query('div.foo');
      expect(div.textContent).toBe('bar');
      expect(componentRef.instance.bar).toBe('bar');
    });
  });
});

describe('RootComponentProjectionStrategy', () => {
  @Component({
    template: '<div class="foo">{{ bar || baz }}</div>',
  })
  class TestComponent {
    bar: string;
    baz = 'baz';
  }

  @Component({ template: '' })
  class HostComponent {}

  let spectator: Spectator<HostComponent>;
  let componentRef: ComponentRef<TestComponent>;

  const createComponent = createComponentFactory({
    component: HostComponent,
    entryComponents: [TestComponent],
  });

  beforeEach(() => {
    spectator = createComponent({});
  });

  afterEach(() => {
    componentRef.destroy();
    spectator.detectChanges();
  });

  describe('#injectContent', () => {
    it('should should insert content into body and return a ComponentRef', () => {
      const strategy = new RootComponentProjectionStrategy(TestComponent);
      componentRef = strategy.injectContent(spectator);
      spectator.detectChanges();

      const div = document.querySelector('body > ng-component > div.foo');
      expect(div.textContent).toBe('baz');
      expect(componentRef).toBeInstanceOf(ComponentRef);
      componentRef.destroy();
      spectator.detectChanges();
    });

    it('should be able to map context to projected component', () => {
      const contextStrategy = CONTEXT_STRATEGY.Component({ bar: 'bar' });
      const strategy = new RootComponentProjectionStrategy(TestComponent, contextStrategy);
      componentRef = strategy.injectContent(spectator);
      spectator.detectChanges();

      const div = document.querySelector('body > ng-component > div.foo');
      expect(div.textContent).toBe('bar');
      expect(componentRef.instance.bar).toBe('bar');
    });
  });
});

describe('TemplateProjectionStrategy', () => {
  @Component({
    template: `
      <ng-template #template let-bar>
        <div class="foo">{{ bar || baz }}</div>
      </ng-template>
      <ng-container #container></ng-container>
    `,
  })
  class HostComponent {
    @ViewChild('container', { static: true, read: ViewContainerRef })
    containerRef: ViewContainerRef;

    @ViewChild('template', { static: true })
    templateRef: TemplateRef<{ $implicit?: string }>;

    baz = 'baz';
  }

  let containerStrategy: ContainerStrategy;
  let spectator: Spectator<HostComponent>;
  let embeddedViewRef: EmbeddedViewRef<{ $implicit?: string }>;

  const createComponent = createComponentFactory({
    component: HostComponent,
  });

  beforeEach(() => {
    spectator = createComponent({});
    containerStrategy = CONTAINER_STRATEGY.Clear(spectator.component.containerRef);
  });

  afterEach(() => {
    embeddedViewRef.destroy();
    spectator.detectChanges();
  });

  describe('#injectContent', () => {
    it('should should insert content into container and return an EmbeddedViewRef', () => {
      const templateRef = spectator.component.templateRef;
      const strategy = new TemplateProjectionStrategy(templateRef, containerStrategy);
      embeddedViewRef = strategy.injectContent(spectator);
      spectator.detectChanges();

      const div = spectator.query('div.foo');
      expect(div.textContent).toBe('baz');
      expect(embeddedViewRef).toHaveProperty('detectChanges');
      expect(embeddedViewRef).toHaveProperty('markForCheck');
      expect(embeddedViewRef).toHaveProperty('detach');
      expect(embeddedViewRef).toHaveProperty('reattach');
      expect(embeddedViewRef).toHaveProperty('destroy');
      expect(embeddedViewRef).toHaveProperty('rootNodes');
      expect(embeddedViewRef).toHaveProperty('context');
    });

    it('should be able to map context to projected template', () => {
      const templateRef = spectator.component.templateRef;
      const contextStrategy = CONTEXT_STRATEGY.Template<typeof templateRef>({ $implicit: 'bar' });
      const strategy = new TemplateProjectionStrategy(
        templateRef,
        containerStrategy,
        contextStrategy,
      );
      embeddedViewRef = strategy.injectContent(spectator);
      spectator.detectChanges();

      const div = spectator.query('div.foo');
      expect(div.textContent).toBe('bar');
      expect(embeddedViewRef.context).toEqual(contextStrategy.context);
    });
  });
});

describe('PROJECTION_STRATEGY', () => {
  const content = undefined;
  const containerRef = ({ length: 0 } as any) as ViewContainerRef;
  test.each`
    name                             | Strategy                       | containerStrategy
    ${'AppendComponentToContainer'}  | ${ComponentProjectionStrategy} | ${CONTAINER_STRATEGY.Append}
    ${'AppendTemplateToContainer'}   | ${TemplateProjectionStrategy}  | ${CONTAINER_STRATEGY.Append}
    ${'PrependComponentToContainer'} | ${ComponentProjectionStrategy} | ${CONTAINER_STRATEGY.Prepend}
    ${'PrependTemplateToContainer'}  | ${TemplateProjectionStrategy}  | ${CONTAINER_STRATEGY.Prepend}
    ${'ProjectComponentToContainer'} | ${ComponentProjectionStrategy} | ${CONTAINER_STRATEGY.Clear}
    ${'ProjectTemplateToContainer'}  | ${TemplateProjectionStrategy}  | ${CONTAINER_STRATEGY.Clear}
  `(
    'should successfully map $name to $Strategy.name with $containerStrategy.name container strategy',
    ({ name, Strategy, containerStrategy }) => {
      expect(PROJECTION_STRATEGY[name](content, containerRef)).toEqual(
        new Strategy(content, containerStrategy(containerRef)),
      );
    },
  );

  const contextStrategy = undefined;
  test.each`
    name                       | Strategy                           | domStrategy
    ${'AppendComponentToBody'} | ${RootComponentProjectionStrategy} | ${DOM_STRATEGY.AppendToBody}
  `(
    'should successfully map $name to $Strategy.name with $domStrategy.name dom strategy',
    ({ name, Strategy, domStrategy }) => {
      expect(PROJECTION_STRATEGY[name](content, contextStrategy)).toEqual(
        new Strategy(content, contextStrategy, domStrategy()),
      );
    },
  );
});
