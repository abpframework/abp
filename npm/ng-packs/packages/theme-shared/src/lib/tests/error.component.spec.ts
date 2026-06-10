import { DOCUMENT } from '@angular/common';
import { Router } from '@angular/router';
import { createComponentFactory, Spectator } from '@ngneat/spectator/vitest';
import { Pipe, PipeTransform } from '@angular/core';
import { Subject } from 'rxjs';
import { vi } from 'vitest';

import { HttpErrorWrapperComponent } from '../components/http-error-wrapper/http-error-wrapper.component';
import { setupComponentResources } from './utils';

/**
 * Mock pipe to avoid ABP DI chain
 */
@Pipe({ name: 'abpLocalization'})
class MockLocalizationPipe implements PipeTransform {
  transform(value: any): any {
    return value;
  }
}

describe('HttpErrorWrapperComponent', () => {
  let spectator: Spectator<HttpErrorWrapperComponent>;
  let createComponent: ReturnType<typeof createComponentFactory<HttpErrorWrapperComponent>>;

  beforeAll(async () => {
    await setupComponentResources(
      '../components/http-error-wrapper',
      import.meta.url,
    );
  });

  beforeEach(() => {
    if (!createComponent) {
      createComponent = createComponentFactory({
        component: HttpErrorWrapperComponent,
        detectChanges: false,

        overrideComponents: [
          [
            HttpErrorWrapperComponent,
            {
              set: {
                template: '<div></div>',
                imports: [MockLocalizationPipe],
              },
            },
          ],
        ],

        providers: [
          {
            provide: DOCUMENT,
            useValue: document,
          },
          {
            provide: Router,
            useValue: {
              navigateByUrl: vi.fn(),
            },
          },
        ],
      });
    }

    spectator = createComponent();

    spectator.component.destroy$ = new Subject<void>();
    spectator.component.title = '_::Oops!';
    spectator.component.details = '_::Sorry, an error has occured.';
  });

  it('should create component', () => {
    expect(spectator.component).toBeTruthy();
  });

  it('should emit destroy$ when destroy is called', () => {
    const spy = vi.fn();
    spectator.component.destroy$.subscribe(spy);

    spectator.component.destroy();

    expect(spy).toHaveBeenCalled();
  });
});
