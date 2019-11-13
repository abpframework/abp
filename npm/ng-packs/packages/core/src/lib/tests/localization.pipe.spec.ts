import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { LocalizationPipe } from '../pipes';
import { Store } from '@ngxs/store';
import { ConfigState } from '../states';

describe('LocalizationPipe', () => {
  let spectator: SpectatorService<LocalizationPipe>;
  let pipe: LocalizationPipe;
  let store: SpyObject<Store>;

  const createService = createServiceFactory({ service: LocalizationPipe, mocks: [Store] });

  beforeEach(() => {
    spectator = createService();
    pipe = spectator.get(LocalizationPipe);
    store = spectator.get(Store);
  });

  it('should call getLocalization selector', () => {
    const storeSpy = jest.spyOn(store, 'selectSnapshot');
    const configStateSpy = jest.spyOn(ConfigState, 'getLocalization');

    pipe.transform('test', '1', '2');
    pipe.transform('test2', ['3', '4'] as any);
    expect(configStateSpy).toHaveBeenCalledWith('test', '1', '2');
    expect(configStateSpy).toHaveBeenCalledWith('test2', '3', '4');
  });
});
