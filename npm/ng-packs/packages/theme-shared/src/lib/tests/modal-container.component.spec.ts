import { Component, ComponentFactoryResolver, ComponentRef } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { ModalContainerComponent } from '../components/modal/modal-container.component';

describe('ModalContainerComponent', () => {
  @Component({ template: '<div class="foo">bar</div>' })
  class TestComponent {}

  let componentRef: ComponentRef<TestComponent>;
  let spectator: Spectator<ModalContainerComponent>;

  const createComponent = createComponentFactory({
    component: ModalContainerComponent,
    entryComponents: [TestComponent],
  });

  beforeEach(() => (spectator = createComponent()));

  afterEach(() => componentRef.destroy());

  describe('#container', () => {
    it('should be a ViewContainerRef', () => {
      let foo = document.querySelector('div.foo');
      expect(foo).toBeNull();

      const cfResolver = spectator.inject(ComponentFactoryResolver);
      const factory = cfResolver.resolveComponentFactory(TestComponent);
      componentRef = spectator.component.container.createComponent(factory);

      foo = document.querySelector('div.foo');
      expect(foo.textContent).toBe('bar');
    });
  });
});
