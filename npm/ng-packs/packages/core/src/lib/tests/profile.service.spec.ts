import { createHttpFactory, HttpMethod, SpectatorHttp } from '@ngneat/spectator/jest';
import { ProfileService, RestService } from '../services';
import { Store } from '@ngxs/store';
import { CORE_OPTIONS } from '../tokens';

describe('ProfileService', () => {
  let spectator: SpectatorHttp<ProfileService>;
  const createHttp = createHttpFactory({
    dataService: ProfileService,
    providers: [RestService, { provide: CORE_OPTIONS, useValue: { environment: {} } }],
    mocks: [Store],
  });

  beforeEach(() => (spectator = createHttp()));

  it('should send a GET to my-profile API', () => {
    spectator.inject(Store).selectSnapshot.andReturn('https://abp.io');
    spectator.service.get().subscribe();
    spectator.expectOne('https://abp.io/api/identity/my-profile', HttpMethod.GET);
  });

  it('should send a POST to change-password API', () => {
    const mock = { currentPassword: 'test', newPassword: 'test' };
    spectator.inject(Store).selectSnapshot.andReturn('https://abp.io');
    spectator.service.changePassword(mock).subscribe();
    const req = spectator.expectOne(
      'https://abp.io/api/identity/my-profile/change-password',
      HttpMethod.POST,
    );
    expect(req.request.body).toEqual(mock);
  });

  it('should send a PUT to my-profile API', () => {
    const mock = {
      email: 'info@volosoft.com',
      userName: 'admin',
      name: 'John',
      surname: 'Doe',
      phoneNumber: '+123456',
      isExternal: false,
      hasPassword: false,
    };
    spectator.inject(Store).selectSnapshot.andReturn('https://abp.io');
    spectator.service.update(mock).subscribe();
    const req = spectator.expectOne('https://abp.io/api/identity/my-profile', HttpMethod.PUT);
    expect(req.request.body).toEqual(mock);
  });
});
