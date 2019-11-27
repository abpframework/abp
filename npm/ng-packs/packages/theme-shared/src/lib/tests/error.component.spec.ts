import { SpectatorHost, createHostFactory } from '@ngneat/spectator/jest';
import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { LocalizationPipe } from '@abp/ng.core';
import { Store } from '@ngxs/store';
import { Renderer2, ElementRef } from '@angular/core';
import { Subject } from 'rxjs';

describe('ErrorComponent', () => {
  let spectator: SpectatorHost<HttpErrorWrapperComponent>;
  const createHost = createHostFactory({
    component: HttpErrorWrapperComponent,
    declarations: [LocalizationPipe],
    mocks: [Store],
    providers: [
      { provide: Renderer2, useValue: { removeChild: () => null } },
      { provide: ElementRef, useValue: { nativeElement: document.createElement('div') } },
    ],
  });

  beforeEach(() => {
    spectator = createHost('<abp-http-error-wrapper></abp-http-error-wrapper>');
    spectator.component.destroy$ = new Subject();
  });

  describe('#destroy', () => {
    it('should be call when pressed the esc key', done => {
      spectator.component.destroy$.subscribe(res => {
        done();
      });

      spectator.keyboard.pressEscape();
    });

    it('should be call when clicked the close button', done => {
      spectator.component.destroy$.subscribe(res => {
        done();
      });

      spectator.click('#abp-close-button');
    });
  });
});
