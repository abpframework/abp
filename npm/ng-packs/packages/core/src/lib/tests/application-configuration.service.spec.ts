import { createHttpFactory, HttpMethod, SpectatorHttp } from '@ngneat/spectator/jest';
import { ApplicationConfigurationService, RestService } from '../services';
import { Store } from '@ngxs/store';
import { CORE_OPTIONS } from '../tokens';

describe('ApplicationConfigurationService', () => {
  let spectator: SpectatorHttp<ApplicationConfigurationService>;
  const createHttp = createHttpFactory({
    dataService: ApplicationConfigurationService,
    providers: [RestService, { provide: CORE_OPTIONS, useValue: { environment: {} } }],
    mocks: [Store],
  });

  beforeEach(() => (spectator = createHttp()));

  it('should send a GET to application-configuration API', () => {
    spectator.inject(Store).selectSnapshot.andReturn('https://abp.io');
    spectator.service.getConfiguration().subscribe();
    spectator.expectOne('https://abp.io/api/abp/application-configuration', HttpMethod.GET);
  });
});
