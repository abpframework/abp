import { TestBed} from '@angular/core/testing';
import { DOCUMENT } from '@angular/common';

import { InternetConnectionService } from '../services/internet-connection-service';

let service: InternetConnectionService;

describe('Internet connection when disconnected', () => {
  let mockDocument = { defaultView: {navigator: {onLine: false}, addEventListener: jest.fn()} }
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers:[{provide:DOCUMENT, useValue: mockDocument}]
    })
    service = TestBed.inject(InternetConnectionService);
  });

  it('document should be created', () => {
    expect(service.document).toEqual(mockDocument);
  });

  it('signal value should be false', () => {
    expect(service.networkStatus()).toEqual(false);
  });

  it('observable value should be false',
    (done: any) => {
    service.networkStatus$.subscribe(value => {
      expect(value).toBe(false)
      done();
    });
  });
});

describe('Internet connection when connected', () => {
  let mockDocument = { defaultView: {navigator: {onLine: true}, addEventListener: jest.fn()} }
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers:[{provide:DOCUMENT, useValue: mockDocument}]
    })
    service = TestBed.inject(InternetConnectionService);
  });


  it('signal value should be true', () => {
    expect(service.networkStatus()).toEqual(true);
  });

  it('observable value should be true',
    (done: any) => {
    service.networkStatus$.subscribe(value => {
      expect(value).toBe(true)
      done();
    });
  });
});