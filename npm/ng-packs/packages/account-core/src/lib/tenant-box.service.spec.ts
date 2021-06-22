import { TestBed } from '@angular/core/testing';

import { TenantBoxService } from './tenant-box.service';

describe('TenantBoxService', () => {
  let service: TenantBoxService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TenantBoxService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
