import { TestBed } from '@angular/core/testing';

import { AbpNavbarService } from './navbar.service';

describe('NavbarService', () => {
  let service: AbpNavbarService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AbpNavbarService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
