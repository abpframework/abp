import { TestBed} from '@angular/core/testing';
import { DOCUMENT } from '@angular/common';

import { InternetConnectionService } from '../services/internet-connection-service';
import { BehaviorSubject, first, of, takeLast } from 'rxjs';
import { computed, signal } from '@angular/core';

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
    service.networkStatus$.pipe(first()).subscribe(value => {
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
    service.networkStatus$.pipe(first()).subscribe(value => {
      expect(value).toBe(true)
      done();
    });
  });
});

describe('when connection value changes', () => {
  let mockDocument = { defaultView: {navigator: {onLine: true}, addEventListener: (eventName,cb)=>{
    window.addEventListener(eventName,cb)
  }, dispatchEvent: function(event){
    this.navigator.onLine = !this.navigator.onLine
  }}}

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers:[{provide:DOCUMENT, useValue: mockDocument}]
    })
    service = TestBed.inject(InternetConnectionService);
  });
  
  it('check is signal value changes', () => {
    expect(service.networkStatus()).toEqual(true);
    const event = new Event("offline");
    service.window.dispatchEvent(event)
    service.setStatus(service.window.navigator.onLine)
    expect(service.networkStatus()).toEqual(false);
  });

  it('check is observable value changes',(done: any) => {
    const event = new Event("online");
    service.window.dispatchEvent(event)
    service.networkStatus$.pipe(first()).subscribe(value => {
      expect(value).toEqual(true)
      done()
    });
    service.window.dispatchEvent(event)
    service.setStatus(service.window.navigator.onLine)
    service.networkStatus$.pipe().subscribe(value => {
      expect(value).toEqual(false)
      done()
    });
  });
});