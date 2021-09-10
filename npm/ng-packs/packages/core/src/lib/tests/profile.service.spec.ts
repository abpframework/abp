import { createHttpFactory, HttpMethod, SpectatorHttp, SpyObject } from '@ngneat/spectator/jest';
import { EnvironmentService, ProfileService, RestService } from '../services';
import { CORE_OPTIONS } from '../tokens';

describe('ProfileService', () => {
  let spectator: SpectatorHttp<ProfileService>;
  let environmentService: SpyObject<EnvironmentService>;

  const createHttp = createHttpFactory({
    service: ProfileService,
    providers: [RestService, { provide: CORE_OPTIONS, useValue: {} }],
    mocks: [EnvironmentService],
  });

  beforeEach(() => {
    spectator = createHttp();
    environmentService = spectator.inject(EnvironmentService);
    const getApiUrlSpy = jest.spyOn(environmentService, 'getApiUrl');
    getApiUrlSpy.mockReturnValue('https://abp.io');
  });

  it('should send a GET to my-profile API', () => {
    spectator.service.get().subscribe();
    spectator.expectOne('https://abp.io/api/identity/my-profile', HttpMethod.GET);
  });

  it('should send a POST to change-password API', () => {
    const mock = { currentPassword: 'test', newPassword: 'test' };
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
    spectator.service.update(mock).subscribe();
    const req = spectator.expectOne('https://abp.io/api/identity/my-profile', HttpMethod.PUT);
    expect(req.request.body).toEqual(mock);
  });
});
