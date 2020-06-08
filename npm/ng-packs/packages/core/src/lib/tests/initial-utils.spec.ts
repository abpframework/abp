import { Component, Injector } from '@angular/core';
import { createComponentFactory, Spectator } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import { of } from 'rxjs';
import { GetAppConfiguration } from '../actions';
import { getInitialData, localeInitializer } from '../utils';

@Component({
  selector: 'abp-dummy',
  template: '',
})
export class DummyComponent {}

describe('InitialUtils', () => {
  let spectator: Spectator<DummyComponent>;
  const createComponent = createComponentFactory({
    component: DummyComponent,
    mocks: [Store],
  });

  beforeEach(() => (spectator = createComponent()));

  describe('#getInitialData', () => {
    test('should dispatch GetAppConfiguration and return', async () => {
      const injector = spectator.get(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const store = spectator.get(Store);
      const dispatchSpy = jest.spyOn(store, 'dispatch');

      injectorSpy.mockReturnValueOnce(store);
      injectorSpy.mockReturnValueOnce({ skipGetAppConfiguration: false });
      injectorSpy.mockReturnValueOnce({ hasValidAccessToken: () => false });
      dispatchSpy.mockReturnValue(of('test'));

      expect(typeof getInitialData(injector)).toBe('function');
      expect(await getInitialData(injector)()).toBe('test');
      expect(dispatchSpy.mock.calls[0][0] instanceof GetAppConfiguration).toBeTruthy();
    });
  });

  describe('#checkAccessToken', () => {
    test('should call logOut fn of OAuthService when token is valid and current user not found', async () => {
      const injector = spectator.get(Injector);
      const injectorSpy = jest.spyOn(injector, 'get');
      const store = spectator.get(Store);
      const dispatchSpy = jest.spyOn(store, 'dispatch');
      const logOutFn = jest.fn();

      injectorSpy.mockReturnValueOnce(store);
      injectorSpy.mockReturnValueOnce({ skipGetAppConfiguration: false });
      injectorSpy.mockReturnValueOnce({ hasValidAccessToken: () => true, logOut: logOutFn });
      dispatchSpy.mockReturnValue(of({ currentUser: { id: null } }));

      getInitialData(injector)();
      expect(logOutFn).toHaveBeenCalled();
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
