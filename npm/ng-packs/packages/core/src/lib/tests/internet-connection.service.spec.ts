import { TestBed } from '@angular/core/testing';

import { InternetConnectionService } from '../services/internet-connection-service';

describe('InternetConnectionService', () => {
  let service: InternetConnectionService;
  const internetConnectionStatus = window.navigator.onLine
  
  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(InternetConnectionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('networkStatus value should be same with current internetConnectionStatus', () => {
    expect(service.networkStatus()).toEqual(internetConnectionStatus)
  });

  it('networkStatus$ return value should be with the current internetConnectionStatus', () => {
    service.networkStatus$.subscribe(val=>{
      expect(val).toEqual(internetConnectionStatus)
    })
  });
});
