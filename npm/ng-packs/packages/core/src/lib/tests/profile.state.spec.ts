import { createServiceFactory, SpectatorService, SpyObject } from '@ngneat/spectator/jest';
import { Session } from '../models/session';
import { ProfileService } from '../services';
import { ProfileState } from '../states';
import { GetAppConfiguration } from '../actions/config.actions';
import { of } from 'rxjs';
import { Profile } from '../models/profile';

export class DummyClass {}

export const PROFILE_STATE_DATA = {
  profile: { userName: 'admin', email: 'info@abp.io', name: 'Admin' },
} as Profile.State;

describe('ProfileState', () => {
  let spectator: SpectatorService<DummyClass>;
  let state: ProfileState;
  let profileService: SpyObject<ProfileService>;
  let patchedData;
  const patchState = jest.fn(data => (patchedData = data));

  const createService = createServiceFactory({
    service: DummyClass,
    mocks: [ProfileService],
  });

  beforeEach(() => {
    spectator = createService();
    profileService = spectator.inject(ProfileService);
    state = new ProfileState(profileService);
  });

  describe('#getProfile', () => {
    it('should return the current language', () => {
      expect(ProfileState.getProfile(PROFILE_STATE_DATA)).toEqual(PROFILE_STATE_DATA.profile);
    });
  });

  describe('#GetProfile', () => {
    it('should call the profile service get method and update the state', () => {
      const mockData = { userName: 'test', email: 'test@abp.io' };
      const spy = jest.spyOn(profileService, 'get');
      spy.mockReturnValue(of(mockData as any));

      state.getProfile({ patchState } as any).subscribe();

      expect(patchedData).toEqual({ profile: mockData });
    });
  });

  describe('#UpdateProfile', () => {
    it('should call the profile service update method and update the state', () => {
      const mockData = { userName: 'test2', email: 'test@abp.io' };
      const spy = jest.spyOn(profileService, 'update');
      spy.mockReturnValue(of(mockData as any));

      state.updateProfile({ patchState } as any, { payload: mockData as any }).subscribe();

      expect(patchedData).toEqual({ profile: mockData });
    });
  });

  describe('#ChangePassword', () => {
    it('should call the profile service changePassword method', () => {
      const mockData = { currentPassword: 'test123', newPassword: 'test123' };
      const spy = jest.spyOn(profileService, 'changePassword');
      spy.mockReturnValue(of(null));

      state.changePassword(null, { payload: mockData }).subscribe();

      expect(spy).toHaveBeenCalledWith(mockData, true);
    });
  });
});
