import { TestBed } from '@angular/core/testing';

import { AbpBreadcrumbService } from './breadcrumb.service';

describe('BreadcrumbService', () => {
  let service: AbpBreadcrumbService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AbpBreadcrumbService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
