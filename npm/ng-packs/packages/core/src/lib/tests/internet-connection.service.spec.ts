import { TestBed } from '@angular/core/testing';

import { InternetConnectionService } from '../services/internet-connection-service';

describe('InternetConnectionService', () => {
  let service: InternetConnectionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InternetConnectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('#networkStatus should return real value (true | false)', () => {
    expect(typeof service.networkStatus()).toEqual('boolean')
    expect(typeof !service.networkStatus()).toEqual('boolean')
  });

  it('#networkStatus$ should return real value (true | false)', () => {
    service.networkStatus$.subscribe(val=>{
      expect(typeof val).toEqual('boolean')
      expect(typeof !val).toEqual('boolean')
    })
  });
});
