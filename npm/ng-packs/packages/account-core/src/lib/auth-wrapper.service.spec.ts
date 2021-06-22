import { TestBed } from '@angular/core/testing';

import { AuthWrapperService } from './auth-wrapper.service';

describe('AuthWrapperService', () => {
  let service: AuthWrapperService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AuthWrapperService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
