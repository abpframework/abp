import { TestBed } from '@angular/core/testing';

import { AbpLocalStorageService } from '../services/local-storage.service';

describe('LocalStorageService', () => {
  let service: AbpLocalStorageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AbpLocalStorageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
