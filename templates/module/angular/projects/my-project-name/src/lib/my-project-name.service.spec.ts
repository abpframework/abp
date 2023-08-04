import { TestBed } from '@angular/core/testing';
import { MyProjectNameService } from './services/my-project-name.service';
import { RestService } from '@abp/ng.core';

describe('MyProjectNameService', () => {
  let service: MyProjectNameService;
  const mockRestService = jasmine.createSpyObj('RestService', ['request']);
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
