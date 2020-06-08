import { Component, TemplateRef, ViewChild } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { ModalContainerComponent } from '../components/modal/modal-container.component';
import { ModalService } from '../services';

describe('ModalContainerComponent', () => {
  @Component({
    template: `
      <ng-template #ref>
        <div class="foo">bar</div>
      </ng-template>
    `,
  })
  class TestComponent {
    @ViewChild('ref', { static: true })
    template: TemplateRef<any>;

    constructor(public modalService: ModalService) {}
  }

  let spectator: Spectator<TestComponent>;
  let service: ModalService;

  const createComponent = createComponentFactory({
    component: TestComponent,
    entryComponents: [ModalContainerComponent],
  });

  beforeEach(() => {
    spectator = createComponent();
    service = spectator.component.modalService;
  });

  afterEach(() => {
    service.getContainer().clear();
    service['containerComponentRef'].changeDetectorRef.detectChanges();
    service['containerComponentRef'].destroy();
  });

  describe('#getContainer', () => {
    it('should return the ViewContainerRef of ModalContainerComponent', () => {
      let foo = document.querySelector('div.foo');
      expect(foo).toBeNull();

      const containerRef = service.getContainer();
      const embeddedViewRef = containerRef.createEmbeddedView(spectator.component.template);

      foo = document.querySelector('div.foo');
      expect(foo).toBe(embeddedViewRef.rootNodes[0]);
      expect(foo.textContent).toBe('bar');
    });
  });

  describe('#renderTemplate', () => {
    it('should render given template using the ViewContainerRef of ModalContainerComponent', () => {
      let foo = document.querySelector('div.foo');
      expect(foo).toBeNull();

      service.renderTemplate(spectator.component.template);

      foo = document.querySelector('div.foo');
      expect(foo.textContent).toBe('bar');
    });
  });

  describe('#detectChanges', () => {
    it('should call detectChanges on the containerComponentRef', () => {
      const spy = jest.spyOn(service['containerComponentRef'].changeDetectorRef, 'detectChanges');

      service.detectChanges();

      expect(spy).toHaveBeenCalledTimes(1);
    });
  });

  describe('#clearModal', () => {
    it('should call clear on the ViewContainerRef and detectChanges', () => {
      const clear = jest.spyOn(service.getContainer(), 'clear');
      const detectChanges = jest.spyOn(service, 'detectChanges');

      service.clearModal();

      expect(clear).toHaveBeenCalledTimes(1);
      expect(detectChanges).toHaveBeenCalledTimes(1);
    });
  });
});
