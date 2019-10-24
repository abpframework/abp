import { createHttpFactory, HttpMethod, SpectatorHttp } from '@ngneat/spectator/jest';
import { ApplicationConfigurationService, RestService } from '../services';
import { Store } from '@ngxs/store';

describe('ApplicationConfigurationService', () => {
  let spectator: SpectatorHttp<ApplicationConfigurationService>;
  const createHttp = createHttpFactory({
    dataService: ApplicationConfigurationService,
    providers: [RestService],
    mocks: [Store],
  });

  beforeEach(() => (spectator = createHttp()));

  it('can test HttpClient.get', () => {
    spectator.get(Store).selectSnapshot.andReturn('https://abp.io');
    spectator.service.getConfiguration().subscribe();
    spectator.expectOne('https://abp.io/api/abp/application-configuration', HttpMethod.GET);
  });
});
