import { getInitialData, localeInitializer } from '../utils';
import { Injector } from '@angular/core';
import { Spectator, createComponentFactory } from '@ngneat/spectator/jest';
import { Component } from '@angular/core';
import { Store } from '@ngxs/store';
import { of } from 'rxjs';
import { GetAppConfiguration } from '../actions';

@Component({
  selector: 'abp-dummy',
  template: '',
})
export class DummyComponent {}

describe('InitialUtils', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory({ component: DummyComponent, mocks: [Store] });

  beforeEach(() => (spectator = createComponent()));

  describe('#getInitialData', () => {
    test('should dispatch GetAppConfiguration and return', async () => {
      const injector = spectator.get(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const store = spectator.get(Store);
      const dispatchSpy = jest.spyOn(store, 'dispatch');

      injectorSpy.mockReturnValue(store);
      dispatchSpy.mockReturnValue(of('test'));

      expect(typeof getInitialData(injector)).toBe('function');
      expect(await getInitialData(injector)()).toBe('test');
      expect(dispatchSpy.mock.calls[0][0] instanceof GetAppConfiguration).toBeTruthy();
    });
  });

  describe('#localeInitializer', () => {
    test('should resolve registerLocale', async () => {
      const injector = spectator.get(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const store = spectator.get(Store);
      store.selectSnapshot.andCallFake(selector => selector({ SessionState: { language: 'tr' } }));
      injectorSpy.mockReturnValue(store);
      expect(typeof localeInitializer(injector)).toBe('function');
      expect(await localeInitializer(injector)()).toBe('resolved');
    });
  });
});
