import { SpectatorHost, createHostFactory } from '@ngneat/spectator/jest';
import { ErrorComponent } from '../components/error/error.component';
import { LocalizationPipe } from '@abp/ng.core';
import { Store } from '@ngxs/store';
import { Renderer2, ElementRef } from '@angular/core';
import { Subject } from 'rxjs';

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

  beforeEach(() => {
    spectator = createHost('<abp-error></abp-error>');
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
