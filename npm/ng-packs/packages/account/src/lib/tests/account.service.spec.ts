import { createHttpFactory, HttpMethod, SpectatorHttp } from '@ngneat/spectator/jest';
import { AccountService } from '../services/account.service';
import { Store } from '@ngxs/store';
import { RestService } from '@abp/ng.core';
import { RegisterRequest } from '../models/user';

describe('AccountService', () => {
  let spectator: SpectatorHttp<AccountService>;
  const createHttp = createHttpFactory({
    dataService: AccountService,
    providers: [RestService],
    mocks: [Store],
  });

  beforeEach(() => (spectator = createHttp()));

  it('should send a GET to find tenant', () => {
    spectator.get(Store).selectSnapshot.andReturn('https://abp.io');
    spectator.service.findTenant('test').subscribe();
    spectator.expectOne('https://abp.io/api/abp/multi-tenancy/tenants/by-name/test', HttpMethod.GET);
  });

  it('should send a POST to register API', () => {
    const mock = {
      userName: 'test',
      emailAddress: 'test@test.com',
      password: 'test1234',
      appName: 'Angular',
    } as RegisterRequest;
    spectator.get(Store).selectSnapshot.andReturn('https://abp.io');
    spectator.service.register(mock).subscribe();
    const req = spectator.expectOne('https://abp.io/api/account/register', HttpMethod.POST);
    expect(req.request.body).toEqual(mock);
  });
});
