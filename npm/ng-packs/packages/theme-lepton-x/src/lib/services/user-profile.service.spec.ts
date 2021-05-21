import { TestBed } from '@angular/core/testing';

import { AbpUserProfileService } from './user-profile.service';

describe('UserProfileService', () => {
  let service: AbpUserProfileService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AbpUserProfileService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
