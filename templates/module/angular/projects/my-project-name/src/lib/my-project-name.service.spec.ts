import { TestBed } from '@angular/core/testing';
import { MyProjectNameService } from './services/my-project-name.service';
import { RestService } from '@abp/ng.core';
import { vi } from 'vitest';

describe('MyProjectNameService', () => {
  let service: MyProjectNameService;
  const mockRestService = {
    request: vi.fn(),
  };

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        {
          provide: RestService,
          useValue: mockRestService,
        },
      ],
    });
    service = TestBed.inject(MyProjectNameService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
