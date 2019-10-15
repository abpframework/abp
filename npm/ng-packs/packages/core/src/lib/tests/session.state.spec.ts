import { createServiceFactory, SpectatorService } from '@ngneat/spectator/jest';
import { Session } from '../models/session';
import { LocalizationService } from '../services';
import { SessionState } from '../states';
import { GetAppConfiguration } from '../actions/config.actions';
import { of } from 'rxjs';

export class DummyClass {}

export const SESSION_STATE_DATA = {
  language: 'tr',
  tenant: { id: 'd5692aef-2ac6-49cd-9f3e-394c0bd4f8b3', name: 'Test' },
} as Session.State;

describe('SessionState', () => {
  let spectator: SpectatorService<DummyClass>;
  let state: SessionState;

  const createService = createServiceFactory({
    service: DummyClass,
    mocks: [LocalizationService],
  });

  beforeEach(() => {
    spectator = createService();
    state = new SessionState(spectator.get(LocalizationService));
  });

  describe('#getLanguage', () => {
    it('should return the current language', () => {
      expect(SessionState.getLanguage(SESSION_STATE_DATA)).toEqual(SESSION_STATE_DATA.language);
    });
  });

  describe('#getTenant', () => {
    it('should return the tenant object', () => {
      expect(SessionState.getTenant(SESSION_STATE_DATA)).toEqual(SESSION_STATE_DATA.tenant);
    });
  });

  describe('#SetLanguage', () => {
    it('should set the language and dispatch the GetAppConfiguration action', () => {
      let patchedData;
      let dispatchedData;
      const patchState = jest.fn(data => (patchedData = data));
      const dispatch = jest.fn(action => {
        dispatchedData = action;
        return of({});
      });
      const spy = jest.spyOn(spectator.get(LocalizationService), 'registerLocale');

      state.setLanguage({ patchState, dispatch } as any, { payload: 'en' }).subscribe();

      expect(patchedData).toEqual({ language: 'en' });
      expect(dispatchedData instanceof GetAppConfiguration).toBeTruthy();
      expect(spy).toHaveBeenCalledWith('en');
    });
  });

  describe('#setTenantId', () => {
    it('should set the tenant', () => {
      let patchedData;
      const patchState = jest.fn(data => (patchedData = data));
      const testTenant = { id: '54ae02ba-9289-4c1b-8521-0ea437756288', name: 'Test Tenant' };
      state.setTenant({ patchState } as any, { payload: testTenant });

      expect(patchedData).toEqual({ tenant: testTenant });
    });
  });
});
