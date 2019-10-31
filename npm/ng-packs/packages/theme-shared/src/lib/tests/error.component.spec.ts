import { SpectatorHost, createHostFactory } from '@ngneat/spectator/jest';
import { ErrorComponent } from '../components/error/error.component';
import { LocalizationPipe } from '@abp/ng.core';
import { Store } from '@ngxs/store';
import { Renderer2, ElementRef } from '@angular/core';

describe('ErrorComponent', () => {
  let spectator: SpectatorHost<ErrorComponent>;
  const createHost = createHostFactory({
    component: ErrorComponent,
    declarations: [LocalizationPipe],
    mocks: [Store],
    providers: [
      { provide: Renderer2, useValue: { removeChild: () => null } },
      { provide: ElementRef, useValue: { nativeElement: document.createElement('div') } },
    ],
  });

  beforeEach(() => (spectator = createHost('<abp-error></abp-error>')));

  describe('#destroy', () => {
    it('should remove the dom', () => {
      const renderer = spectator.get(Renderer2);
      const rendererSpy = jest.spyOn(renderer, 'removeChild');
      spectator.component.renderer = renderer;

      const elementRef = spectator.get(ElementRef);
      spectator.component.elementRef = elementRef;
      spectator.component.host = spectator.hostComponent;

      spectator.click('button#abp-close-button');
      spectator.detectChanges();
      expect(rendererSpy).toHaveBeenCalledWith(spectator.hostComponent, elementRef.nativeElement);
    });
  });
});
