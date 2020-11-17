import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Store } from '@ngxs/store';
import * as ConfigActions from '../actions';
import { ApplicationConfiguration } from '../models/application-configuration';
import { Config } from '../models/config';
import { ConfigStateService } from '../services/config-state.service';
import { ConfigState } from '../states';

describe('ConfigStateService', () => {
  let service: ConfigStateService;
  let spectator: SpectatorService<ConfigStateService>;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({ service: ConfigStateService, mocks: [Store] });
  beforeEach(() => {
    spectator = createService();
    service = spectator.service;
    store = spectator.inject(Store);
  });
  test('should have the all ConfigState static methods', () => {
    const reg = /(?<=static )(.*)(?=\()/gm;
    ConfigState.toString()
      .match(reg)
      .forEach(fnName => {
        expect(service[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'selectSnapshot');
        spy.mockClear();

        const isDynamicSelector = ConfigState[fnName].name !== 'memoized';

        if (isDynamicSelector) {
          ConfigState[fnName] = jest.fn((...args) => args);
          service[fnName]('test', 0, {});
          expect(ConfigState[fnName]).toHaveBeenCalledWith('test', 0, {});
        } else {
          service[fnName]();
          expect(spy).toHaveBeenCalledWith(ConfigState[fnName]);
        }
      });
  });

  test('should have a dispatch method for every ConfigState action', () => {
    const reg = /(?<=dispatch)(\w+)(?=\()/gm;
    ConfigStateService.toString()
      .match(reg)
      .forEach(fnName => {
        expect(ConfigActions[fnName]).toBeTruthy();

        const spy = jest.spyOn(store, 'dispatch');
        spy.mockClear();

        const params = Array.from(new Array(ConfigActions[fnName].length));

        service[`dispatch${fnName}`](...params);
        expect(spy).toHaveBeenCalledWith(new ConfigActions[fnName](...params));
      });
  });
});
