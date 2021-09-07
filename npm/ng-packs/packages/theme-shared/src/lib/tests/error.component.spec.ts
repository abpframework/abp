import { CORE_OPTIONS, LocalizationPipe } from '@abp/ng.core';
import { HttpClient } from '@angular/common/http';
import { ElementRef, Renderer2 } from '@angular/core';
import { createHostFactory, SpectatorHost } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { Subject } from 'rxjs';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';

describe('ErrorComponent', () => {
  let spectator: SpectatorHost<HttpErrorWrapperComponent>;
  const createHost = createHostFactory({
    component: HttpErrorWrapperComponent,
    declarations: [LocalizationPipe],
    mocks: [Store, HttpClient],
    providers: [
      { provide: CORE_OPTIONS, useValue: {} },
      { provide: Renderer2, useValue: { removeChild: () => null } },
      {
        provide: ElementRef,
        useValue: { nativeElement: document.createElement('div') },
      },
    ],
  });

  beforeEach(() => {
    spectator = createHost('<abp-http-error-wrapper></abp-http-error-wrapper>');
    spectator.component.destroy$ = new Subject();
  });

  describe('#destroy', () => {
    it('should be call when pressed the esc key', done => {
      spectator.component.destroy$.subscribe(() => {
        done();
      });

      spectator.keyboard.pressEscape();
    });

    it('should be call when clicked the close button', done => {
      spectator.component.destroy$.subscribe(() => {
        done();
      });

      spectator.click('#abp-close-button');
    });
  });
});
