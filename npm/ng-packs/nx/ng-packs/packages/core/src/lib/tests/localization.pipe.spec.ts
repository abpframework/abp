import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { LocalizationPipe } from '../pipes';
import { LocalizationService } from '../services';

describe('LocalizationPipe', () => {
  let spectator: SpectatorService<LocalizationPipe>;
  let pipe: LocalizationPipe;
  let localizationService: SpyObject<LocalizationService>;

  const createService = createServiceFactory({
    service: LocalizationPipe,
    mocks: [LocalizationService],
  });

  beforeEach(() => {
    spectator = createService();
    pipe = spectator.inject(LocalizationPipe);
    localizationService = spectator.inject(LocalizationService);
  });

  it('should call getLocalization selector', () => {
    const translateSpy = jest.spyOn(localizationService, 'instant');

    pipe.transform('test', '1', '2');
    pipe.transform('test2', ['3', '4'] as any);
    expect(translateSpy).toHaveBeenCalledWith('test', '1', '2');
    expect(translateSpy).toHaveBeenCalledWith('test2', '3', '4');
  });
});
