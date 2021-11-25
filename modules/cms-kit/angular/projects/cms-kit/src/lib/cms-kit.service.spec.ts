import { TestBed } from '@angular/core/testing';

import { CmsKitService } from './cms-kit.service';

describe('CmsKitService', () => {
  let service: CmsKitService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CmsKitService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
