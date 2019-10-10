import { createHttpFactory, HttpMethod, SpectatorHttp, SpyObject } from '@ngneat/spectator/jest';
import { RestService } from '../services/rest.service';
import { Store } from '@ngxs/store';

describe('HttpClient testing', () => {
  let spectator: SpectatorHttp<RestService>;
  let store: SpyObject<Store>;
  const api = 'https://abp.io';

  const createHttp = createHttpFactory({ dataService: RestService, mocks: [Store] });

  beforeEach(() => {
    spectator = createHttp();
    store = spectator.get(Store);
    store.selectSnapshot.andReturn(api);
  });

  test('should send a GET request with params', () => {
    spectator.service.request({ method: HttpMethod.GET, url: '/test', params: { id: 1 } }).subscribe();
    spectator.expectOne(api + '/test?id=1', HttpMethod.GET);
  });
});
