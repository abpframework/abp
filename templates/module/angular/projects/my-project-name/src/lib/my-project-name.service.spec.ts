import { TestBed } from '@angular/core/testing';

import { MyProjectNameService } from './my-project-name.service';

describe('MyProjectNameService', () => {
  let service: MyProjectNameService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MyProjectNameService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
