import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { of } from 'rxjs';
import { ApplicationConfigurationService, RestService } from '../services';

describe('ApplicationConfigurationService', () => {
  let spectator: SpectatorService<ApplicationConfigurationService>;
  const createService = createServiceFactory({
    service: ApplicationConfigurationService,
    mocks: [RestService],
  });

  beforeEach(() => (spectator = createService()));

  it('should send a GET to application-configuration API', () => {
    const rest = spectator.inject(RestService);

    const requestSpy = jest.spyOn(rest, 'request');
    requestSpy.mockReturnValue(of(null));

    spectator.service.getConfiguration().subscribe();

    expect(requestSpy).toHaveBeenCalledWith(
      { method: 'GET', url: '/api/abp/application-configuration' },
      {},
    );
  });
});
