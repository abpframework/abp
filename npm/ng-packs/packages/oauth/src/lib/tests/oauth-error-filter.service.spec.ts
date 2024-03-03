import { createServiceFactory, SpectatorService } from '@ngneat/spectator';
import { OAuthErrorFilterService } from '../services';
import { AuthErrorEvent, AuthErrorFilter } from '@abp/ng.core';
import { OAuthErrorEvent } from 'angular-oauth2-oidc';

const ids = {
  firstFilter: 'firstFilter',
  secondFilter: 'secondFilter',
};
type Reason = object & { error: { grant_type: string | undefined; }; };


describe('AuthService', () => {
  let spectator: SpectatorService<OAuthErrorFilterService>;
  let oAuthErrorFilterService: OAuthErrorFilterService;
  const createService = createServiceFactory(OAuthErrorFilterService);

  let firstFilter: AuthErrorFilter = {
    id: '',
    executable: Boolean(),
    execute: () => Boolean(),
  };
  let secondFilter: AuthErrorFilter = {
    id: '',
    executable: Boolean(),
    execute: () => Boolean(),
  };

  beforeEach(() => {
    spectator = createService();
    oAuthErrorFilterService = spectator.inject(OAuthErrorFilterService);
    firstFilter = {
      id: ids.firstFilter,
      executable: true,
      execute: (event: AuthErrorEvent) => {
        const { reason } = event;
        const {
          error: { grant_type },
        } = <Reason>(reason || {});
  
        return !!grant_type && grant_type === ids.firstFilter;
      },
    };
  
    secondFilter = {
      id: ids.secondFilter,
      executable: true,
      execute: (event: AuthErrorEvent) => {
        const { reason } = event;
        const {
          error: { grant_type },
        } = <Reason>(reason || {});
  
        return !!grant_type && grant_type === ids.secondFilter;
      },
    };
  });

  it('should add filter to filter service', () => {
    oAuthErrorFilterService.add(firstFilter);

    expect(oAuthErrorFilterService.filters()).toContain(firstFilter);
    expect(oAuthErrorFilterService.filters()).toHaveLength(1);

    oAuthErrorFilterService.add(secondFilter);
    expect(oAuthErrorFilterService.filters()).toHaveLength(2);
  });

  it('should be able to get item by id', () => {
    oAuthErrorFilterService.add(firstFilter);

    expect(oAuthErrorFilterService.get(ids.firstFilter)).toBe(firstFilter);
  });

  it('should be able to patch item', () => {
    oAuthErrorFilterService.add(firstFilter);
    oAuthErrorFilterService.patch({ id: ids.firstFilter, executable: false });

    expect(oAuthErrorFilterService.get(ids.firstFilter)?.executable).toBe(false);
  });

  it('should be able to remove item', () => {
    oAuthErrorFilterService.add(firstFilter);
    oAuthErrorFilterService.add(secondFilter);

    oAuthErrorFilterService.remove(ids.firstFilter);
    expect(oAuthErrorFilterService.filters()).not.toContain(firstFilter);
    expect(oAuthErrorFilterService.filters()).toContain(secondFilter);
    expect(oAuthErrorFilterService.filters()).toHaveLength(1);
  });

  it('if any execute method returns true, it should return the true value.', () => {
    oAuthErrorFilterService.add(firstFilter);
    oAuthErrorFilterService.add(secondFilter);

    const event = {
      reason: {
        error: {
          grant_type: ids.secondFilter,
        },
      },
    } as OAuthErrorEvent;

    expect(oAuthErrorFilterService.run(event)).toBe(true);
  });

  it('if none of execute method returns true, it should return the false.', () => {
    oAuthErrorFilterService.add(firstFilter);
    oAuthErrorFilterService.add(secondFilter);

    const event = {
      reason: {
        error: {
          grant_type: 'random-event',
        },
      },
    } as OAuthErrorEvent;

    expect(oAuthErrorFilterService.run(event)).toBe(false);
  });
});